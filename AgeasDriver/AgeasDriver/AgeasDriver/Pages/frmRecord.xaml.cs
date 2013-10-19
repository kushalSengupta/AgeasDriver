using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AgeasDriver.Common;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Newtonsoft.Json;
using Windows.Devices.Geolocation;
using AgeasDriver.Helper;
using System.Device.Location;

namespace AgeasDriver.Pages
{
 
     
    public partial class frmRecord : PhoneApplicationPage
    {
        Boolean IsImageChange = false;
        DispatcherTimer timer;
        DispatcherTimer timerGPS;
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

        private Boolean IsFirst ;
        private int lastJourneyID;
        ClsSubmitJourneyLocation ObjSubmitJourneyLocation;
        private long _startTime;
        private TimeSpan runTime;
        public frmRecord()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                try
                {
                    StrCommonObject = NavigationContext.QueryString["common"];
                    //Waypoints = new List<GeoCoordinate>()
                    ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                }
                catch
                {
                    StrCommonObject = "";
                }

                Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/mainBg"));
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/upperStrip"));
                ImgProgressDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/progress"));
                ImgRecordSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/recordSelected"));
                ImgHelpDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/help"));
                ImgQuoteDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/quote"));

                ImgGreen.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/greenlight"));
                ImgRed.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/Redlight"));
                ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/startButton"));


                ImgLoader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/Progress_bg"));
                LoaderMsg.Text = "Oops, sorry your journey was less than 0.5 miles\n and not long enough to count. \n Please wait...";
                progressBar1.Visibility = Visibility.Collapsed;
                IsFirst = false;
                if (StrCommonObject != "")
                {
                   // JourneySave();

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

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
                
           

        }


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
        }

        public void gotoProgress()
        {


            var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
            int NoOfRecord = (from a in context.tblJourneyDetails select a.Last_distance).Count();



            if (NoOfRecord > 0)
            {
                ObjCommon.TotalJourney = Convert.ToDouble((from a in context.tblJourneyDetails select a.Last_distance).Sum());
                // Double DTotalJourney = Convert.ToDouble(LastDistIDQuery.Sum());
                //ObjCommon.TotalJourney = Convert.ToDouble(LastDistIDQuery);
            }
            else
            {
                if (ObjCommon != null)
                    ObjCommon.TotalJourney = 0;
            }


            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
            });

           
        }
        private void ProgressTab_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {


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
                 //StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
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
                            //StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                NavigationService.Navigate(new Uri("/Pages/FrmQuote.xaml?common=" + StrCommonObject, UriKind.Relative));
                             });
                
            }
            catch
            {
            }
        }

        private void ImgButton_Tab(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(!IsImageChange)
            {
                objDBHelper = new ClsDBHelper.ClsDBHelper();
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                int JourneyIDQuery = (from a in context.tblLocation select  a.Journeyid).Count();

                lastJourneyID = JourneyIDQuery + 1;

                IsFirst = true;
                IsImageChange = true;
                ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/stopButton"));
                ImgGreen.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/greenlightActive"));
                ImgRed.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/redlight"));
                cnvBackground.Visibility = Visibility.Visible;
                ImageAnimation();
                //IntervalForGpsTrack();
                App.RunningInBackground = true;
                App.Geolocator = new Geolocator();
                App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
                App.Geolocator.ReportInterval = (uint)TimeSpan.FromSeconds(22).TotalMilliseconds;
                //App.Geolocator.MovementThreshold = 1; // The units are meters.
               

                App.Geolocator.PositionChanged += geolocator_PositionChanged;
                
            }
            else
            {
                
                

                IsImageChange = false;
                ImgButton.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/startButton"));
                ImgGreen.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/greenlight"));
                ImgRed.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Record/RedlightActive"));
                timer.Stop();
               
                //timerGPS.Stop();
                App.RunningInBackground = false;
                cnvBackground.Visibility = Visibility.Collapsed;
                OfflineJourneySave();
            }

        }

       

        private void OfflineJourneySave()
        {
            string TotalRun = runTime.ToString(@"hh\:mm\:ss");
            Double Totaldistance = Getdistance(StartLatitude, StartLongitude, EndLatitude, EndLongitude);

           

            if (Totaldistance > 0.5)
            {
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                int JourneyIDQuery = (from a in context.tblJourneyDetails select a.JourneyId).Count();
                DataAccessLayer.JourneyDetails ObjJourney = new DataAccessLayer.JourneyDetails();
                if (JourneyIDQuery > 0)
                {

                    decimal LastDistIDQuery = (from a in context.tblJourneyDetails select a.Total_distance).Sum();
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
                ObjJourney.Last_distance = Convert.ToDecimal(Totaldistance);
                ObjJourney.TimeDuration = TotalRun;
                ObjJourney.JDate = System.DateTime.Now.ToString();
                objDBHelper.InsertJourney(ObjJourney);
                ObjJourney = null;

            }
            else
            {
                IntervalForwait();
                ImgLoader.Visibility = Visibility.Visible;
                LoaderMsg.Visibility = Visibility.Visible;
                progressBar1.Visibility = Visibility.Visible;
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                objDBHelper = new ClsDBHelper.ClsDBHelper();
                IQueryable<DataAccessLayer.LocationTable> DeleteLocation = from c in context.tblLocation where c.Journeyid == lastJourneyID select c;
                objDBHelper.DeleteLocation(DeleteLocation, context);
            }
        }

        private Double Getdistance(double sLatitude, double sLongitude, double eLatitude,
                               double eLongitude)
        {
            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(eLatitude, eLongitude);

            return sCoord.GetDistanceTo(eCoord);
        }
        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            App.Geolocator.PositionChanged -= geolocator_PositionChanged;
            App.Geolocator = null;
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
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);

                if (IsFirst)
                {
                    StartLatitude = args.Position.Coordinate.Latitude;
                    StartLongitude = args.Position.Coordinate.Longitude;
                    EndLatitude = args.Position.Coordinate.Latitude;
                    EndLongitude = args.Position.Coordinate.Longitude;
                    IsFirst = false;

                }
                else
                {
                    EndLatitude = args.Position.Coordinate.Latitude;
                    EndLongitude = args.Position.Coordinate.Longitude;
                }
                objDBHelper.InsertLocation(ObjLocationTbl);
            }

            catch { }

            ObjLocationTbl = null;
            if (!App.RunningInBackground)
            {

                Dispatcher.BeginInvoke(() =>
                {
                    ;
                });
            }
            else
            {

                Microsoft.Phone.Shell.ShellToast toast = new Microsoft.Phone.Shell.ShellToast();
                toast.Content = args.Position.Coordinate.Latitude.ToString("0.00") + "," + args.Position.Coordinate.Longitude.ToString("0.00");
                toast.Title = "Location: ";
                toast.NavigationUri = new Uri("/Page2.xaml", UriKind.Relative);
                toast.Show();

            }
        }

        private void IntervalForwait()
        {
            try
            {
                timerLoading = new DispatcherTimer();
                timerLoading.Interval = TimeSpan.FromMilliseconds(500);
                timerLoading.Tick += new EventHandler(WaitTrick);

                timerLoading.Start();


            }
            catch
            { }

        }
      


        private void IntervalForGpsTrack()
        {
             try
        {
            timerGPS = new DispatcherTimer();
            timerGPS.Interval = TimeSpan.FromMilliseconds(1000);
            timerGPS.Tick += new EventHandler(timerGPS_Tick);
            timerGPS.Start();
        }
        catch
        {}
          
    }


        private void WaitTrick(object sender, EventArgs e)
        {

            try
            {

                LoadIntervalCount +=1;
                if (LoadIntervalCount > 10)
                {
                    timerLoading.Stop();
                   
                        var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);

                        decimal LastDistIDQuery = (from a in context.tblJourneyDetails select a.Last_distance).Sum();
                        // Double DTotalJourney = Convert.ToDouble(LastDistIDQuery.Sum());
                        ObjCommon.TotalJourney = Convert.ToDouble(LastDistIDQuery);

                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                            NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
                        });
                   
                }
            }
            catch { }
        }


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
        {}
          
    }


 private void timerGPS_Tick(object sender, EventArgs e)
{
    //App.Geolocator = new Geolocator();
    //App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
    //App.Geolocator.MovementThreshold = 1; // The units are meters.
    //App.Geolocator.PositionChanged += geolocator_PositionChanged;

}

  private void timer_Tick(object sender, EventArgs e)
    {
        try
        {
            
               String StrImageName = "/Images/animationImage/AGEAS(" + ImageIndex.ToString() + ")";
                this.Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri(StrImageName));
                runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
                ImageIndex = ImageIndex+1;
               if (ImageIndex == 16)
               {
                   ImageIndex = 0;                 
               }
        }
        catch { }
    }

        
    }
}