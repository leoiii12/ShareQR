using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ShareQR.Models;
using ShareQR.Views;

using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class ItemsViewModel : BaseItemViewModel
    {      
        public ObservableCollection<QRCodeItem> QRCodeItems { get; set; }
        public Command LoadQRCodeItemsCommand { get; set; }
        
        public ItemsViewModel()
        {
            Title = "Browse";
            QRCodeItems = new ObservableCollection<QRCodeItem>();
            LoadQRCodeItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, QRCodeItem>(this, "AddItem", async (obj, item) =>
            {
                var qrCodeItem = item as QRCodeItem;

                if (qrCodeItem == null)
                {
                    throw new Exception("Not a QRCodeItem.");
                }

                QRCodeItems.Add(qrCodeItem);
                //QRCodeItems = new ObservableCollection<QRCodeItem>(QRCodeItems.OrderBy(x => x.CreateDate).ToList());

                await DataStore.AddItemAsync(qrCodeItem);
            });

            MessagingCenter.Subscribe<ItemsPage, QRCodeItem>(this, "RemoveItem", async (obj, item) =>
            {
                var qrCodeItem = item as QRCodeItem;

                if (qrCodeItem == null)
                {
                    throw new Exception("Not a QRCodeItem.");
                }

                QRCodeItems.Remove(qrCodeItem);
                //QRCodeItems = new ObservableCollection<QRCodeItem>(QRCodeItems.OrderBy(x => x.CreateDate).ToList());
                
                await DataStore.DeleteItemAsync(item.Data);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                QRCodeItems.Clear();

                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    QRCodeItems.Add(item);
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
