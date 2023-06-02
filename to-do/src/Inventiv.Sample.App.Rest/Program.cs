using Gazel.ServiceClient;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseGazelServiceProvider();

builder.Services.AddGazelApiApplication(configuration,
    serviceClient: c => c.Routine(ServiceUrl.Localhost(5000)),
    restApi: c => c.Standard(),
    logging: c => c.Log4Net(LogLevel.Information, l => l.DefaultConsoleAppenders())
);

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseGazel();

app.Run();
