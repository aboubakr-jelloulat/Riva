using System;
using System.Collections.Generic;
using System.Text;

namespace Riva.DTO;

public class VillaDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
}
