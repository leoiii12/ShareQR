using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        public AboutPageViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://leochoi.info")));
        }

        public ICommand OpenWebCommand { get; }
    }
}