using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bufunfa.Mvc
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication()
            //    .AddCookie(options =>
            //    {
            //        options.Cookie = new CookieBuilder { Name = "BufunfaCookie" };
            //        options.AccessDeniedPath = new PathString("/login");
            //        options.LoginPath = new PathString("/login");
            //    });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //app.UseAuthentication();

            //app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
