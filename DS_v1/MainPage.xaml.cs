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

namespace DS_v1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnLightControl_Click(object sender, RoutedEventArgs e)
        {
            Uri theUri = new Uri("/LightControl.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
        }

        private void btnNavDisplay_Click(object sender, RoutedEventArgs e)
        {
            Uri theUri = new Uri("/NavDisplay.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
        }

        private void btnDrive_Click(object sender, RoutedEventArgs e)
        {
            Uri theUri = new Uri("/Drive.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
        }

       
    }
}