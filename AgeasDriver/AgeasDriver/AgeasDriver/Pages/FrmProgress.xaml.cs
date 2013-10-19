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
using AgeasDriver.ClsDBHelper;
using AgeasDriver.Helper;
using System.Windows.Threading;
using DataAccessLayer;
using System.Windows.Media.Imaging;
using System.Windows.Media;


namespace AgeasDriver.Pages
{
    public partial class FrmProgress : PhoneApplicationPage
    {
        string StrCommonObject = "";
        ClsCommon ObjCommon;
        RootObjectJourneydetail ObjRootJouneyDtls;
       private double IntIncrease=0.5;
        private ClsDBHelper.ClsDBHelper objDBHelper;
        private DispatcherTimer timer;
        ClsSubmitJourneyLocation ObjSubmitJourneyLocation;
        Journeydetail jlist;
        public FrmProgress()
        {
            InitializeComponent();

            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                //if (ClsCommon.GetScaledSizeFactor() == 1)
                //    stackprogress.Margin = new Thickness(80, 0, 0, 25);
                //else if (ClsCommon.GetScaledSizeFactor() == 1.6)
                //    stackprogress.Margin = new Thickness(80, 0, 0, 25);
            }
            catch
            { }
        }

        
        private void RecordTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                            NavigationService.Navigate(new Uri("/Pages/frmRecord.xaml?common=" + StrCommonObject, UriKind.Relative));
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
                StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                NavigationService.Navigate(new Uri("/Pages/FrmHelp.xaml?common=" + StrCommonObject, UriKind.Relative));
            }
            catch
            {
            }

        }

        private void QuoteTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                NavigationService.Navigate(new Uri("/Pages/FrmQuote.xaml?common=" + StrCommonObject, UriKind.Relative));
            }
            catch
            {
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
           
                try
                {
                    StrCommonObject = NavigationContext.QueryString["common"];
                    //Waypoints = new List<GeoCoordinate>()
                    ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);


                

                base.OnNavigatedTo(e);
                //Imgbackground.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/graphpageBg"));
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/upperStrip"));
                ImgProgressSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/progressSelected"));
                ImgRecordDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/record"));
                ImgHelpDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/help"));
                ImgQuoteDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/General/quote"));
                ImgProgress.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/mainBg_progress1"));
                ImgProgressButtom.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/mainBg_progress2"));
                
                ObjCommon.TotalJourney = 70;
                if (ObjCommon != null)
                {
                    ObjRootJouneyDtls = JsonConvert.DeserializeObject<RootObjectJourneydetail>(ObjCommon.JourneyListDetails);
                    if (ObjCommon.TotalJourney < 51)
                    {
                        ImageBrush _ImageBrush = new ImageBrush();
                        _ImageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/displayUnselected"));
                        stackprogress.Background = _ImageBrush;
                        _ImageBrush = null;
                    }
                    else if (ObjCommon.TotalJourney < 100)
                    {
                        ImageBrush _ImageBrush = new ImageBrush();
                        _ImageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/display(50)"));
                        stackprogress.Background = _ImageBrush;
                        _ImageBrush = null;
                    }
                    else if (ObjCommon.TotalJourney < 150)
                    {
                        ImageBrush _ImageBrush = new ImageBrush();
                        _ImageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/display(100)"));
                        stackprogress.Background = _ImageBrush;
                        _ImageBrush = null;
                    }
                    else if (ObjCommon.TotalJourney >= 150)
                    {

                        ImageBrush _ImageBrush = new ImageBrush();
                        _ImageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/display(150)"));
                        stackprogress.Background = _ImageBrush;
                        _ImageBrush = null;
                        StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
                        NavigationService.Navigate(new Uri("/Pages/FrmScore.xaml?common=" + StrCommonObject, UriKind.Relative));
                    }
                }
                else
                {
                    ImageBrush _ImageBrush = new ImageBrush();
                    _ImageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/displayUnselected"));
                    stackprogress.Background = _ImageBrush;
                    _ImageBrush = null;
                }
                TxtBudget.Text = "LATEST BUDGET EARNED!";
                TxtBudgetDetails.Text = "Well done,you handle those \n comer safely";
                TxtShare.Text = "Share with:";
                TxtJourneyCmplt.Text = "JOURNEYS COMPLETED";

                Imgfacebk.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/facebookIcon"));
                Imgtwitter.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/twitterIcon"));

                ImgTblHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Progress/tableHeader"));

                objDBHelper = new ClsDBHelper.ClsDBHelper();
                var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString); 
               

                    List<JourneyDetails> Journelist = (from a in context.tblJourneyDetails select a).ToList();

                    List<Journeydetail> databaseJlist =new  List<Journeydetail>();
                    Journeydetail _Journey;
                    for(int i=0;i<Journelist.Count;i++)
                    {
                    _Journey=new Journeydetail();
                        _Journey.journey_id=Journelist[i].JourneyId.ToString();
                        _Journey.distance = Convert.ToDouble(Journelist[i].Last_distance);
                        _Journey.time=Journelist[i].TimeDuration.ToString();
                         _Journey.date=Journelist[i].JDate.ToString();
                         databaseJlist.Add(_Journey);
                         _Journey = null;
                    }
                    //ObjRootJouneyDtls.user_journey.journeydetails = databaseJlist;
                    lstjourneyList.ItemsSource = databaseJlist;

                    if (Journelist.Count>0)
                    {
                        TxtlatestdisValue.Text = Journelist[Journelist.Count - 1].Last_distance.ToString("0.00");

                    }
                

                }
                catch(Exception ex)
                {

                }
                
        }

       



        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ObjCommon.TotalJourney > IntIncrease)
                {
                    IntIncrease = IntIncrease + .1;
                    txtprogressvalue.Text = Math.Round(IntIncrease, 2).ToString();



                    if (Math.Round(IntIncrease, 2) % 1.0 == 0.0)
                   // double intcrement = 385.0 / 150.0;
                    ImgProgressIndicator.Margin = new Thickness(ImgProgressIndicator.Margin.Left +2, 0, 0, 0); 
                }
                else
                {
                    timer.Stop();
                }
            }
            catch { }
        }

        public void GotoMapView(RootObjectJourney JDetails)
        {

            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            timer.Stop();
                            progressBar1.Visibility = Visibility.Collapsed;
                            progressBar1.Visibility = Visibility.Collapsed;
                            ObjCommon.JourneyDetails = JsonConvert.SerializeObject(JDetails);
                            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
                            NavigationService.Navigate(new Uri("/Pages/FrmMap.xaml?common=" + strCommonObj, UriKind.Relative));
                        });
            }
            catch { }
        }

        public void GetJourneyDetails(RootObjectJourneydetail Journey)
        {
            ObjCommon.JourneyListDetails = JsonConvert.SerializeObject(Journey);
            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
            lstjourneyList.ItemsSource = ObjRootJouneyDtls.user_journey.journeydetails;
            if (jlist != null)
            {
                string number = jlist.journey_id;
                ClsJourney objJourney = new ClsJourney();
                objJourney.GetJourneyDetails(ObjCommon.DeviceId, jlist.journey_id, this);
                //NavigationService.Navigate(new Uri("/Pages/frmDetailMessage.xaml?ph_num=" + number +"&Contact_Header=" + mlist.ContactName, UriKind.Relative));
            }
        }

        private void lstSelectJourney(object sender, SelectionChangedEventArgs e)
        {
            ListBox cmd = (ListBox)sender;
            jlist = new Journeydetail();
            jlist = (Journeydetail)cmd.SelectedItem;
            cnvBackground.Visibility = Visibility.Visible;
            progressBar1.Visibility = Visibility.Visible;
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
            {
                progressBar1.Visibility = Visibility.Visible;
                if (ObjCommon.IsRecordHaveForUpload())
                {
                    ObjSubmitJourneyLocation = new ClsSubmitJourneyLocation();
                    ObjSubmitJourneyLocation.PostJourneyLocationDetails(ObjCommon.DeviceId, this);
                    var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);
                    objDBHelper = new ClsDBHelper.ClsDBHelper();
                    IQueryable<DataAccessLayer.LocationTable> DeleteLocation = from c in context.tblLocation select c;
                    objDBHelper.DeleteLocation(DeleteLocation, context);
                }
                else
                {
                    if (jlist != null)
                    {
                        string number = jlist.journey_id;
                        ClsJourney objJourney = new ClsJourney();
                        objJourney.GetJourneyDetails(ObjCommon.DeviceId, jlist.journey_id, this);
                        //NavigationService.Navigate(new Uri("/Pages/frmDetailMessage.xaml?ph_num=" + number +"&Contact_Header=" + mlist.ContactName, UriKind.Relative));
                    }
                }

              
            }
            else
            {
                MessageBox.Show("Internet connection not available. Please try again later!");
            }
            
           
        }

        private void click_facebook(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmFacebookPost.xaml?common=" + StrCommonObject, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/Pages/FrmFacebookPost.xaml", UriKind.Relative));
        }

       

        private void Click_Twitter(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StrCommonObject = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmTwiterePost.xaml?common=" + StrCommonObject, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/Pages/FrmFacebookPost.xaml", UriKind.Relative));
        }

        

        //private bool IsRecordHaveForUpload()
        //{
        //    ClsDBHelper.ClsDBHelper objDBHelper = new ClsDBHelper.ClsDBHelper();
        //    var context = new DataAccessLayer.AgeasDriverDetails(ClsDBHelper.ClsDBHelper.DBConnectionString);

        //    List<LocationTable> JourneyLocationlist = (from a in context.tblLocation select a).ToList();

        //    if (JourneyLocationlist.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        }
    }
