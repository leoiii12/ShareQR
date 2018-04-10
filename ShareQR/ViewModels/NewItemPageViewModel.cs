using System;
using System.Collections.Generic;
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
        private readonly ICollection<NewItemPageViewModelCache> _caches = new List<NewItemPageViewModelCache>();

        private string _inputText = "";
        private QRCodeItem _item = new QRCodeItem("");

        public string InputText
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText, value); }
        }

        public QRCodeItem Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public NewItemPageViewModel() : this("")
        {
        }

        public NewItemPageViewModel(string initialInputText)
        {
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                _fileHelper = AppContainer.Container.Resolve<IFileHelper>();
            }

            Title = "New QR Code";
            IsBusy = false;

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(InputText))
                {
                    IsBusy = true;

                    // Cancel previous generations
                    foreach (var cache in _caches.Where(h => h.CancellationTokenSource.IsCancellationRequested == false))
                        cache.CancellationTokenSource.Cancel();

                    if (_inputText == "")
                    {
                        IsBusy = false;
                        Item = null;

                        return;
                    }

                    GenerateNewCache(new QRCodeItem(_inputText));
                }
            };

            InputText = initialInputText ?? throw new ArgumentNullException(nameof(initialInputText));
        }

        private void GenerateNewCache(QRCodeItem qrCodeItem)
        {
            var newCache = new NewItemPageViewModelCache
            {
                QRCodeItem = qrCodeItem,
                CancellationTokenSource = new CancellationTokenSource()
            };
            _caches.Add(newCache);

            var cancellationToken = newCache.CancellationTokenSource.Token;
            newCache.Task = Task.Factory.StartNew(async () =>
            {
                // Cancel when request
                await Task.Delay(700);
                cancellationToken.ThrowIfCancellationRequested();

                // Save file
                _fileHelper.SaveByteArray(qrCodeItem.GenerateQRCodeByteArray(), qrCodeItem.Path);

                // Cancel when request
                await Task.Delay(300);
                cancellationToken.ThrowIfCancellationRequested();

                // Assign new item
                Item = qrCodeItem;
                IsBusy = false;
            });
        }

        public void SaveAll()
        {
            if (Item == null) return;
            if (string.IsNullOrEmpty(Item.Data)) return;

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1000);

                foreach (var cache in _caches.Where(h => h.CancellationTokenSource.IsCancellationRequested == true))
                {
                    _fileHelper.RemoveFile(cache.QRCodeItem.Path);
                }
            });
        }
    }
}