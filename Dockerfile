# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el csproj primero (para usar cache de Docker)
COPY ExamenParcial2Back/ExamenParcial2Back.csproj ./

# Restaurar dependencias
RUN dotnet restore ExamenParcial2Back.csproj

# Copiar todo el proyecto
COPY ExamenParcial2Back/ ./

# Publicar la aplicación
RUN dotnet publish ExamenParcial2Back.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out .

# Ejecutar la app
ENTRYPOINT ["dotnet", "ExamenParcial2Back.dll"]
