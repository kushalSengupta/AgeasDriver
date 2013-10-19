using AgeasDriver.Common;
using AgeasDriver.Helper;
using DataAccessLayer;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Devices.Geolocation;

namespace AgeasDriver.Pages
{


    public partial class frmRecord : PhoneApplicationPage
    {
        Boolean IsImageChange = false;
        DispatcherTimer timer;
        DispatcherTimer timerLoading;
        string StrCommonObject = "";
        int LoadIntervalCount = 0;
        int ImageIndex = 0;
        ClsCommon ObjCommon;
        ClsDBHelper.ClsDBHelper objDBHelper;
        DataAccessLayer.LocationTable ObjLocationTbl;

        private double StartLatitude;
        private double StartLongitude;
        private double EndLatitude;
        private double EndLongitude;

        private int lastJourneyID;
        private long _startTime;
        private TimeSpan runTime;
        private string m_Type = "";
        private Double Totaldistance;
        public frmRecord()
        {
            InitializeComponent();
        }

        #region NevigationControl
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                try
                {
                    StrCommonObject = NavigationContext.QueryString["common"];

                    ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                }
                catch
                {
                    StrCommonObject = "";
                }
                ObjCommon.LoadSettings();
                if (App.IsJourneyStartStage)
                {
                    ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/startButton"));
                }
                else
                {
                    ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/stopButton"));
                }

                Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/mainBg"));
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/upperStrip"));
                ImgProgressDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/progress"));
                ImgRecordSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/recordSelected"));
                ImgHelpDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/help"));
                ImgQuoteDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/quote"));

                ImgGreen.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/greenlight"));
                ImgRed.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/Redlight"));
                ImgLoader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/progressBg"));
                progressBar1.Visibility = Visibility.Collapsed;
                if (StrCommonObject != "")
                {
                    ObjCommon.JourneySave();
                }

            }
            catch
            {
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

        }

#endregion
        


        public void GetJourneyDetails(RootObjectJourneydetail Journey)
        {

            ObjCommon.JourneyListDetails = JsonConvert.SerializeObject(Journey);
            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
            var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
            context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
            objDBHelper = new ClsDBHelper.ClsDBHelper();
            IQueryable<DataAccessLayer.LocationTable> DeleteLocation = from c in context.tblLocation select c;
            objDBHelper.DeleteLocation(DeleteLocation, context);

            ObjCommon.JourneySave();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                progressBar1.Visibility = Visibility.Collapsed;
            });
        }


        #region TabControl
        public void gotoProgress()
        {
            var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
            int NoOfRecord = (from a in context.tblJourneyDetails select a.Last_distance).Count();
            ObjCommon.SaveSettings();  

            if (NoOfRecord > 0)
            {
                ObjCommon.TotalJourney = Convert.ToDouble((from a in context.tblJourneyDetails select a.Last_distance).Sum());
               

            }
            else
            {
                if (ObjCommon != null)
                    ObjCommon.TotalJourney = 0.0;
            }


            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                m_Type = "Progress";
                NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
            });


        }
        private void ProgressTab_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                ObjCommon.SaveSettings();  

                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    progressBar1.Visibility = Visibility.Visible;
                    if (ObjCommon.IsRecordHaveForUpload())
                    {
                        ClsSubmitJourneyLocation ObjSubmitJourneyLocation = new ClsSubmitJourneyLocation();
                        ObjSubmitJourneyLocation.PostJourneyLocationDetails(ObjCommon.DeviceId, this);
                    }
                    else
                    {
                        gotoProgress();
                    }
                }

                else
                {
                    gotoProgress();
                }
            }
            catch
            {
            }
        }

        private void HelpTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                 {
                     m_Type = "Help";
                     NavigationService.Navigate(new Uri("/Pages/FrmHelp.xaml?common=" + StrCommonObject, UriKind.Relative));
                 });
            }
            catch
            {
            }
        }

        private void QuoteTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Pages/FrmQuote.xaml?common=" + StrCommonObject, UriKind.Relative));
                });
            }
            catch
            {
            }
        }

        #endregion     

        private void OfflineJourneySave()
        {

            string TotalRun = runTime.ToString(@"hh\:mm\:ss");
            Totaldistance = ObjCommon.GetFirstLastLatLong(lastJourneyID);
            
            if (Totaldistance > 0.5)
            {
                LoaderMsg.Text = "Your Journey Is Recorded. \n Please wait...";
                ImgLoader.Visibility = Visibility.Visible;
                LoaderMsg.Visibility = Visibility.Visible;
                progressBar1.Visibility = Visibility.Visible;
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                int JourneyIDQuery = (from a in context.tblJourneyDetails select a.JourneyId).Count();
                DataAccessLayer.JourneyDetails ObjJourney = new DataAccessLayer.JourneyDetails();
                ObjJourney.Last_distance = Convert.ToDecimal(Totaldistance.ToString("0.00"));
                if (JourneyIDQuery > 0)
                {
                    decimal LastDistIDQuery = (from a in context.tblJourneyDetails select a.Last_distance).Sum();
                    double DLastDistIDQuery = Convert.ToDouble(LastDistIDQuery);
                    Totaldistance = Totaldistance + DLastDistIDQuery;
                    ObjJourney.Total_distance = Convert.ToDecimal(Totaldistance);
                }
                else
                {
                    ObjJourney.Total_distance = Convert.ToDecimal(Totaldistance);
                }
                lastJourneyID = JourneyIDQuery + 1;

                ObjJourney.JourneyId = lastJourneyID;

                ObjJourney.TimeDuration = TotalRun;
                ObjJourney.JDate = System.DateTime.Now.ToString("dd-MM-yyyy");
                objDBHelper.InsertJourney(ObjJourney);
                ObjJourney = null;
                IntervalForwait();
            }
            else
            {
                LoaderMsg.Text = "Oops, sorry your journey was less than 0.5 miles\n and not long enough to count. \n Please wait...";
                ImgLoader.Visibility = Visibility.Visible;
                LoaderMsg.Visibility = Visibility.Visible;
                progressBar1.Visibility = Visibility.Visible;
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                objDBHelper = new ClsDBHelper.ClsDBHelper();
                IQueryable<DataAccessLayer.LocationTable> DeleteLocation = from c in context.tblLocation where c.Journeyid == lastJourneyID select c;
                objDBHelper.DeleteLocation(DeleteLocation, context);
                IntervalForwait();
            }
        }


        #region GPSFunction
        private async void ImgButton_Tab(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (!IsImageChange)
                {
                    objDBHelper = new ClsDBHelper.ClsDBHelper();
                    var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                    int JourneyIDQuery = (from a in context.tblLocation select a.Journeyid).Count();
                    lastJourneyID = JourneyIDQuery + 1;
                    TxtGPS.Visibility = Visibility.Visible;
                    IsImageChange = true;
                    ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/stopButton"));
                    ImgGreen.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/greenlightActive"));
                    ImgRed.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/redlight"));
                    cnvBackground.Visibility = Visibility.Visible;
                    ImageAnimation();
                    App.IsJourneyStartStage = false;
                    App.objGeolocator = new Geolocator();
                    App.objGeolocator.DesiredAccuracy = PositionAccuracy.High;
                    App.objGeolocator.ReportInterval = 1000 * 22;
                    App.objGeolocator.PositionChanged += geolocator_PositionChanged;
                }
                else
                {
                    timer.Stop();
                    App.objGeolocator.PositionChanged -= geolocator_PositionChanged;
                    App.objGeolocator = null;
                    IsImageChange = false;
                    ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/startButton"));
                    ImgGreen.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/greenlight"));
                    App.IsJourneyStartStage = true;
                    cnvBackground.Visibility = Visibility.Collapsed;
                    OfflineJourneySave();
                }
            }
            catch
            { }
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            ObjLocationTbl = new DataAccessLayer.LocationTable();
            try
            {
                ObjLocationTbl.Journeyid = lastJourneyID;
                ObjLocationTbl.Lat = args.Position.Coordinate.Latitude.ToString();
                ObjLocationTbl.Long = args.Position.Coordinate.Longitude.ToString();
                ObjLocationTbl.DateTime = String.Format("{0:yyyy-MM-dd H:mm:ss}", System.DateTime.Now.ToLocalTime());
                objDBHelper.InsertLocation(ObjLocationTbl);
            }
            catch { }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                TxtGPS.Text = "GPS Active";
            });

            ObjLocationTbl = null;
        }

        #endregion

        #region ExitFromApp
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        #endregion

        private void IntervalForwait()
        {
            try
            {
                timerLoading = new DispatcherTimer();
                timerLoading.Interval = TimeSpan.FromMilliseconds(2000);
                timerLoading.Tick += new EventHandler(WaitTrick);

                timerLoading.Start();
            }
            catch
            { }
        }       

        private void WaitTrick(object sender, EventArgs e)
        {
            try
            {
                LoadIntervalCount += 1;
                timerLoading.Stop();
                double DLastDistIDQuery;
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                int JourneyIDQuery = (from a in context.tblJourneyDetails select a.JourneyId).Count();
                DataAccessLayer.JourneyDetails ObjJourney = new DataAccessLayer.JourneyDetails();
                if (JourneyIDQuery > 0)
                {
                    decimal LastDistIDQuery = (from a in context.tblJourneyDetails select a.Total_distance).Sum();
                    DLastDistIDQuery = Convert.ToDouble(LastDistIDQuery);
                    Totaldistance = Totaldistance + DLastDistIDQuery;
                    ObjJourney.Total_distance = Convert.ToDecimal(Totaldistance);
                }
                else
                {
                    ObjJourney.Total_distance = Convert.ToDecimal(Totaldistance);
                }

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                    NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
                });
            }
           
            catch { }
        }

        # region ImageAnimation
        private void ImageAnimation()
        {
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                _startTime = System.Environment.TickCount;
            }
            catch
            { }

        }
       
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                String StrImageName = "/Images/animationImage/AGEAS(" + ImageIndex.ToString() + ")_768.png";
                this.Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(StrImageName, UriKind.RelativeOrAbsolute));
                runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
                ImageIndex = ImageIndex + 1;
                if (ImageIndex == 16)
                {
                    ImageIndex = 0;
                }
            }
            catch { }
        }
# endregion

    }
}