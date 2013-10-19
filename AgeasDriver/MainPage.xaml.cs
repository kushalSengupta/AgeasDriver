using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AgeasDriver.Resources;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Info;
using AgeasDriver.Common;
using AgeasDriver.Helper;
using Newtonsoft.Json;
using System.Device.Location;
using System.IO.IsolatedStorage;

namespace AgeasDriver
{

    public partial class MainPage : PhoneApplicationPage
    {
        string StrDeviceId = "";
        ClsCommon objclsCommon;
        string strCommonObj;       

        // Constructor
        public MainPage()
        {
            InitializeComponent();        
        }


        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                imgBackground.Height = Application.Current.Host.Content.ActualHeight;
                imgBackground.Width = Application.Current.Host.Content.ActualWidth;
                imgBackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Splash/SplashPage"));
                objclsCommon = new ClsCommon();
                objclsCommon.LoadSettings();
            }
            catch
            {

            }
        }

        public void GetJourneyDetails(RootObjectJourneydetail Journey)
        {
           
            objclsCommon.JourneyListDetails = JsonConvert.SerializeObject(Journey);
            strCommonObj = JsonConvert.SerializeObject(objclsCommon);
            objclsCommon.ClearLocationRecord();
            objclsCommon.JourneySave();
            NormalProcess();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            
                byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                StrDeviceId = Convert.ToBase64String(myDeviceID); ;
                objclsCommon.DeviceId = StrDeviceId;
                objclsCommon.DeleteLocationRecord();
                objclsCommon.DeviceId = "123466785"; //test
                objclsCommon.SaveSettings();
                objclsCommon.LoadSettings();
                if (objclsCommon.getIsAccessCode)
                {
                    cnvBackground.Visibility = Visibility.Collapsed;
                    txtAccessCode.Visibility = Visibility.Collapsed;
                    btnAccesscode.Visibility = Visibility.Collapsed;
                    if (objclsCommon.IsRecordHaveForUpload())
                    {
                        if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                        {
                            ClsSubmitJourneyLocation ObjSubmitJourneyLocation = new ClsSubmitJourneyLocation();
                            ObjSubmitJourneyLocation.PostJourneyLocationDetails(objclsCommon.DeviceId, this);
                        }
                        else
                        {
                            NormalProcess();
                        }
                    }
                    else
                    {
                        NormalProcess();
                    }
                }
                else
                {
                    cnvBackground.Visibility = Visibility.Visible;
                    txtAccessCode.Visibility = Visibility.Visible;
                    btnAccesscode.Visibility = Visibility.Visible;
                }
                
        }

        private void NormalProcess()
        {
             if (!objclsCommon.RegStatus)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            strCommonObj = JsonConvert.SerializeObject(objclsCommon);
                            NavigationService.Navigate(new Uri("/Pages/FrmRegistration.xaml?FromSplash=true&common=" + strCommonObj, UriKind.Relative));
                        });
                    }

                    else if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                    {
                        ClsJourneyDetails objClsJourneyDetails = new ClsJourneyDetails();
                        objClsJourneyDetails.GetJourneylist(objclsCommon.DeviceId, this);
                        objClsJourneyDetails = null;
                    }
                    else
                    {
                        MessageBox.Show("Internet connection not available. Please try again later!");

                        strCommonObj = JsonConvert.SerializeObject(objclsCommon);
                        if (objclsCommon.RegStatus)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                System.Threading.Thread.Sleep(2000);
                                NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + strCommonObj, UriKind.Relative));
                            });
                        }
                    }
               
        }

        public void IsDeviceRegistered(RootObjectJourneydetail JDetails)
        {
            try
            {
                objclsCommon.JourneyListDetails = JsonConvert.SerializeObject(JDetails);
                objclsCommon.SaveSettings();
                objclsCommon.LoadSettings();
                strCommonObj = JsonConvert.SerializeObject(objclsCommon);

                if (!JDetails.complete && !objclsCommon.RegStatus)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        objclsCommon.DeviceId = StrDeviceId;
                        objclsCommon.RegStatus = true;// JDetails.complete;
                        objclsCommon.SaveSettings();
                        NavigationService.Navigate(new Uri("/Pages/FrmRegistration.xaml?FromSplash=true&common=" + strCommonObj, UriKind.Relative));
                    });
                }

                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                   {

                       System.Threading.Thread.Sleep(2000);
                       NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + strCommonObj, UriKind.Relative));
                   });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAccessCode.Text == ClsCommon.AccessCode)
                {
                    cnvBackground.Visibility = Visibility.Collapsed;
                    txtAccessCode.Visibility = Visibility.Collapsed;
                    btnAccesscode.Visibility = Visibility.Collapsed;
                    objclsCommon.getIsAccessCode = true;
                    objclsCommon.SaveSettings();
                    if (objclsCommon.IsRecordHaveForUpload())
                    {
                        if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                        {
                            ClsSubmitJourneyLocation ObjSubmitJourneyLocation = new ClsSubmitJourneyLocation();
                            ObjSubmitJourneyLocation.PostJourneyLocationDetails(objclsCommon.DeviceId, this);
                        }
                        else
                        {
                            NormalProcess();
                        }
                    }
                    else
                    {
                        NormalProcess();
                    }

                }
                else
                {
                    MessageBox.Show("Please Enter proper Access code Otherwise you can't access this app!");
                    txtAccessCode.Text = "";
                }
            }
            catch
            {
            }
        }       


    }
}