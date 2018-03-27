using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ShareQR.Helpers;
using ShareQR.Models;

namespace ShareQR.ViewModels
{
    public class NewItemPageViewModelCache
    {
        public QRCodeItem QRCodeItem { get; set; }
        public Task Task { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }

    public class NewItemPageViewModel : BaseViewModel
    {
        private readonly IFileHelper _fileHelper;
		private readonly ICollection<NewItemPageViewModelCache> caches = new List<NewItemPageViewModelCache>();

        private string inputText = "";
        public string InputText
        {
            get { return inputText; }
            set { SetProperty(ref inputText, value); }
        }

        private QRCodeItem item = new QRCodeItem("");
        public QRCodeItem Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public NewItemPageViewModel()
        {
            Title = "New QR Code";
			IsBusy = false;

            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                _fileHelper = AppContainer.Container.Resolve<IFileHelper>();
            }

            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == nameof(InputText))
				{
					IsBusy = true;

					// Cancel previous
					foreach (var cache in caches.Where(h => h.CancellationTokenSource.IsCancellationRequested == false))
						cache.CancellationTokenSource.Cancel();

					if (inputText == "") {
						Item = new QRCodeItem("");
						IsBusy = false;

						return;
					}

					var qrCodeItem = new QRCodeItem(inputText);
					GenerateNewCache(qrCodeItem);
				}
			};
        }

		private void GenerateNewCache(QRCodeItem qrCodeItem)
		{
			var newCache = new NewItemPageViewModelCache();
			newCache.QRCodeItem = qrCodeItem;
			newCache.CancellationTokenSource = new CancellationTokenSource();

			var cancellationToken = newCache.CancellationTokenSource.Token;
			newCache.Task = Task.Factory.StartNew(async () =>
			{
				await Task.Delay(1000);            
				cancellationToken.ThrowIfCancellationRequested();

				// Save file
				_fileHelper.SaveByteArray(qrCodeItem.GenerateQRCodeByteArray(), qrCodeItem.Path);

				await Task.Delay(100);            
				cancellationToken.ThrowIfCancellationRequested();

				// Assign new item
				Item = qrCodeItem;            
				IsBusy = false;
			});

			this.caches.Add(newCache);
		}

		public void SaveAll()
        {
            if (Item == null) return;
            if (Item.Data == null || Item.Data == "") return;

			foreach (var cache in caches)
			{
				_fileHelper.RemoveFile(cache.QRCodeItem.Path);
			}

            _fileHelper.SaveByteArray(Item.GenerateQRCodeByteArray(), Item.Path);
        }

    }
}
