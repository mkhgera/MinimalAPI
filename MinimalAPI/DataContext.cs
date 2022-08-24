using Microsoft.EntityFrameworkCore;

namespace MinimalAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Actor> Actors => Set<Actor>();

    }
}
