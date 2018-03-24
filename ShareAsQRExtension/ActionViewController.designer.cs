// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ShareAsQRExtension
{
    [Register ("ActionViewController")]
    partial class ActionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem saveButton { get; set; }
        
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView view { get; set; }

        [Action ("DoneClicked:")]
        partial void DoneClicked (UIKit.UIBarButtonItem sender);

        [Action ("SaveClicked:")]
        partial void SaveClicked (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (saveButton != null) {
                saveButton.Dispose ();
                saveButton = null;
            }

            if (view != null) {
                view.Dispose ();
                view = null;
            }
        }
    }
}