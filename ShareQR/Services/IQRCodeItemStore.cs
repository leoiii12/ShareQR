using System.Collections.Generic;
using System.Threading.Tasks;
using ShareQR.Models;

namespace ShareQR.Services
{
    public interface IQRCodeItemStore
    {
        Task<bool> AddItemAsync(QRCodeItem item);
        Task<bool> DeleteItemAsync(string id);
        Task<QRCodeItem> GetItemAsync(string path);
        Task<IEnumerable<QRCodeItem>> GetItemsAsync(bool forceRefresh = false);
        Task<bool> UpdateItemAsync(QRCodeItem item);
    }
}
