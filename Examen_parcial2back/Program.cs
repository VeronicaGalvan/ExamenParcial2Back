using CongresoTICsAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ParticipanteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConexionSQLite") ?? "Data Source=participantes.db"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = null;
});

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// EJECUTAR MIGRACIONES AL INICIAR
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ParticipanteDbContext>();
        context.Database.Migrate();
        Console.WriteLine("✅ Migraciones de base de datos aplicadas exitosamente");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error aplicando migraciones: {ex.Message}");
    }
}

// Swagger siempre disponible
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirTodo");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

// ENDPOINT RAÍZ
app.MapGet("/", () =>
{
    return new
    {
        message = "API Examen Parcial 2 - Backend is running!",
        status = "Active",
        timestamp = DateTime.UtcNow,
        endpoints = new
        {
            listar_participantes = "/api/listado",
            buscar_participantes = "/api/listado/buscar?q=texto",
            obtener_por_id = "/api/gafete/{id}",
            crear_participante = "POST /api/registro",
            swagger = "/swagger",
            health_check = "/health"
        }
    };
});

// HEALTH CHECK para verificar base de datos
app.MapGet("/health", async (IServiceProvider serviceProvider) =>
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ParticipanteDbContext>();
        var count = await context.Participantes.CountAsync();
        return Results.Ok(new
        {
            status = "Healthy",
            database = "Connected",
            participantes_count = count,
            timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Database connection failed",
            detail: ex.Message,
            statusCode: 500
        );
    }
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");