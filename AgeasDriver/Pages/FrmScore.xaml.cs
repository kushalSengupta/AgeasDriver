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
        private double avgScore = 8.7;
        private double yourScore = 5.7;

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
                ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                txtPoint.Text = "8.7";
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/upperStrip"));
                ImgMain.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/mainBg"));
                Imgprogress.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/progressBg"));
                ImgQuote.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/getaquoteButton"));
                ImgblueLine.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/blueLine"));
                ImggreyLine1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/greyRoad"));
                Imgbubble1.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/bubble"));
                txtBlue.Text = "";
                ImggreenLine.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/greenLine"));
                ImggreyLine2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/greyRoad"));
                Imgbubble2.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/ScorePage/bubble"));
                txtGreen.Text = "";
                txtFalg1.Text = "Great driving.Save up to 20% off \n comprehensive car insurance";
                txtFalg2.Text = "What does my score mean?";
                txtFalg3.Text = "Existing car customer? Give us \n at renewal to claim your discount";
            }
            catch { }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (txtYourscore.Text.Length > 0 && Convert.ToDouble(txtYourscore.Text) >= yourScore && txtAvgscore.Text.Length > 0 && Convert.ToDouble(txtAvgscore.Text) >= avgScore)
                {
                    timer.Stop();
                }
                else
                {
                    IntIncrease = IntIncrease + .1;
                    if (IntIncrease <= avgScore)
                    {
                        txtAvgscore.Text = Math.Round(IntIncrease / 10, 1).ToString();
                        ImggreenLine.Margin = new Thickness(ImggreenLine.Margin.Left + 1.5, 0, 0, 0);
                    }
                    if (IntIncrease <= yourScore)
                    {
                        txtYourscore.Text = Math.Round(IntIncrease / 10, 1).ToString();
                        ImgblueLine.Margin = new Thickness(ImgblueLine.Margin.Left + 1.5, 0, 0, 0);
                    }
                }
            }
            catch { }
        }

        #region ExitFromApp
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        #endregion
    }
}