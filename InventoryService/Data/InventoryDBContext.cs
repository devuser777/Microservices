using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options) : base(options)
        {

        }

        public DbSet<Inventory> Inventories { get; set; }
    }
}
