using Gazel.Logging;
using Gazel.ServiceClient;
using Gazel.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Routine;
using System.Threading.Tasks;

namespace Inventiv.Todo.App.Rest
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration) => this.configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();
            services.AddGazelApiApplication(configuration,
                serviceClient: c => c.Routine(ServiceUrl.Localhost(5000)),
                restApi: c => c.Standard(),
                logging: c => c.Log4Net(LogLevel.Info, l => l.DefaultConsoleAppenders())
            );
            services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(x => x.FullName);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseGazel();
            app.UseCors();
            app.UseEndpoints(endpoints =>
                        endpoints.MapGet("/", context =>
                        {
                            context.Response.Redirect("/swagger/index.html");

                            return Task.CompletedTask;
                        })
                    );
        }
    }
}