using CongresoTICsAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registrar el contexto de base de datos
builder.Services.AddDbContext<ParticipanteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConexionSQLite")));


// Agregar controladores
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //enviara los datos en forma de un array plano osea sin id ni values
    options.JsonSerializerOptions.ReferenceHandler = null;
});

// Habilitar CORS (para conectar con React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseCors("PermitirTodo");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
