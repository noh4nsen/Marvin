using Atm.Clientes.Dados.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void SetupEndpoints(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();
            applicationBuilder.UseCors();
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireAuthorization();
                endpoints.MapGet(
                    "/",
                    (context) =>
                    {
                        context.Response
                               .Redirect(context.Request.PathBase + "/swagger/index.html", permanent: false);
                        return Task.CompletedTask;
                    }
                );
                endpoints.MapHealthChecks("/health");
            });
        }

        public static void SetupSwagger(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", configuration.GetValue<string>("swagger:endpoint"));
            });
        }

        public static void SetupLocalization(this IApplicationBuilder applicationBuilder, string localization)
        {
            applicationBuilder.UseRequestLocalization
                            (options =>
                                options.AddSupportedCultures(localization)
                                       .AddSupportedUICultures(localization)
                                       .SetDefaultCulture(localization)
                            );
        }

        public static void SetupDatabase(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.GetServiceScope()
                              .DatabaseMigrate();
        }

        private static IServiceScope GetServiceScope(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.ApplicationServices
                                     .GetRequiredService<IServiceScopeFactory>()
                                     .CreateScope();
        }
    }
}
