using Microsoft.EntityFrameworkCore;

namespace CareServicesServer.Models
{
    public class CareServicesDbContext : DbContext
    {

        public CareServicesDbContext(DbContextOptions<CareServicesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<CoronaData> CoronaData { get; set; }
        public DbSet<CoronaVaccineData> CoronaVaccineData { get; set; }

    }
}
