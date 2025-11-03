namespace PageLightPrime.API.Models.Entities;

public partial class District
{
    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public int StateId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LocationMapping> LocationMappings { get; set; } = new List<LocationMapping>();

    public virtual State State { get; set; } = null!;
}
