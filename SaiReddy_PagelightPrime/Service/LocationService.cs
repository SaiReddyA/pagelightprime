using PageLightPrime.API.Models.Entities;

namespace PageLightPrime.API.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly ILogger<LocationService> _logger;

        public LocationService(ILocationRepository repository, ILogger<LocationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync() =>
            await _repository.GetCountriesAsync();

        public async Task<IEnumerable<StateDto>> GetStatesByCountryAsync(int countryId) =>
            await _repository.GetStatesByCountryAsync(countryId);

        public async Task<IEnumerable<DistrictDto>> GetDistrictsByStateAsync(int stateId) =>
            await _repository.GetDistrictsByStateAsync(stateId);

        public async Task<IEnumerable<LocationMappingDto>> GetAllMappingsAsync()
        {
            return await _repository.GetAllMappingsAsync();
        }

        public async Task<bool> SaveOrUpdateMappingAsync(LocationMappingInputModel mapping)
        {
            try
            {
                await _repository.SaveOrUpdateMappingAsync(mapping);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving location mapping");
                return false;
            }
        }

        public async Task<bool> DeleteMappingAsync(int mappingId)
        {
            try
            {
                await _repository.DeleteMappingAsync(mappingId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting location mapping");
                return false;
            }
        }
    }
}
