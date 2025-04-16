using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace volunteerMatch.Models;

public class Cause
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }

    public ICollection<Organization> Organizations { get; set; }
}
