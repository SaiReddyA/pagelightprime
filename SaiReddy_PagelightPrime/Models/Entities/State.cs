namespace PageLightPrime.API.Models.Entities;

public partial class State
{
    public int StateId { get; set; }

    public string StateName { get; set; } = null!;

    public int CountryId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<LocationMapping> LocationMappings { get; set; } = new List<LocationMapping>();
}
