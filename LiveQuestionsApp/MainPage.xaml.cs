using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LiveQuestionsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://sabbourchatroom.azure-mobile.net/",
            "DukynDbJjBwavIJjwicAQlCozuQKCs61"
        );
        public MobileServiceUser CurrentUser { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Authenticate();
            var questions = await MobileService.GetTable<Question>().ToCollectionAsync();
            questionsListView.ItemsSource = questions;
        }

        private async System.Threading.Tasks.Task Authenticate()
        {
            string message;
            while (CurrentUser == null)
            {
                try
                {
                    CurrentUser = await MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                    message = "Logged in";
                }
                catch (InvalidOperationException)
                {
                    message = "You must login.";
                }

                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                dialog.ShowAsync();
            }
        }
    }
}
