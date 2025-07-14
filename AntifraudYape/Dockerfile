FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Presentation/AntifraudYape.Presentation/AntifraudYape.Api/AntifraudYape.Api.csproj","src/Presentation/AntifraudYape.Presentation/"]
COPY ["src/Application/AntifraudYape.Application.csproj","src/Application/AntifraudYape.Application/"]
COPY ["src/Domain/AntifraudYape.Domain.csproj","src/Application/AntifraudYape.Domain/"]
COPY ["src/Infrastructure/AntifraudYape.Infrastructure/AntifraudYape.Infrastructure.csproj","src/Infrastructure/AntifraudYape.Infrastructure/"]
COPY ["src/Infrastructure/AntifraudYape.Persistence.Sql/AntifraudYape.Persistence.Sql.csproj","src/Infrastructure/Persistence/AntifraudYape.Persistence.Sql/"]

RUN dotnet restore "src/Presentation/AntifraudYape.Presentation/AntifraudYape.Api.csproj"
COPY . .
WORKDIR "src/Presentation"
RUN dotnet build "./AntifraudYape.Api.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AntifraudYape.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntifraudYape.Api.dll"]

#FROM ubuntu
#WORKDIR /app
#COPY --from=publish /app/publish .
#CMD ["tail", "-f", "/dev/null"] 