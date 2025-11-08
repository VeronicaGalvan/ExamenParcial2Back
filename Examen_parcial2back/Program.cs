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

// ENDPOINT RAÍZ con las rutas correctas
app.MapGet("/", () => new {
    message = "API Examen Parcial 2 - Backend is running!",
    status = "Active",
    timestamp = DateTime.UtcNow,
    endpoints = new
    {
        listar_participantes = "/api/listado",
        buscar_participantes = "/api/listado/buscar?q=texto",
        obtener_por_id = "/api/gafete/{id}",
        crear_participante = "POST /api/registro",
        swagger = "/swagger"
    }
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");