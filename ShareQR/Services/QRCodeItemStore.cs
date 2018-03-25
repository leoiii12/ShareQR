using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ShareQR.SQLite;
using ShareQR.Models;
using Microsoft.EntityFrameworkCore;
using Autofac;

namespace ShareQR.Services
{
    public class QRCodeItemStore : IQRCodeItemStore
    {
        private readonly ShareQRDbContext _db;

        /// <summary>
        /// <para>Construct with DI.</para>
        /// </summary>
        public QRCodeItemStore()
        {
            Console.WriteLine("QRCodeItemStore");
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                _db = AppContainer.Container.Resolve<ShareQRDbContext>();
                Console.WriteLine(_db);
            }
        }

        /// <summary>
        /// <para>Construct without DI.</para>
        /// </summary>
        public QRCodeItemStore(ShareQRDbContext db)
        {
            SQLitePCL.Batteries_V2.Init();

            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> AddItemAsync(QRCodeItem item)
        {
            if (await _db.QRCodeItems.AnyAsync(i => i.Data == item.Data))
            {
                return true;
            }

            await _db.QRCodeItems.AddAsync(item);

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var item = await _db.FindAsync<QRCodeItem>(id);

            _db.QRCodeItems.Remove(item);

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<QRCodeItem> GetItemAsync(string path)
        {
            return await _db.QRCodeItems.FirstAsync(qrci => qrci.Path == path);
        }

        public async Task<IEnumerable<QRCodeItem>> GetItemsAsync(bool forceRefresh = false)
        {
			return await _db.QRCodeItems.OrderByDescending(qrci => qrci.CreateDate).ToListAsync();
        }

        public Task<bool> UpdateItemAsync(QRCodeItem item)
        {
            throw new NotImplementedException();
        }
    }
}
