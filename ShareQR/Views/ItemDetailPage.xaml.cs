using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ShareQR.Models;
using ShareQR.ViewModels;

namespace ShareQR.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
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