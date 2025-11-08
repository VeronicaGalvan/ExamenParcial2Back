# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el csproj primero
COPY Examen_parcial2back/Examen_parcial2back.csproj ./

# Restaurar dependencias
RUN dotnet restore Examen_parcial2back.csproj

# Copiar todo el proyecto
COPY Examen_parcial2back/ ./

# Publicar la aplicación
RUN dotnet publish Examen_parcial2back.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out .

# Ejecutar la app
ENTRYPOINT ["dotnet", "Examen_parcial2back.dll"]
