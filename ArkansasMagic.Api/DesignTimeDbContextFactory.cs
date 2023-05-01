using ArkansasMagic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace ArkansasMagic.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var version = new MySqlServerVersion(new Version(8, 0, 25));
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySql("server=127.0.0.1;database=ArkansasMagic;User Id=root;Password=password123!;port=3309;", version);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
