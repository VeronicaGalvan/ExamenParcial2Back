# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Buscar automáticamente el .csproj dentro del repo
# Esto evita errores si cambias el nombre de la carpeta
COPY Examen_parcial2back/**/*.csproj ./

# Restaurar dependencias usando el csproj que se copió
RUN for f in *.csproj; do dotnet restore "$f"; done

# Copiar todo el contenido del proyecto
COPY Examen_parcial2back/ ./

# Publicar la aplicación
RUN for f in *.csproj; do dotnet publish "$f" -c Release -o /app/out; done

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out .

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "Examen_parcial2back.dll"]
