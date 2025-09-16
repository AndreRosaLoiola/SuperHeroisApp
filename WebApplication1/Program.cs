using Microsoft.EntityFrameworkCore;
using ViceriSeidorHero.Models;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ViceriSeidorHeroContext>(options =>
    options.UseInMemoryDatabase("HeroisDb"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicione o CORS antes do Build
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ViceriSeidorHeroContext>();
    if (!context.Superpoderes.Any())
    {
        context.Superpoderes.AddRange(
            new Superpoderes { Superpoder = "Voar", Descricao = "Capacidade de voar pelos céus." },
            new Superpoderes { Superpoder = "Super Força", Descricao = "Força física sobre-humana." },
            new Superpoderes { Superpoder = "Invisibilidade", Descricao = "Ficar invisível aos olhos humanos." }
        );
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
