# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RecordingGrpcService.Grpc/RecordingGrpcService.Grpc.csproj", "RecordingGrpcService.Grpc/"]
COPY ["RecordingGrpcService.Application/RecordingGrpcService.Application.csproj", "RecordingGrpcService.Application/"]
COPY ["RecordingGrpcService.Infrastructure/RecordingGrpcService.Infrastructure.csproj", "RecordingGrpcService.Infrastructure/"]
COPY ["RecordingGrpcService.Domain/RecordingGrpcService.Domain.csproj", "RecordingGrpcService.Domain/"]
RUN dotnet restore "./RecordingGrpcService.Grpc/RecordingGrpcService.Grpc.csproj"
COPY . .
WORKDIR "/src/RecordingGrpcService.Grpc"
RUN dotnet build "./RecordingGrpcService.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RecordingGrpcService.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecordingGrpcService.Grpc.dll"]
