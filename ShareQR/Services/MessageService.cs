using System;
using Xamarin.Forms;

namespace ShareQR.Services
{
    public class MessageService : IMessageService
    {
        public const string APP_RESUMED = "AppResumed";
        public const string APP_LAUNCHD_FROM_DEEP_LINK = "AppLaunchedFromDeepLink";

        public void AppResumed()
        {
            MessagingCenter.Send(this, APP_RESUMED);
        }

        public void AppLaunchedFromDeepLink(string data)
        {
            MessagingCenter.Send(this, APP_LAUNCHD_FROM_DEEP_LINK, data);
        }
    }
}