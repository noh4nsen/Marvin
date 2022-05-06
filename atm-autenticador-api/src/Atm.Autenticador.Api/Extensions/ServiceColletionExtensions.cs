using Atm.Autenticador.Api.Validation;
using FluentValidation;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Atm.Autenticador.Api.Extensions
{
    [ExcludeFromCodeCoverage]
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
    }
}
