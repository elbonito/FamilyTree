using System;
using Acr.UserDialogs;
using AppCore.ViewModels;
using AppCoreX.Helpers;
using AppCoreX.Interface;
using AppCoreX.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using Plugin.Settings.Abstractions;

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
            Mvx.LazyConstructAndRegisterSingleton<IMvxTrace,DebugTrace>();
           Mvx.RegisterType<IAppSettings,AppSettings>();
            RegisterNavigationServiceAppStart<MainViewModel>();
        }
    }
}
