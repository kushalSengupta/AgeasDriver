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
using Microsoft.Phone.Tasks;

namespace AgeasDriver.Pages
{
    public partial class FrmHelp : PhoneApplicationPage
    {
        string StrCommonObject = "";
        ClsCommon ObjCommon;

        public FrmHelp()
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
            }
            catch
            {
                StrCommonObject = "";
            }
                base.OnNavigatedTo(e);
                Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Help/helpFullCloudBg"));
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/help/helpFeedbackTopStrip"));
                ImgProgressDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/progress"));
                ImgRecordDeSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/record"));
                ImgHelpSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/helpSelected"));
                ImgQuoteDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/quote"));
               
                TxtFirst.Text = "Got a suggestion,or would like to \n report a problem?";
                TxtSecond.Text = "We'd really appreciate hearing any \n thoughts you may have.";
                TxtThird.Text = "Use the button below to report an \n accident to us";

                ImgEmail.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Help/emailUs"));
                ImgAccident.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Help/reportAccident"));
         }
        

        private void ProgressTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + StrCommonObject, UriKind.Relative));
                });
            }
            catch
            {
            }
        }
        private void RecordTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Pages/FrmRecord.xaml?common=" + StrCommonObject, UriKind.Relative));
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



        private void Email_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask _EmailComposeTask = new EmailComposeTask();


            _EmailComposeTask.Subject = "Ageas Drive";
            _EmailComposeTask.To = "abc@gmail.com";
            _EmailComposeTask.Show();

            _EmailComposeTask = null;
        }

        
        private void AccidentCall_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneCallTask _PhoneCallTask = new PhoneCallTask();

            _PhoneCallTask.PhoneNumber = "111";


            _PhoneCallTask.Show();
        }
    }
}