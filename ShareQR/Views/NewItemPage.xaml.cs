using System;
using ShareQR.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareQR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        NewItemPageViewModel viewModel;

        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new NewItemPageViewModel();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var hasNoInput = viewModel.InputText == "";
            if (hasNoInput)
            {
                var answer = await DisplayAlert("Alert", "We will save nothing.", "Yes", "No");

                if (answer)
                    await Navigation.PopModalAsync();
            }
            else
            {
                viewModel.SaveAll();
                MessagingCenter.Send(this, "AddItem", viewModel.Item);
                await Navigation.PopModalAsync();
            }
        }
    }
}