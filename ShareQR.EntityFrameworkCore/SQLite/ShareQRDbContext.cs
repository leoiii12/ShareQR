using System;
using Microsoft.EntityFrameworkCore;
using ShareQR.Models;
using ShareQR.SQLite;
using Xamarin.Forms;

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

		protected ShareQRDbContext(string databasePath)
        {
            if (!databasePath.EndsWith(".db", StringComparison.Ordinal))
            {
                throw new Exception($"Give a path ending with \".db\".");
            }

            Console.WriteLine("Database created at " + databasePath);

            DatabasePath = databasePath;
        }

		protected ShareQRDbContext() : this("ShareQR.db")
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
        }
    }
}