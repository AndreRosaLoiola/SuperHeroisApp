using WebApplication1.IoC;
using WebApplication1.Repositories;
using WebApplication1.MockDeDados;
using Microsoft.EntityFrameworkCore;
using ViceriSeidorHero.Models;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);
// Registrar dependências do projeto
builder.Services.AddProjectDependencies();
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
    MockDeDados.PopularBanco(context);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
