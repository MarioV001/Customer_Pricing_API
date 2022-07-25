using Microsoft.EntityFrameworkCore;

namespace Customer_Pricing_API.Data
{
    public class DBProducts_Context : DbContext
    {
        public DBProducts_Context(DbContextOptions<DBProducts_Context> options):base(options)//retive tables from DB_file
        {

        }
        public DbSet<DBProducts> DBProducts { get; set; }//Save instance of DBProducts class into quories
        public DbSet<SpecificPricesDB> SpecificPricesDB { get; set; }//Save instance of SpecificPricesDB class

    }
}
