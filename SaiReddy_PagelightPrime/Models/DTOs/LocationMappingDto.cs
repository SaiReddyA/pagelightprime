namespace PageLightPrime.API.Models.DTOs
{
    public class LocationMappingDto
    {
        public int MappingId { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;

        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;

        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = string.Empty;

        public string? Remarks { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
