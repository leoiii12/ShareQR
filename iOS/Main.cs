using System.Linq;
using Foundation;
using SQLitePCL;
using UIKit;

[assembly: Preserve(typeof(Queryable), AllMembers = true)]

namespace ShareQR.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Batteries_V2.Init();


            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}