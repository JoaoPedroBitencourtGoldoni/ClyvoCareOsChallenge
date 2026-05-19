using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoCareOSChallenge.Data;
using ClyvoCareOSChallenge.DTOs;
using ClyvoCareOSChallenge.Models;

namespace ClyvoCareOSChallenge.Controllers;

[ApiController]
[Route("api/tutores")]
[Produces("application/json")]
public class TutoresController : ControllerBase
{
    private readonly AppDbContext _context;
    public TutoresController(AppDbContext context) { _context = context; }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Tutores.Select(t => MapToDto(t)).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);
        if (tutor is null) return NotFound(new { mensagem = $"Tutor {id} não encontrado." });
        return Ok(MapToDto(tutor));
    }

    [HttpGet("cidade/{cidade}")]
    public async Task<IActionResult> GetByCidade(string cidade) =>
        Ok(await _context.Tutores.Where(t => t.Cidade.ToLower() == cidade.ToLower()).Select(t => MapToDto(t)).ToListAsync());

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var tutor = await _context.Tutores.FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
        if (tutor is null) return NotFound(new { mensagem = "Tutor não encontrado." });
        return Ok(MapToDto(tutor));
    }

    [HttpGet("{id:int}/pets")]
    public async Task<IActionResult> GetPetsByTutor(int id)
    {
        if (!await _context.Tutores.AnyAsync(t => t.Id == id))
            return NotFound(new { mensagem = $"Tutor {id} não encontrado." });
        var pets = await _context.Pets.Include(p => p.Tutor).Where(p => p.TutorId == id)
            .Select(p => new PetResponseDto(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.Sexo, p.PesoKg, p.FaseDeVida, p.TutorId, p.Tutor!.NomeCompleto))
            .ToListAsync();
        return Ok(pets);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TutorCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (await _context.Tutores.AnyAsync(t => t.Email == dto.Email))
            return BadRequest(new { mensagem = "E-mail já cadastrado." });
        var tutor = new Tutor { NomeCompleto = dto.NomeCompleto, Email = dto.Email, Telefone = dto.Telefone, Endereco = dto.Endereco, Cidade = dto.Cidade, Estado = dto.Estado };
        _context.Tutores.Add(tutor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, MapToDto(tutor));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TutorUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var tutor = await _context.Tutores.FindAsync(id);
        if (tutor is null) return NotFound(new { mensagem = $"Tutor {id} não encontrado." });
        tutor.NomeCompleto = dto.NomeCompleto; tutor.Email = dto.Email; tutor.Telefone = dto.Telefone;
        tutor.Endereco = dto.Endereco; tutor.Cidade = dto.Cidade; tutor.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);
        if (tutor is null) return NotFound(new { mensagem = $"Tutor {id} não encontrado." });
        _context.Tutores.Remove(tutor);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static TutorResponseDto MapToDto(Tutor t) =>
        new(t.Id, t.NomeCompleto, t.Email, t.Telefone, t.Cidade, t.Estado, t.DataCadastro);
}
