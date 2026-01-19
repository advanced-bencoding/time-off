using System.ComponentModel.DataAnnotations;

namespace Auth.Api.DTOs;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string EmailId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
};
