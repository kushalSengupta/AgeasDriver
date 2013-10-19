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
        //PeriodicTask periodicTask;
        //Boolean agentsAreEnabled;
        
        string StrDeviceId="";
        ClsCommon objclsCommon;
        string strCommonObj;
       // string periodicTaskName = "PeriodicAgent";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            /*  string periodicTaskName = "PeriodicAgent";
           // Constructor
 
         
              // Sample code to localize the ApplicationBar
              //BuildLocalizedApplicationBar();
              agentsAreEnabled = true;
 
              // Obtain a reference to the period task, if one exists
              periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;
 
              if (periodicTask != null)
              {
                  RemoveAgent(periodicTaskName);
              }
 
              periodicTask = new PeriodicTask(periodicTaskName);
 
              // The description is required for periodic agents. This is the string that the user
              // will see in the background services Settings page on the device.
              periodicTask.Description = "This demonstrates a periodic task.";
 
              // Place the call to Add in a try block in case the user has disabled agents.
              try
              {
                  ScheduledActionService.Add(periodicTask);*/
 
                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
/*#if(DEBUG_AGENT)
                Microsoft.Phone.Scheduler.ScheduledActionService.LaunchForTest("MyTaskID", TimeSpan.FromSeconds(5));
                ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(1));
#endif*/
/*
#if DEBUG
                
                Microsoft.Phone.Scheduler.ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(5));
#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                    //agentsAreEnabled = false;
 
                }
 
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
 
                }
 
            }
            catch (SchedulerServiceException)
            {
                // No user action required.  
            }
 
 */
        
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



        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (objclsCommon.DeviceId!=null)
                {
                    byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                    StrDeviceId = Convert.ToBase64String(myDeviceID); ;


                    if (StrDeviceId.Length == 0)
                    {
                        StrDeviceId = "0";
                    }
                }
                
                if (!objclsCommon.RegStatus)
                {
                    
             //StrDeviceId = "8d5953f4cf10e8e291cab2c5a4959f224e06de70";
                 StrDeviceId = "X KVicElWiaSlb6v1ZVf9Bzdgo0";

                 // StrDeviceId = "123456789";
                    if (!IsolatedStorageSettings.ApplicationSettings.Contains("complete"))
                    {
                        IsolatedStorageSettings.ApplicationSettings.Add("complete", "False");
                        IsolatedStorageSettings.ApplicationSettings.Save();
                    }


                    objclsCommon.DeviceId = StrDeviceId;
                   

                   if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                    {
                        ClsJourneyDetails objClsJourneyDetails = new ClsJourneyDetails();
                        objClsJourneyDetails.GetJourneylist(objclsCommon.DeviceId, this);
                        objClsJourneyDetails = null;


                    }
                    else
                    {
                        MessageBox.Show("Internet connection not available. Please try again later!");

                        //objclsCommon.JourneyListDetails = null;
                        strCommonObj = JsonConvert.SerializeObject(objclsCommon);

                        if (IsolatedStorageSettings.ApplicationSettings["complete"] == "False")
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {

                                objclsCommon.DeviceId = StrDeviceId;
                                
                                objclsCommon.SaveSettings();
                                NavigationService.Navigate(new Uri("/Pages/FrmRegistration.xaml?FromSplash=true&common=" + strCommonObj, UriKind.Relative));
                            });
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {


                                NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + strCommonObj, UriKind.Relative));
                            });
                        }
                    }
                }
            }
            catch
            {
            } 
        }


        public void IsDeviceRegistered(RootObjectJourneydetail JDetails)
        {
            
            try
            {

                objclsCommon.JourneyListDetails = JsonConvert.SerializeObject(JDetails);

                

                 strCommonObj = JsonConvert.SerializeObject(objclsCommon);

            
                IsolatedStorageSettings.ApplicationSettings["complete"] = JDetails.complete;

                IsolatedStorageSettings.ApplicationSettings.Save();

                if (!JDetails.complete)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                           
                            objclsCommon.DeviceId = StrDeviceId;
                            objclsCommon.SaveSettings();
                            NavigationService.Navigate(new Uri("/Pages/FrmRegistration.xaml?FromSplash=true&common=" + strCommonObj, UriKind.Relative));
                        });
                    }
                
                else
                {
                    //DoorOpen.Begin();
                    //DoorOpen.Completed += new EventHandler(DoorOpen_Completed);
                     Deployment.Current.Dispatcher.BeginInvoke(() =>
            {


                NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + strCommonObj, UriKind.Relative));
            });
        
                    
                        }
           
        }
            catch(Exception ex)
            {
            }
            
        }

        void DoorOpen_Completed(object sender, EventArgs e)
        {
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {


                NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + strCommonObj, UriKind.Relative));
            });
        }

        
    }
}