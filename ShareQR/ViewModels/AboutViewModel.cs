using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class AboutViewModel : BaseItemViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}
