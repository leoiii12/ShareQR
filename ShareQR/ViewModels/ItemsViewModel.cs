using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

		public ItemsViewModel() : base()
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
                await DataStore.AddItemAsync(qrCodeItem);
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
