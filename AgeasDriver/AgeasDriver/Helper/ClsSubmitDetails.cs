using System;
using AgeasDriver.Common;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Diagnostics;
using System.Net;
using AgeasDriver.Pages;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace AgeasDriver.Helper
{
    class ClsSubmitDetails
    {
        private string envelope = "";
        string AvlJourneyData = "";
        private FrmRegistration ObjMainPage;

        public void PostSubmitDetails(String StrDeviceId, String strFirstName, String LastName, String strEmail, FrmRegistration obj)
        {
            try
            {
                ObjMainPage = obj;
                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    envelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    " <user_details>" +
                    " <deviceid>" + StrDeviceId.ToString() + "</deviceid>" +
                    " <first_name>" + strFirstName.ToString() + "</first_name>" +
                    " <last_name>" + LastName.ToString() + "</last_name>" +
                    " <email>" + strEmail.ToString() + "</email>" +
                    " </user_details>";


                    //envelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?><r:user_details xmlns:r=\"http://www.namespace.org/user_details\"> <deviceid>XKVicElWiaSlb6v1ZVf9Bzdgo0</deviceid> <first_name>ttyyy</first_name> <last_name>ydgff</last_name> <email>d@ui.in</email> </r:user_details>";

                    
                    Uri RequestSubmitDetailsUri = new Uri(ClsCommon.WebserviceLink + "?action=register");


                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(RequestSubmitDetailsUri);
                    req.Credentials = ClsCommon.cred;
                    req.ContentType = "text/xml;charset=\"utf-8\"";
                    req.Accept = "text/xml";
                    req.Method = "POST";
                    
                    req.BeginGetRequestStream(searchOnlineRequest, req);
                    RequestSubmitDetailsUri = null;
                }

                else
                {
                    MessageBox.Show("Internet connection not available. Please try again later!");
                }
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
                                            ParseResonseJSonData(AvlJourneyData);
                                        }

                                    }
                                }
                            }
                        }


                    }
                }
            }
            catch (WebException ex)
            {
                HttpStatusCode code = ((HttpWebResponse)(ex).Response).StatusCode;

                Debug.WriteLine("Errorr: " + code.ToString());
            }
        }


        private void ParseResonseJSonData(string AllJourneyData)
        {
            try
            {
                JObject jObj = JObject.Parse(AllJourneyData);
                ResponseSubmitDtls objRootHistoryDetails = JsonConvert.DeserializeObject<ResponseSubmitDtls>(AllJourneyData);
                ObjMainPage.IsSubmitComplete(objRootHistoryDetails);
                ObjMainPage = null;
                jObj = null;
            }
            catch
            { }
        }

    }
}


public class ResponseSubmitDtls
{
    public string complete { get; set; }
}