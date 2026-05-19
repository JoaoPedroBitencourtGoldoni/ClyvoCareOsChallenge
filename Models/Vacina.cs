using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClyvoCareOSChallenge.Models;

[Table("TB_VACINAS")]
public class Vacina
{
    [Key]
    [Column("ID_VACINA")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("NOME_VACINA")]
    public string NomeVacina { get; set; } = string.Empty;

    [Column("DATA_APLICACAO")]
    public DateTime DataAplicacao { get; set; }

    [Column("DATA_PROXIMA_DOSE")]
    public DateTime? DataProximaDose { get; set; }

    [MaxLength(100)]
    [Column("VETERINARIO")]
    public string Veterinario { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("CLINICA")]
    public string Clinica { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("OBSERVACOES")]
    public string Observacoes { get; set; } = string.Empty;

    [Column("ID_PET")]
    public int PetId { get; set; }

    [ForeignKey("PetId")]
    public Pet? Pet { get; set; }
}
