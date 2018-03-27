using ShareQR.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareQR.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]   
    public partial class ExtensionPage : ContentPage
    {
        public ExtensionPage()
        {
            InitializeComponent();

			BindingContext = new ExtensionPageViewModel();
        }
    }
}
