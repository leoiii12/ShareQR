using Microsoft.EntityFrameworkCore;
using ShareQR.Models;

namespace ShareQR.Services
{
    public class ShareQRDbContext : DbContext
    {
		public DbSet<QRCodeItem> QRCodeItems { get; set; }

        public static ShareQRDbContext Create(string databasePath)
        {
			var dbContext = new ShareQRDbContext(databasePath);
			dbContext.Database.EnsureCreated();
			dbContext.Database.Migrate();

            return dbContext;
        }

        protected string DatabasePath { get; set; }

        protected ShareQRDbContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
