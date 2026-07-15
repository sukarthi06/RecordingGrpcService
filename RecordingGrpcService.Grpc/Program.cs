using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Application.Services;
using RecordingGrpcService.Grpc.Common;
using RecordingGrpcService.Grpc.Mappers;
using RecordingGrpcService.Grpc.Services;
using RecordingGrpcService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Program.cs
builder.Services.AddGrpc(options => options.Interceptors.Add<ExceptionInterceptor>());
builder.Services.AddGrpcReflection();

builder.Host.AddHostInfrastructure(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<RecordingMapper>();
builder.Services.AddSingleton<RecordingChunkMapper>();

builder.Services.AddScoped<IRecordingService, RecordingService>();
builder.Services.AddScoped<IRecordingChunkService, RecordingChunkService>();

var app = builder.Build();

if(app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.MapGrpcReflectionService();
}

// Configure the HTTP request pipeline.
app.MapGrpcService<RecordingsService>();
app.MapGrpcService<RecordingsChunkService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
