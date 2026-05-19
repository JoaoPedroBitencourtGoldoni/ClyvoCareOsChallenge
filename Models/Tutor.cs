using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClyvoCareOSChallenge.Models;

[Table("TB_TUTORES")]
public class Tutor
{
    [Key]
    [Column("ID_TUTOR")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("NOME_COMPLETO")]
    public string NomeCompleto { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("EMAIL")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("TELEFONE")]
    public string Telefone { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("ENDERECO")]
    public string Endereco { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("CIDADE")]
    public string Cidade { get; set; } = string.Empty;

    [MaxLength(2)]
    [Column("ESTADO")]
    public string Estado { get; set; } = string.Empty;

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
