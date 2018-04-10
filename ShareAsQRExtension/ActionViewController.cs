using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MobileCoreServices;
using ShareQR.Helpers;
using ShareQR.Models;
using ShareQR.Services;
using ShareQR.SQLite;
using UIKit;

[assembly: Preserve(typeof(Queryable), AllMembers = true)]

namespace ShareAsQRExtension
{
    public partial class ActionViewController : UIViewController
    {
        protected ActionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic. 

            _fileHelper = new FileHelper();
            _db = ShareQRDbContext.Create(_fileHelper.GetSharedFilePath("ShareQR.db"));
            _qrCodeItemStore = new QRCodeItemStore(_fileHelper, _db);
        }

        private readonly IReadOnlyList<NSString> _acceptedTypes = new List<NSString> {UTType.URL, UTType.PlainText};

        private readonly IFileHelper _fileHelper;
        private readonly ShareQRDbContext _db;
        private readonly IQRCodeItemStore _qrCodeItemStore;

        private QRCodeItem _qrCodeItem;
        private byte[] _qrCodeByteArray;
        private bool _isOpenedInContainingApp;

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

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (_isOpenedInContainingApp)
                Done();
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
                    var foundType = _acceptedTypes.FirstOrDefault(at => itemProvider.HasItemConformingTo(at));

                    if (foundType == null) continue;


                    itemProvider.LoadItem(foundType, null, (urlObj, error) =>
                    {
                        var url = urlObj.ToString();
                        Console.WriteLine(url);

                        if (url.Length > 256)
                        {
                            var deepLinkingUrl = string.Format("ShareQR://com.ettech.ShareQR?data={0}", url);
                            NSOperationQueue.MainQueue.AddOperation(() =>
                            {
                                _isOpenedInContainingApp = true;

                                OpenURL(new NSUrl(deepLinkingUrl));
                            });
                        }
                        else
                        {
                            dataLabel.Text = url;

                            Task.Factory.StartNew(() =>
                            {
                                _qrCodeItem = new QRCodeItem(_fileHelper, url);
                                _qrCodeByteArray = _qrCodeItem.GenerateQRCodeByteArray();

                                using (var qrCodeByteBuffer = NSData.FromArray(_qrCodeByteArray))
                                {
                                    var image = UIImage.LoadFromData(qrCodeByteBuffer);
                                    NSOperationQueue.MainQueue.AddOperation(() => imageView.Image = image);
                                }
                            });
                        }
                    });


                    imageFound = true;
                    break;
                }

                if (imageFound)
                {
                    NSOperationQueue.MainQueue.AddOperation(() => saveButton.Enabled = true);

                    break;
                }
            }
        }

        partial void DoneClicked(UIBarButtonItem sender)
        {
            Done();
        }

        private void Done()
        {
            _db.Dispose();

            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }

        partial void SaveClicked(UIBarButtonItem sender)
        {
            var path = _qrCodeItem.Path;

            if (!_fileHelper.SaveByteArray(_qrCodeByteArray, path))
                throw new Exception("Cannot save the QR code.");

            Console.WriteLine("Saved the QR code to " + path + ".");
            NSOperationQueue.MainQueue.AddOperation(() =>
            {
                saveButton.Enabled = false;
                saveButton.Title = "Saved";
            });

            _qrCodeItemStore.AddItemAsync(_qrCodeItem);
            Console.WriteLine("Inserted into the database.");
        }

        [Export("openURL:")]
        private bool OpenURL(NSUrl url)
        {
            UIResponder responder = this as UIResponder;

            while (responder != null)
            {
                var application = responder as UIApplication;

                if (application != null)
                {
                    var _url = url;

                    application.PerformSelector(new ObjCRuntime.Selector("openURL:"), _url, 0);

                    return true;
                }

                responder = responder.NextResponder;
            }

            return false;
        }
    }
}