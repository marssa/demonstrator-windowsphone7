using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using Newtonsoft.Json.Linq;


namespace DS_v1
{
    public partial class NavDisplay : PhoneApplicationPage
    {
        public NavDisplay()
        {
            InitializeComponent();

            dt.Tick += new EventHandler(Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();
                       
        
        }
        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();

        private static readonly GeoCoordinate DefaultLocation = new GeoCoordinate(47.639631, -122.127713);

       

        public GeoCoordinate Center;
         

        void Tick(object sender, EventArgs e)
        {
            try
            {
                var response = new WebClient();

                response.DownloadStringCompleted += (s, ea) =>
                {
                    JObject parsejson = JObject.Parse(ea.Result);
                    double lat = double.Parse( parsejson["latitude"]["DMS"]["value"].ToString());
                    double lon = double.Parse( parsejson["longitude"]["DMS"]["value"].ToString());

                   // double lat = 35.88923;
                   // double lon = 14.51721;

                    map.Center.Latitude = lat;
                    map.Center.Longitude = lon;

                    Pushpin mypin = new Pushpin();
                    mypin.Location = new GeoCoordinate(lat,lon);
                    map.Children.Add(mypin);


                };

                response.DownloadStringAsync(new Uri(ip + "gps/coordinates"));
            }
            catch (Exception ex)
            {
                var response = new WebClient();
                response.DownloadStringCompleted += (s, ea) =>
                {
                    System.Windows.MessageBox.Show(ea.Result);
                };

            }

        }

        //when using emulator
        //private const string ip = "http://localhost:8182/";

        //when using device
        // private const string ip = "http://192.168.2.71:8182/";

        //good for both
       private const string ip = "http://192.168.1.1:8182/";



        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Uri theUri = new Uri("/MainPage.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
        }
    }
}