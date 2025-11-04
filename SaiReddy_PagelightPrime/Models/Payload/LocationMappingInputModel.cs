namespace PageLightPrime.API.Models.Payload
{
    public class LocationMappingInputModel
    {
        public int MappingId { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "District is required")]
        public int DistrictId { get; set; }

        [StringLength(200, ErrorMessage = "Remarks cannot exceed 200 characters")]
        public string? Remarks { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; } = "Admin";
    }
}
