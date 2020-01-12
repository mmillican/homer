using AutoMapper;
using Homer.Api.Config;
using Homer.Shared;
using Homer.Shared.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

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
                    .WithOrigins("http://localhost:8080", "https://home.millican.me")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            var awsRegion = Configuration["AWS:Region"];
            var awsCognitoUserPoolId = Configuration["AWS:CognitoUserPoolId"];
            var awsCognitoUrl = $"https://cognito-idp.{awsRegion}.amazonaws.com/{awsCognitoUserPoolId}";
            var awsCognitoAppClientId = Configuration["AWS:CognitoAppClientId"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                    {
                        // get JsonWebKeySet from AWS
                        var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                        // serialize the result
                        var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                        // cast the result to be the type expected by IssuerSigningKeyResolver
                        return (IEnumerable<SecurityKey>)keys;
                    },

                    ValidIssuer = awsCognitoUrl,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudience = awsCognitoAppClientId,
                    ValidateAudience = true
                };
                options.Audience = awsCognitoAppClientId;
                options.Authority = awsCognitoUrl;
            });
            
            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Homer API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    OpenIdConnectUrl = new Uri($"{awsCognitoUrl}/.well-known/openid-configuration"),
                    Flows = new OpenApiOAuthFlows { 
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl= new Uri($"{awsCognitoUrl}/oauth2/authorize"),
                            TokenUrl = new Uri($"{awsCognitoUrl}/oauth2/token"),
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

            services.AddHomerServices(Configuration);
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
                c.SwaggerEndpoint("swagger/v1/swagger.json", "Homer API V1");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "Homer API Docs";

                c.OAuthClientId(Configuration["AWS:CognitoAppClientId"]);
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "nonce", Guid.NewGuid().ToString().Replace("-", "") }
                });
            });

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
