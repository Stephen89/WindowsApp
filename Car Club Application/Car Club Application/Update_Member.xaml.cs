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
using System.Text;

namespace Car_Club_Application
{
    public partial class Update_Member : PhoneApplicationPage
    {
        Member member = new Member();
        string id_number;

        public Update_Member()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("text"))
            {
                id_number = NavigationContext.QueryString["text"];
                getmembership();
            }
            base.OnNavigatedTo(e);
        }

        public void getmembership()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/member/" + id_number);
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

                    Dispatcher.BeginInvoke(() => updateview());
                }
                catch
                {

                }
            }
            , null);
        }

        public void updateview()
        {
            textBox2.Text = member.Forename;
            textBox3.Text = member.Surname;
            textBox4.Text = member.Address.AddressLine1_HouseNameNumber;
            textBox9.Text = member.Address.AddressLine2_Street;
            textBox6.Text = member.Address.AddressLine3_Locality;
            textBox5.Text = member.Address.Postcode;
            textBox8.Text = member.Address.Country;
            textBox7.Text = member.Address.County_Region;
            textBox1.Text = member.Email;
            textBox10.Text = member.Telephone;
            textBox11.Text = member.Mobile;
            textBox12.Text = member.Address.City;

            
        }

        public void UpdateMemberDetails()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/member");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";

            webRequest.BeginGetRequestStream(a =>
            {
                Stream datastream;
                DataContractJsonSerializer ser;
                StreamWriter sw;

                try
                {
                    datastream = webRequest.EndGetRequestStream(a);

                    MemoryStream ms = new MemoryStream();

                    ser = new DataContractJsonSerializer(typeof(Member));
                    ser.WriteObject(ms, member);

                    string json = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                    sw = new StreamWriter(datastream);
                    sw.Write(json);
                    sw.Close();

                    ms.Close();

                    datastream.Close();
                }
                catch
                {

                }

                webRequest.BeginGetResponse(b =>
                {
                    try
                    {
                        bool result;

                        WebResponse response = webRequest.EndGetResponse(b);
                        datastream = response.GetResponseStream();
                        ser = new DataContractJsonSerializer(typeof(bool));
                        result = (bool)ser.ReadObject(datastream);

                        datastream.Close();
                        response.Close();

                        if (result)
                        {
                            Dispatcher.BeginInvoke(() => showresult());
                        }
                    }
                    catch
                    {

                    }
                }
                , null);
            }
            , null);
        }

        public void showresult()
        {
            MessageBox.Show("Member updated");
        }

        private void updatebutton_Click(object sender, RoutedEventArgs e)
        {
            member.Forename = textBox2.Text;
            member.Surname = textBox3.Text;
            member.Address.AddressLine1_HouseNameNumber = textBox4.Text;
            member.Address.AddressLine2_Street = textBox9.Text;
            member.Address.AddressLine3_Locality = textBox6.Text;
            member.Address.Postcode = textBox5.Text;
            member.Address.Country = textBox8.Text;
            member.Address.County_Region = textBox7.Text;
            member.Email = textBox1.Text;
            member.Telephone = textBox10.Text;
            member.Mobile = textBlock11.Text;
            member.Address.City = textBlock12.Text;
            UpdateMemberDetails();
        }  
    }
}