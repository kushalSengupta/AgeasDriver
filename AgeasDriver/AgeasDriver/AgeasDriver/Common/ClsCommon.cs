using AgeasDriver.Helper;
using DataAccessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;


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
        public static string WebserviceLink = "http://dev3.indusnettechnologies.com/driveapp/dev/apps/"; //Local
       // public static string WebserviceLink = "http://text2insure.com/dev/driveapp/dev/apps/"; //Live 

        //public static string WebserviceLink = "http://173.0.140.35:5052/mobile/MobiTaxiWebService.asmx?";

        public static NetworkCredential cred = new NetworkCredential("dev3", "oss3");//new NetworkCredential("Administrator", "Passw0rD1!");
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        # region "Class Property"

        private Boolean RegisterStatus = false;
        private String DeviceUniqueId = "";
        private String _JourneyDetails = "";
        private String _JourneyListDetails = "";
        private Double _TotalJourney;
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
                if (appSettings.Contains("DeviceId"))
                {
                    DeviceUniqueId = (String)appSettings["DeviceId"];
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
             }
             catch { }
         }

         public static Uri GetScaledImageUri(String imageName)
         {
             int scaleFactor = (int)Application.Current.Host.Content.ScaleFactor;
             switch (scaleFactor)
             {
                 case 100: return new Uri(imageName + "_480.png", UriKind.RelativeOrAbsolute);
                 case 150: return new Uri(imageName + "_720.png", UriKind.RelativeOrAbsolute);
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
             //ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);

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
                 //JourneyIDQuery = (from a in context.tblJourneyDetails select a.JourneyId).Max();

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

             // ObjRootJouneyDtls.user_journey.journeydetails.
         }


         public bool IsRecordHaveForUpload()
         {
             ClsDBHelper.ClsDBHelper objDBHelper = new ClsDBHelper.ClsDBHelper();
             var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);

             List<LocationTable> JourneyLocationlist = (from a in context.tblLocation select a).ToList();

             if (JourneyLocationlist.Count > 0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
         }

         
        
   }
}
