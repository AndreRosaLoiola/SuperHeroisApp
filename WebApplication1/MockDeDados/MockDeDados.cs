using ViceriSeidorHero.Models;
using WebApplication1.Data;

namespace WebApplication1.MockDeDados
{
    public static class MockDeDados
    {
        public static void PopularBanco(ViceriSeidorHeroContext context)
        {
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
    }
}
