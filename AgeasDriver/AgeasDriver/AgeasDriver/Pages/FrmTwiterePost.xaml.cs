using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AgeasDriver.Common;
using Newtonsoft.Json;
using System.Text;
using Hammock.Authentication.OAuth;
using TweetSharp;
using System.IO.IsolatedStorage;
using SampleTwitterApp;
using System.IO;
using Hammock.Web;
using Hammock;

namespace AgeasDriver.Pages
{
        

    public partial class FrmTwiterePost : PhoneApplicationPage
    {
        string StrCommonObject = "";
        ClsCommon ObjCommon;

        string OAuthTokenKey = string.Empty;
        string tokenSecret = string.Empty;
        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;
        string userID = string.Empty;
        string userScreenName = string.Empty;
        OAuthWebQuery AccessTokenQuery;
        public static string consumerKey = "jfSgm58bVChUigLeYMdvQ";
        public static string consumerKeySecret = "99R9f0OmDx6Obqq6esOfr6D87OIuzea3mg67EJp0";
       // public static string consumerKey = "WiZYyxRFdxOVqW1QIKvsw";
       // public static string consumerKeySecret = "9c1H1HwDYnpt2pjsiHHLTVp2tQGxhiL0eEFlQTEttOA";

        public FrmTwiterePost()
        {
            InitializeComponent();
            this.loginBrowserControl.Visibility = Visibility.Visible;
            if (isAlreadyLoggedIn())
            {
                userLoggedIn();
                VisibileControl(true);
            }

            else
            {
                VisibileControl(false);
                var requestTokenQuery = OAuthUtil.GetRequestTokenQuery();
                requestTokenQuery.RequestAsync(AppSettings.RequestTokenUri, null);
                requestTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(requestTokenQuery_QueryResponse);
            }
            
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //TxtPost.Text = "Well done,you handle those \n comer safely";

            StrCommonObject = NavigationContext.QueryString["common"];
            //Waypoints = new List<GeoCoordinate>()
            ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);


            base.OnNavigatedTo(e);

            TxtPost.Text = "I am using #AgeasDrive to try and \n save up to 20% on my comprehensive \n car insurance. You could too";
            Txtlink.Text = " https://www.ageas.co.uk/";
        }

        private void VisibileControl(bool flag)
        {
            if (flag)
            {
                Txtlink.Visibility = System.Windows.Visibility.Visible;
                TxtPost.Visibility = System.Windows.Visibility.Visible;
                ImgBackground.Visibility = System.Windows.Visibility.Visible;
                ImgHeader.Visibility = System.Windows.Visibility.Visible;
                ImgPost.Visibility = System.Windows.Visibility.Visible;
                btnCancle.Visibility = System.Windows.Visibility.Visible;
                btnSubmit.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Txtlink.Visibility = System.Windows.Visibility.Collapsed;
                TxtPost.Visibility = System.Windows.Visibility.Collapsed;
                ImgBackground.Visibility = System.Windows.Visibility.Collapsed;
                ImgHeader.Visibility = System.Windows.Visibility.Collapsed;
                ImgPost.Visibility = System.Windows.Visibility.Collapsed;
                btnCancle.Visibility = System.Windows.Visibility.Collapsed;
                btnSubmit.Visibility = System.Windows.Visibility.Collapsed;
            }

        }
        private void saveAccessToken(OAuthAccessToken token)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("accessToken"))
                IsolatedStorageSettings.ApplicationSettings["accessToken"] = token;
            else
                IsolatedStorageSettings.ApplicationSettings.Add("accessToken", token);

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private OAuthAccessToken getAccessToken()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("accessToken"))
                return IsolatedStorageSettings.ApplicationSettings["accessToken"] as OAuthAccessToken;
            else
                return null;
        }

        private Boolean isAlreadyLoggedIn()
        {
            accessToken = MainUtil.GetKeyValue<string>("AccessToken");
            accessTokenSecret =  MainUtil.GetKeyValue<string>("AccessTokenSecret");
            userScreenName = MainUtil.GetKeyValue<string>("ScreenName");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
                return false;
            else
                return true;
        }

        private void userLoggedIn()
        {
            Dispatcher.BeginInvoke(() =>
            {
                VisibileControl(true);
               OAuthAccessToken token= getAccessToken();
               saveAccessToken(token);
                //var SignInMenuItem = (Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0];
                //SignInMenuItem.IsEnabled = false;

                //var SignOutMenuItem = (Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[1];
                //SignOutMenuItem.IsEnabled = true;

                //TweetPanel.Visibility = System.Windows.Visibility.Visible;
                //txtUserName.Text = "Welcome " + userScreenName;
            });
        }

        private void MenuItemSignIn_Click(object sender, EventArgs e)
        {
            var requestTokenQuery = OAuthUtil.GetRequestTokenQuery();
            requestTokenQuery.RequestAsync(AppSettings.RequestTokenUri, null);
            requestTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(requestTokenQuery_QueryResponse);
        }

        void requestTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(e.Response);
                string strResponse = reader.ReadToEnd();
                var parameters = MainUtil.GetQueryParameters(strResponse);
                OAuthTokenKey = parameters["oauth_token"];
                tokenSecret = parameters["oauth_token_secret"];
                var authorizeUrl = AppSettings.AuthorizeUri + "?oauth_token=" + OAuthTokenKey;

                Dispatcher.BeginInvoke(() =>
                {
                    this.loginBrowserControl.Navigate(new Uri(authorizeUrl, UriKind.RelativeOrAbsolute));


                });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }

        void AccessTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(e.Response);
                string strResponse = reader.ReadToEnd();
                var parameters = MainUtil.GetQueryParameters(strResponse);
                accessToken = parameters["oauth_token"];
                accessTokenSecret = parameters["oauth_token_secret"];
                userID = parameters["user_id"];
                userScreenName = parameters["screen_name"];

                MainUtil.SetKeyValue<string>("AccessToken", accessToken);
                MainUtil.SetKeyValue<string>("AccessTokenSecret", accessTokenSecret);
                MainUtil.SetKeyValue<string>("ScreenName", userScreenName);

                userLoggedIn();

                if (userID != "" && userScreenName != "")
                {
                    var service = new TwitterService(consumerKey, consumerKeySecret);
                    service.AuthenticateWith(accessToken, accessTokenSecret);

                    service.ListTweetsOnHomeTimeline(new TweetSharp.ListTweetsOnHomeTimelineOptions() { Count = 20 }, (ts, rep) =>
                    {

                        if (rep.StatusCode == HttpStatusCode.OK)
                        {
                            
                            
                        }
                    });

                    

                }
            }

            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }


        private void loginBrowserControl_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            
            this.loginBrowserControl.Navigated -= loginBrowserControl_Navigated;
        }

        private void loginBrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString().StartsWith(AppSettings.CallbackUri))
            {
                var AuthorizeResult = MainUtil.GetQueryParameters(e.Uri.ToString());
                var VerifyPin = AuthorizeResult["oauth_verifier"];
                this.loginBrowserControl.Visibility = Visibility.Collapsed;
                AccessTokenQuery = OAuthUtil.GetAccessTokenQuery(OAuthTokenKey, tokenSecret, VerifyPin);
                AccessTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(AccessTokenQuery_QueryResponse);
                AccessTokenQuery.RequestAsync(AppSettings.AccessTokenUri, null);

            }
        }

       

         private void MenuItemSignOut_Click(object sender, EventArgs e)
        {
            
        }


         private void btnCalcle_Click(object sender, RoutedEventArgs e)
         {
             Deployment.Current.Dispatcher.BeginInvoke(() =>
             {
                 StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                 NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
             });
         }

         private void btnPost_Click(object sender, RoutedEventArgs e)
         {
             //MainUtil.SetKeyValue<string>("AccessToken", string.Empty);
             //MainUtil.SetKeyValue<string>("AccessTokenSecret", string.Empty);
             
             //Dispatcher.BeginInvoke(() =>
             //{
             //    var SignInMenuItem = (Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0];
             //    SignInMenuItem.IsEnabled = true;

             //    var SignOutMenuItem = (Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[1];
             //    SignOutMenuItem.IsEnabled = false;

             //    //TweetPanel.Visibility = System.Windows.Visibility.Collapsed;

             //    MessageBox.Show("You have been signed out successfully.");
             //});  


             var service = new TwitterService(consumerKey, consumerKeySecret);     
             SendTweetOptions tweetOptions = new SendTweetOptions();
             tweetOptions.Status = TxtPost.Text + " " + Txtlink.Text;
             try
             {
                 service.AuthenticateWith(accessToken, accessTokenSecret);
                 service.SendTweet(new SendTweetOptions { Status = TxtPost.Text + " " + Txtlink.Text }, (tweet, response) =>
                 {
                     if (response.StatusCode == HttpStatusCode.OK)
                     {
                         Deployment.Current.Dispatcher.BeginInvoke(() =>
                 {
                     MessageBox.Show("You have successfully tweeted the message!");

                     StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                     NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
                 });
                     }

                     else
                     {
                         Dispatcher.BeginInvoke(() =>
                {
                         MessageBox.Show(response.Error.Message.ToString());
                         StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                         NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
                });
                     }
                 });

                 


             }
             catch(Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
       
         }



    }
    }
