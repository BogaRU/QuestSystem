FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["tests/QuestSystem.Application.Tests/QuestSystem.Application.Tests.csproj", "tests/QuestSystem.Application.Tests/"]
COPY ["tests/QuestSystem.Domain.Tests/QuestSystem.Domain.Tests.csproj", "tests/QuestSystem.Domain.Tests/"]
COPY ["src/QuestSystem.Application/QuestSystem.Application.csproj", "src/QuestSystem.Application/"]
COPY ["src/QuestSystem.Domain/QuestSystem.Domain.csproj", "QuestSystem.Domain/"]
COPY ["src/QuestSystem.Infrastructure/QuestSystem.Infrastructure.csproj", "src/QuestSystem.Infrastructure/"]

RUN dotnet restore "tests/QuestSystem.Application.Tests/QuestSystem.Application.Tests.csproj"
RUN dotnet restore "tests/QuestSystem.Domain.Tests/QuestSystem.Domain.Tests.csproj"

COPY . .

RUN dotnet build "tests/QuestSystem.Application.Tests/QuestSystem.Application.Tests.csproj" -c Release
RUN dotnet build "tests/QuestSystem.Domain.Tests/QuestSystem.Domain.Tests.csproj" -c Release

ENTRYPOINT ["dotnet", "test", "--logger:trx", "--results-directory", "/src/test-results"]
