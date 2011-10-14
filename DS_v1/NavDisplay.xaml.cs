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

namespace DS_v1
{
    public partial class NavDisplay : PhoneApplicationPage
    {
        public NavDisplay()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Uri theUri = new Uri("/MainPage.xaml", UriKind.Relative);
            NavigationService.Navigate(theUri);
        }
    }
}