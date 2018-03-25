using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ShareQR.Models;
using ShareQR.Views;
using ShareQR.ViewModels;

namespace ShareQR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as QRCodeItem;
            if (item == null) return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        public void OnDeleteClicked(object sender, EventArgs e)
        {
			var menuItem = sender as MenuItem;
			if (menuItem == null) return;

			MessagingCenter.Send<ItemsPage, QRCodeItem>(this, "RemoveItem", (QRCodeItem) menuItem.CommandParameter);
        }

        async void AddItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.QRCodeItems.Count == 0)
                viewModel.LoadQRCodeItemsCommand.Execute(null);
        }
    }
}