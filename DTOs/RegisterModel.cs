using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class RegisterModel
{
  [Required(ErrorMessage = "Username is required")]
  public string? UserName { get; set; }
  [EmailAddress]
  [Required(ErrorMessage = "Email is required")]
  public string? Email { get; set; }
  [Required(ErrorMessage = "Passoword is required")]
  public string? Password { get; set; }
}