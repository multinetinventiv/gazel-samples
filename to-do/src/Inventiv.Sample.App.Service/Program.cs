var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseGazelServiceProvider();

builder.Services.AddGazelServiceApplication(configuration,
    database: c => c.Sqlite("gazel.tutorial.db"),
    service: c => c.Routine("http://localhost:5000/service"),
    businessLogic: c => c.Routine(
        developmentMode: builder.Environment.IsDevelopment()
    ),
    logging: c => c.Log4Net(LogLevel.Information, l => l.DefaultConsoleAppenders()),
    fileSystem: c => c.Local("Files")
);

var app = builder.Build();

app.UseGazel();

app.Run();
