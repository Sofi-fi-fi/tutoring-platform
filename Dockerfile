FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY tutoring-platform.sln ./
COPY src/TutoringPlatform/TutoringPlatform.csproj src/TutoringPlatform/
COPY tests/TutoringPlatform.Tests/TutoringPlatform.Tests.csproj tests/TutoringPlatform.Tests/

RUN dotnet restore

COPY src/ src/
COPY tests/ tests/

WORKDIR /src/src/TutoringPlatform
RUN dotnet build TutoringPlatform.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish TutoringPlatform.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN groupadd -r -g 1001 appuser && \
    useradd -r -u 1001 -g appuser -m -s /sbin/nologin appuser

COPY --from=publish /app/publish .

RUN chown -R appuser:appuser /app

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=3s --start-period=60s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TutoringPlatform.dll"]