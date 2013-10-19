using AgeasDriver.Helper;
using DataAccessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using TweetSharp;


namespace AgeasDriver.Common
{
    class ClsCommon
    {
        /*

         #define PARENT_URL @"http://10.0.1.101/driveapp/dev/apps/"
#define FACH_URL @"http://10.0.1.101/driveapp/dev/Apps/?action=get_points"
#define REGISTRATION_URL @"http://10.0.1.101/driveapp/dev/Apps/?action=register"
#define REGISTERDEVICE_URL @"http://10.0.1.101/driveapp/dev/Apps/?action=check_registered_device"

//dev
//#define PARENT_URL @"http://text2insure.com/dev/driveapp/dev/apps/"
//#define FACH_URL @"http://text2insure.com/dev/driveapp/dev/apps/?action=get_points"
         
//#define REGISTRATION_URL @"http://text2insure.com/dev/driveapp/dev/apps/?action=register"
//#define REGISTERDEVICE_URL @"http://text2insure.com/dev/driveapp/dev/apps/?action=check_registered_device"

        */
        public static string AccessCode = "12345";
        public static string WebserviceLink = "http://dev3.indusnettechnologies.com/driveapp/dev/apps/"; //Local
      //public static string WebserviceLink = "http://text2insure.com/dev/driveapp/dev/apps/"; //Live 

        //public static string WebserviceLink = "http://173.0.140.35:5052/mobile/MobiTaxiWebService.asmx?";

      public static NetworkCredential cred = new NetworkCredential("dev3", "oss3");//new NetworkCredential("Administrator", "Passw0rD1!");//
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        # region "Class Property"

        private Boolean RegisterStatus = false;
        private String DeviceUniqueId = "";
        private String _JourneyDetails = "";
        private String _JourneyListDetails = "";
        private Double _TotalJourney;
        private String FBtoken = "";
        private String TWtoken = null;
        private String TWAccessTokenSecret = null;
        private String TWScreenName = null;
        private Boolean IsAccessCode ;

        public Boolean RegStatus
        {
            get
            {
                return RegisterStatus;
            }
            set
            {
                RegisterStatus = value;
            }
        }


        public string JourneyDetails
        {
            get
            {
                return _JourneyDetails;
            }
            set
            {
                _JourneyDetails = value;
            }
        }

        public string JourneyListDetails
        {
            get
            {
                return _JourneyListDetails;
            }
            set
            {
                _JourneyListDetails = value;
            }
        }
        public String DeviceId
        {
            get
            {
                return DeviceUniqueId;
            }
            set
            {
                DeviceUniqueId = value;
            }
        }

        public Boolean getIsAccessCode
        {
            get
            {
                return IsAccessCode;
            }
            set
            {
                IsAccessCode = value;
            }
        }

        public String FaceBooktoken
        {
            get
            {
                return FBtoken;
            }
            set
            {
                FBtoken = value;
            }
        }

        public String Twittertoken
        {
            get
            {
                return TWtoken;
            }
            set
            {
                TWtoken = value;
            }
        }

        public String TwitterAccessTokenSecret
        {
            get
            {
                return TWAccessTokenSecret;
            }
            set
            {
                TWAccessTokenSecret = value;
            }
        }

        public String TwitterScreenName
        {
            get
            {
                return TWScreenName;
            }
            set
            {
                TWScreenName = value;
            }
        }

        public Double TotalJourney
        {
            get
            {
                return _TotalJourney;
            }
            set
            {
                _TotalJourney = value;
            }
        }

        

        #endregion

        


         public void LoadSettings()
        {
            try
            {
                if (appSettings.Contains("RegStatus"))
                {
                    RegisterStatus = (Boolean)appSettings["RegStatus"];
                 }
                else
                {
                    appSettings.Add("RegStatus",false);
                    appSettings.Save();
                    RegisterStatus = (Boolean)appSettings["RegStatus"];
                }

                if (appSettings.Contains("DeviceId"))
                {
                    DeviceUniqueId = (String)appSettings["DeviceId"];
                }
                else
                {
                    appSettings.Add("DeviceId", "0");
                    appSettings.Save();
                    DeviceUniqueId = (String)appSettings["DeviceId"];
                }

                 if (appSettings.Contains("FBToken"))
                {
                    FBtoken = (String)appSettings["FBToken"];
                }
                 else
                 {
                     appSettings.Add("FBToken", "");
                     appSettings.Save();
                     FBtoken = (String)appSettings["FBToken"];
                 }

                 if (appSettings.Contains("TWtoken"))
                 {
                     TWtoken = (String)appSettings["TWtoken"];
                 }
                 else
                 {
                     appSettings.Add("TWtoken", null);
                     appSettings.Save();
                     TWtoken = (String)appSettings["TWtoken"];
                 }

                 if (appSettings.Contains("TWAccessTokenSecret"))
                 {
                     TWAccessTokenSecret = (String)appSettings["TWAccessTokenSecret"];
                 }
                 else
                 {
                     appSettings.Add("TWAccessTokenSecret", "");
                     appSettings.Save();
                     TWAccessTokenSecret = (String)appSettings["TWAccessTokenSecret"];
                 }

                 if (appSettings.Contains("TWScreenName"))
                 {
                     TWAccessTokenSecret = (String)appSettings["TWScreenName"];
                 }
                 else
                 {
                     appSettings.Add("TWScreenName", "");
                     appSettings.Save();
                     TWAccessTokenSecret = (String)appSettings["TWScreenName"];
                 }

                 if (appSettings.Contains("IsAccessCode"))
                 {
                     IsAccessCode = (Boolean)appSettings["IsAccessCode"];
                 }
                 else
                 {
                     appSettings.Add("IsAccessCode", false);
                     appSettings.Save();
                     IsAccessCode = (Boolean)appSettings["IsAccessCode"];
                 }
                
            }
            catch { }
        }



         public void SaveSettings()
         {
             try
             {
                 appSettings["RegStatus"] = RegStatus;
                 appSettings["DeviceId"] = DeviceId;
                 appSettings["FBToken"] = FBtoken;
                 appSettings["TWtoken"] = TWtoken;
                 appSettings["TWAccessTokenSecret"] = TWAccessTokenSecret;
                 appSettings["TWScreenName"] = TWScreenName;
                 appSettings["IsAccessCode"] = IsAccessCode;
                 appSettings.Save();
             }
             catch { }
         }

         public static Uri GetScaledImageUri(String imageName)
         {
             int scaleFactor = (int)Application.Current.Host.Content.ScaleFactor;
             switch (scaleFactor)
             {
                 case 100: return new Uri(imageName + "_480.png", UriKind.RelativeOrAbsolute);
                 case 150: return new Uri(imageName + "_768.png", UriKind.RelativeOrAbsolute);
                 case 160: return new Uri(imageName + "_768.png", UriKind.RelativeOrAbsolute);
                 default: throw new InvalidOperationException("Unknown resolution type");
             }
         }
         public static double GetScaledSizeFactor()
         {
             int scaleFactor = (int)Application.Current.Host.Content.ScaleFactor;
             switch (scaleFactor)
             {
                 case 100: return 1;
                 case 150: return 1.5;
                 case 160: return 1.6;
                 default: throw new InvalidOperationException("Unknown resolution type");
             }
         }


         public void JourneySave()
         {
             RootObjectJourneydetail ObjRootJouneyDtls = JsonConvert.DeserializeObject<RootObjectJourneydetail>(this.JourneyListDetails);
             if (ObjRootJouneyDtls.complete)
             {
                 ClsDBHelper.ClsDBHelper objDBHelper;
                 decimal Totaldistance = 0;
                 var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                 objDBHelper = new ClsDBHelper.ClsDBHelper();
                 IQueryable<DataAccessLayer.JourneyDetails> DeleteJourney = from c in context.tblJourneyDetails select c;
                 objDBHelper.DeleteJourneyDetails(DeleteJourney, context);
                 DataAccessLayer.JourneyDetails ObjJourney;
                 for (int i = 0; i < ObjRootJouneyDtls.user_journey.journeydetails.Count; i++)
                 {
                     ObjJourney = new DataAccessLayer.JourneyDetails();
                     Totaldistance = Totaldistance + Convert.ToDecimal(ObjRootJouneyDtls.user_journey.journeydetails[i].distance);
                     ObjJourney.Total_distance = Convert.ToDecimal(Totaldistance);
                     ObjJourney.JourneyId = Convert.ToInt16(ObjRootJouneyDtls.user_journey.journeydetails[i].journey_id);
                     ObjJourney.Last_distance = Convert.ToDecimal(ObjRootJouneyDtls.user_journey.journeydetails[i].distance);
                     ObjJourney.TimeDuration = ObjRootJouneyDtls.user_journey.journeydetails[i].time;
                     ObjJourney.JDate = ObjRootJouneyDtls.user_journey.journeydetails[i].date.ToString();
                     objDBHelper.InsertJourney(ObjJourney);
                     ObjJourney = null;
                 }
             }             
         }


         public bool IsRecordHaveForUpload()
         {
             ClsDBHelper.ClsDBHelper objDBHelper = new ClsDBHelper.ClsDBHelper();
             var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
             List<LocationTable> JourneyLocationlist = (from a in context.tblLocation select a).ToList();
             context = null;
             objDBHelper = null;
             if (JourneyLocationlist.Count > 0)
             {
                 JourneyLocationlist = null;
                 return true;
             }
             else
             {
                 JourneyLocationlist = null;
                 return false;
             }
         }

         public void ClearLocationRecord()
         {
             var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
             ClsDBHelper.ClsDBHelper objDBHelper = new ClsDBHelper.ClsDBHelper();
             IQueryable<DataAccessLayer.LocationTable> DeleteLocation = from c in context.tblLocation select c;
             objDBHelper.DeleteLocation(DeleteLocation, context);
             context = null;
             objDBHelper = null;
         }
         public void DeleteLocationRecord()
         {
             ClsDBHelper.ClsDBHelper objDBHelper = new ClsDBHelper.ClsDBHelper();
             var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
             Double Totaldistance;
             var distinctTypeIDs = (from a in context.tblLocation select a.Journeyid).Distinct().ToList();
             for (int j = 0; j < distinctTypeIDs.Count; j++)
             {
                 Totaldistance = GetFirstLastLatLong(distinctTypeIDs[j]);
                 if (Totaldistance <= 0.5)
                 {
                     IQueryable<DataAccessLayer.LocationTable> DeleteLocation = from c in context.tblLocation where c.Journeyid == distinctTypeIDs[j] select c;
                     objDBHelper.DeleteLocation(DeleteLocation, context);
                 }
             }
             context = null;
             objDBHelper = null;
         }

         public Double GetFirstLastLatLong(int lastJourneyID)
         {
             double StartLatitude;
             double StartLongitude;
             double EndLatitude;
             double EndLongitude;
             Double Totaldistance;
             var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
             List<LocationTable> JourneyLocationlist = (from a in context.tblLocation orderby a.LocationID ascending where a.Journeyid == lastJourneyID select a).ToList();
             if (JourneyLocationlist.Count > 0)
             {
                 StartLatitude = Convert.ToDouble(JourneyLocationlist[0].Lat.ToString());
                 StartLongitude = Convert.ToDouble(JourneyLocationlist[0].Long.ToString());
             }
             else
             {
                 StartLatitude = Convert.ToDouble("0.00");
                 StartLongitude = Convert.ToDouble("0.00");
             }
             JourneyLocationlist = null;
             JourneyLocationlist = (from a in context.tblLocation orderby a.LocationID descending where a.Journeyid == lastJourneyID select a).ToList();
             if (JourneyLocationlist.Count > 0)
             {
                 EndLatitude = Convert.ToDouble(JourneyLocationlist[0].Lat.ToString());
                 EndLongitude = Convert.ToDouble(JourneyLocationlist[0].Long.ToString());
             }
             else
             {
                 EndLatitude = Convert.ToDouble("0.00");
                 EndLongitude = Convert.ToDouble("0.00");
             }
             Totaldistance = Getdistance(StartLatitude, StartLongitude, EndLatitude, EndLongitude) / 1609.34;
             return Totaldistance;
         }

         private Double Getdistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
         {
             var sCoord = new GeoCoordinate(sLatitude, sLongitude);
             var eCoord = new GeoCoordinate(eLatitude, eLongitude);
             return (sCoord.GetDistanceTo(eCoord));
         }
        
   }
}
