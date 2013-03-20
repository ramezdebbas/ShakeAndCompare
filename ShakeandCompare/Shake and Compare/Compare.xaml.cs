using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Shake_and_Compare
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Compare : Shake_and_Compare.Common.LayoutAwarePage
    {
        private string N1 = "";
        private string N2 = "";

        public Compare()
        {
            this.InitializeComponent();
            this.backButton.Click+=BackButtonOnClick;
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));

            // Place the frame in the current window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var names = (SearchNames) e.Parameter;
            if (names != null)
            {
                N1 = names.N1;
                N2 = names.N2;
            }

            N1_textBox.Text = N1;
            N2_Textbox.Text = N2;
            per_textbox.Text = getFriendShipPer() + "%";

            QuoteChange();
        }
        public async void QuoteChange()
        {

            var res = await App.MobileService.GetTable<Quotes>().ToListAsync();

            var r = new Random();
            quote_textBox.Text = "'" + res[r.Next(0, res.Count())].qt + "'";


        }

        private List<string> GetCount()
        {
            var list = new List<string>();
            const string friendShip = "FRIENDSHIP";
            String name1 = N1;
            String name2 = N2;
            for (var i = 0; i < name1.Length; i++)
            {
                var temp = name1[i] + "";

                if (list.Contains(temp))
                {
                    int indexOfElement = list.IndexOf(temp);
                    int prevCount = Int32.Parse(list[++indexOfElement]);
                    prevCount++;
                    String newCount = (prevCount) + "";
                    list[indexOfElement] = newCount;
                    continue;
                }

                list.Add(temp);
                list.Add("1");
            }

            for (int i = 0; i < name2.Length; i++)
            {
                String temp = name2[i] + "";

                if (list.Contains(temp))
                {
                    int indexOfElement = list.IndexOf(temp);
                    int prevCount = Int32.Parse(list[++indexOfElement]);
                    prevCount++;
                    String newCount = (prevCount) + "";
                    list[indexOfElement] = newCount;
                    continue;
                }

                list.Add(temp);
                list.Add("1");
            }

            for (int i = 0; i < friendShip.Length; i++)
            {
                String temp = friendShip[i] + "";

                if (list.Contains(temp))
                {
                    int indexOfElement = list.IndexOf(temp);
                    int prevCount = Int32.Parse(list[++indexOfElement]);
                    prevCount++;
                    String newCount = (prevCount) + "";
                    list[indexOfElement] = newCount;
                    continue;
                }

                list.Add(temp);
                list.Add("1");
            }

            var result = new List<string>();

            for (int i = 1; i < list.Count; i += 2)
            {
                result.Add(list[i]);
            }

            //System.out.println( result );
            return result;
        }

        public int getFriendShipPer()
        {
            List<string> count = GetCount();
            if (count.Count == 1)
            {
                String result = count[0] + "";
                return Int32.Parse(result);
            }
            if (count.Count == 2)
            {
                String result = (Int32.Parse(count[0]) + Int32.Parse(count[1])).ToString();
                return Int32.Parse(result);
            }

            do
            {
                List<string> sub = new List<string>();
                int size = count.Count / 2;
                //System.out.println( count.size() / 2 );
                for (int i = 0; i < size; i++)
                {
                    String newC = (Int32.Parse(count[i]) + Int32.Parse(count[count.Count - 1 - i])).ToString();

                    if (newC.Length == 2)
                    {
                        sub.Add((newC[0].ToString()));
                        sub.Add((newC[1].ToString()));
                    }
                    else
                    {
                        sub.Add(newC);
                    }
                }

                if ((size * 2) != count.Count)
                    sub.Add(count[size]);

                count = new List<string>();
                count = sub;
                //System.out.println( count );
            } while (count.Count != 2);

            String res = count[0] + count[1];
            return Int32.Parse(res);
        }

        public class Quotes
        {
            public int Id { get; set; }
            public string qt { get; set; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
