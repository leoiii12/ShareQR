using Autofac;
using ShareQR.Helpers;
using ShareQR.Services;
using ShareQR.SQLite;
using Xamarin.Forms;

namespace ShareQR
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            var db = new ShareQRDbContext(DependencyService.Get<IFileHelper>().GetSharedFilePath("ShareQR.db"));

            cb.RegisterInstance(db).As<ShareQRDbContext>();
            cb.RegisterType<QRCodeItemStore>().As<IQRCodeItemStore>();
        }
    }
}