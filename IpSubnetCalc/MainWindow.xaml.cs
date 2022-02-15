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
    public partial class MainWindow : Window
    {
        private StackPanel currSelected = null;
        private List<int> subnetStack = new List<int>();
        private List<int> powerOfTwo = new List<int>();
        private string ipAddress = "";
        public static bool wrongIpInput;
        public static bool wrongSubnetBoxInput;
        public static bool wrongMaskInput;
        public MainWindow()
        {
            InitializeComponent();
        }
        

        private List<string> equalSubnetsHosts(string ipAddr, string prefix, string numberOf)
        {
            subnetStack.Clear();
            powerOfTwo.Clear();
            List<string> decAddresses = new List<string>();
            int hosts = 0; 
            try
            {
                hosts = int.Parse(numberOf);
            }
            catch (Exception)
            {
                wrongSubnetBoxInput = true;
                rosszIp hiba = new rosszIp();
                hiba.ShowDialog();
                Environment.Exit(0);
            }
            byte bits = 2;
            while (hosts > (Math.Pow(2, bits)-2))
            {
                ++bits;
            }
            int mask = 32 - bits;
            int index = mask;
            int subNetworkBits = mask - int.Parse(prefix);

            for (int i = 0; i < Math.Pow(2, subNetworkBits); i++)
            {
                powerOfTwo.Add(bits);
                subnetStack.Add(hosts);
            }

            foreach (var item in VLSM(ipAddr))
            {
                decAddresses.Add(item);
            }

            return decAddresses;

        }
        private List<string> equalSubnetsSubnet(string ipAddr, string prefix, string numberOf)
        {
            subnetStack.Clear();
            powerOfTwo.Clear();
            List<string> decAddresses = new List<string>();
            int subnets=0;
            try
            {
                subnets = int.Parse(numberOf);
            }
            catch (Exception)
            {
                wrongSubnetBoxInput = true;
                rosszIp hiba = new rosszIp();
                hiba.ShowDialog();
                Environment.Exit(0);
            }
            byte bits = 2;
            while (subnets > Math.Pow(2, bits))
            {
                ++bits;
            }
            int mask = int.Parse(prefix) + bits;
            byte numberOfHostsByTwoPower = Convert.ToByte(32 - mask);
            int hosts = Convert.ToInt32(Math.Pow(2,numberOfHostsByTwoPower));

            for (int i = 0; i < Math.Pow(2, bits); i++)
            {
                powerOfTwo.Add(numberOfHostsByTwoPower);
                subnetStack.Add(hosts);
            }

            foreach (var item in VLSM(ipAddr))
            {
                decAddresses.Add(item);
            }
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
                
                    --firstPrefix;
                while (firstAddr[firstPrefix] == '1')
                {
                    if (firstAddr[firstPrefix]=='1')
                    {
                        int addr = Convert.ToInt32(Convert.ToChar(firstAddr[firstPrefix]));
                        int mask = Convert.ToInt32(ToBinMask(32 - powerOfTwo[i])[firstPrefix]);
                        int value = addr ^ mask;
                        firstAddr=firstAddr.Remove(firstPrefix, 1).Insert(firstPrefix, Convert.ToString(value));
                    }
                    --firstPrefix;
                }
                binAddresses.Add(firstAddr.Remove(firstPrefix,1).Insert(firstPrefix, "1"), (32 - powerOfTwo[i]));
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
                decAddr += "/" + item.Value;// +" -> "+ToDecAddress(broadcast);
                decAddresses.Add(decAddr);

            }
            return decAddresses;
        } 
        private void ReadInSubnets()
        {
            subnetStack.Clear();
            foreach (StackPanel item in SubnetStack.Items)
            {
                try
                {
                    int.Parse((item.Children[1] as TextBox).Text);
                }
                catch (Exception)
                {
                    wrongSubnetBoxInput = true;
                    rosszIp hiba = new rosszIp();
                    hiba.ShowDialog();
                    Environment.Exit(0);
                }
            }
            foreach (StackPanel item in SubnetStack.Items)
            {
                subnetStack.Add(int.Parse((item.Children[1] as TextBox).Text));
            }
            subnetStack = subnetStack.OrderByDescending(x => x).ToList();
        }
        private void SubnetsInTwoPower()
        {
            powerOfTwo.Clear();
            byte bits;
            foreach (var item in subnetStack)
            {
                bits = 2;
                while (item > (Math.Pow(2, bits)-2))
                {
                    ++bits;
                }
                powerOfTwo.Add(bits);
            }
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
            /*
            binaryForm = binaryForm.Insert(8, ".");
            binaryForm = binaryForm.Insert(17, ".");
            binaryForm = binaryForm.Insert(26, ".");
            */

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
            for (int i = 0; i < octetValues.Length; i++)
            {
                try
                {
                    int helper = int.Parse(octetValues[i]);
                    if (helper>255)
                    {
                        throw new System.Exception();
                    }
                }
                catch (Exception)
                {
                    wrongIpInput = true;
                    rosszIp hiba = new rosszIp();
                    hiba.ShowDialog();
                    Environment.Exit(0);
                }

            }
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
        private string ToDecAddress(int address)
        {
            string decAddr = "";
            for (int i = 0; i < 32; i += 8)
            {
                string octet = address.ToString().Substring(i, 8);
                decAddr += ToDec(octet) + ".";
            }
            return decAddr;
        }
        private void SelectRow(object sender, RoutedEventArgs e)
        {
            if (currSelected != null)
            {
                currSelected.Background = Brushes.Transparent;
            }
            currSelected = (StackPanel)sender;
        }
        private void AddSubnetBtn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel row = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(4) };
            row.Children.Add(new Label() { Content = $"Number of users:" });
            row.Children.Add(new TextBox() { Width = 120 });
            row.MouseLeftButtonDown += SelectRow;
            SubnetStack.Items.Add(row);
           
        }
        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
           
            outPutPanel.Items.Clear();
            ipAddress = ipAddressTextBox.Text;
            if (vlsmRadioBtn.IsChecked==true)
            {
                ReadInSubnets();
                SubnetsInTwoPower();
                foreach (var item in VLSM(ToBinAddress(ipAddress)))
                {
                    StackPanel row = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(4) };
                    row.Children.Add(new Label() { Content = item });
                    row.MouseLeftButtonDown += SelectRow;
                    outPutPanel.Items.Add(row);
                }
            }
            else
            {
                if (hostRadioBtn.IsChecked==true)
                {
                    foreach (var item in equalSubnetsHosts(ToBinAddress(ipAddress), netmaskPrefix.Text, numberOf.Text))
                    {
                        StackPanel row = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(4) };
                        row.Children.Add(new Label() { Content = item });
                        row.MouseLeftButtonDown += SelectRow;
                        outPutPanel.Items.Add(row);
                    }
                   
                }
                else
                {
                    foreach (var item in equalSubnetsSubnet(ToBinAddress(ipAddress), netmaskPrefix.Text, numberOf.Text))
                    {
                        StackPanel row = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(4) };
                        row.Children.Add(new Label() { Content = item });
                        row.MouseLeftButtonDown += SelectRow;
                        outPutPanel.Items.Add(row);
                    }
                }
            }



            
        }
        private void RemoveSubnetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currSelected != null)
            {
                SubnetStack.Items.Remove(SubnetStack.SelectedItem);
            }
            
        }

        private void vlsmRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            eqSubPanel.Visibility = Visibility.Hidden;
            vlsmButtons.Visibility = Visibility.Visible;
            prefixLbl.Visibility = Visibility.Hidden;
            netmaskPrefix.Visibility = Visibility.Hidden;
            SubnetStack.Visibility = Visibility.Visible;
        }

        private void eqSubRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            eqSubPanel.Visibility = Visibility.Visible;
            vlsmButtons.Visibility = Visibility.Hidden;
            prefixLbl.Visibility = Visibility.Visible;
            netmaskPrefix.Visibility = Visibility.Visible;
            SubnetStack.Visibility = Visibility.Hidden;
        }

    }
}