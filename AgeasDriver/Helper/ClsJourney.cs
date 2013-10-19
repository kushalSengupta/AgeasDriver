using AgeasDriver.Common;
using AgeasDriver.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgeasDriver.Helper
{
    class ClsJourney
    {
        private string envelope = "";
        string AllJourneyData = "";

        private FrmProgress ObjFrmProgress;
        public void GetJourneyDetails(String StrDeviceId, string JourneyId, FrmProgress obj)
        {
            ObjFrmProgress = obj;
            try
            {
                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                   

                    envelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                   "<journeydetails><journey_id>"+ JourneyId.ToString() +"</journey_id><deviceid>"+ StrDeviceId.ToString() +"</deviceid></journeydetails>";
                    Uri RequestJourneyDetailslUri = new Uri(ClsCommon.WebserviceLink + "?action=get_points");
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(RequestJourneyDetailslUri);
                    req.Credentials = ClsCommon.cred; 
                    req.ContentType = "text/xml;charset=\"utf-8\"";
                    req.Accept = "text/xml";
                    req.Method = "POST";
                    req.BeginGetRequestStream(searchOnlineRequest, req);
                    RequestJourneyDetailslUri = null;


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
            
            // string ResultMessage = "";
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
                                            AllJourneyData = Xmlreader.Value;
                                            ParseJourneyDetailslJSonData(AllJourneyData);
                                        }

                                    }
                                }
                            }
                        }


                    }
                }
            }
            catch 
            { }
        }


        private void ParseJourneyDetailslJSonData(string AvlJourneyData)
        {
            try
            {
                JObject jObj = JObject.Parse(AvlJourneyData);

                RootObjectJourney objRootHistoryDetails = JsonConvert.DeserializeObject<RootObjectJourney>(AvlJourneyData);
               ObjFrmProgress.GotoMapView(objRootHistoryDetails);
            }
            catch 
            { }
        }

    }
}


public class Journey
{
    public string total_distance { get; set; }
    public string total_time { get; set; }
    public string max_velocity { get; set; }
    public string avg_velocity { get; set; }
    public string max_acceleration { get; set; }
    public double breaking { get; set; }
    public string date { get; set; }
}

public class Latlong
{
    public string latitude { get; set; }
    public string longitude { get; set; }
    public string datetime { get; set; }
}

public class Data
{
    public List<Latlong> latlong { get; set; }
}

public class Plot
{
    public double distance { get; set; }
    public double velocity { get; set; }
    public double acceleration { get; set; }
    public double time { get; set; }
    public string max_road_speed { get; set; }
}

public class Graph
{
    public List<Plot> plot { get; set; }
}

public class RootObjectJourney
{
    public Journey journey { get; set; }
    public Data data { get; set; }
    public Graph graph { get; set; }
    public string complete { get; set; }
}