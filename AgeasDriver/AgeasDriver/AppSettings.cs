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
        public static string CallbackUri = "https://www.postorio.com";


        public static string consumerKey = "WiZYyxRFdxOVqW1QIKvsw";
        public static string consumerKeySecret = "9c1H1HwDYnpt2pjsiHHLTVp2tQGxhiL0eEFlQTEttOA";

        public static string oAuthVersion = "1.1";
    }
}
