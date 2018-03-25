using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class AboutPageViewModel : QRCodeItemBaseViewModel
    {
        public AboutPageViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}