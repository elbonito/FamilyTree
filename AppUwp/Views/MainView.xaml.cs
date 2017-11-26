using Windows.UI.Xaml.Controls;
using MvvmCross.Uwp.Attributes;
using MvvmCross.Uwp.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppUwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxPagePresentation]
    public sealed partial class MainView : MvxWindowsPage
    {
        public MainView()
        {
            this.InitializeComponent();
        }
    }
}
