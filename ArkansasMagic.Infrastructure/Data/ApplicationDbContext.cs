using ArkansasMagic.Core.Data;
using ArkansasMagic.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions)
        { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        public Task ExecuteSqlAsync(string sql) => Database.ExecuteSqlRawAsync(sql);

        public void Migrate()
        {
            Database.Migrate();
        }

        public void Reset()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State != EntityState.Unchanged)
                .ToArray();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public void HandleConcurrencyException(DbUpdateConcurrencyException exception)
        {
            foreach (var entry in exception.Entries)
            {
                var databaseValues = entry.GetDatabaseValues();
                entry.OriginalValues.SetValues(databaseValues);
            }

            Reset();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IHasConcurrencyToken>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.ConcurrencyToken = Guid.NewGuid().ToByteArray();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
