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
using System.Runtime.Serialization.Json;
using Microsoft.Phone.Net.NetworkInformation;

namespace Car_Club_Application
{
    public partial class MainPage : PhoneApplicationPage
    {
        Member member;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                textBlock1.Text = "Online";
                textBlock1.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                textBlock1.Text = "Offline";
                textBlock1.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        public void getmembership(string id)
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/member/" + id);
            webRequest.Method = "GET";

            webRequest.BeginGetResponse(a =>
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(a);
                    Stream datastream = response.GetResponseStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Member));
                    member = (Member)ser.ReadObject(datastream);

                    datastream.Close();
                    response.Close();

                    Dispatcher.BeginInvoke(() => getdetails());
                }
                catch
                {
                    Dispatcher.BeginInvoke(() => showerrormessage());
                }
            }
            , null);
        }

        public void getdetails()
        {
            NavigationService.Navigate(new Uri("/Page1.xaml?text=" + member.ID, UriKind.Relative));
        }

        public void showerrormessage()
        {
            MessageBox.Show("Member does not exist");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != "")
            {
                getmembership(textBox1.Text);
            }
        }
    }
}