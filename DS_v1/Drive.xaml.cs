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
using System.IO;
using Microsoft.Phone.Reactive;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Threading;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Devices.Sensors;



namespace DS_v1
{
    public partial class Drive : PhoneApplicationPage
    {
        Accelerometer _ac = new Accelerometer();

        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();

        public Drive()
        {
            InitializeComponent();

            dt.Tick += new EventHandler(Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();

           
        }

        //when using emulator
        //private const string ip = "http://localhost:8182/";

        //when using device
        // private const string ip = "http://192.168.0.26:8182/";

        //good for both
       private const string ip = "http://192.168.1.1:8182/";

        void Tick(object sender, EventArgs e)
        {
            try
            {
                var response = new WebClient();

                response.DownloadStringCompleted += (s, ea) =>
                {
                    JObject parsejson = JObject.Parse(ea.Result);
                    string st = parsejson["motor"]["value"].ToString();

                   //slider1.
                    string ruddervalue = parsejson["rudder"]["value"].ToString();
                    txtRudder.Text = ruddervalue;
                    txtspd.Text = "Speed \n" +st;

                    double v = double.Parse(ruddervalue);
                    if (v > 0)
                    {
                        soverz.Value = v;
                        sunderz.Value = 0;
                        sunderz.Foreground = new SolidColorBrush(Colors.White);

                    }
                    else if (v < 0)
                    {
                        sunderz.Value = v;
                        soverz.Value = 0;
                        sunderz.Foreground = new SolidColorBrush(Colors.White);

                    }
                    else
                    {
                        sunderz.Value = 0;
                        soverz.Value = 0;
                    }

                    //txtspeed.Text = "Speed: " + st + " Rudder: "+ ruddervalue;
                   
                };

                response.DownloadStringAsync(new Uri(ip + "motionControlPage/rudderAndSpeed"));
            }
            catch (Exception ex)
            {
               System.Windows.MessageBox.Show("Connection error. Please try again. " + ex.Message);
            }
        }


        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.dt.Stop();

            Uri theUri = new Uri("/MainPage.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
        }

        private void btndoubleup_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "motor/speed/100");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");

           
          
        }

        private void btnup_Click(object sender, RoutedEventArgs e)
        {

            var uri = new Uri(ip + "motor/increaseSpeed");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");

            
          
        }

        private void btndown_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "motor/decreaseSpeed");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");

          
        }

        private void btndoubledown_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "motor/speed/-100");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");

          
        }

        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "motor/speed/0");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");
        }


        private void btnright_Click(object sender, RoutedEventArgs e)
        {
            var response = new WebClient();

            response.DownloadStringCompleted += (s, ea) =>
            {
                JObject parsejson = JObject.Parse(ea.Result);
                soverz.Value = double.Parse(parsejson["rudder"]["value"].ToString());
                sunderz.Value = 0;
                txtRudder.Text = parsejson["rudder"]["value"].ToString();
            };           
            

            var uri = new Uri(ip + "rudder/rotateMore/true");
            IObservable<string> st = HttpGet(uri);

            HttpPost(uri, "");
        }

        private void btndoubleright_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "rudder/rotateFull/true");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");
        }

        private void btnleft_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "rudder/rotateMore/false");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");
        }

        private void btndoubleleft_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(ip + "rudder/rotateFull/false");
            IObservable<string> s = HttpGet(uri);

            HttpPost(uri, "");
        }

        












        public string ContentType { get; set; }

        public ICredentials Credentials { get; set; }

        public IObservable<string> HttpGet(Uri uri)
        {
            return HttpGet(uri, (s, e) => { });
        }

        public IObservable<string> HttpGet(Uri uri, Action<HttpStatusCode, string> error)
        {
            return HttpGet(uri, error, (e) => e.PrepareForRethrow());
        }

        public IObservable<string> HttpGet(Uri uri, Action<HttpStatusCode, string> error,
            Action<Exception> onException)
        {
            ValidateStandardParameters(uri, error, onException);
            var result = new Subject<string>();

            var request = CreateRequest(uri, MethodType.Get);
            var requestAsync = Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse);
            requestAsync();

            return result;
        }
       
        public IObservable<string> HttpPost(Uri uri, string postData)
        {
            return HttpPost(uri, postData, (s, e) => { }, (e) => e.PrepareForRethrow());
        }

        public IObservable<string> HttpPost(Uri uri, string postData,
            Action<HttpStatusCode, string> error)
        {
            return HttpPost(uri, postData, error, (e) => e.PrepareForRethrow());
        }


        public IObservable<string> HttpPost(Uri uri, string postData,
            Action<HttpStatusCode, string> error, Action<Exception> onException)
        {
            ValidateStandardParameters(uri, error, onException);
            var result = new Subject<string>();

            var request = CreateRequest(uri, MethodType.Post);
            var requestAsync = Observable.FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream);
            var responseAsync = Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse);

            requestAsync().SelectMany(requestStream =>
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    if (postData != "")
                    {
                        writer.Write(postData);
                    }
                }
                return responseAsync();
            });

            return result;
        }

        private static void ValidateStandardParameters(Uri uri, Action<HttpStatusCode, string> error, Action<Exception> onException)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            if (error == null)
            {
                throw new ArgumentNullException("completed");
            }
            if (onException == null)
            {
                throw new ArgumentNullException("error");
            }
        }

        private HttpWebRequest CreateRequest(Uri uri, MethodType methodType)
        {
            var request = WebRequest.CreateHttp(uri);
            request.Credentials = Credentials;

            switch (methodType)
            {
                case MethodType.Get:
                    request.Method = "GET";
                    break;
                case MethodType.Post:
                    request.ContentType = ContentType;
                    request.Method = "POST";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("methodType");
            }

            return request;
        }

        public enum MethodType
        {
            Get,
            Post
        }

    
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dt.Stop();
        }

       

       

        

      

        

        

        

      

        
    }
}
