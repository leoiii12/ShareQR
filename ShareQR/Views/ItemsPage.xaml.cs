using System;
using ShareQR.Models;
using ShareQR.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareQR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsPageViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.QRCodeItems.Count == 0)
                viewModel.LoadQRCodeItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as QRCodeItem;
            if (item == null) return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailPageViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        public void OnItemDeleteClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return;

            MessagingCenter.Send(this, "RemoveItem", (QRCodeItem) menuItem.CommandParameter);
        }

        public void OnItemVisitClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return;

            var qrCodeItem = (QRCodeItem) menuItem.CommandParameter;

            Device.OpenUri(new Uri(qrCodeItem.Data));
        }

        async void AddButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}