namespace RadisCacheImplementation.Context
{
    public class databaseContext:DbContext
    {
        public databaseContext(DbContextOptions<databaseContext> options) : base(options)
        {

        }

        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }
    }
}
