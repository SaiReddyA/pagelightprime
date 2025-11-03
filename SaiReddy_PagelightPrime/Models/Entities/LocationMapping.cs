namespace PageLightPrime.API.Models.Entities;

public partial class LocationMapping
{
    public int MappingId { get; set; }

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual District District { get; set; } = null!;

    public virtual State State { get; set; } = null!;
}
