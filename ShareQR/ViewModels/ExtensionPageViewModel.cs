using System.Collections.Generic;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShareQR.ViewModels
{
    public class ExtensionPageViewModel : BaseViewModel
    {
        private readonly List<string> _tutorialImages = new List<string>
        {
            "extension_tutorial_2.png",
            "extension_tutorial_3.png",
            "extension_tutorial_4.png"
        };

        private readonly Timer _timer;

        private string _tutorialImage = "";

        public string TutorialImage
        {
            get { return _tutorialImage; }
            set { SetProperty(ref _tutorialImage, value); }
        }

        public ICommand NextImageCommand { get; }

        public ExtensionPageViewModel()
        {
            Title = "Extension";

            _timer = new Timer();
            _timer.Elapsed += OnTimedEvent;
            _timer.Interval = 1000;
            _timer.Enabled = true;

            TutorialImage = _tutorialImages[0];
            NextImageCommand = new Command(() =>
            {
                _timer.Enabled = false;

                NextImage();
            });
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            NextImage();
        }

        private void NextImage()
        {
            var currentIndex = _tutorialImages.FindIndex(ti => ti == TutorialImage);
            var newIndex = currentIndex + 1 >= _tutorialImages.Count ? 0 : currentIndex + 1;

            TutorialImage = _tutorialImages[newIndex];
        }
    }
}