using Microsoft.EntityFrameworkCore;

namespace InventoryProducer.Models
{
    public class InventoryDbContext : DbContext 
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> optionns):base(optionns)
        {
            
        }


        public DbSet<InventoryDbContext> InventoryUpdates { get; set; }

    }
}
