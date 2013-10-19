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
using System.Windows.Threading;

namespace AgeasDriver.Pages
{
    public partial class FrmScore : PhoneApplicationPage
    {
        string StrCommonObject = "";
        ClsCommon ObjCommon;
        private DispatcherTimer timer;
        private double IntIncrease;
        public FrmScore()
        {
            InitializeComponent();


        }

        private void ScoreLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();

            }
            catch
            { }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            try
            {
                StrCommonObject = NavigationContext.QueryString["common"];
                //Waypoints = new List<GeoCoordinate>()
                ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                txtPoint.Text = "8.7";
            }
            catch { }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IntIncrease <100)
                {
                    IntIncrease = IntIncrease + .1;

                    if (IntIncrease / 10>8.7)
                    {
                        timer.Stop();
                        return;
                    }
                    txtYourscore.Text = Math.Round(IntIncrease/10, 1).ToString();
                    txtAvgscore.Text = Math.Round(IntIncrease / 10, 1).ToString();


                    if (Math.Round(IntIncrease, 2) % 1.0 == 0.0)
                        // double intcrement = 385.0 / 150.0;
                        ImgblueLine.Margin = new Thickness(ImgblueLine.Margin.Left + 2, 0, 0, 0);
                        ImggreenLine.Margin = new Thickness(ImgblueLine.Margin.Left + 2, 0, 0, 0);
                        //txtYourscore.Margin = new Thickness(txtYourscore.Margin.Left + 2, 0, 0, 0); 
                }
                else
                {
                    timer.Stop();
                }
            }
            catch { }
        }
    }
}