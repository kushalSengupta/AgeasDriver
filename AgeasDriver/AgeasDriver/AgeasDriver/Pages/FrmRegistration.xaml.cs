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
using AgeasDriver.Helper;
using System.Text.RegularExpressions;

namespace AgeasDriver.Pages
{
    public partial class FrmRegistration : PhoneApplicationPage
    {
        string FromSplash = "";
        string StrCommonObject = "";
        ClsCommon ObjCommon;
        public FrmRegistration()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                FromSplash = NavigationContext.QueryString["FromSplash"];
                StrCommonObject = NavigationContext.QueryString["common"];
                //Waypoints = new List<GeoCoordinate>()
                ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
            }
            catch 
            {
                FromSplash = "";
            }
            if (FromSplash == "true")
            {
                System.Threading.Thread.Sleep(2000);
                NavigationContext.QueryString["FromSplash"] = "";
            }
            
            base.OnNavigatedTo(e);
            txtHeader.Text = "Welcome to Ageas Driver ,we'd like to take a few contact details from u so we can email your progress while using the app.";
            txtFooter.Text = "Just so you know,we'll need some more personal details from you before we can provide you with a score and apply any discount you may have earned";
            Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Registration/fullCloudBg"));
            ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Registration/wellcomeStrip"));
            ImgBtnSubmit.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Registration/submitButton"));
            ImgBtnSkip.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Registration/skipButton"));
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //check the FirstName empty or not less than 2-
           

          
        }

        public static bool ValidateEmail(string str)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }



        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    ClsJourneyDetails objClsJourneyDetails = new ClsJourneyDetails();
                    objClsJourneyDetails.GetJourneylist(ObjCommon.DeviceId.ToString(), this);
                    objClsJourneyDetails = null;
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Internet connection not available. Please try again later!");
                        NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + StrCommonObject, UriKind.Relative));
                    });
                }
            }
            catch
            {
            }
        }

        public void IsSubmitComplete(RootObjectJourneydetail JDetails)
        {

            try
            {
                cnvBackground.Visibility = Visibility.Collapsed;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + StrCommonObject, UriKind.Relative));
                });
            }

            catch { }
        }

        public void IsSkip(RootObjectJourneydetail JDetails)
        {
            ObjCommon.JourneyListDetails = JsonConvert.SerializeObject(JDetails);
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + strCommonObj, UriKind.Relative));
                });
            }

            catch { }
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

        }



        private void submit(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (txtFirstname.Text != "" && txtFirstname.Text.Length > 2 && txtFirstname.Text.Length < 21)
            {
                //check the LastName empty or not less than 2
                if (txtLastname.Text != "" && txtLastname.Text.Length > 2 && txtLastname.Text.Length < 21)
                {
                    //validate email
                    if (ValidateEmail(txtEmailAddress.Text))
                    {

                        if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                        {
                            cnvBackground.Visibility = Visibility.Visible;
                            ClsSubmitDetails objClsSubmitDetails = new ClsSubmitDetails();
                            objClsSubmitDetails.PostSubmitDetails(ObjCommon.DeviceId.ToString(), txtFirstname.Text.ToString(), txtLastname.Text.ToString(), txtEmailAddress.Text.ToString(), this);
                            objClsSubmitDetails = null;
                        }
                        else
                        {
                            MessageBox.Show("Internet connection not available. Please try again later! Or you can skip it to press skip button");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Emailid!");
                    }
                }
                else
                {
                    MessageBox.Show("nvalid LastName!It should be number of character between 2 and 20.");
                }


            }

            else
            {
                MessageBox.Show("Invalid FirstName!It should be number of character between 2 and 20.");
            }
        }

    }
}