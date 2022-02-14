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
        MainWindow main = new MainWindow();
        public rosszIp()
        {
            InitializeComponent();
            BitmapImage hibaBitmapKep = new BitmapImage();
            hibaBitmapKep.BeginInit();
            string startPath = Environment.CurrentDirectory;

            if (MainWindow.wrongIpInput)
            {
                hibaBitmapKep.UriSource = new Uri(startPath+"/Images/wrongIpAddressMan.png", UriKind.RelativeOrAbsolute);
            }
            else if (MainWindow.wrongSubnetBoxInput)
            {
                hibaBitmapKep.UriSource = new Uri(startPath + "/Images/wrongSubnetMan.jpg", UriKind.RelativeOrAbsolute);
            }
            else if (MainWindow.wrongMaskInput)
            {
                hibaBitmapKep.UriSource = new Uri(startPath + "/Images/wrongIpAddressMan.png", UriKind.RelativeOrAbsolute);
            }
            else
            {
                hibaBitmapKep.UriSource = new Uri(startPath + "/Images/wrongIpAddressMan.png", UriKind.RelativeOrAbsolute);
            }
            hibaBitmapKep.EndInit();
            image.Source = hibaBitmapKep;
        }
    }
}
