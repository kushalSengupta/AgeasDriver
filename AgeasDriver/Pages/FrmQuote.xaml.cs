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
    public partial class FrmQuote : PhoneApplicationPage
    {
        string StrCommonObject = "";
        ClsCommon ObjCommon;
        public FrmQuote()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
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
                base.OnNavigatedTo(e);
                Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Help/helpFullCloudBg"));
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Quote/getaquoteStrip"));
                ImgProgressDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/progress"));
                ImgRecordDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/record"));
                ImgHelpDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/help"));
                ImgQuoteSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/quoteSelected"));

                TxtFirst.Text = "Default 5 star comprehensive car \n insurance from £198";
                TxtSecond.Text = "That&apos;s what 10% of our customers paid.";
                TxtThird.Text = "If you want, get a car insurance quote now, but \nto get the Ageas Drive discount of up to 20%\n you'll need to keep going and complete those \n 150 miles";

                ImgBtnQuote.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Quote/getaquoteButton"));
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
        private void HelpTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Pages/FrmHelp.xaml?common=" + StrCommonObject, UriKind.Relative));
                });
            }
            catch
            {
            }
        }

        private void btn_Quote_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Under construction.");
        }

        
    }
}