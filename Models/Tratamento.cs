using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClyvoCareOSChallenge.Models;

[Table("TB_TRATAMENTOS")]
public class Tratamento
{
    [Key]
    [Column("ID_TRATAMENTO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("NOME_TRATAMENTO")]
    public string NomeTratamento { get; set; } = string.Empty;

    [Column("DATA_INICIO")]
    public DateTime DataInicio { get; set; }

    [Column("DATA_FIM")]
    public DateTime? DataFim { get; set; }

    [MaxLength(500)]
    [Column("DESCRICAO")]
    public string Descricao { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column("STATUS")]
    public string Status { get; set; } = "Em andamento";

    [MaxLength(100)]
    [Column("VETERINARIO")]
    public string Veterinario { get; set; } = string.Empty;

    [Column("ID_PET")]
    public int PetId { get; set; }

    [ForeignKey("PetId")]
    public Pet? Pet { get; set; }
}
