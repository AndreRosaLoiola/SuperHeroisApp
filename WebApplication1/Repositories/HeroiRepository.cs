using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Mapping;
using ViceriSeidorHero.Models;
using WebApplication1.Mapping.WebApplication1.Mapping;

namespace WebApplication1.Repositories
{
    public class HeroiRepository : IHeroiRepository
    {
        private readonly ViceriSeidorHeroContext _context;
        public HeroiRepository(ViceriSeidorHeroContext context)
        {
            _context = context;
        }

        public async Task<List<SuperpoderResponseDto>> GetSuperpoderesAsync()
        {
            var poderes = await _context.Superpoderes
                .Select(s => new SuperpoderResponseDto
                {
                    Id = s.Id,
                    Superpoder = s.Superpoder,
                    Descricao = s.Descricao
                })
                .ToListAsync();
            return poderes;
        }

        public async Task<List<HeroiResponseDto>> GetHeroisAsync()
        {
            var herois = await _context.Herois
                .Include(h => h.HeroisSuperpoderes)
                .ThenInclude(hs => hs.Superpoder)
                .ToListAsync();
            return herois.Select(HeroiMapper.MapToHeroiResponseDto).ToList();
        }

        public async Task<HeroiResponseDto?> GetHeroiByIdAsync(int id)
        {
            var heroi = await _context.Herois
                .Include(h => h.HeroisSuperpoderes)
                .ThenInclude(hs => hs.Superpoder)
                .FirstOrDefaultAsync(h => h.Id == id);
            return heroi == null ? null : HeroiMapper.MapToHeroiResponseDto(heroi);
        }

        public async Task<HeroiResponseDto?> CreateHeroiAsync(HeroiCreateDto dto)
        {
            if (await _context.Herois.AnyAsync(h => h.NomeHeroi == dto.NomeHeroi))
                return null;

            var heroi = new Heroi
            {
                Nome = dto.Nome,
                NomeHeroi = dto.NomeHeroi,
                DataNascimento = dto.DataNascimento,
                Altura = dto.Altura,
                Peso = dto.Peso
            };

            foreach (var superpoderId in dto.SuperpoderesIds)
            {
                var superpoder = await _context.Superpoderes.FindAsync(superpoderId);
                if (superpoder == null)
                    return null;
                heroi.HeroisSuperpoderes.Add(new HeroiSuperpoder { SuperpoderId = superpoderId });
            }

            _context.Herois.Add(heroi);
            await _context.SaveChangesAsync();

            await _context.Entry(heroi)
                .Collection(h => h.HeroisSuperpoderes)
                .Query()
                .Include(hs => hs.Superpoder)
                .LoadAsync();

            return HeroiMapper.MapToHeroiResponseDto(heroi);
        }

        public async Task<HeroiResponseDto?> UpdateHeroiAsync(int id, HeroiUpdateDto dto)
        {
            var heroi = await _context.Herois
                .Include(h => h.HeroisSuperpoderes)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (heroi == null)
                return null;

            if (await _context.Herois.AnyAsync(h => h.Id != id && EF.Functions.Like(h.NomeHeroi.Trim().ToLower(), dto.NomeHeroi.Trim().ToLower())))
                return null;

            heroi.Nome = dto.Nome;
            heroi.NomeHeroi = dto.NomeHeroi;
            heroi.DataNascimento = dto.DataNascimento;
            heroi.Altura = dto.Altura;
            heroi.Peso = dto.Peso;

            heroi.HeroisSuperpoderes.Clear();
            foreach (var superpoderId in dto.SuperpoderesIds)
            {
                var superpoder = await _context.Superpoderes.FindAsync(superpoderId);
                if (superpoder == null)
                    return null;
                heroi.HeroisSuperpoderes.Add(new HeroiSuperpoder { HeroiId = id, SuperpoderId = superpoderId });
            }

            await _context.SaveChangesAsync();

            await _context.Entry(heroi)
                .Collection(h => h.HeroisSuperpoderes)
                .Query()
                .Include(hs => hs.Superpoder)
                .LoadAsync();

            return HeroiMapper.MapToHeroiResponseDto(heroi);
        }

        public async Task<bool> DeleteHeroiAsync(int id)
        {
            var heroi = await _context.Herois.FindAsync(id);
            if (heroi == null)
                return false;
            _context.Herois.Remove(heroi);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
