using ShareQR.Models;

namespace ShareQR.ViewModels
{
    public class ItemDetailPageViewModel : QRCodeItemBaseViewModel
    {
        public QRCodeItem Item { get; set; }

        public ItemDetailPageViewModel(QRCodeItem item = null) : base()
        {
            Title = item?.Data;
            Item = item;
        }
    }
}