using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
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
  public decimal Preco { get; set; }
  [Required]
  [MaxLength(300)]
  public string? ImagemUrl { get; set; }

  public int CategoriaId { get; set; }
}