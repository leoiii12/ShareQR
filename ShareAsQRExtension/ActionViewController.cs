using System;

using MobileCoreServices;
using Foundation;
using UIKit;
using QRCoder;
using System.IO;
using System.Security.Cryptography;

namespace ShareAsQRExtension
{
    public partial class ActionViewController : UIViewController
    {
        protected ActionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        private string url;
        private NSData qrCodeImage;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.

            if (imageView.Image != null)
                imageView.Image.Dispose();
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
                        itemProvider.LoadItem(UTType.URL, null, delegate (NSObject urlObj, NSError error)
                        {
                            url = urlObj.ToString();

                            // Generate QR Code
                            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);

                            // This is an image. We'll load it, then place it in our image view.
                            var image = UIImage.LoadFromData(NSData.FromArray(qrCodeAsBitmapByteArr));
                            qrCodeImage = image.AsJPEG();

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
                        this.doneButton.Enabled = true;
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
            var appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.ChangeExtension(Path.Combine(appDirectory, ComputeHashString(this.url)), ".jpg");

            if (this.qrCodeImage.Save(path, false, out NSError error))
                Console.WriteLine("Saved at " + path + ".");
            else
                Console.WriteLine("Cannot save because " + error.LocalizedDescription + ".");
        }

        private string ComputeHashString(string str)
        {
            byte[] computedHash;

            using (var algorithm = SHA256.Create())
            {
                computedHash = algorithm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(str));
            }

            return BitConverter.ToString(computedHash);
        }
    }
}
