using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Shake_and_Compare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ShakeDetector _shakeDecetor;
        public SearchNames sn;
        public MainPage()
        {

            this.InitializeComponent();
            sn = new SearchNames { N1 = "", N2 = "" };
            ContentPanel.DataContext = sn;
            _shakeDecetor = new ShakeDetector();
            _shakeDecetor.ShakeEvent += _shakeDecetor_ShakeEvent;
            _shakeDecetor.Start();
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            bool afound = false;

            bool pfound = false;
            foreach (var command in args.Request.ApplicationCommands.ToList())
            {
                if (command.Label == "About")
                {
                    afound = true;
                }

                if (command.Label == "Policy")
                {
                    pfound = true;
                }
            }
            if (!afound)
                args.Request.ApplicationCommands.Add(new SettingsCommand("s", "About", (p) => { cfoAbout.IsOpen = true; }));

            //if (!pfound)
            //    args.Request.ApplicationCommands.Add(new SettingsCommand("s", "Policy", (p) => { cfoPolicy.IsOpen = true; }));

            args.Request.ApplicationCommands.Add(new SettingsCommand("privacypolicy", "Privacy policy", OpenPrivacyPolicy));

        }

        private async void OpenPrivacyPolicy(IUICommand command)
        {
            var uri = new Uri("http://www.thatslink.com/privacy-statment/ ");
            await Launcher.LaunchUriAsync(uri);
        }

        async void _shakeDecetor_ShakeEvent(object sender, EventArgs e)
        {
            var res = new List<string>();

            var cp = new ContactPicker();
            var contacts = await cp.PickMultipleContactsAsync();
            if (contacts != null && contacts.Count > 0)
            {
                res.AddRange(contacts.Select(contactInformation => contactInformation.Name));
            }

            if (res.Count < 2)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => new MessageDialog("Not enough contacts.").ShowAsync());
            }
            else
            {
                var ran = new Random();
                var r1 = ran.Next(0, res.Count);
                var r2 = r1;
                while (r1 == r2)
                {
                    r2 = ran.Next(0, res.Count);
                }

                // App.Current.Resources.Remove("N1");
                // App.Current.Resources.Add("N1",res[r1].DisplayName);
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    sn = new SearchNames { N1 = res[r1], N2 = res[r2] };
                    ContentPanel.DataContext = sn;
                });
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void compare_but_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (Compare),sn);
        }
    }
}
