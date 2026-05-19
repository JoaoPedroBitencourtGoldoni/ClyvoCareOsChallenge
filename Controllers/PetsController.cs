using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoCareOSChallenge.Data;
using ClyvoCareOSChallenge.DTOs;
using ClyvoCareOSChallenge.Models;

namespace ClyvoCareOSChallenge.Controllers;

[ApiController]
[Route("api/pets")]
[Produces("application/json")]
public class PetsController : ControllerBase
{
    private readonly AppDbContext _context;
    public PetsController(AppDbContext context) { _context = context; }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Pets.Include(p => p.Tutor)
            .Select(p => new PetResponseDto(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.Sexo, p.PesoKg, p.FaseDeVida, p.TutorId, p.Tutor!.NomeCompleto))
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _context.Pets.Include(x => x.Tutor).FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound(new { mensagem = $"Pet {id} não encontrado." });
        return Ok(new PetResponseDto(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.Sexo, p.PesoKg, p.FaseDeVida, p.TutorId, p.Tutor!.NomeCompleto));
    }

    [HttpGet("especie/{especie}")]
    public async Task<IActionResult> GetByEspecie(string especie) =>
        Ok(await _context.Pets.Include(p => p.Tutor).Where(p => p.Especie.ToLower() == especie.ToLower())
            .Select(p => new PetResponseDto(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.Sexo, p.PesoKg, p.FaseDeVida, p.TutorId, p.Tutor!.NomeCompleto))
            .ToListAsync());

    [HttpGet("fase/{fase}")]
    public async Task<IActionResult> GetByFase(string fase)
    {
        var todos = await _context.Pets.Include(p => p.Tutor).ToListAsync();
        return Ok(todos.Where(p => p.FaseDeVida.ToLower() == fase.ToLower())
            .Select(p => new PetResponseDto(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.Sexo, p.PesoKg, p.FaseDeVida, p.TutorId, p.Tutor!.NomeCompleto)));
    }

    [HttpGet("buscar/{nome}")]
    public async Task<IActionResult> GetByNome(string nome) =>
        Ok(await _context.Pets.Include(p => p.Tutor).Where(p => p.Nome.ToLower().Contains(nome.ToLower()))
            .Select(p => new PetResponseDto(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.Sexo, p.PesoKg, p.FaseDeVida, p.TutorId, p.Tutor!.NomeCompleto))
            .ToListAsync());

    [HttpGet("{id:int}/historico")]
    public async Task<IActionResult> GetHistorico(int id)
    {
        var pet = await _context.Pets.Include(p => p.Tutor).Include(p => p.Vacinas)
            .Include(p => p.Consultas).Include(p => p.Tratamentos).FirstOrDefaultAsync(p => p.Id == id);
        if (pet is null) return NotFound(new { mensagem = $"Pet {id} não encontrado." });
        return Ok(new {
            pet = new PetResponseDto(pet.Id, pet.Nome, pet.Especie, pet.Raca, pet.DataNascimento, pet.Sexo, pet.PesoKg, pet.FaseDeVida, pet.TutorId, pet.Tutor!.NomeCompleto),
            vacinas = pet.Vacinas.OrderByDescending(v => v.DataAplicacao),
            consultas = pet.Consultas.OrderByDescending(c => c.DataConsulta),
            tratamentos = pet.Tratamentos.OrderByDescending(t => t.DataInicio)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PetCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _context.Tutores.AnyAsync(t => t.Id == dto.TutorId))
            return BadRequest(new { mensagem = $"Tutor {dto.TutorId} não encontrado." });
        var pet = new Pet { Nome = dto.Nome, Especie = dto.Especie, Raca = dto.Raca, DataNascimento = dto.DataNascimento, Sexo = dto.Sexo, PesoKg = dto.PesoKg, TutorId = dto.TutorId };
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
        await _context.Entry(pet).Reference(p => p.Tutor).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = pet.Id },
            new PetResponseDto(pet.Id, pet.Nome, pet.Especie, pet.Raca, pet.DataNascimento, pet.Sexo, pet.PesoKg, pet.FaseDeVida, pet.TutorId, pet.Tutor!.NomeCompleto));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PetUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var pet = await _context.Pets.FindAsync(id);
        if (pet is null) return NotFound(new { mensagem = $"Pet {id} não encontrado." });
        pet.Nome = dto.Nome; pet.Especie = dto.Especie; pet.Raca = dto.Raca;
        pet.DataNascimento = dto.DataNascimento; pet.Sexo = dto.Sexo; pet.PesoKg = dto.PesoKg;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pet = await _context.Pets.FindAsync(id);
        if (pet is null) return NotFound(new { mensagem = $"Pet {id} não encontrado." });
        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
