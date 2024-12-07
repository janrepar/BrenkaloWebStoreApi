using BrenkaloWebStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {
            try
            {
                var stores = await _storeService.GetAllStoresAsync();
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            try
            {
                var store = await _storeService.GetStoreByIdAsync(id);

                if (store == null)
                {
                    return NotFound(new { message = "Store not found" });
                }

                return Ok(store);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
