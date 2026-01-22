# Этап 1: Сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копирование файлов проекта и восстановление зависимостей
COPY backend.csproj .
RUN dotnet restore

# Копирование исходного кода и сборка
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Этап 2: Запуск
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Установка переменных окружения
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

# Копирование собранного приложения
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "backend.dll"]