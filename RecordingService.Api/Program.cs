using FluentValidation;
using RecordingGrpcService.Application;
using RecordingGrpcService.Infrastructure;
using RecordingService.Api.Endpoints;
using RecordingService.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.AddGatewayKeyVault();
builder.AddServiceDefaults();

builder.Host.AddHostInfrastructure(builder.Configuration);
await builder.Services.AddInfrastructure(builder.Configuration);
if (!builder.Environment.IsEnvironment("Local"))
{
    builder.Services.AddObservability(builder.Configuration);
}

builder.Services.AddApplication();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<GatewaySecretMiddleware>();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

app.MapGet("/", () => "RecordingService.Api is running.");
app.MapRecordingEndpoints();

app.Run();