using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Riva.DTO;

public class RegisterationRequestDTO
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    public required string Password { get; set; }

    [MaxLength(50)]
    public string Role { get; set; } = "Customer";

}
