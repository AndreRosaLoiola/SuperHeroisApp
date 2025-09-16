using ViceriSeidorHero.Models;
using WebApplication1.Dtos;

namespace WebApplication1.Mapping
{
    namespace WebApplication1.Mapping
    {
        public static class HeroiMapper
        {
            public static HeroiResponseDto MapToHeroiResponseDto(Heroi heroi)
            {
                return new HeroiResponseDto
                {
                    Id = heroi.Id,
                    Nome = heroi.Nome,
                    NomeHeroi = heroi.NomeHeroi,
                    DataNascimento = heroi.DataNascimento,
                    Altura = heroi.Altura,
                    Peso = heroi.Peso,
                    Superpoderes = heroi.HeroisSuperpoderes?
                        .Select(hs => new SuperpoderResponseDto
                        {
                            Id = hs.Superpoder.Id,
                            Superpoder = hs.Superpoder.Superpoder,
                            Descricao = hs.Superpoder.Descricao
                        }).ToList() ?? new List<SuperpoderResponseDto>()
                };
            }
        }
    }


}
