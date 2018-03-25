using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ShareQR.Models;
using ShareQR.Views;
using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class ItemsPageViewModel : QRCodeItemBaseViewModel
    {
        public ObservableCollection<QRCodeItem> QRCodeItems { get; set; }
        public Command LoadQRCodeItemsCommand { get; set; }

        public ItemsPageViewModel()
        {
            Title = "Browse";
            QRCodeItems = new ObservableCollection<QRCodeItem>();
            LoadQRCodeItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, QRCodeItem>(this, "AddItem", async (obj, item) =>
            {
                var qrCodeItem = item as QRCodeItem;

                if (qrCodeItem == null)
                    throw new Exception($"Not a {nameof(QRCodeItem)}.");

                QRCodeItems.Insert(0, qrCodeItem);
                await DataStore.AddItemAsync(qrCodeItem);
            });

            MessagingCenter.Subscribe<ItemsPage, QRCodeItem>(this, "RemoveItem", async (obj, item) =>
            {
                var qrCodeItem = item as QRCodeItem;

                if (qrCodeItem == null)
                    throw new Exception($"Not a {nameof(QRCodeItem)}.");

                QRCodeItems.Remove(qrCodeItem);
                await DataStore.DeleteItemAsync(qrCodeItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                QRCodeItems.Clear();
				IsBusy = false;

                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    QRCodeItems.Add(item);
                }
            }
            catch (Exception ex)
            {
				IsBusy = true;
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}