using Gazel.ServiceClient;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseGazelServiceProvider();

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddGazelApiApplication(configuration,
    serviceClient: c => c.Routine(ServiceUrl.Localhost(5000)),
    restApi: c => c.Standard(),
    logging: c => c.Log4Net(LogLevel.Information, l => l.DefaultConsoleAppenders())
);
builder.Services.AddSwaggerGen(config =>
{
    config.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();

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

app.Run();
