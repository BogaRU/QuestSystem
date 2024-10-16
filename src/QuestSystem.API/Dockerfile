FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuestSystem.API/QuestSystem.API.csproj", "QuestSystem.API/"]
COPY ["QuestSystem.Application/QuestSystem.Application.csproj", "QuestSystem.Application/"]
COPY ["QuestSystem.Domain/QuestSystem.Domain.csproj", "QuestSystem.Domain/"]
COPY ["QuestSystem.Infrastructure/QuestSystem.Infrastructure.csproj", "QuestSystem.Infrastructure/"]
RUN dotnet restore "QuestSystem.API/QuestSystem.API.csproj"

COPY . .
WORKDIR "/src/QuestSystem.API"
RUN dotnet build "QuestSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuestSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestSystem.API.dll"]
