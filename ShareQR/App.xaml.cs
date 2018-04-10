using Autofac;
using ShareQR.Services;
using ShareQR.ViewModels;
using ShareQR.Views;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace ShareQR
{
    public partial class App : Application
    {
        public App(AppSetup setup)
        {
            InitializeComponent();

            AppContainer.Container = setup.CreateContainer();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MainPage();
            else
                MainPage = new NavigationPage(new MainPage());

            MessagingCenter.Subscribe<MessageService, string>(this, MessageService.APP_LAUNCHD_FROM_DEEP_LINK, (sender, data) => {
                MainPage.Navigation.PushModalAsync(new NavigationPage(new NewItemPage(new NewItemPageViewModel(data))));
            });
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();

            IMessageService messageService;
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                messageService = AppContainer.Container.Resolve<IMessageService>();
            }

            messageService.AppResumed();
        }
    }
}