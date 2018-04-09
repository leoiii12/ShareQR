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

			OpenPersonalWebsiteCommand = new Command(() => Device.OpenUri(new Uri("https://leochoi.info")));
            OpenGithubWebsiteCommand = new Command(() => Device.OpenUri(new Uri("https://leochoi.info")));
        }

		public ICommand OpenPersonalWebsiteCommand { get; }
		public ICommand OpenGithubWebsiteCommand { get; }
    }
}