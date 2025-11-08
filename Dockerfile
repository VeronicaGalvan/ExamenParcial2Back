# Etapa 1: construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

# Encuentra automáticamente el archivo csproj
RUN find . -name "*.csproj" -type f | head -1 | xargs -I {} dotnet restore {}
RUN find . -name "*.csproj" -type f | head -1 | xargs -I {} dotnet publish {} -c Release -o /app/out

# Etapa 2: ejecutar el proyecto
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Encuentra automáticamente el DLL principal
CMD find . -name "*.dll" -type f | grep -v "ref/" | head -1 | xargs -I {} dotnet {}