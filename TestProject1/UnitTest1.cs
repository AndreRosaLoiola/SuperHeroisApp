using Microsoft.EntityFrameworkCore;
using ViceriSeidorHero.Models;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Repositories;

namespace TestProject1
{
    public class UnitTest1
    {
        private ViceriSeidorHeroContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ViceriSeidorHeroContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ViceriSeidorHeroContext(options);

            // Seed Superpoderes
            context.Superpoderes.AddRange(
                new Superpoderes { Id = 1, Superpoder = "Voar", Descricao = "Voar" },
                new Superpoderes { Id = 2, Superpoder = "Força", Descricao = "Força" }
            );
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task CreateHeroiAsync_DeveCriarHeroiComSucesso()
        {
            var context = GetContext();
            var repo = new HeroiRepository(context);

            var dto = new HeroiCreateDto
            {
                Nome = "Clark Kent",
                NomeHeroi = "Superman",
                DataNascimento = new DateTime(1938, 6, 1),
                Altura = 1.90,
                Peso = 90,
                SuperpoderesIds = new List<int> { 1 }
            };

            var result = await repo.CreateHeroiAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Superman", result.NomeHeroi);
            Assert.Single(result.Superpoderes);
        }

        [Fact]
        public async Task CreateHeroiAsync_NomeHeroiDuplicado_DeveRetornarNull()
        {
            var context = GetContext();
            var repo = new HeroiRepository(context);


            await repo.CreateHeroiAsync(new HeroiCreateDto
            {
                Nome = "Bruce Wayne",
                NomeHeroi = "Batman",
                DataNascimento = new DateTime(1939, 5, 1),
                Altura = 1.88,
                Peso = 95,
                SuperpoderesIds = new List<int> { 2 }
            });

        
            var result = await repo.CreateHeroiAsync(new HeroiCreateDto
            {
                Nome = "Outro",
                NomeHeroi = "Batman",
                DataNascimento = new DateTime(2000, 1, 1),
                Altura = 1.80,
                Peso = 80,
                SuperpoderesIds = new List<int> { 1 }
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task GetHeroisAsync_DeveRetornarLista()
        {
            var context = GetContext();
            var repo = new HeroiRepository(context);

            await repo.CreateHeroiAsync(new HeroiCreateDto
            {
                Nome = "Diana",
                NomeHeroi = "Mulher Maravilha",
                DataNascimento = new DateTime(1941, 1, 1),
                Altura = 1.78,
                Peso = 75,
                SuperpoderesIds = new List<int> { 1, 2 }
            });

            var herois = await repo.GetHeroisAsync();

            Assert.NotNull(herois);
            Assert.Single(herois);
        }

        [Fact]
        public async Task UpdateHeroiAsync_NomeHeroiDuplicado_DeveRetornarNull()
        {
            var context = GetContext();
            var repo = new HeroiRepository(context);

            var h1 = await repo.CreateHeroiAsync(new HeroiCreateDto
            {
                Nome = "Barry Allen",
                NomeHeroi = "Flash",
                DataNascimento = new DateTime(1956, 1, 1),
                Altura = 1.80,
                Peso = 80,
                SuperpoderesIds = new List<int> { 1 }
            });

            var h2 = await repo.CreateHeroiAsync(new HeroiCreateDto
            {
                Nome = "Hal Jordan",
                NomeHeroi = "Lanterna Verde",
                DataNascimento = new DateTime(1959, 1, 1),
                Altura = 1.85,
                Peso = 85,
                SuperpoderesIds = new List<int> { 2 }
            });

            // Tenta atualizar o segundo para ter o mesmo NomeHeroi do primeiro
            var result = await repo.UpdateHeroiAsync(h2.Id, new HeroiUpdateDto
            {
                Nome = "Hal Jordan",
                NomeHeroi = "Flash",
                DataNascimento = h2.DataNascimento,
                Altura = h2.Altura,
                Peso = h2.Peso,
                SuperpoderesIds = new List<int> { 2 }
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteHeroiAsync_DeveRemoverHeroi()
        {
            var context = GetContext();
            var repo = new HeroiRepository(context);

            var h = await repo.CreateHeroiAsync(new HeroiCreateDto
            {
                Nome = "Arthur Curry",
                NomeHeroi = "Aquaman",
                DataNascimento = new DateTime(1941, 11, 1),
                Altura = 1.85,
                Peso = 90,
                SuperpoderesIds = new List<int> { 1 }
            });

            var deleted = await repo.DeleteHeroiAsync(h.Id);

            Assert.True(deleted);
            var heroi = await repo.GetHeroiByIdAsync(h.Id);
            Assert.Null(heroi);
        }
    }
}
