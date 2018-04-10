using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using ShareQR.Helpers;
using ShareQR.Models;
using ShareQR.SQLite;
using SQLitePCL;

namespace ShareQR.Services
{
    public class QRCodeItemStore : IQRCodeItemStore
    {
        private readonly IFileHelper _fileHelper;
        private readonly ShareQRDbContext _db;

        public QRCodeItemStore(IFileHelper fileHelper, ShareQRDbContext db)
        {
            Batteries_V2.Init();

            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> AddItemAsync(QRCodeItem item)
        {
            if (await _db.QRCodeItems.AnyAsync(qrci => qrci.Data == item.Data)) return true;

            await _db.QRCodeItems.AddAsync(item);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemAsync(QRCodeItem item)
        {
            if (await _db.QRCodeItems.AnyAsync(i => i.Data == item.Data) == false)
                throw new Exception("The item does not exist.");

            _fileHelper.RemoveFile(item.Path);

            _db.QRCodeItems.Remove(item);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<QRCodeItem> GetItemAsync(string data)
        {
            return await _db.QRCodeItems.FirstAsync(qrci => qrci.Data == data);
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