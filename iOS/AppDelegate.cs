using Autofac;
using Foundation;
using ShareQR.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ShareQR.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App(new Setup()));


            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            if (url != null)
            {
                NSUrlComponents urlComponents = new NSUrlComponents(url, false);

                string data = "";

                NSUrlQueryItem[] allItems = urlComponents.QueryItems;
                foreach (NSUrlQueryItem item in allItems)
                {
                    if (item.Name == "data")
                        data = item.Value;
                }

                if (!string.IsNullOrEmpty(data))
                {
                    IMessageService messageService;

                    using (var scope = AppContainer.Container.BeginLifetimeScope())
                    {
                        messageService = AppContainer.Container.Resolve<IMessageService>();
                    }

                    messageService.AppLaunchedFromDeepLink(data);
                }
            }

            return true;
        }
    }
}