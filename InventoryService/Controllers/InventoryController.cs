using InventoryService.Data;
using InventoryService.RedisCache;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDBContext inventoryDBContext;
        private readonly IInventoryCacheService _cache;

        public InventoryController(InventoryDBContext inventoryDBContext, IInventoryCacheService cache)
        {
            this.inventoryDBContext = inventoryDBContext;
            _cache = cache;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public IEnumerable<Inventory> Get()
        {
            return inventoryDBContext.Inventories.ToList();
        }

        // GET api/inventory/ABC123
        [HttpGet("{sku}")]
        public async Task<IActionResult> Get(int sku)
        {
            var item = await _cache.GetInventoryAsync(sku);

            if (item != null)
                return Ok(item);

            item = inventoryDBContext.Inventories.FirstOrDefault(i => i.Id == sku);
            
            await _cache.SetInventoryAsync(item);

            return Ok(item);
        }

        // POST api/inventory
        [HttpPost]
        public async Task<IActionResult> Set([FromBody] Inventory item)
        {

            inventoryDBContext.Inventories.Add(item);

            inventoryDBContext.SaveChanges();

            await _cache.SetInventoryAsync(item);
            return Ok("Stored in cache");
        }
    }
}
