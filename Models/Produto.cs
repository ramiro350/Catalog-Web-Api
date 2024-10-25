using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using APICatalogo.Validations;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
  [Key]
  public int ProdutoId { get; set; }
  [Required(ErrorMessage = "O nome é obrigatório.")]
  [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres.", 
  MinimumLength = 5)]
  // [PrimeiraLetraMaiscula]
  public string? Nome { get; set; }
  [Required]
  [MaxLength(300)]
  public string? Descricao { get; set; }
  [Required]
  [Column(TypeName = "decimal(10,2)")]
  public decimal Preco { get; set; }
  [Required]
  [MaxLength(300)]
  public string? ImagemUrl { get; set; }

  public float Estoque { get; set; }

  public DateTime DataCadastro { get; set; }

  public int CategoriaId { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if(!string.IsNullOrEmpty(this.Nome))
    {
      var primeiraLetra = this.Nome[0].ToString();
      if(primeiraLetra != primeiraLetra.ToUpper())
      {
          yield return new ValidationResult("A primeira letra deve ser maiúscula",
          new []
          { nameof(this.Nome) }
          );
      }
    }
    if(this.Estoque <= 0)
    {
      yield return new ValidationResult("O estoque deve ser maior que zero",
      new[]
      { nameof(this.Estoque) }
      );
    }
  }

  [JsonIgnore]
  public Categoria? Categoria { get; set; }
}