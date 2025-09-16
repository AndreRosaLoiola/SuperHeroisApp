using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViceriSeidorHero.Models;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Mapping;
using WebApplication1.Mapping.WebApplication1.Mapping;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HeroisController : ControllerBase
{
    private readonly IHeroiRepository _heroiRepository;

    public HeroisController(IHeroiRepository heroiRepository)
    {
        _heroiRepository = heroiRepository;
    }

    [HttpGet("superpoderes")]
    public async Task<IActionResult> GetSuperpoderes()
    {
        var poderes = await _heroiRepository.GetSuperpoderesAsync();
        if (poderes == null || !poderes.Any())
            return NotFound("Nenhum superpoder cadastrado.");
        return Ok(poderes);
    }

    [HttpGet]
    public async Task<IActionResult> GetHerois()
    {
        var herois = await _heroiRepository.GetHeroisAsync();
        if (herois == null || !herois.Any())
            return NotFound("Nenhum super-herói cadastrado.");
        return Ok(herois);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHeroiById(int id)
    {
        var heroi = await _heroiRepository.GetHeroiByIdAsync(id);
        if (heroi == null)
            return NotFound("Super-herói não encontrado.");
        return Ok(heroi);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHeroi([FromBody] HeroiCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _heroiRepository.CreateHeroiAsync(dto);
        if (result == null)
            return Conflict("Já existe um herói com esse nome de herói ou superpoder não encontrado.");
        return CreatedAtAction(nameof(GetHeroiById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHeroi(int id, [FromBody] HeroiUpdateDto dto)
    {
        var result = await _heroiRepository.UpdateHeroiAsync(id, dto);
        if (result == null)
            return Conflict("Super-herói não encontrado, nome de herói já está em uso ou superpoder não encontrado.");
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHeroi(int id)
    {
        var deleted = await _heroiRepository.DeleteHeroiAsync(id);
        if (!deleted)
            return NotFound("Super-herói não encontrado.");
        return Ok("Super-herói excluído com sucesso.");
    }
}
