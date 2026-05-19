using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClyvoCareOSChallenge.Models;

[Table("TB_PETS")]
public class Pet
{
    [Key]
    [Column("ID_PET")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("ESPECIE")]
    public string Especie { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("RACA")]
    public string Raca { get; set; } = string.Empty;

    [Column("DATA_NASCIMENTO")]
    public DateTime DataNascimento { get; set; }

    [MaxLength(10)]
    [Column("SEXO")]
    public string Sexo { get; set; } = string.Empty;

    [Column("PESO_KG")]
    public decimal PesoKg { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    [Column("ID_TUTOR")]
    public int TutorId { get; set; }

    [ForeignKey("TutorId")]
    public Tutor? Tutor { get; set; }

    [NotMapped]
    public string FaseDeVida => CalcularFaseDeVida();

    private string CalcularFaseDeVida()
    {
        var idadeAnos = (DateTime.UtcNow - DataNascimento).TotalDays / 365;
        return Especie.ToLower() switch
        {
            "cachorro" => idadeAnos < 1 ? "Filhote" : idadeAnos < 7 ? "Adulto" : "Idoso",
            "gato"     => idadeAnos < 1 ? "Filhote" : idadeAnos < 10 ? "Adulto" : "Idoso",
            _          => idadeAnos < 1 ? "Filhote" : idadeAnos < 8  ? "Adulto" : "Idoso",
        };
    }

    public ICollection<Vacina> Vacinas { get; set; } = new List<Vacina>();
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    public ICollection<Tratamento> Tratamentos { get; set; } = new List<Tratamento>();
}
