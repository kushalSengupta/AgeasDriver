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
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Windows.Media;
using OsmSharp.Osm;

using System.Windows.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace AgeasDriver.Pages
{
    public partial class FrmMap : PhoneApplicationPage
    {
        private int ZoomLevel = 15;
        string StrCommonObject = "";
        ClsCommon ObjCommon;
        RootObjectJourney ObjRootObjectJourney;
        private GeoCoordinate geo1;
        private GeoCoordinate geo2;
        private MapRoute mapRoute;
        RouteQuery query;
        
        MapRotationGesture getureR;
        public FrmMap()
        {
            InitializeComponent();
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {            
            AddPinOnMap();
            DrawLinePathOnMap();
            getureR = new MapRotationGesture(mapJourney,true);
            getureR.SuppressMapGestures = true;
            getureR.touch = true;
            getureR.test();
        }


        #region ExitFromApp
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            getureR.touch = false;
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        #endregion

        private void AddPinOnMap()
        {
            geo1 = new GeoCoordinate(Convert.ToDouble(ObjRootObjectJourney.data.latlong[0].latitude), Convert.ToDouble(ObjRootObjectJourney.data.latlong[0].longitude));
            geo2 = new GeoCoordinate(Convert.ToDouble(ObjRootObjectJourney.data.latlong[ObjRootObjectJourney.data.latlong.Count - 1].latitude), Convert.ToDouble(ObjRootObjectJourney.data.latlong[ObjRootObjectJourney.data.latlong.Count - 1].longitude));
            Image pinIMG = new Image();
            pinIMG.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/map/pin_green.png", UriKind.Relative));
            pinIMG.Width = 50;
            pinIMG.Height = 50;
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = pinIMG;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = geo1;
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            mapJourney.Layers.Add(myLocationLayer);
            myLocationLayer = null;
            myLocationOverlay = null;
            pinIMG = null;
            pinIMG = new Image();
            pinIMG.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/map/pin_red.png", UriKind.Relative));
            pinIMG.Width = 50;
            pinIMG.Height = 50;
            myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = pinIMG;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = geo2;
            myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            mapJourney.Layers.Add(myLocationLayer);
            myLocationLayer = null;
            myLocationOverlay = null;
            pinIMG = null;
            mapJourney.ZoomLevel = ZoomLevel;
            mapJourney.Center = geo2;
        }

        private void DrawLinePathOnMap()
        {
            for (int i = 0; i < ObjRootObjectJourney.data.latlong.Count - 1; i++)
            {
                geo1 = new GeoCoordinate(Convert.ToDouble(ObjRootObjectJourney.data.latlong[i].latitude), Convert.ToDouble(ObjRootObjectJourney.data.latlong[i].longitude));
                geo2 = new GeoCoordinate(Convert.ToDouble(ObjRootObjectJourney.data.latlong[i + 1].latitude), Convert.ToDouble(ObjRootObjectJourney.data.latlong[i + 1].longitude));
                query = new RouteQuery()
               {
                   TravelMode = TravelMode.Driving,
                   Waypoints = new List<GeoCoordinate>()
                     {
                         geo1,
                         geo2 
                     }
               };
                query.QueryCompleted += routeQuery_QueryCompleted;
                query.QueryAsync();
            }
            progressBar1.Visibility = Visibility.Collapsed;
        }

        void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            try
            {
                mapRoute = new MapRoute(e.Result);
                mapRoute.Color = System.Windows.Media.Color.FromArgb(255, 187, 18, 161);
                mapJourney.AddRoute(mapRoute);
                query.QueryCompleted -= routeQuery_QueryCompleted;
            }
            catch { }           
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //mapRoute = null;
            //mapJourney = null;
            //query = null;
            //geo1 = null;
            //geo2 = null;
            getureR = null;
            getureR = new MapRotationGesture(mapJourney, false);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                StrCommonObject = NavigationContext.QueryString["common"];
                ObjCommon = JsonConvert.DeserializeObject<ClsCommon>(StrCommonObject);
                ObjRootObjectJourney = JsonConvert.DeserializeObject<RootObjectJourney>(ObjCommon.JourneyDetails);
                base.OnNavigatedTo(e);
                ImgBack.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/back"));
                ImgHeader.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/Map/mapTopStrip"));
                ImgMapSelect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/mapActive"));
                ImgGrapDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/graph"));
                ImgDetailsDeselect.Source = new System.Windows.Media.Imaging.BitmapImage(ClsCommon.GetScaledImageUri("/Images/InnerTab/details"));
               
            }
            catch { }
        }
        #region Tapcontrol

        private void GraphTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            getureR.touch = false;
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/frmGraph.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        private void DetailsTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            getureR.touch = false;
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmDetails.xaml?common=" + strCommonObj, UriKind.Relative));
        }

        private void BackToProgress(object sender, System.Windows.Input.GestureEventArgs e)
        {
            getureR.touch = false;
            string strCommonObj = JsonConvert.SerializeObject(ObjCommon);
            NavigationService.Navigate(new Uri("/Pages/FrmProgress.xaml?common=" + strCommonObj, UriKind.Relative));
        }

    }
        #endregion

    #region  MapGesture
    public class MapGestureBase
    {
        /// <summary>
        /// Gets or sets whether to suppress the existing gestures/
        /// </summary>
        public bool SuppressMapGestures { get; set; }

        protected Map Map { get; private set; }

        public MapGestureBase(Map map)
        {
            Map = map;
            map.Loaded += (s, e) => CrawlTree(Map);
        }

        private void CrawlTree(FrameworkElement el)
        {
            el.ManipulationDelta += MapElement_ManipulationDelta;
            for (int c = 0; c < VisualTreeHelper.GetChildrenCount(el); c++)
            {
                CrawlTree(VisualTreeHelper.GetChild(el, c) as FrameworkElement);
            }
        }

     
        private void MapElement_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (SuppressMapGestures)
                e.Handled = true;
        }
    }

    public class MapRotationGesture : MapGestureBase
    {
        /// <summary>
        /// Gets or sets the minimuum rotation that the user must apply in order to initiate this gesture.
        /// </summary>
        public double MinimumRotation { get; set; }

        private double? _previousAngle;

        private bool _isRotating;
        public bool touch { get; set; }
        public MapRotationGesture(Map map,bool flag)
            : base(map)
        {
            touch = flag;
            MinimumRotation = 10.0;
        }

        public void test()
        {
            if (touch)
            {
                Touch.FrameReported += Touch_FrameReported;
            }
        }
        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            if (touch)
            {
                var touchPoints = e.GetTouchPoints(Map);

                if (touchPoints.Count == 2)
                {
                    // for the initial touch, record the angle between the fingers
                    if (!_previousAngle.HasValue)
                    {
                        _previousAngle = AngleBetweenPoints(touchPoints[0], touchPoints[1]);
                    }

                    // should we rotate?
                    if (!_isRotating)
                    {
                        double angle = AngleBetweenPoints(touchPoints[0], touchPoints[1]);
                        double delta = angle - _previousAngle.Value;
                        if (Math.Abs(delta) > MinimumRotation)
                        {
                            _isRotating = true;
                            SuppressMapGestures = true;
                        }
                    }

                    // rotate me
                    if (_isRotating)
                    {
                        double angle = AngleBetweenPoints(touchPoints[0], touchPoints[1]);
                        double delta = angle - _previousAngle.Value;
                        Map.Heading -= delta;
                        _previousAngle = angle;
                    }
                }
                else
                {
                    _previousAngle = null;
                    _isRotating = false;
                    SuppressMapGestures = false;
                }
            }
        }

        private double AngleBetweenPoints(TouchPoint p1, TouchPoint p2)
        {
            return Math.Atan2(p1.Position.Y - p2.Position.Y, p1.Position.X - p2.Position.X)
                    * (180 / Math.PI);
        }
    #endregion



    }

}