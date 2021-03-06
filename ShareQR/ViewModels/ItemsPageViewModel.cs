﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ShareQR.Models;
using ShareQR.Services;
using ShareQR.Views;
using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class ItemsPageViewModel : BaseViewModel
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
                var qrCodeItem = item;

                if (qrCodeItem == null)
                    throw new Exception($"Not a {nameof(QRCodeItem)}.");

                if (QRCodeItems.All(qci => qci.Data != item.Data))
                {
                    QRCodeItems.Insert(0, qrCodeItem);
                    await DataStore.AddItemAsync(qrCodeItem);
                }
            });

            MessagingCenter.Subscribe<ItemsPage, QRCodeItem>(this, "RemoveItem", async (obj, item) =>
            {
                var qrCodeItem = item;

                if (qrCodeItem == null)
                    throw new Exception($"Not a {nameof(QRCodeItem)}.");

                QRCodeItems.Remove(qrCodeItem);
                await DataStore.DeleteItemAsync(qrCodeItem);
            });

            MessagingCenter.Subscribe<MessageService>(this, MessageService.APP_RESUMED, obj => { LoadQRCodeItemsCommand.Execute(null); });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                await Task.Delay(1000);

                QRCodeItems.Clear();

                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    QRCodeItems.Add(item);
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