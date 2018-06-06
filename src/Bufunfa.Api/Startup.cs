using JNogueira.Bufunfa.Api;
using JNogueira.Bufunfa.Api.Filters;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Servicos;
using JNogueira.Bufunfa.Infraestrutura.Dados;
using JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

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

            var tokenParams = new TokenParametros();

            new ConfigureFromConfigurationOptions<TokenParametros>(Configuration.GetSection("TokenParametros"))
                .Configure(tokenParams);

            services.AddSingleton(tokenParams);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Valida a assinatura de um token recebido
                        ValidateIssuerSigningKey = true,
                        // Verifica se um token recebido ainda é válido
                        ValidateLifetime = true,
                        ValidIssuer = tokenParams.Issuer,
                        ValidAudience = tokenParams.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenParams.SecretKey)),
                        // Tempo de tolerância para a expiração de um token (utilizado
                        // caso haja problemas de sincronismo de horário entre diferentes
                        // computadores envolvidos no processo de comunicação)
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Ativa o uso do token como forma de autorizar o acesso a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(CustomExceptionFilter));
            });

            services.AddScoped<BufunfaDataContext, BufunfaDataContext>(x => new BufunfaDataContext(Configuration["BufunfaConnectionString"]));
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();

            services.AddTransient<IUsuarioServico, UsuarioServico>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
        }
    }
}
