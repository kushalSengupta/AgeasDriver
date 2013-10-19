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
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace SampleTwitterApp
{
    public class OAuthUtil
    {
        internal static OAuthWebQuery GetRequestTokenQuery()
        {
            var oauth = new OAuthWorkflow
            {
                ConsumerKey = AppSettings.consumerKey,
                ConsumerSecret = AppSettings.consumerKeySecret,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                RequestTokenUrl = AppSettings.RequestTokenUri,
                Version = AppSettings.oAuthVersion,
                CallbackUrl = AppSettings.CallbackUri
            };

            var info = oauth.BuildRequestTokenInfo(WebMethod.Get);
            var objOAuthWebQuery = new OAuthWebQuery(info, false);
            objOAuthWebQuery.HasElevatedPermissions = true;
            objOAuthWebQuery.SilverlightUserAgentHeader = "Hammock";            
            return objOAuthWebQuery;
        }

        internal static OAuthWebQuery GetAccessTokenQuery(string requestToken, string RequestTokenSecret, string oAuthVerificationPin)
        {
            var oauth = new OAuthWorkflow
            {
                AccessTokenUrl = AppSettings.AccessTokenUri,
                ConsumerKey = AppSettings.consumerKey,
                ConsumerSecret = AppSettings.consumerKeySecret,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Token = requestToken,
                Verifier = oAuthVerificationPin,
                Version = AppSettings.oAuthVersion
            };

            var info = oauth.BuildAccessTokenInfo(WebMethod.Post);
            OAuthWebQuery objOAuthWebQuery = new OAuthWebQuery(info, false);
            objOAuthWebQuery.HasElevatedPermissions = true;
            objOAuthWebQuery.SilverlightUserAgentHeader = "Hammock";
            return objOAuthWebQuery;             
        }

    }
}

