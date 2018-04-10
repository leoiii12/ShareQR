using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ShareQR.Helpers;
using ShareQR.Models;
using ShareQR.Services;
using Xamarin.Forms;

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
        private readonly ICollection<NewItemPageViewModelCache> _caches = new List<NewItemPageViewModelCache>();

        private string _inputText = "";

        public string InputText
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText, value); }
        }

        private QRCodeItem item = new QRCodeItem("");

        public QRCodeItem Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public NewItemPageViewModel() : this("")
		{
		}

		public NewItemPageViewModel(string initialData)
        {
            Title = "New QR Code";
            IsBusy = false;

            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                _fileHelper = AppContainer.Container.Resolve<IFileHelper>();
            }

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(InputText))
                {
                    IsBusy = true;

                    // Cancel previous
                    foreach (var cache in _caches.Where(h => h.CancellationTokenSource.IsCancellationRequested == false))
                        cache.CancellationTokenSource.Cancel();

                    if (_inputText == "")
                    {
                        Item = new QRCodeItem("");
                        IsBusy = false;

                        return;
                    }

                    GenerateNewCache(new QRCodeItem(_inputText));
                }
            };
			InputText = initialData;
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

            _caches.Add(newCache);
        }

        public void SaveAll()
        {
            if (Item == null) return;
            if (string.IsNullOrEmpty(Item.Data)) return;

            foreach (var cache in _caches)
            {
                _fileHelper.RemoveFile(cache.QRCodeItem.Path);
            }

            _fileHelper.SaveByteArray(Item.GenerateQRCodeByteArray(), Item.Path);
        }
    }
}