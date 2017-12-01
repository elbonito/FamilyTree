using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AppCoreX.Helpers;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;
using MvvmCross.Uwp.Platform;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AppUwp
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

          
            Mvx.RegisterSingleton<ISettings>(CrossSettings.Current);
        }

        protected override IMvxApplication CreateApp()
        {
            return new AppCoreX.AppCore();
        }

        protected override MvxLogProviderType GetDefaultLogProviderType()
        {
            return MvxLogProviderType.None;
        }
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
