using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoCareOSChallenge.Data;
using ClyvoCareOSChallenge.DTOs;
using ClyvoCareOSChallenge.Models;

namespace ClyvoCareOSChallenge.Controllers;

[ApiController]
[Route("api/tratamentos")]
[Produces("application/json")]
public class TratamentosController : ControllerBase
{
    private readonly AppDbContext _context;
    public TratamentosController(AppDbContext context) { _context = context; }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Tratamentos.Include(t => t.Pet).Select(t => MapToDto(t)).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var t = await _context.Tratamentos.Include(x => x.Pet).FirstOrDefaultAsync(x => x.Id == id);
        if (t is null) return NotFound(new { mensagem = $"Tratamento {id} não encontrado." });
        return Ok(MapToDto(t));
    }

    [HttpGet("pet/{petId:int}")]
    public async Task<IActionResult> GetByPet(int petId)
    {
        if (!await _context.Pets.AnyAsync(p => p.Id == petId))
            return NotFound(new { mensagem = $"Pet {petId} não encontrado." });
        return Ok(await _context.Tratamentos.Include(t => t.Pet).Where(t => t.PetId == petId)
            .OrderByDescending(t => t.DataInicio).Select(t => MapToDto(t)).ToListAsync());
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status) =>
        Ok(await _context.Tratamentos.Include(t => t.Pet).Where(t => t.Status.ToLower() == status.ToLower())
            .Select(t => MapToDto(t)).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TratamentoCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _context.Pets.AnyAsync(p => p.Id == dto.PetId))
            return BadRequest(new { mensagem = $"Pet {dto.PetId} não encontrado." });
        var tratamento = new Tratamento { NomeTratamento = dto.NomeTratamento, DataInicio = dto.DataInicio, DataFim = dto.DataFim, Descricao = dto.Descricao, Status = dto.Status, Veterinario = dto.Veterinario, PetId = dto.PetId };
        _context.Tratamentos.Add(tratamento);
        await _context.SaveChangesAsync();
        await _context.Entry(tratamento).Reference(t => t.Pet).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = tratamento.Id }, MapToDto(tratamento));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TratamentoUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var tratamento = await _context.Tratamentos.FindAsync(id);
        if (tratamento is null) return NotFound(new { mensagem = $"Tratamento {id} não encontrado." });
        tratamento.NomeTratamento = dto.NomeTratamento; tratamento.DataInicio = dto.DataInicio; tratamento.DataFim = dto.DataFim;
        tratamento.Descricao = dto.Descricao; tratamento.Status = dto.Status; tratamento.Veterinario = dto.Veterinario;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tratamento = await _context.Tratamentos.FindAsync(id);
        if (tratamento is null) return NotFound(new { mensagem = $"Tratamento {id} não encontrado." });
        _context.Tratamentos.Remove(tratamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static TratamentoResponseDto MapToDto(Tratamento t) =>
        new(t.Id, t.NomeTratamento, t.DataInicio, t.DataFim, t.Descricao, t.Status, t.Veterinario, t.PetId, t.Pet?.Nome ?? "");
}
