FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7124

ENV ASPNETCORE_URLS=http://+:7124

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=debug
WORKDIR /src
COPY ["src/services/Identity/Identity.Application/Identity.Application.csproj", "services/Identity/Identity.Application/"]
COPY ["src/services/Identity/Identity.Domain/Identity.Domain.csproj", "services/Identity/Identity.Domain/"]
COPY ["src/services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "services/Identity/Identity.Infrastructure/"]
COPY ["src/services/Identity/Identity.WebAPI/Identity.WebAPI.csproj", "services/Identity/Identity.WebAPI/"]

RUN dotnet restore "services/Identity/Identity.WebAPI/Identity.WebAPI.csproj"

WORKDIR "/src/services/Identity/Identity.WebAPI"
COPY . .
RUN dotnet build "Identity.WebAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=debug
COPY ["src/services/Identity/Identity.WebAPI/signing_key.pfx", "/app/"]
RUN dotnet publish "Identity.WebAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.WebAPI.dll"]
