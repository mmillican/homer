using AutoMapper;
using Homer.Shared;
using Homer.Shared.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Homer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<HomerDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddCors(config =>
            {
                config.AddPolicy("default", policy => policy
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Homer API", Version = "v1" });
                //c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    AuthorizationUrl = "https://dev-510139.oktapreview.com/oauth2/default/v1/authorize",
                //    TokenUrl = "https://dev-510139.oktapreview.com/oauth2/default/v1/token",
                //    Flow = "implicit",
                //    Scopes = new Dictionary<string, string>
                //    {
                //        { "openid", "" },
                //        { "profile", "" },
                //        // { "https://loanr.localhost/loanr", "Access to Loanr API" }
                //    }
                //});
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //{
                //    {"oauth2", new [] { "https://loanr.localhost/loanr" } }
                //});
            });

            services.AddHomerServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("default");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homer API V1");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "Homer API Docs";

                //c.OAuthClientId("0oam5vnvhdJ285M160h7");
                //c.OAuthClientSecret("Hv36qRZzX92H3_kbu0pIb2jC8uc5PnPlgcD-Dr-b");
                //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                //{
                //    { "nonce", Guid.NewGuid().ToString().Replace("-", "") }
                //});
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
