using Atm.Autenticador.Api.Extensions;
using Atm.Autenticador.Dados.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Autenticador.Api
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
            services.SetupCors();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            services.AddMediatR(GetType().Assembly);
            services.SetupDatabase(Configuration);
            services.SetupSwagger(Configuration);
            services.SetupFluentValidation(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("Development"))
                app.UseDeveloperExceptionPage();

            app.SetupLocalization("pt-BR");
            app.SetupSwagger(Configuration);
            app.SetupDatabase();
            app.SetupEndpoints();
        }
    }
}
