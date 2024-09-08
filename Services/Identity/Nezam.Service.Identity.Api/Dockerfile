FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Identity/Nezam.Service.Identity.Api/Nezam.Service.Identity.Api.csproj", "Services/Identity/Nezam.Service.Identity.Api/"]
COPY ["Nezam.Common/Nezam.Common.csproj", "Nezam.Common/"]
COPY ["Services/Identity/Nezam.Service.Identity.Domain/Nezam.Service.Identity.Domain.csproj", "Services/Identity/Nezam.Service.Identity.Domain/"]
COPY ["Services/Identity/Nezam.Service.Identity.Infrastructure/Nezam.Service.Identity.Infrastructure.csproj", "Services/Identity/Nezam.Service.Identity.Infrastructure/"]
RUN dotnet restore "Services/Identity/Nezam.Service.Identity.Api/Nezam.Service.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Nezam.Service.Identity.Api"
RUN dotnet build "Nezam.Service.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Nezam.Service.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nezam.Service.Identity.Api.dll"]
