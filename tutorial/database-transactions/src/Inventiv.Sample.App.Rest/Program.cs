var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseGazelServiceProvider();

builder.Services.AddGazelApiApplication(configuration,
    serviceClient: c => c.Routine("http://localhost:5000/service"),
    restApi: c => c.Standard(),
    logging: c => c.Log4Net(LogLevel.Information, l => l.DefaultConsoleAppenders()),
    authentication: c => c.AllowAnonymous()
);

var app = builder.Build();

app.UseGazel();
app.Run();
