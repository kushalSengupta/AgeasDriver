using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook.Client;
using System.Threading.Tasks;
using AgeasDriver.Common;
using Newtonsoft.Json;

namespace AgeasDriver.Pages
{
    public partial class FrmFBLogin : PhoneApplicationPage
    {
        
        public FrmFBLogin()
        {
            InitializeComponent();

        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FacebookLoginPage.xaml", UriKind.Relative));
        }

        async void FacebookLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.Loaded += FacebookLoginPage_Loaded;

            System.Threading.Thread.Sleep(2000);
        }
       
        private FacebookSession session;
        private async Task Authenticate()
        {
            string message = String.Empty;
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("user_about_me,read_stream");
                App.AccessToken = session.AccessToken;
                App.FacebookId = session.FacebookId;
                App.fbSession = session;
                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/Pages/FrmFacebookPost.xaml", UriKind.Relative)));
            }
            catch (InvalidOperationException e)
            {
                message = "Login failed! Exception details: " + e.Message;
                MessageBox.Show(message);
            }
        }
    }
}