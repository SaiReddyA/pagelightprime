namespace PageLightPrime.API.Models.ViewModel
{
    public class LocationViewModel
    {
        public List<CountryDto> Countries { get; set; } = new();
        public List<StateDto> States { get; set; } = new();
        public List<DistrictDto> Districts { get; set; } = new();

        public List<LocationMappingDto> LocationMappings { get; set; } = new();
        public LocationMappingInputModel locationMappingInputModel { get; set; } = new();

    }
}
