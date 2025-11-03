namespace PageLightPrime.API.Models.Entities;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LocationMapping> LocationMappings { get; set; } = new List<LocationMapping>();

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
