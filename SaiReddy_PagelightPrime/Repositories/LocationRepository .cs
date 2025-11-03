namespace PageLightPrime.API.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly PageLightPrimeContext _context;

        public LocationRepository(PageLightPrimeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
         => await _context.CountryDtos
             .FromSqlRaw("EXEC dbo.usp_GetCountries")
             .ToListAsync();

        public async Task<IEnumerable<StateDto>> GetStatesByCountryAsync(int countryId)
        {
            var param = new SqlParameter("@CountryId", countryId);
            return await _context.StateDtos
                .FromSqlRaw("EXEC dbo.usp_GetStatesByCountry @CountryId", param)
                .ToListAsync();
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsByStateAsync(int stateId)
        {
            var param = new SqlParameter("@StateId", stateId);
            return await _context.DistrictDtos
                .FromSqlRaw("EXEC dbo.usp_GetDistrictsByState @StateId", param)
                .ToListAsync();
        }

        public async Task<IEnumerable<LocationMappingDto>> GetAllMappingsAsync()
            => await _context.LocationMappingDtos
                .FromSqlRaw("EXEC dbo.usp_GetLocationMappings")
                .ToListAsync();
        
        public async Task SaveOrUpdateMappingAsync(LocationMappingInputModel mapping)
        {
            var parameters = new[]
            {
                new SqlParameter("@MappingId", mapping.MappingId == 0 ? DBNull.Value : mapping.MappingId),
                new SqlParameter("@CountryId", mapping.CountryId),
                new SqlParameter("@StateId", mapping.StateId),
                new SqlParameter("@DistrictId", mapping.DistrictId),
                new SqlParameter("@Remarks", mapping.Remarks),
                new SqlParameter("@UserName", "Admin")
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.usp_SaveOrUpdateLocationMapping @MappingId, @CountryId, @StateId, @DistrictId, @Remarks, @UserName", parameters);
        }

        public async Task DeleteMappingAsync(int mappingId)
        {
            var param = new SqlParameter("@MappingId", mappingId);
            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.usp_DeleteLocationMapping @MappingId", param);
        }
    }
}
