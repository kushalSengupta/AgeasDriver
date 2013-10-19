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
using Newtonsoft.Json;

namespace AgeasDriver.Pages
{
    public partial class FrmDetails : PhoneApplicationPage
    {
        string StrCommonObject = "";
        ClsCommon ObjCommon;
        RootObjectJourney ObjRootObjectJourney;

        public FrmDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                StrCommonObject = NavigationContext.QueryString["common"];
                //Waypoints = new List<GeoCoordinate>()
                ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                ObjRootObjectJourney = JsonConvert.DeserializeObject<RootObjectJourney>(ObjCommon.JourneyDetails);
               
            }
            catch
            {
                StrCommonObject = "";
            }
            base.OnNavigatedTo(e);
            Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/DetailsBg"));
           // ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/DetailsBg"));


            ImgMapSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/map"));
            ImgGraphDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/graph"));
            ImgDetailsDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/detailsActive"));

            ImgTblR0C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR0C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));
            ImgTblR1C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR1C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));
            ImgTblR2C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR2C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));
            ImgTblR3C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR3C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));
            ImgTblR4C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR4C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));
            ImgTblR5C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR5C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));
            ImgTblR6C1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/leftTd"));
            ImgTblR6C2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Details/rightTd"));

            TxtR0C2.Text = ObjRootObjectJourney.journey.date.ToString();
            TxtR1C2.Text = ObjRootObjectJourney.journey.total_distance.ToString();
            TxtR2C2.Text = ObjRootObjectJourney.journey.total_time.ToString();
            TxtR3C2.Text = ObjRootObjectJourney.journey.max_velocity.ToString();
            TxtR4C2.Text = ObjRootObjectJourney.journey.avg_velocity.ToString();
            TxtR5C2.Text = ObjRootObjectJourney.journey.max_acceleration.ToString();
            TxtR6C2.Text = ObjRootObjectJourney.journey.breaking.ToString();



            //TxtFirst.Text = "Got a suggestion,or would like to \n report a problem?";
            //TxtSecond.Text = "We'd really appreciate hearing any \n thoughts you may have.";
            //TxtThird.Text = "Use the button below to report an \n accident to us";


        }


         #region Tapcontrol

        private void GraphTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
           // ObjCommon.JourneyDetails = JsonConvert.SerializeObject(JDetails);
           
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/frmGraph.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        private void MapTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmMap.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        private void BackToProgress(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + strCommonObj, UriKind.Relative));
        }

   
        #endregion

    }
}