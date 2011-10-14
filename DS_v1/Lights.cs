using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;

namespace DS_v1
{
    public class Lights
    {
        [DataMember(Name = "NavigationLights")]// mapping navigation lights
        public bool NavigationLights { get; set; }

        [DataMember(Name = "UnderwaterLights")]//mapping underwater lights
        public bool UnderwaterLights { get; set; }
    }
}
