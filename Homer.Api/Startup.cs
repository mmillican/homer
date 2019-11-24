using AutoMapper;
using Homer.Api.Config;
using Homer.Shared;
using Homer.Shared.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using System;
using System.Collections.Generic;

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
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDbContext<HomerDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<AuthConfig>(Configuration.GetSection("Auth"));

            services.AddCors(config =>
            {
                config.AddPolicy("default", policy => policy
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            var oktaDomain = $"https://{Configuration["Auth:OktaDomain"]}";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            })
            .AddOktaWebApi(new OktaWebApiOptions()
            {
                OktaDomain = oktaDomain,
                Audience = "api://default"
            });
            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Homer API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    OpenIdConnectUrl = new Uri($"{oktaDomain}/.well-known/openid-configuration"),
                    Flows = new OpenApiOAuthFlows { 
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl= new Uri($"{oktaDomain}/oauth2/default/v1/authorize"),
                            TokenUrl = new Uri($"{oktaDomain}/oauth2/default/v1/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "" },
                                { "profile", "" },
                                { "https://homer.localhost/homer", "" }
                            }
                        }
                    },
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                });
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
            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "Homer API V1");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "Homer API Docs";

                c.OAuthClientId("0oaoizww4uOTFm8hO0h7");
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "nonce", Guid.NewGuid().ToString().Replace("-", "") }
                });
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
