using System;
using Acr.UserDialogs;
using AppCore.ViewModels;
using AppCoreX.Interface;
using AppCoreX.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace AppCoreX
{
    public class AppCore:MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            Mvx.LazyConstructAndRegisterSingleton<IComosDBService,CosmosDBService>();
            RegisterNavigationServiceAppStart<MainViewModel>();
        }
    }
}
