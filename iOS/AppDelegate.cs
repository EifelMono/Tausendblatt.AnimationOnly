using Foundation;
using UIKit;

namespace Tausendblatt.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //global::EifelMono.Forms.iOS.Forms.Init();

            // Backdoor init (search over all assemblies)
            //global::EifelMono.BackDoor.Implementation.BackDoor.Init(AppDomain.CurrentDomain.GetAssemblies());

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

