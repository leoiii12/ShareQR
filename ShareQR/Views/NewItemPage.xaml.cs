using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ShareQR.Models;

namespace ShareQR.Views
{
    public class TempQRCodeItem
    {
        public string Data { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
		public TempQRCodeItem TempItem { get; set; }

		public NewItemPage()
        {
            InitializeComponent();

            TempItem = new TempQRCodeItem
            {
                Data = ""
            };

            BindingContext = this;
        }

        async void SaveClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", new QRCodeItem(TempItem.Data));
            await Navigation.PopModalAsync();
        }

    }
}