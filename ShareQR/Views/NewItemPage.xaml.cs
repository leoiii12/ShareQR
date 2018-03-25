using System;
using System.IO;
using Autofac;
using ShareQR.Helpers;
using ShareQR.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareQR.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
		public QRCodeItem QRCodeItem
		{
			get { 
				return QRCodeItem; 
			}
			set { 
				QRCodeItem = value;

				if (TempMemoryStream != null)
					TempMemoryStream.Dispose();

				TempMemoryStream = new MemoryStream(QRCodeItem.GenerateQRCodeByteArray());
			}
		}

		public string TempData { get; set; } = "";
		public MemoryStream TempMemoryStream { get; set; }

		private readonly IFileHelper _fileHelper;

        public NewItemPage()
        {
            InitializeComponent();
            
            BindingContext = this;

			QRCodeItem = new QRCodeItem("");

			using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
				_fileHelper = AppContainer.Container.Resolve<IFileHelper>();
            }
        }

		protected override void OnDisappearing()
        {
            base.OnDisappearing();

			if (TempMemoryStream != null)
                TempMemoryStream.Dispose();
        }

		async void OnSaveButtonClicked(object sender, EventArgs e)
        {
			var hasNoInput = QRCodeItem.Data == "";
			if (hasNoInput)
            {
                var answer = await DisplayAlert("Alert", "We will save nothing.", "Yes", "No");

                if (answer == true)
					await Navigation.PopModalAsync();
            }
            else
            {
				_fileHelper.Save(TempMemoryStream.ToArray(), QRCodeItem.Path);
				MessagingCenter.Send(this, "AddItem", QRCodeItem.Data);
                await Navigation.PopModalAsync();
            }
        }

        void OnTempDataTextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;

			QRCodeItem = new QRCodeItem(newText);
        }

    }
}