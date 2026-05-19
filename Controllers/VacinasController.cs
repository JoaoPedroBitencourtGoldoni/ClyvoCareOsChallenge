using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoCareOSChallenge.Data;
using ClyvoCareOSChallenge.DTOs;
using ClyvoCareOSChallenge.Models;

namespace ClyvoCareOSChallenge.Controllers;

[ApiController]
[Route("api/vacinas")]
[Produces("application/json")]
public class VacinasController : ControllerBase
{
    private readonly AppDbContext _context;
    public VacinasController(AppDbContext context) { _context = context; }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Vacinas.Include(v => v.Pet)
            .Select(v => new VacinaResponseDto(v.Id, v.NomeVacina, v.DataAplicacao, v.DataProximaDose, v.Veterinario, v.Clinica, v.Observacoes, v.PetId, v.Pet!.Nome))
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var v = await _context.Vacinas.Include(x => x.Pet).FirstOrDefaultAsync(x => x.Id == id);
        if (v is null) return NotFound(new { mensagem = $"Vacina {id} não encontrada." });
        return Ok(new VacinaResponseDto(v.Id, v.NomeVacina, v.DataAplicacao, v.DataProximaDose, v.Veterinario, v.Clinica, v.Observacoes, v.PetId, v.Pet!.Nome));
    }

    [HttpGet("pet/{petId:int}")]
    public async Task<IActionResult> GetByPet(int petId)
    {
        if (!await _context.Pets.AnyAsync(p => p.Id == petId))
            return NotFound(new { mensagem = $"Pet {petId} não encontrado." });
        return Ok(await _context.Vacinas.Include(v => v.Pet).Where(v => v.PetId == petId)
            .Select(v => new VacinaResponseDto(v.Id, v.NomeVacina, v.DataAplicacao, v.DataProximaDose, v.Veterinario, v.Clinica, v.Observacoes, v.PetId, v.Pet!.Nome))
            .ToListAsync());
    }

    [HttpGet("proximas/{dias:int}")]
    public async Task<IActionResult> GetProximas(int dias)
    {
        var limite = DateTime.UtcNow.AddDays(dias);
        return Ok(await _context.Vacinas.Include(v => v.Pet)
            .Where(v => v.DataProximaDose.HasValue && v.DataProximaDose <= limite && v.DataProximaDose >= DateTime.UtcNow)
            .Select(v => new VacinaResponseDto(v.Id, v.NomeVacina, v.DataAplicacao, v.DataProximaDose, v.Veterinario, v.Clinica, v.Observacoes, v.PetId, v.Pet!.Nome))
            .ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VacinaCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _context.Pets.AnyAsync(p => p.Id == dto.PetId))
            return BadRequest(new { mensagem = $"Pet {dto.PetId} não encontrado." });
        var vacina = new Vacina { NomeVacina = dto.NomeVacina, DataAplicacao = dto.DataAplicacao, DataProximaDose = dto.DataProximaDose, Veterinario = dto.Veterinario, Clinica = dto.Clinica, Observacoes = dto.Observacoes, PetId = dto.PetId };
        _context.Vacinas.Add(vacina);
        await _context.SaveChangesAsync();
        await _context.Entry(vacina).Reference(v => v.Pet).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = vacina.Id },
            new VacinaResponseDto(vacina.Id, vacina.NomeVacina, vacina.DataAplicacao, vacina.DataProximaDose, vacina.Veterinario, vacina.Clinica, vacina.Observacoes, vacina.PetId, vacina.Pet!.Nome));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] VacinaUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var vacina = await _context.Vacinas.FindAsync(id);
        if (vacina is null) return NotFound(new { mensagem = $"Vacina {id} não encontrada." });
        vacina.NomeVacina = dto.NomeVacina; vacina.DataAplicacao = dto.DataAplicacao; vacina.DataProximaDose = dto.DataProximaDose;
        vacina.Veterinario = dto.Veterinario; vacina.Clinica = dto.Clinica; vacina.Observacoes = dto.Observacoes;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vacina = await _context.Vacinas.FindAsync(id);
        if (vacina is null) return NotFound(new { mensagem = $"Vacina {id} não encontrada." });
        _context.Vacinas.Remove(vacina);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
