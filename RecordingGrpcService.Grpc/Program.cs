using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Application.Services;
using RecordingGrpcService.Grpc.Common;
using RecordingGrpcService.Grpc.Mappers;
using RecordingGrpcService.Grpc.Services;
using RecordingGrpcService.Infrastructure;
using RecordingGrpcService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Program.cs
builder.Services.AddGrpc(options => options.Interceptors.Add<ExceptionInterceptor>());

builder.WebHost.AddWebHostInfrastructure();
builder.Host.AddHostInfrastructure(builder.Configuration);
await builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<RecordingMapper>();
builder.Services.AddSingleton<RecordingChunkMapper>();

builder.Services.AddScoped<IRecordingService, RecordingService>();
builder.Services.AddScoped<IRecordingChunkService, RecordingChunkService>();

if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Local"))
{
    builder.Services.AddGrpcReflection();
}

var app = builder.Build();

if(app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.MapGrpcReflectionService();
}

await app.Services.InitialiseDatabaseAsync();

// Configure the HTTP request pipeline.
app.MapGrpcService<RecordingsService>();
app.MapGrpcService<RecordingsChunkService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
