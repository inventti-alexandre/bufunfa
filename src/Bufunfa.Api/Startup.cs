using JNogueira.Bufunfa.Api;
using JNogueira.Bufunfa.Api.Attributes;
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
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace Bufunfa.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddScoped<EfDataContext, EfDataContext>(x => new EfDataContext(Configuration["BufunfaConnectionString"]));
            services.AddScoped<IUow, Uow>();

            // AddTransient: determina que referências desta classe sejam geradas toda vez que uma dependência for encontrada
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddTransient<IContaRepositorio, ContaRepositorio>();
            services.AddTransient<ICartaoCreditoRepositorio, CartaoCreditoRepositorio>();
            services.AddTransient<IPeriodoRepositorio, PeriodoRepositorio>();
            services.AddTransient<IPessoaRepositorio, PessoaRepositorio>();
            services.AddTransient<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddTransient<IAgendamentoRepositorio, AgendamentoRepositorio>();
            services.AddTransient<ILancamentoRepositorio, LancamentoRepositorio>();
            services.AddTransient<IParcelaRepositorio, ParcelaRepositorio>();

            services.AddTransient<IUsuarioServico, UsuarioServico>();
            services.AddTransient<IContaServico, ContaServico>();
            services.AddTransient<ICartaoCreditoServico, CartaoCreditoServico>();
            services.AddTransient<IPeriodoServico, PeriodoServico>();
            services.AddTransient<IPessoaServico, PessoaServico>();
            services.AddTransient<ICategoriaServico, CategoriaServico>();
            services.AddTransient<IAgendamentoServico, AgendamentoServico>();
            services.AddTransient<ILancamentoServico, LancamentoServico>();

            // Configuração realizada, seguindo o artigo "ASP.NET Core 2.0: autenticação em APIs utilizando JWT" 
            // (https://medium.com/@renato.groffe/asp-net-core-2-0-autentica%C3%A7%C3%A3o-em-apis-utilizando-jwt-json-web-tokens-4b1871efd)

            var tokenConfig = new JwtTokenConfig();

            // Extrai as informações do arquivo appsettings.json, criando um instância da classe "JwtTokenConfig"
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
                options.AddPolicy(PermissaoAcesso.Usuarios, policy => policy.RequireClaim(PermissaoAcesso.Usuarios).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.Contas, policy => policy.RequireClaim(PermissaoAcesso.Contas).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.Periodos, policy => policy.RequireClaim(PermissaoAcesso.Periodos).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.Pessoas, policy => policy.RequireClaim(PermissaoAcesso.Pessoas).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.Categorias, policy => policy.RequireClaim(PermissaoAcesso.Categorias).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.CartoesCredito, policy => policy.RequireClaim(PermissaoAcesso.CartoesCredito).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.Agendamentos, policy => policy.RequireClaim(PermissaoAcesso.Agendamentos).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(PermissaoAcesso.Lancamentos, policy => policy.RequireClaim(PermissaoAcesso.Lancamentos).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });

            services.AddMvc(options => options.Filters.Add(typeof(CustomModelStateValidationFilter)))
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

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

                options.AddSwaggerExamples(services.BuildServiceProvider()); // Permite a exibição de exemplos para o request e response.

                options.OperationFilter<DescriptionOperationFilter>(); // Permite a documentação das propriedades das classes de exemplos com o atributo [Description]
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization

                string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                options.IncludeXmlComments(caminhoXmlDoc);
                options.EnableAnnotations();
            });

            // Habilita a compressão do response
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Entende que a página default é a "index.html" dentro da pasta "wwwroot"
            app.UseDefaultFiles();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Middleware customizado para interceptar erros HTTP e exceptions não tratadas
                app.UseCustomExceptionHandler();
            }

            // Middleware para utilização do Swagger.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            // Middleware para utilização do Swagger UI (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs"; // Define a documentação no endereço http://{url}/docs/
                options.SwaggerEndpoint("/docs/v1/swagger.json", "v1");
                options.DefaultModelsExpandDepth(-1); // Oculta a sessão "Models"
                options.DocExpansion(DocExpansion.None);
                options.InjectStylesheet("/swagger-ui/custom.css");
                options.DocumentTitle = "Bufunfa API v1";
                options.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("JNogueira.Bufunfa.Api.Swagger.UI.index.html"); // Permite a utilização de um index.html customizado
            });

            // Utiliza a compressão do response
            app.UseResponseCompression();

            app.UseMvc();
        }
    }
}
