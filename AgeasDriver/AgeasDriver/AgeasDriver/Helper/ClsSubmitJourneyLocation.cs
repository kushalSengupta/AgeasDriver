using AgeasDriver.Common;
using AgeasDriver.Pages;
using DataAccessLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgeasDriver.Helper
{
    class ClsSubmitJourneyLocation
    {
        private string envelope = "";
        
        string AvlJourneyData = "";
        private FrmProgress ObjProgressPage;
        private frmRecord ObjRecordPage;

        private void CreteXmlBodyForLocPost(String StrDeviceId)
        {
            ClsDBHelper.ClsDBHelper objDBHelper = new ClsDBHelper.ClsDBHelper();
            var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
            //var MessageIDQuery = (from a in context.tblJourneyDetails  select a.JourneyId).ToList();


          //  List<JourneyDetails> Journeylist = ( from p in context.tblJourneyDetails
          //join c in context.tblLocation on p.JourneyId equals c.Journeyid
          //select p).ToList();


          // List<JourneyDetails> Journeylist = (from a in context.tblJourneyDetails  select a).ToList();

           List<LocationTable> Journeylist = (from a  in context.tblLocation select a).Distinct().ToList();
           // List<JourneyDetails> Journeylist = (from a in context.tblJourneyDetails where a.JourneyId cont select a).ToList();

           var distinctTypeIDs = (from a in context.tblLocation select a.Journeyid).Distinct().ToList();

           
            try
            {
                envelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                        " <journeydetails> " +
                            "<deviceid>" + StrDeviceId.ToString() + "</deviceid>\n";
                for (int j = 0; j < distinctTypeIDs.Count; j++)
                {
                    List<LocationTable> JourneyLocationlist = (from a in context.tblLocation where a.Journeyid == distinctTypeIDs[j] select a).ToList();

                    if (JourneyLocationlist.Count > 0)
                    {
                        envelope += "<journey id=\"" + JourneyLocationlist[j].Journeyid.ToString() + "\">";

                        for (int i = 0; i < JourneyLocationlist.Count; i++)
                        {
                            envelope += "<latlong> <latitude>" + JourneyLocationlist[i].Lat + "</latitude> <longitude>" + JourneyLocationlist[i].Long + "</longitude> <datetime>" +  JourneyLocationlist[i].DateTime+ "</datetime> </latlong>";

                        }

                        envelope += " </journey>";
                    }
                   
                }

                envelope += " </journeydetails>";
            }
            catch
            { }


        }

        public void PostJourneyLocationDetails(String StrDeviceId, FrmProgress obj)
        {
            try
            {

                ObjProgressPage = obj;
                CreteXmlBodyForLocPost(StrDeviceId);

                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    //envelope = "";


                    Uri RequestSubmitDetailsUri = new Uri(ClsCommon.WebserviceLink );


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

        public void PostJourneyLocationDetails(String StrDeviceId, frmRecord obj)
        {
            try
            {

                ObjRecordPage = obj;
                CreteXmlBodyForLocPost(StrDeviceId);

                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    //envelope = "";


                    Uri RequestSubmitDetailsUri = new Uri(ClsCommon.WebserviceLink);


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
                //ResponseSubmitDtls objRootHistoryDetails = JsonConvert.DeserializeObject<ResponseSubmitDtls>(AllJourneyData);
                RootObjectJourneydetail objRootHistoryDetails = JsonConvert.DeserializeObject<RootObjectJourneydetail>(AllJourneyData);



                if (ObjProgressPage != null)
                    ObjProgressPage.GetJourneyDetails(objRootHistoryDetails);
                else
                    ObjRecordPage.GetJourneyDetails(objRootHistoryDetails);
                //ObjMainPage.IsDeviceRegistered(ResponseSubmitDtls);
                //ObjMainPage = null;
                jObj = null;
            }
            catch
            { }
        }

    }
}

public class tempJourneylist
{
    public Int16 journey_id { get; set; }
}