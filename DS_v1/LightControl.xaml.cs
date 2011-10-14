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
using System.Net;
using System.Windows.Threading;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json.Linq;



namespace DS_v1
{
    public partial class LightControl : PhoneApplicationPage
    {


        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
        
        public LightControl()
        {
            InitializeComponent();

            
            dt.Tick += new EventHandler(Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();
            
        }

       
        void Tick(object sender, EventArgs e)
        {
            try
            {
                var response = new WebClient();

                response.DownloadStringCompleted += (s, ea) =>
                {
                    JObject parsejson = JObject.Parse(ea.Result);
                    string st = parsejson["navLights"]["value"].ToString();

                    if (st == "False")
                    {
                        nl1.Fill = new SolidColorBrush(Colors.Transparent);
                        nl2.Fill = new SolidColorBrush(Colors.Transparent);
                       
                    }
                    else if (st == "True")
                    {
                        nl1.Fill = new SolidColorBrush(Colors.Green);
                        nl2.Fill = new SolidColorBrush(Colors.Red);                        
                    }                    


                    string under = parsejson["underwaterLights"]["value"].ToString();

                    if (under == "False")
                    {
                        uwl3.Fill = new SolidColorBrush(Colors.Transparent);
                        ruw1.Fill = new SolidColorBrush(Colors.Transparent);
                        ruwl2.Fill = new SolidColorBrush(Colors.Transparent);
                    }
                    else if (under == "True")
                    {
                        uwl3.Fill = new SolidColorBrush(Colors.White);
                        ruw1.Fill = new SolidColorBrush(Colors.White);
                        ruwl2.Fill = new SolidColorBrush(Colors.White);
                    }
                };

                response.DownloadStringAsync(new Uri(ip + "lightControlPage/statusAll"));
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

        private void btnon_Click(object sender, RoutedEventArgs e)
        {

            nl1.Fill = new SolidColorBrush(Colors.Green);
            nl2.Fill = new SolidColorBrush(Colors.Red);


            var uri = new Uri(ip + "lighting/navigationLights/true");
            IObservable<string> s = HttpGet(uri);
            
            HttpPost(uri, "true");

            IObservable<string> get = HttpGet(uri);
          

        }


        private void btnoff_Click(object sender, RoutedEventArgs e)
        {
            nl1.Fill = new SolidColorBrush(Colors.Transparent);
            nl2.Fill = new SolidColorBrush(Colors.Transparent);

            var uri = new Uri(ip + "lighting/navigationLights/false");
            IObservable<string> s = HttpGet(uri);
                     

        }

        private void btnuwlon_Click(object sender, RoutedEventArgs e)
        {
            ruwl2.Fill = new SolidColorBrush(Colors.White);
            ruw1.Fill = new SolidColorBrush(Colors.White);
            uwl3.Fill = new SolidColorBrush(Colors.White);

            var uri = new Uri(ip + "lighting/underwaterLights/true");
            IObservable<string> s = HttpGet(uri);


        }

        private void btnuwloff_Click(object sender, RoutedEventArgs e)
        {
            ruwl2.Fill = new SolidColorBrush(Colors.Transparent);
            ruw1.Fill = new SolidColorBrush(Colors.Transparent);
            uwl3.Fill = new SolidColorBrush(Colors.Transparent);

            var uri = new Uri(ip + "lighting/underwaterLights/false");
            IObservable<string> s = HttpGet(uri);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.dt.Stop();

            Uri theUri = new Uri("/MainPage.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
            
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
                
                
                //WebResponse myResponse = request.BeginGetResponse(new AsyncCallback(
               // ResponseHandler(
                
               // ResponseHandler(request, result, error);

                //response = ResponseHandler(response, result, error);

                 //ResponseHandler(response, result, error).Subscribe(response =>
                //{
                //    ResponseHandler(response, result, error);
                //}, onException);
                //System.Windows.MessageBox.Show(result.ToString());
                return result;
            }

            public System.Net.WebResponse response { get; set; }

            public IObservable<string> HttpPost(Uri uri, string postData)
            {
                return HttpPost(uri, postData, (s, e) => { }, (e) => e.PrepareForRethrow());
            }

            public IObservable<string> HttpPost(Uri uri, string postData,
                Action<HttpStatusCode, string> error)
            {
                return HttpPost(uri, postData, error, (e) => e.PrepareForRethrow());
            }


           
            HttpWebRequest Webrequest { get; set; }

            

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
                        writer.Write(postData);
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

            private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
            {
                dt.Stop();
            }

            private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
            {

            }
          
    }
        public enum MethodType
        {
            Get,
            Post
        }

    }

