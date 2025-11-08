# Etapa 1: construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Examen_parcial2Back/Examen_parcial2Back.csproj"
RUN dotnet publish "./Examen_parcial2Back/Examen_parcial2Back.csproj" -c Release -o /app/out

# Etapa 2: ejecutar el proyecto
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Examen_parcial2Back.dll"]