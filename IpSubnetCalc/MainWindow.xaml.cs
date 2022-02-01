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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IpSubnetCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StackPanel currSelected = null;
        private List<int> subnetStack = new List<int>();
        private List<int> powerOfTwo = new List<int>();
        private string ipAddress = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<string> equalSubnets(string ipAddr)
        {
            if (hostRadioBtn.IsChecked==true)
            {

            }
            else
            {

            }
            List<string> decAddresses = new List<string>();
            string firstAddr = ipAddr;
            
            return decAddresses;

        }
        private List<string> VLSM(string ipAddr)
        {       
            Dictionary<string,int> binAddresses = new Dictionary<string, int>();
            string firstAddr = ipAddr;
            int firstPrefix = (32 -(powerOfTwo[0]));
            string decAddr = "";
            List<string> decAddresses = new List<string>();

            binAddresses.Add(firstAddr, firstPrefix);
            for (int i = 1; i < subnetStack.Count; i++)
            {
                binAddresses.Add(firstAddr.Remove(--firstPrefix,1).Insert(firstPrefix, "1"), (32 - powerOfTwo[i]));
                firstAddr = binAddresses.Keys.Last();
                firstPrefix = binAddresses.Values.Last();
                 
            }
            foreach (var item in binAddresses)
            {
                decAddr = "";
                string seged = item.Key;
                for (int i = 0; i < 32; i+=8)
                {
                    string octet = seged.Substring(i, 8);
                    decAddr+=ToDec(octet)+".";
                }
                decAddr = decAddr.TrimEnd('.');
                decAddr += "/"+item.Value;
                decAddresses.Add(decAddr);
               // MessageBox.Show(decAddr);

            }
            return decAddresses;
        } 
        private void ReadInSubnets()
        {
            subnetStack.Clear();
            foreach (StackPanel item in SubnetStack.Children)
            {
                subnetStack.Add(int.Parse((item.Children[1] as TextBox).Text));
                // MessageBox.Show((item.Children[1] as TextBox).Text);
            }
            subnetStack = subnetStack.OrderByDescending(x => x).ToList();
            /*
             foreach (var item in subnetStack)
             {
                 MessageBox.Show(item.ToString());
             }
            */
        }
        private void SubnetsInTwoPower()
        {
            byte bits;
            foreach (var item in subnetStack)
            {
                bits = 2;
                while (item > Math.Pow(2, bits))
                {
                    ++bits;
                }
                powerOfTwo.Add(bits);
            }
            /*
            foreach(var item in powerOfTwo)
            {
                MessageBox.Show(item.ToString());
            }
            */

        }
        private string ToBin(int inputDec)
        {
            string binaryValue = Convert.ToString(inputDec, 2);
            return binaryValue;
        }
        private int ToDec(string inputBin)
        {
            int decimalValue = Convert.ToInt32(inputBin, 2);
            return decimalValue;
        }
        private string ToBinMask(int maskLength)
        {
            string binaryForm = "";
            for (int i = 0; i < maskLength; i++)
            {
                binaryForm += "1";
            }
            while (binaryForm.Length < 32)
            {
                binaryForm += "0";
            }
            binaryForm = binaryForm.Insert(8, ".");
            binaryForm = binaryForm.Insert(17, ".");
            binaryForm = binaryForm.Insert(26, ".");

            return binaryForm;
        }
        private string ToDecMask(int maskLength)
        {
            string maskInBin = ToBinMask(maskLength);
            string[] octetValues = maskInBin.Split('.');
            string maskInDec = "";
            for (int i = 0; i < octetValues.Length; i++)
            {
                if (i == octetValues.Length - 1)
                {
                    maskInDec += Convert.ToString(ToDec(octetValues[i]));
                }
                else
                {
                    maskInDec += Convert.ToString(ToDec(octetValues[i])) + '.';
                }
            }
            return maskInDec;
        }
        private string ToBinAddress(string address)
        {
            string[] octetValues = address.Split('.');
            string[] addressInBin = new string[octetValues.Length];
            string outputAddress = "";
            for (int i = 0; i < octetValues.Length; i++)
            {
                addressInBin[i] = Convert.ToString(ToBin(int.Parse(octetValues[i])));
                while (addressInBin[i].Length < 8)
                {
                    addressInBin[i] = "0" + addressInBin[i];
                }
            }
            foreach (var item in addressInBin)
            {
                outputAddress += item;
            }
            return outputAddress;
        }
        private void SelectRow(object sender, RoutedEventArgs e)
        {
            if (currSelected != null)
            {
                currSelected.Background = Brushes.Transparent;
            }
            currSelected = (StackPanel)sender;
            currSelected.Background = Brushes.LightGray;
        }
        private void AddSubnetBtn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel row = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(4) };
            row.Children.Add(new Label() { Content = $"Number of users:" });
            row.Children.Add(new TextBox() { Width = 120 });
            row.MouseLeftButtonDown += SelectRow;
            SubnetStack.Children.Add(row);
           
        }
        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            ReadInSubnets();
            SubnetsInTwoPower();
            ipAddress = ipAddressTextBox.Text;
            /*
            foreach (var item in VLSM(ToBinAddress(ipAddress)))
            {
                MessageBox.Show(item);
            }
            */
        }
        private void RemoveSubnetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currSelected != null)
            {
                SubnetStack.Children.Remove(currSelected);
            }
        }

        private void vlsmRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            eqSubPanel.Visibility = Visibility.Hidden;
            vlsmButtons.Visibility = Visibility.Visible;
            prefixLbl.Visibility = Visibility.Hidden;
            netmaskPrefix.Visibility = Visibility.Hidden;
        }

        private void eqSubRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            eqSubPanel.Visibility = Visibility.Visible;
            vlsmButtons.Visibility = Visibility.Hidden;
            prefixLbl.Visibility = Visibility.Visible;
            netmaskPrefix.Visibility = Visibility.Visible;
        }
        private void hostRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            hostDock.Visibility = Visibility.Visible;
            subnetDock.Visibility = Visibility.Hidden;
        }
        private void subRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            hostDock.Visibility = Visibility.Hidden;
            subnetDock.Visibility = Visibility.Visible;
        }
    }
}