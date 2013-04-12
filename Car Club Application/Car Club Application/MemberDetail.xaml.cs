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

namespace Car_Club_Application
{
    public partial class UpdateMethod : PhoneApplicationPage
    {
        string id_number;
        Person person = new Person();
        Member member_detail = new Member();
        public UpdateMethod()

        {
            InitializeComponent();
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
                    member_detail = (Member)ser.ReadObject(datastream);

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
            FirstName.Text = "First Name: " + member_detail.Forename + "\n";
            Surname.Text = " Surname: " + member_detail.Surname +"\n";
            Decoration.Text = "Decoration: " + member_detail.Decorations +"\n";
            Address1.Text = "Address Line 1 : " + member_detail.Address.AddressLine1_HouseNameNumber + "\n";
            Address2.Text = "Address Line 2 : " + member_detail.Address.AddressLine2_Street + "\n";
            City.Text = "City: " + member_detail.Address.City + "\n";
            Number.Text ="Mobile Number: " + member_detail.Mobile +"\n";
            textBlock1.Text = "County :" + member_detail.Address.County_Region + "\n";
            textBlock2.Text = "ID :" + member_detail.ID + "\n";
            textBlock5.Text = "Renewal Date :" + member_detail.MembershipRenewalDate + "\n";
            textBlock4.Text = "Postal Code :" + member_detail.Address.Postcode + "\n";
            textBlock3.Text = "Title : " + member_detail.Title +"\n";

           
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("text"))
            {
                id_number = NavigationContext.QueryString["text"];
                getmembership();
            }

            base.OnNavigatedFrom(e);
        }
      
    }
}