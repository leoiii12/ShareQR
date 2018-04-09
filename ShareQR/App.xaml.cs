using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ShareQR.Views;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace ShareQR
{
    public partial class App : Application
    {
		public App() {
			InitializeComponent();
		}

		public App(AppSetup setup) : base()
        {         
            AppContainer.Container = setup.CreateContainer();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MainPage();
            else
                MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();

            MessagingCenter.Send(this, "OnResume");
        }
    }
}