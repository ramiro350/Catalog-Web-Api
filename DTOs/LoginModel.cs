using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class LoginModel
{
  [Required(ErrorMessage = "Username is required")]
  public string? UserName { get; set; }
  [Required(ErrorMessage = "Passoword is required")]
  public string? Password { get; set; }
}