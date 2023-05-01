using ArkansasMagic.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Data
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<Event> Events { get; set; }
        DbSet<Organization> Organizations { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Reset();
        void Migrate();
        Task ExecuteSqlAsync(string sql);
    }
}
