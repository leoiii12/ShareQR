using System;
using Microsoft.EntityFrameworkCore;
using ShareQR.Models;
using ShareQR.SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareQRDbContext))]
namespace ShareQR.SQLite
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

        public ShareQRDbContext(string databasePath)
        {
			Console.WriteLine("Database created at " + databasePath);

            DatabasePath = databasePath;
        }

		public ShareQRDbContext() : this("ShareQR.db")
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
        }
    }
}
