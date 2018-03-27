using System;
using System.Windows.Input;
using ShareQR.Models;
using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class ItemDetailPageViewModel : BaseViewModel
    {
		public QRCodeItem Item { get; protected set; }
        public bool IsUrl { get; protected set; }
		public ICommand OpenUrlCommand { get; }

        public ItemDetailPageViewModel(QRCodeItem item = null) : base()
        {
            Title = item?.Data;

            Item = item;
			IsUrl = Uri.TryCreate(item?.Data, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

			OpenUrlCommand = new Command(() => Device.OpenUri(new Uri(Item.Data)));
        }
    }
}