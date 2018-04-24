using Microsoft.EntityFrameworkCore;

namespace AI_Trainer.Models
{
    public class AIContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public AIContext(DbContextOptions<AIContext> options) : base(options) { }
        public DbSet<AI_Trainer.Models.User> users { get; set; }
    }
}