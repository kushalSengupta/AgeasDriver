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
        String accessToken = null;
        string accessTokenSecret = string.Empty;
        string userID = string.Empty;
        string userScreenName = string.Empty;
        OAuthWebQuery AccessTokenQuery;
        //public static string consumerKey = "jfSgm58bVChUigLeYMdvQ";
        //public static string consumerKeySecret = "99R9f0OmDx6Obqq6esOfr6D87OIuzea3mg67EJp0";
        public static string consumerKey = "IDKvM9kiJyja0DII3Ahmw";
        public static string consumerKeySecret = "2Kf8e8BuaEKjtEiHGG4L5H75gOXwQNBiOZv1lGszZs";
        OAuthAccessToken token;

        public FrmTwiterePost()
        {
            InitializeComponent();
            this.loginBrowserControl.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            StrCommonObject = NavigationContext.QueryString["common"];

            ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
            base.OnNavigatedTo(e);
            TxtPost.Text = "I am using #AgeasDrive to try and \n save up to 20% on my comprehensive \n car insurance. You could too";
            Txtlink.Text = " https://www.ageas.co.uk/";
            
            if (isAlreadyLoggedIn())
            {
               // userLoggedIn();
                this.loginBrowserControl.Visibility = Visibility.Collapsed;
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
          
        }

        private OAuthAccessToken getAccessToken()
        {
            return null;
        }

        private Boolean isAlreadyLoggedIn()
        {
          
            accessTokenSecret = ObjCommon.TwitterAccessTokenSecret;
            accessToken = ObjCommon.Twittertoken;
            userScreenName = ObjCommon.TwitterScreenName;
           
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
                OAuthAccessToken token = new OAuthAccessToken();
                token.Token = ObjCommon.Twittertoken;
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

                ObjCommon.Twittertoken = parameters["oauth_token"];
                ObjCommon.TwitterAccessTokenSecret = parameters["oauth_token_secret"];
                ObjCommon.TwitterScreenName = parameters["screen_name"];
                ObjCommon.SaveSettings();
                userLoggedIn();
                if (userID != "" && userScreenName != "")
                {
                    var service = new TwitterService(consumerKey, consumerKeySecret);
                    service.AuthenticateWith(accessToken.ToString(), accessTokenSecret);

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

            var service = new TwitterService(consumerKey, consumerKeySecret);

            SendTweetOptions tweetOptions = new SendTweetOptions();
            tweetOptions.Status = TxtPost.Text + " " + Txtlink.Text;
            try
            {
                service.AuthenticateWith(ObjCommon.Twittertoken.ToString(), ObjCommon.TwitterAccessTokenSecret);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
    }
