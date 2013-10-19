using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


using Facebook;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Resources;
using AgeasDriver.Common;

namespace AgeasDriver.Pages
{
    public partial class FrmFacebookPost : PhoneApplicationPage
    {
       
        private const string FBApiID = "482270025187410";
        private const string ExtendedPermisions = "user_about_me,read_stream,publish_stream";
        private static FacebookClient client;
        string ClientAccesstoken = "";
        string StrCommonObject = "";
        ClsCommon ObjCommon;

        public FrmFacebookPost()
        {

            client = new FacebookClient();


           

            client.PostCompleted += (o, args) =>
            {

                if (args.Error != null)
                {
                    var error = args.Error as FacebookApiException;
                    if (error.ErrorCode == 190)
                    {
                        Dispatcher.BeginInvoke(() => MessageBox.Show("Authorization Error"));
                        SaveToken(null);
                        client.AccessToken = null;
                        return;
                    }
                    Dispatcher.BeginInvoke(() => MessageBox.Show(error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)args.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Message posted successfully");
                    Browser.Visibility = System.Windows.Visibility.Collapsed;
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                        NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
                    });
                });
            };

            if (GetToken() != null)
                client.AccessToken = GetToken();


           

            InitializeComponent();

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Dispatcher.BeginInvoke(() => { MessageBox.Show("No Internet Connection, Try again later"); });
                return;
            }

            //If we already have an access token, then just post, otherwise move to the login page
            if (client.AccessToken == null)
            {
                var parameters = new Dictionary<string, object>();
                parameters["client_id"] = FBApiID;
                parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
                parameters["response_type"] = "token";
                parameters["display"] = "touch";
                parameters["scope"] = ExtendedPermisions;
                Browser.Visibility = System.Windows.Visibility.Visible;
                string t = client.GetLoginUrl(parameters).AbsoluteUri;
                Browser.Navigate(client.GetLoginUrl(parameters));
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


       private void BrowserNavitaged(object sender, System.Windows.Navigation.NavigationEventArgs e)
       {
           FacebookOAuthResult oauthResult;
           if (!client.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
           {
               return;
           }
           if (oauthResult.IsSuccess)
           {
               //Process result
               client.AccessToken = oauthResult.AccessToken;
               ClientAccesstoken = client.AccessToken;
               SaveToken(client.AccessToken);
               LoginSucceded();
           }
           else
           {
               //Process Error
               MessageBox.Show(oauthResult.ErrorDescription);
               Browser.Visibility = System.Windows.Visibility.Collapsed;
           }
       }

       private void LoginSucceded()
       {
           Browser.Visibility = System.Windows.Visibility.Collapsed;
          // PostToWall();
       }

       private void PostToWall()
       {
         


           var parameters = new Dictionary<string, object>();
           parameters["message"] = "";
           parameters["description"] = TxtPost.Text + " " + Txtlink.Text;
           parameters["name"] = "Ageas Drive";
           parameters["picture"] = "http://www.freeimagehosting.net/newuploads/tiu2a.png";
           parameters["link"] = "https://ageas.co.uk";// new Uri(@"https://ageas.co.uk", UriKind.Absolute);
          // client.PostAsync(@"me/photos", parameters);
           client.PostAsync("me/feed", parameters);


           
       }
   
       private void SaveToken(String token)
       {
           //If it is the first save, create the key on ApplicationSettings and save the token, otherwise just modify the key
           if (!IsolatedStorageSettings.ApplicationSettings.Contains("token"))
               IsolatedStorageSettings.ApplicationSettings.Add("token", token);
           else
               IsolatedStorageSettings.ApplicationSettings["token"] = token;

           IsolatedStorageSettings.ApplicationSettings.Save();
       }

       private string GetToken()
       {
           //If there's no Token on memory, just return null, otherwise return the token as string
           if (!IsolatedStorageSettings.ApplicationSettings.Contains("token"))
               return null;
           else
               return IsolatedStorageSettings.ApplicationSettings["token"] as string;
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
           if (!NetworkInterface.GetIsNetworkAvailable())
           {
               Dispatcher.BeginInvoke(() => { MessageBox.Show("No Internet Connection, Try again later"); });
               return;
           }

           //If we already have an access token, then just post, otherwise move to the login page
           if (client.AccessToken == null)
           {
               var parameters = new Dictionary<string, object>();
               parameters["client_id"] = FBApiID;
               parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
               parameters["response_type"] = "token";
               parameters["display"] = "touch";
               parameters["scope"] = ExtendedPermisions;
               Browser.Visibility = System.Windows.Visibility.Visible;
               string t = client.GetLoginUrl(parameters).AbsoluteUri;
               Browser.Navigate(client.GetLoginUrl(parameters));
           }
           else
           {
               PostToWall();
           }
       }


    }
}