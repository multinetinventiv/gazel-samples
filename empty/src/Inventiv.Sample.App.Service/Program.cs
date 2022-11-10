var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseGazelServiceProvider();

builder.Services.AddGazelServiceApplication(configuration,
    database: c => c.Sqlite("gazel.tutorial.db"),
    service: c => c.Routine("http://localhost:5000/service"),
    logging: c => c.Log4Net(LogLevel.Information, l => l.DefaultConsoleAppenders()),
    authentication: c => c.AllowAnonymous(),
    authorization: c => c.AllowAll()
);

var app = builder.Build();

app.UseGazel();
app.Run();