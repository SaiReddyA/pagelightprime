using PageLightPrime.API.Models.Entities;

namespace PageLightPrime.API.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync();
        Task<IEnumerable<StateDto>> GetStatesByCountryAsync(int countryId);
        Task<IEnumerable<DistrictDto>> GetDistrictsByStateAsync(int stateId);
        Task<IEnumerable<LocationMappingDto>> GetAllMappingsAsync();
        Task SaveOrUpdateMappingAsync(LocationMappingInputModel mapping);
        Task DeleteMappingAsync(int mappingId);
    }
}
