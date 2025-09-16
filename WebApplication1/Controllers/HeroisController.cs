using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViceriSeidorHero.Models;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Mapping;
using WebApplication1.Mapping.WebApplication1.Mapping;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroisController : ControllerBase
    {
        private readonly ViceriSeidorHeroContext _context;

        public HeroisController(ViceriSeidorHeroContext context)
        {
            _context = context;
        }

        [HttpGet("superpoderes")]
        public async Task<IActionResult> GetSuperpoderes()
        {
            var poderes = await _context.Superpoderes
                .Select(s => new {
                    s.Id,
                    s.Superpoder,
                    s.Descricao
                })
                .ToListAsync();

            if (!poderes.Any())
                return NotFound("Nenhum superpoder cadastrado.");

            return Ok(poderes);
        }

        [HttpGet]
        public async Task<IActionResult> GetHerois()
        {
            var herois = await _context.Herois
                .Include(h => h.HeroisSuperpoderes)
                .ThenInclude(hs => hs.Superpoder)
                .ToListAsync();

            if (!herois.Any())
                return NotFound("Nenhum super-herói cadastrado.");

            var result = herois.Select(HeroiMapper.MapToHeroiResponseDto).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHeroiById(int id)
        {
            var heroi = await _context.Herois
                .Include(h => h.HeroisSuperpoderes)
                .ThenInclude(hs => hs.Superpoder)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (heroi == null)
                return NotFound("Super-herói não encontrado.");

            return Ok(HeroiMapper.MapToHeroiResponseDto(heroi));
        }

        [HttpPost]
        public async Task<IActionResult> CreateHeroi([FromBody] HeroiCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Herois.AnyAsync(h => h.NomeHeroi == dto.NomeHeroi))
                return Conflict("Já existe um herói com esse nome de herói.");

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
                    return NotFound($"Superpoder com Id {superpoderId} não encontrado.");

                heroi.HeroisSuperpoderes.Add(new HeroiSuperpoder { SuperpoderId = superpoderId });
            }

            _context.Herois.Add(heroi);
            await _context.SaveChangesAsync();

            await _context.Entry(heroi)
                .Collection(h => h.HeroisSuperpoderes)
                .Query()
                .Include(hs => hs.Superpoder)
                .LoadAsync();

            return CreatedAtAction(nameof(GetHeroiById), new { id = heroi.Id }, HeroiMapper.MapToHeroiResponseDto(heroi));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHeroi(int id, [FromBody] HeroiUpdateDto dto)
        {
            var heroi = await _context.Herois
                .Include(h => h.HeroisSuperpoderes)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (heroi == null)
                return NotFound("Super-herói não encontrado.");

            if (await _context.Herois
     .AnyAsync(h => h.Id != id &&
         EF.Functions.Like(h.NomeHeroi.Trim().ToLower(), dto.NomeHeroi.Trim().ToLower())))
            {
                return Conflict("Nome de herói já está em uso por outro super-herói.");
            }

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
                    return NotFound($"Superpoder com Id {superpoderId} não encontrado.");

                heroi.HeroisSuperpoderes.Add(new HeroiSuperpoder { HeroiId = id, SuperpoderId = superpoderId });
            }

            await _context.SaveChangesAsync();

            await _context.Entry(heroi)
                .Collection(h => h.HeroisSuperpoderes)
                .Query()
                .Include(hs => hs.Superpoder)
                .LoadAsync();

            return Ok(HeroiMapper.MapToHeroiResponseDto(heroi));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeroi(int id)
        {
            var heroi = await _context.Herois.FindAsync(id);
            if (heroi == null)
                return NotFound("Super-herói não encontrado.");

            _context.Herois.Remove(heroi);
            await _context.SaveChangesAsync();

            return Ok("Super-herói excluído com sucesso.");
        }
    }
}
