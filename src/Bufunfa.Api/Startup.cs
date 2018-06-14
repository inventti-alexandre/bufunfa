using JNogueira.Bufunfa.Api;
using JNogueira.Bufunfa.Api.Middlewares;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Servicos;
using JNogueira.Bufunfa.Infraestrutura.Dados;
using JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bufunfa.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<BufunfaDataContext, BufunfaDataContext>(x => new BufunfaDataContext(Configuration["BufunfaConnectionString"]));

            // AddTransient: determina que referências desta classe sejam geradas toda vez que uma dependência for encontrada
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();

            services.AddTransient<IUsuarioServico, UsuarioServico>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            // Configuração realizada, seguindo o artigo "ASP.NET Core 2.0: autenticação em APIs utilizando JWT" 
            // (https://medium.com/@renato.groffe/asp-net-core-2-0-autentica%C3%A7%C3%A3o-em-apis-utilizando-jwt-json-web-tokens-4b1871efd)

            var tokenConfig = new JwtTokenConfig();

            // Extrai as informações do arquivo appsettings.json, criando um instância da classe "TokenJwtConfig"
            new ConfigureFromConfigurationOptions<JwtTokenConfig>(Configuration.GetSection("JwtTokenConfig"))
                .Configure(tokenConfig);

            // AddSingleton: instância configurada de forma que uma única referência das mesmas seja empregada durante todo o tempo em que a aplicação permanecer em execução
            services.AddSingleton(tokenConfig);

            services
                // AddAuthentication: especificará os schemas utilizados para a autenticação do tipo Bearer
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // AddJwtBearer: definidas configurações como a chave e o algoritmo de criptografia utilizados, a necessidade de analisar se um token ainda é válido e o tempo de tolerância para expiração de um token
                .AddJwtBearer(options =>
                {
                    var paramsValidation = options.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = tokenConfig.Key;
                    paramsValidation.ValidAudience = tokenConfig.Audience;
                    paramsValidation.ValidIssuer = tokenConfig.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            // AddAuthorization: ativará o uso de tokens com o intuito de autorizar ou não o acesso a recursos da aplicação
            services.AddAuthorization(options =>
            {
                // Adiciona as policies de acesso, definindo os claimns existentes em cada policy.
                options.AddPolicy(PermissaoAcesso.ConsultarUsuario, policy => policy.RequireClaim(PermissaoAcesso.ConsultarUsuario).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });

            // Configuração do Swagger para documentação da API
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info());


                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Name = "Authorization",
                    Description = "Digite \"Bearer \" e cole seu token no campo abaixo.",
                    In = "header"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                options.OperationFilter<ExamplesOperationFilter>(); // Permite a exibição de exemplos para o request e response.
                options.OperationFilter<DescriptionOperationFilter>(); // Permite a documentação das propriedades das classes de exemplos com o atributo [Description]
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization

                string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                options.IncludeXmlComments(caminhoXmlDoc);
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseStaticFiles();

            // Middleware customizado para interceptar erros HTTP e exceptions não tratadas
            app.UseCustomExceptionHandler();

            // Middleware para utilização do Swagger.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            // Middleware para utilização do Swagger UI (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs"; // Define a documentação no endereço http://localhost/docs/
                options.SwaggerEndpoint("/docs/v1/swagger.json", "v1");
                options.DefaultModelsExpandDepth(-1); // Oculta a sessão "Models"
                options.DocExpansion(DocExpansion.List);
                options.InjectStylesheet("/swagger-ui/custom.css");
                options.DocumentTitle = "Bufunfa API v1";
                options.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("JNogueira.Bufunfa.Api.Swagger.UI.index.html"); // Permite a utilização de um index.html customizado
            });

            app.UseMvc();
        }
    }
}
