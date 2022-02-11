using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IpSubnetCalc
{
    /// <summary>
    /// Interaction logic for rosszIp.xaml
    /// </summary>
    public partial class rosszIp : Window
    {
        public rosszIp()
        {
            InitializeComponent();
            BitmapImage hibaBitmapKep = new BitmapImage();
            hibaBitmapKep.BeginInit();
            string startPath = Environment.CurrentDirectory;
            hibaBitmapKep.UriSource = new Uri(startPath+"/Images/wrongIpAddressMan.png", UriKind.RelativeOrAbsolute);
            hibaBitmapKep.DecodePixelWidth = 200;
            hibaBitmapKep.EndInit();
            kep.Source = hibaBitmapKep;
        }
    }
}
