FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY *.sln ./
COPY ./*.csproj ./src/

# Копіювання інших файлів проекту та збірка
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 6565

ENTRYPOINT ["dotnet", "App.dll", "--urls", "http://*:6565"]
