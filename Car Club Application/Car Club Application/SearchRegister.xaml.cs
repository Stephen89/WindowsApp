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
using System.Net.NetworkInformation;

namespace Car_Club_Application
{
    public partial class SearchRegister : PhoneApplicationPage
    {
        List<string> carregistrationslist = new List<string>();


        public SearchRegister()
        {
            InitializeComponent();
            getregistrations();

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

        public void getregistrations()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/cars");
            webRequest.Method = "GET";

            webRequest.BeginGetResponse(a =>
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(a);
                    Stream datastream = response.GetResponseStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<string>));
                    carregistrationslist = (List<string>)ser.ReadObject(datastream);

                    datastream.Close();
                    response.Close();

                    Dispatcher.BeginInvoke(() => updatelistview());
                }
                catch
                {
                    //Dispatcher.BeginInvoke(() => showerrormessage());
                }
            }
            , null);
        }

        public void updatelistview()
        {
            foreach (string reg in carregistrationslist)
            {
                listBox1.Items.Add(reg);
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                listBox1.Items.Clear();

                for (int i = 0; i < carregistrationslist.Count; i++)
                {
                    if (carregistrationslist[i].Contains(textBox1.Text.ToUpper()))
                    {
                        listBox1.Items.Add(carregistrationslist[i]);
                    }
                }
            }
            else
            {
                updatelistview();
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchRegister2.xaml?carlist=" + listBox1.SelectedItem.ToString(), UriKind.Relative)); 
        }
    }
}