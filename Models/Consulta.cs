using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClyvoCareOSChallenge.Models;

[Table("TB_CONSULTAS")]
public class Consulta
{
    [Key]
    [Column("ID_CONSULTA")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("DATA_CONSULTA")]
    public DateTime DataConsulta { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("MOTIVO")]
    public string Motivo { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("DIAGNOSTICO")]
    public string Diagnostico { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("PRESCRICAO")]
    public string Prescricao { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("VETERINARIO")]
    public string Veterinario { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("CLINICA")]
    public string Clinica { get; set; } = string.Empty;

    [Column("PESO_NA_CONSULTA")]
    public decimal PesoNaConsulta { get; set; }

    [Column("ID_PET")]
    public int PetId { get; set; }

    [ForeignKey("PetId")]
    public Pet? Pet { get; set; }
}
