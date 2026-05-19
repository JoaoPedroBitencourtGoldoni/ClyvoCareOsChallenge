using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoCareOSChallenge.Data;
using ClyvoCareOSChallenge.DTOs;
using ClyvoCareOSChallenge.Models;

namespace ClyvoCareOSChallenge.Controllers;

[ApiController]
[Route("api/consultas")]
[Produces("application/json")]
public class ConsultasController : ControllerBase
{
    private readonly AppDbContext _context;
    public ConsultasController(AppDbContext context) { _context = context; }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Consultas.Include(c => c.Pet).Select(c => MapToDto(c)).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var c = await _context.Consultas.Include(x => x.Pet).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound(new { mensagem = $"Consulta {id} não encontrada." });
        return Ok(MapToDto(c));
    }

    [HttpGet("pet/{petId:int}")]
    public async Task<IActionResult> GetByPet(int petId)
    {
        if (!await _context.Pets.AnyAsync(p => p.Id == petId))
            return NotFound(new { mensagem = $"Pet {petId} não encontrado." });
        return Ok(await _context.Consultas.Include(c => c.Pet).Where(c => c.PetId == petId)
            .OrderByDescending(c => c.DataConsulta).Select(c => MapToDto(c)).ToListAsync());
    }

    [HttpGet("clinica/{clinica}")]
    public async Task<IActionResult> GetByClinica(string clinica) =>
        Ok(await _context.Consultas.Include(c => c.Pet).Where(c => c.Clinica.ToLower().Contains(clinica.ToLower()))
            .Select(c => MapToDto(c)).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ConsultaCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _context.Pets.AnyAsync(p => p.Id == dto.PetId))
            return BadRequest(new { mensagem = $"Pet {dto.PetId} não encontrado." });
        var consulta = new Consulta { DataConsulta = dto.DataConsulta, Motivo = dto.Motivo, Diagnostico = dto.Diagnostico, Prescricao = dto.Prescricao, Veterinario = dto.Veterinario, Clinica = dto.Clinica, PesoNaConsulta = dto.PesoNaConsulta, PetId = dto.PetId };
        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();
        await _context.Entry(consulta).Reference(c => c.Pet).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, MapToDto(consulta));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ConsultaUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta is null) return NotFound(new { mensagem = $"Consulta {id} não encontrada." });
        consulta.DataConsulta = dto.DataConsulta; consulta.Motivo = dto.Motivo; consulta.Diagnostico = dto.Diagnostico;
        consulta.Prescricao = dto.Prescricao; consulta.Veterinario = dto.Veterinario; consulta.Clinica = dto.Clinica; consulta.PesoNaConsulta = dto.PesoNaConsulta;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta is null) return NotFound(new { mensagem = $"Consulta {id} não encontrada." });
        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static ConsultaResponseDto MapToDto(Consulta c) =>
        new(c.Id, c.DataConsulta, c.Motivo, c.Diagnostico, c.Prescricao, c.Veterinario, c.Clinica, c.PesoNaConsulta, c.PetId, c.Pet?.Nome ?? "");
}
