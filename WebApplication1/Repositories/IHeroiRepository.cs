using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Dtos;
using ViceriSeidorHero.Models;

namespace WebApplication1.Repositories
{
    public interface IHeroiRepository
    {
        Task<List<SuperpoderResponseDto>> GetSuperpoderesAsync();
        Task<List<HeroiResponseDto>> GetHeroisAsync();
        Task<HeroiResponseDto?> GetHeroiByIdAsync(int id);
        Task<HeroiResponseDto?> CreateHeroiAsync(HeroiCreateDto dto);
        Task<HeroiResponseDto?> UpdateHeroiAsync(int id, HeroiUpdateDto dto);
        Task<bool> DeleteHeroiAsync(int id);
    }
}
