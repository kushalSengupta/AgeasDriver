using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using AgeasDriver.Common;
using System.Collections.ObjectModel;

namespace AgeasDriver.Pages
{
    public partial class FrmGraph : PhoneApplicationPage
    {

        string StrCommonObject = "";
        ClsCommon ObjCommon;
        RootObjectJourney ObjRootObjectJourney;
        public FrmGraph()
        {
            InitializeComponent();
        }

        private void MapTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmMap.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            try
            {
                StrCommonObject = NavigationContext.QueryString["common"];
                ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                ObjRootObjectJourney = JsonConvert.DeserializeObject<RootObjectJourney>(ObjCommon.JourneyDetails);
                base.OnNavigatedTo(e);
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Graph/graphTopStrip"));
                ImgBack.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/back"));
                ImgMapDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/map"));
                ImgDetailsDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/details"));
                ImgGrapSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/graphActive"));
                ImgAccelerationDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/acceleration"));
                ImgVelocitySelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/velocityActive"));
             }
            catch { }
        }

        private void DetailsTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmDetails.xaml?common=" + strCommonObj, UriKind.Relative));
        }
        private void VelocitySelect(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AddItem(1);
            ImgAccelerationDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/acceleration"));
            ImgVelocitySelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/velocityActive"));

        }
        private void AccelerationSelect(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AddItem(2);
            ImgAccelerationDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/accelerationActive"));
            ImgVelocitySelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/velocity"));
        }

        private void BackToProgress(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        private ObservableCollection<VelocityDataItem> _data = new ObservableCollection<VelocityDataItem>()
        {
                   
        };

        public ObservableCollection<VelocityDataItem> Data { get { return _data; } }

        private void Graph_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            AddItem(1);
        }

        private void AddItem(int flagGraph)
        {

            _data.Clear();
            if (flagGraph == 1)
            {
                for (int i = 0; i < ObjRootObjectJourney.graph.plot.Count - 1; i++)
                {
                    _data.Add(new VelocityDataItem()
                    {
                        cat1 = Math.Round(Convert.ToDecimal(ObjRootObjectJourney.graph.plot[i].distance), 2).ToString(),
                        val3 = Math.Round(Convert.ToDecimal(ObjRootObjectJourney.graph.plot[i].velocity), 2).ToString()
                    });
                }

            }
            else
            {
                for (int i = 0; i < ObjRootObjectJourney.graph.plot.Count - 1; i++)
                {
                    _data.Add(new VelocityDataItem()
       {
           cat1 = Math.Round(Convert.ToDecimal(ObjRootObjectJourney.graph.plot[i].distance), 2).ToString(),
           val3 = Math.Round(Convert.ToDecimal(ObjRootObjectJourney.graph.plot[i].acceleration), 2).ToString()
       });
                }
            }
        }
    }


    public class VelocityDataItem
    {
        public string cat1 { get; set; }
        public string val3 { get; set; }
    }
}