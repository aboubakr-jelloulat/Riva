using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Riva.DTO;


public class VillaAmentiesCreateDTO
{
    [Required, MaxLength(100)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public int VillaId { get; set; }
}
