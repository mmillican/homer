using AutoMapper;
using Homer.Shared;
using Homer.Shared.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<HomerDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Homer API", Version = "v1" });
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homer API V1");
                c.RoutePrefix = string.Empty;

                //c.OAuthClientId("0oam5vnvhdJ285M160h7");
                //c.OAuthClientSecret("Hv36qRZzX92H3_kbu0pIb2jC8uc5PnPlgcD-Dr-b");
                //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                //{
                //    { "nonce", Guid.NewGuid().ToString().Replace("-", "") }
                //});
            });

            app.UseMvc();
        }
    }
}
