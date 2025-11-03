using PageLightPrime.API.Models.Entities;

namespace SaiReddy_PagelightPrime.Controllers
{
    [Route("Location")]
    public class LocationController : Controller
    {
        private readonly ILocationService _service;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationService service, ILogger<LocationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = new LocationViewModel
                {
                    Countries = (await _service.GetCountriesAsync()).ToList(),
                    States = new List<StateDto>(),
                    Districts = new List<DistrictDto>(),
                    LocationMappings = (await _service.GetAllMappingsAsync()).ToList(),
                    locationMappingInputModel = new LocationMappingInputModel()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Location Index view.");
                TempData["ToastMessage"] = "Something went wrong while loading data!";
                TempData["ToastType"] = "error";
                return View(new LocationViewModel());
            }
        }

        [HttpGet("reload-table")]
        public async Task<IActionResult> ReloadTable()
        {
            try
            {
                var mappings = await _service.GetAllMappingsAsync();
                return PartialView("_LocationMappingTable", mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reloading table data.");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetStates")] 
        public async Task<JsonResult> GetStates(int countryId) 
        {
            try
            {
                var states = await _service.GetStatesByCountryAsync(countryId);
                return Json(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error States data.");
                return Json(new object());
            }
            
        }
        [HttpGet("GetDistricts")]
        public async Task<JsonResult> GetDistricts(int stateId)
        {
            try
            {
                var districts = await _service.GetDistrictsByStateAsync(stateId);
                return Json(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Districts data.");
                return Json(new object());
            }
        }

        [HttpPost("SaveMapping")]
        public async Task<IActionResult> SaveMapping([FromForm] LocationMappingInputModel locationMappingInputModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            try
            {
                var success = await _service.SaveOrUpdateMappingAsync(locationMappingInputModel);

                if (success)
                {
                    TempData["ToastMessage"] = locationMappingInputModel.MappingId == 0
                        ? "Location mapping saved successfully!"
                        : "Location mapping updated successfully!";
                    TempData["ToastType"] = "success";
                }
                else
                {
                    TempData["ToastMessage"] = "Failed to save mapping.";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving location mapping.");
                TempData["ToastMessage"] = "An unexpected error occurred while saving.";
                TempData["ToastType"] = "error";
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int mappingId)
        {
            try
            {
                await _service.DeleteMappingAsync(mappingId);
                TempData["ToastMessage"] = "Mapping deleted successfully!";
                TempData["ToastType"] = "warning";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting mapping with ID {mappingId}");
                TempData["ToastMessage"] = "Failed to delete mapping.";
                TempData["ToastType"] = "error";
            }

            return RedirectToAction("Index");
        }
    }
}
