using System;
using ShareQR.Models;

namespace ShareQR.ViewModels
{
    public class ItemDetailViewModel : BaseItemViewModel
    {
		public QRCodeItem Item { get; set; }

		public ItemDetailViewModel(QRCodeItem item = null) : base()
        {
			Title = item?.Data;
            Item = item;
        }
    }
}
