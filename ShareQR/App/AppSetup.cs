﻿using Autofac;
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
			// Be careful of Captive Dependency
			cb.Register(cc => ShareQRDbContext.Create(cc.Resolve<IFileHelper>().GetSharedFilePath("ShareQR.db"))).As<ShareQRDbContext>().SingleInstance();
			cb.RegisterType<QRCodeItemStore>().As<IQRCodeItemStore>().SingleInstance();
        }
    }
}