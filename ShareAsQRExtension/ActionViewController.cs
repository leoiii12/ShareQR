using System;

using MobileCoreServices;
using Foundation;
using UIKit;
using ShareQR.Models;
using ShareQR;
using ShareQR.SQLite;
using ShareQR.Services;

[assembly: Preserve(typeof(System.Linq.Queryable), AllMembers = true)]
namespace ShareAsQRExtension
{
    public partial class ActionViewController : UIViewController
    {
        protected ActionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic. 

            _fileHelper = new FileHelper();
            _db = new ShareQRDbContext(_fileHelper.GetSharedFilePath("ShareQR.db"));
            _qrCodeItemStore = new QRCodeItemStore(_db);
        }

        private readonly IFileHelper _fileHelper;
        private readonly ShareQRDbContext _db;
        private readonly IQRCodeItemStore _qrCodeItemStore;

        private QRCodeItem _qrCodeItem;
        private NSData _qrCodeByteBuffer;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override bool PrefersStatusBarHidden()
        {
            return false;
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            return UIInterfaceOrientation.Portrait;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // For example, look for an image and place it into an image view.
            // Replace this with something appropriate for the type[s] your extension supports.
            bool imageFound = false;

            foreach (var item in ExtensionContext.InputItems)
            {
                foreach (var itemProvider in item.Attachments)
                {
                    if (itemProvider.HasItemConformingTo(UTType.URL))
                    {
                        itemProvider.LoadItem(UTType.URL, null, delegate (NSObject urlObj, NSError error)
                        {
                            var url = urlObj.ToString();
                            Console.WriteLine(url);

                            _qrCodeItem = new QRCodeItem(_fileHelper, url);
                            _qrCodeByteBuffer = NSData.FromArray(_qrCodeItem.GenerateQRCodeByteArray());

                            var image = UIImage.LoadFromData(_qrCodeByteBuffer);
                            NSOperationQueue.MainQueue.AddOperation(delegate
                            {
                                imageView.Image = image;
                            });
                        });

                        imageFound = true;
                        break;
                    }
                }

                if (imageFound)
                {
                    NSOperationQueue.MainQueue.AddOperation(delegate
                    {
                        saveButton.Enabled = true;
                    });

                    break;
                }
            }
        }

        partial void DoneClicked(UIBarButtonItem sender)
        {
            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }

        partial void SaveClicked(UIBarButtonItem sender)
        {
            var path = _qrCodeItem.Path;

            if (_qrCodeByteBuffer.Save(path, false, out NSError error))
            {
                Console.WriteLine("Saved at " + path + ".");
                NSOperationQueue.MainQueue.AddOperation(delegate
                {
                    saveButton.Enabled = false;
                    saveButton.Title = "Saved";
                });

                _qrCodeItemStore.AddItemAsync(_qrCodeItem);
				Console.WriteLine("Inserted into db.");
            }
            else
            {
                Console.WriteLine("Cannot save because " + error.LocalizedDescription + ".");
            }
        }
    }
}
