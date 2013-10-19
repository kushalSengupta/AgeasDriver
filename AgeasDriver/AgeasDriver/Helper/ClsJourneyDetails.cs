using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AgeasDriver.Common;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Diagnostics;
using AgeasDriver.Pages;

namespace AgeasDriver.Helper
{
    class ClsJourneyDetails
    {


        
        private string envelope = "";
        string AvlJourneyData = "";
        private MainPage ObjMainPage;
        private FrmRegistration ObjFrmRegistration;

        public void GetJourneylist(String StrDeviceId, MainPage obj)
        {
            try
            {
                ObjMainPage = obj;
                envelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<device><deviceid>" + StrDeviceId.ToString() + "</deviceid></device>";
                Uri RequestJourneylistlUri = new Uri(ClsCommon.WebserviceLink + "?action=check_registered_device");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(RequestJourneylistlUri);
                req.Credentials = ClsCommon.cred;
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";
                req.BeginGetRequestStream(searchOnlineRequest, req);
                RequestJourneylistlUri = null;
            }
            catch
            {}
        }


        public void GetJourneylist(String StrDeviceId, FrmRegistration obj)
        {
            try
            {
                ObjFrmRegistration = obj;
                envelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<device><deviceid>" + StrDeviceId.ToString() + "</deviceid></device>";
                Uri RequestJourneylistlUri = new Uri(ClsCommon.WebserviceLink + "?action=check_registered_device");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(RequestJourneylistlUri);
                req.Credentials = ClsCommon.cred;
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";
                req.BeginGetRequestStream(searchOnlineRequest, req);
                RequestJourneylistlUri = null;
            }
            catch
            { }
        }

        private void searchOnlineRequest(IAsyncResult asyncResult)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            System.IO.Stream _body = request.EndGetRequestStream(asyncResult);

            byte[] formBytes = encoding.GetBytes(envelope);
            _body.Write(formBytes, 0, formBytes.Length);
            _body.Close();
            request.BeginGetResponse(SearchResponseCallback, request);
        }



        private void SearchResponseCallback(IAsyncResult asyncResult)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
                System.IO.Stream content = response.GetResponseStream();

                if (request != null && response != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(content))
                        {
                            string _responseString = reader.ReadToEnd();
                            reader.Close();
                            using (System.Xml.XmlReader Xmlreader = System.Xml.XmlReader.Create(new System.IO.StringReader(_responseString)))
                            {
                                while (Xmlreader.Read())
                                {
                                    if (Xmlreader.IsStartElement())
                                    {
                                        if (Xmlreader.Name == "responsexml")
                                        {
                                            Xmlreader.Read();
                                            AvlJourneyData = Xmlreader.Value;
                                            ParseJourneylistlJSonData(AvlJourneyData);
                                        }

                                    }
                                }
                            }
                        }


                    }
                }
            }
            catch (WebException  ex)
                
            {
                HttpStatusCode code = ((HttpWebResponse)(ex).Response).StatusCode;

                Debug.WriteLine("Errorr: " + code.ToString());
            }
        }


        private void ParseJourneylistlJSonData(string AllJourneyData)
        {
            try
            {
                JObject jObj = JObject.Parse(AllJourneyData);
                RootObjectJourneydetail objRootHistoryDetails = JsonConvert.DeserializeObject<RootObjectJourneydetail>(AllJourneyData);

                if (ObjFrmRegistration==null)
                {
                
                ObjMainPage.IsDeviceRegistered(objRootHistoryDetails);
                ObjMainPage = null;
                 jObj = null;
                }
                else
                {

                    ObjFrmRegistration.IsSkip(objRootHistoryDetails);
                    ObjFrmRegistration = null;
                    jObj = null;
                }
            }
            catch
            { }
        }

        }
}

public class Journeydetail
{
    public string journey_id { get; set; }
    public string date { get; set; }
    public string time { get; set; }
    public double distance { get; set; }
}

public class UserJourney
{
    public List<Journeydetail> journeydetails { get; set; }
}

public class RootObjectJourneydetail
{
    public Boolean complete { get; set; }
    public UserJourney user_journey { get; set; }
}