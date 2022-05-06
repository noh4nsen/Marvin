using Atm.Clientes.Api.Validation;
using FluentValidation;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Atm.Clientes.Api.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static void SetupFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            services.AddTransient<IValidatorFactory, FluentValidationFactory>();
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidatorBehavior<,>));

            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(new FluentValidationExceptionFilter());
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
                    options.OutputFormatters.RemoveType<StringOutputFormatter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        public static void SetupSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationRulesToSwagger();
            services.AddSwaggerGen(options =>
            {
                options.MapType<FileResult>(() => new OpenApiSchema { Type = "file" });
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = configuration.GetValue<string>("swagger:title"),
                        Version = configuration.GetValue<string>("swagger:version")
                    });
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Por favor insira o token JWT: Bearer <jwtToken>",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
            });
        }

        public static void SetupCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void SetupAuthentication(this IServiceCollection services)
        {
            var secret = Encoding.ASCII.GetBytes("d60QQTGeSeZ5UesRf9jH6oL3c8GS49L3U2p62sPCFlYt9LHvFZI8n1agMfyn");
            services.AddAuthentication(authn =>
            {
                authn.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authn.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(authn =>
            {
                authn.RequireHttpsMetadata = false;
                authn.SaveToken = true;
                authn.Audience = "Marvin";
                authn.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret)
                };
            });
        }
    }
}
