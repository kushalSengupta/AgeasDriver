using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SampleTwitterApp
{
    public class AppSettings
    {
        // twitter
        public static string RequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public static string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public static string AccessTokenUri = "https://api.twitter.com/oauth/access_token";
        public static string CallbackUri = "http://text2insure.com/dev/driveapp/dev/apps/";//"https://ageas.co.uk";


        public static string consumerKey = "jfSgm58bVChUigLeYMdvQ";
        public static string consumerKeySecret = "99R9f0OmDx6Obqq6esOfr6D87OIuzea3mg67EJp0";

        //public static string consumerKey = "WiZYyxRFdxOVqW1QIKvsw";
        //public static string consumerKeySecret = "9c1H1HwDYnpt2pjsiHHLTVp2tQGxhiL0eEFlQTEttOA";

        public static string oAuthVersion = "1.1";
    }
}
