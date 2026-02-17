using System.ComponentModel.DataAnnotations;

namespace Riva.API.Models;

public class Villa
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Details { get; set; }
    public double Rate { get; set; }    // the price of the villa per night/week
    public int Sqft { get; set; }       // Square feet of the villa  => the size of the villa
    public int Occupancy { get; set; }  // How many people can stay in the villa.
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public ICollection<VillaAmenities>? Amenities { get; set; }

}

