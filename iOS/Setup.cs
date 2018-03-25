﻿using System;
using Autofac;

namespace ShareQR.iOS
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<FileHelper>().As<IFileHelper>();

            base.RegisterDependencies(cb);
        }
    }
}
