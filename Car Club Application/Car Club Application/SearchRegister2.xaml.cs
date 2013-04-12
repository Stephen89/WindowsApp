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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Windows.Media.Imaging;
namespace Car_Club_Application
{
    public partial class SearchRegister2 : PhoneApplicationPage
    {
     
        Car car = new Car ();
        List<CarPhoto> carphotoslist = new List<CarPhoto>();
        string regnumber;
        public SearchRegister2()
        {
            InitializeComponent();
        }
        public void getcardetails()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/car/" + regnumber);
            webRequest.Method = "GET";

            webRequest.BeginGetResponse(a =>
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(a);
                    Stream datastream = response.GetResponseStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Car));
                    car = (Car)ser.ReadObject(datastream);

                    datastream.Close();
                    response.Close();

                    Dispatcher.BeginInvoke(() => updatecarlist());
                }
                catch
                {
                    //Dispatcher.BeginInvoke(() => showerrormessage());
                }
            }
            , null);
        }

        public void GetCarPhotos(string registration)
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/image/" + registration);
            webRequest.Method = "GET";

            webRequest.BeginGetResponse(a =>
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(a);
                    Stream datastream = response.GetResponseStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<CarPhoto>));
                    carphotoslist = (List<CarPhoto>)ser.ReadObject(datastream);

                    datastream.Close();
                    response.Close();

                    Dispatcher.BeginInvoke(() => updatephotos());
                }
                catch
                {

                }
            }
            , null);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("carlist"))
            {
                regnumber = NavigationContext.QueryString["carlist"];
                getcardetails();
                GetCarPhotos(regnumber);
            }

            base.OnNavigatedFrom(e);
        }

        public void updatecarlist()
        {
            textBlock1.Text += "Body Style: " + car.BodyStyle + "\n";
            textBlock1.Text += "Model: " + car.Model + "\n";
            textBlock1.Text += "Chassis: " + car.ChassisNumber + "\n";
            textBlock1.Text += "Condotion: " + car.Condition + "\n";
            textBlock1.Text += "Colour: " + car.Colour + "\n";
            textBlock1.Text += "Registration Number: " + car.RegistrationNumber + "\n";
            textBlock1.Text += "Year: " + car.Year + "\n";
            textBlock1.Text += "Body Style: " + car.BodyStyle + "\n";
            foreach (CarOwnershipRecord owner in car.Owners)
            {
                listBox1.Items.Add(owner.Owner.Forename + " " + owner.Owner.Surname);
            }
        }

        public void updatephotos()
        {
            int i = 0;

            foreach (CarPhoto photo in carphotoslist)
            {
                i++;
                listBox2.Items.Add("Photo " + i);
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Forename.Text += car.Owners[listBox1.SelectedIndex].Owner.Forename;
            Title.Text += car.Owners[listBox1.SelectedIndex].Owner.Title;
            Surname.Text += car.Owners[listBox1.SelectedIndex].Owner.Surname;
            Decorations.Text += car.Owners[listBox1.SelectedIndex].Owner.Decorations;
            Address1.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.AddressLine1_HouseNameNumber;
            Address2.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.AddressLine2_Street;
            Address3.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.AddressLine3_Locality;
            City.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.City;
            Country.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.Country;
            County.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.County_Region;
            Mobile.Text += car.Owners[listBox1.SelectedIndex].Owner.Mobile;
            Telephone.Text += car.Owners[listBox1.SelectedIndex].Owner.Telephone;
            DateSold.Text += car.Owners[listBox1.SelectedIndex].DateSold.ToString();
            DateBought.Text += car.Owners[listBox1.SelectedIndex].DateBought.ToString();
            Email.Text += car.Owners[listBox1.SelectedIndex].Owner.Email;
            PostalCode.Text += car.Owners[listBox1.SelectedIndex].Owner.Address.Postcode;

        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var bitmapimage = new BitmapImage();

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(carphotoslist[listBox2.SelectedIndex].Photo, 0, carphotoslist[listBox2.SelectedIndex].Photo.Length);
                    bitmapimage.SetSource(ms);
                }
                image1.Source = bitmapimage;
            }
            catch
            {
                MessageBox.Show("Error showing photo");
            }
        }

        //private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        //{

       // }

    }
}