#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InvestmentFundManager.Api/InvestmentFundManager.Api.csproj", "InvestmentFundManager.Api/"]
COPY ["InvestmentFundManager.Application/InvestmentFundManager.Application.csproj", "InvestmentFundManager.Application/"]
COPY ["InvestmentFundManager.Domain/InvestmentFundManager.Domain.csproj", "InvestmentFundManager.Domain/"]
COPY ["InvestmentFundManager.Infrastructure/InvestmentFundManager.Infrastructure.csproj", "InvestmentFundManager.Infrastructure/"]
RUN dotnet restore "./InvestmentFundManager.Api/InvestmentFundManager.Api.csproj"
COPY . .
WORKDIR "/src/InvestmentFundManager.Api"
RUN dotnet build "./InvestmentFundManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InvestmentFundManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvestmentFundManager.Api.dll"]