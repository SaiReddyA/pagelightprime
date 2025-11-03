namespace PageLightPrime.API.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync();
        Task<IEnumerable<StateDto>> GetStatesByCountryAsync(int countryId);
        Task<IEnumerable<DistrictDto>> GetDistrictsByStateAsync(int stateId);
        Task<IEnumerable<LocationMappingDto>> GetAllMappingsAsync();
        Task<bool> SaveOrUpdateMappingAsync(LocationMappingInputModel mapping);
        Task<bool> DeleteMappingAsync(int mappingId);
    }
}
