using System;

namespace ShareQR.Services
{
    public interface IMessageService
    {
        void AppResumed();
        void AppLaunchedFromDeepLink(string data);
    }
}