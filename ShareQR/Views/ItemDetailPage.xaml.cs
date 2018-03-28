using ShareQR.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareQR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailPageViewModel viewModel;

        public ItemDetailPage(ItemDetailPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private ItemDetailPage()
        {
            InitializeComponent();
        }
    }
}