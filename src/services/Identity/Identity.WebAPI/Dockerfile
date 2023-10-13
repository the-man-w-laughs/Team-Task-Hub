FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=debug
WORKDIR /src
COPY ["src/services/Identity/Identity.WebAPI/Identity.WebAPI.csproj", "src/services/Identity/Identity.WebAPI/"]
RUN dotnet restore "src/services/Identity/Identity.WebAPI/Identity.WebAPI.csproj"
COPY . .
COPY src/services/Identity/Identity.WebAPI/signing_key.pfx /src/signing_key.pfx
WORKDIR "/src/src/services/Identity/Identity.WebAPI"
RUN dotnet build "Identity.WebAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=debug
RUN dotnet publish "Identity.WebAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /src/signing_key.pfx /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.WebAPI.dll"]
