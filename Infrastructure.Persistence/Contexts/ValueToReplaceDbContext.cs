namespace Infrastructure.Persistence.Contexts
{
    using Application.Interfaces;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValueToReplaceDbContext : DbContext
    {
        public ValueToReplaceDbContext(
            DbContextOptions<ValueToReplaceDbContext> options) : base(options) 
        {
        }

        public DbSet<ValueToReplace> ValueToReplaces { get; set; }

    }
}
