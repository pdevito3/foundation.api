namespace Foundation.Api.Data
{
    using Foundation.Api.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ValueToReplaceDbContext : DbContext
    {
        public ValueToReplaceDbContext(DbContextOptions<ValueToReplaceDbContext> options) : base(options) { }

        public DbSet<ValueToReplace> ValueToReplaces { get; set; }
    }
}
