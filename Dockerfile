# Usar la imagen de SDK de .NET 9.0 para construir
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el proyecto y restaurar dependencias
COPY . .
RUN dotnet restore "Examen_parcial2back/Examen_parcial2back.csproj"

# Publicar la aplicación
RUN dotnet publish "Examen_parcial2back/Examen_parcial2back.csproj" -c Release -o /app/publish

# Usar la imagen de runtime de .NET 9.0 para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto y ejecutar
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENTRYPOINT ["dotnet", "Examen_parcial2back.dll"]