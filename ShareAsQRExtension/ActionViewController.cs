using System;

using MobileCoreServices;
using Foundation;
using UIKit;
using QRCoder;

namespace ShareAsQRExtension
{
    public partial class ActionViewController : UIViewController
    {
        protected ActionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

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

        public override bool ShouldAutorotate()
        {
            return false;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Get the item[s] we're handling from the extension context.

            // For example, look for an image and place it into an image view.
            // Replace this with something appropriate for the type[s] your extension supports.
            bool imageFound = false;

            foreach (var item in ExtensionContext.InputItems)
            {
                foreach (var itemProvider in item.Attachments)
                {
                    if (itemProvider.HasItemConformingTo(UTType.URL))
                    {
                        itemProvider.LoadItem(UTType.URL, null, delegate (NSObject url, NSError error)
                        {
                            // Generate QR Code
                            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(url.ToString(), QRCodeGenerator.ECCLevel.Q);
                            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);

                            // This is an image. We'll load it, then place it in our image view.
                            var image = UIImage.LoadFromData(NSData.FromArray(qrCodeAsBitmapByteArr));
                            NSOperationQueue.MainQueue.AddOperation(delegate
                            {
                                imageView.Image = image;
                                imageView.SetNeedsUpdateConstraints();
                            });
                        });

                        imageFound = true;
                        break;
                    }
                }

                if (imageFound)
                {
                    // We only handle one image, so stop looking for more.
                    break;
                }
            }
        }

        partial void DoneClicked(NSObject sender)
        {
            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }
    }
}
