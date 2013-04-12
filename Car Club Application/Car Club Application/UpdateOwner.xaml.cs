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
    public partial class UpdateOwner : PhoneApplicationPage
    {   Person person = new Person();
        CarOwnershipRecord record = new CarOwnershipRecord();
        Car car = new Car();
        string registrationNumber;

        public UpdateOwner()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("text"))
            {
                registrationNumber = NavigationContext.QueryString["text"];
                getcar();
            }
            base.OnNavigatedTo(e);
        }

        public void getcar()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/car/" + registrationNumber);
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

                    Dispatcher.BeginInvoke(() => updateview());
                }
                catch
                {
                    show_result(false);
                    return;
                }
                //show_result(true);
            }
            , null);


        }

        public void updateview()
        {
            foreach (CarOwnershipRecord owner in car.Owners)
            {
                listBoxOwner.Items.Add(owner.Owner.Forename + " " + owner.Owner.Surname);
            }
        }

       

        public void UpdateCar()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/car");
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

                    ser = new DataContractJsonSerializer(typeof(Car));
                    ser.WriteObject(ms, car);

                    string json = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                    sw = new StreamWriter(datastream);
                    sw.Write(json);
                    sw.Close();

                    ms.Close();

                    datastream.Close();
                }
                catch
                {
                    Dispatcher.BeginInvoke(() => show_result(false));
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
                            //Dispatcher.BeginInvoke(() => show_result(true));
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
        public void show_result(bool ok)
        {
            if (ok)
            {
                MessageBox.Show("Car updated");
            }
            else
            {
                MessageBox.Show("error");
            }
        }


        private void listBoxOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Forename != null)
            {
                textBoxForName.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Forename;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Title != null)
            {
                textBoxTitle.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Title;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Surname != null)
            {
                textBoxSurname.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Surname;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Decorations != null)
            {
                textBoxDecorations.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Decorations;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine1_HouseNameNumber != null)
            {
                textBoxAddress1.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine1_HouseNameNumber;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine2_Street != null)
            {
                textBoxAddress2.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine2_Street;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine3_Locality != null)
            {
                textBoxAddress3.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine3_Locality;
            }
            //text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.City; //city
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Address.Country != null)
            {
                textBoxCountry.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.Country;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Address.County_Region != null)
            {
                textBoxCounty.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.County_Region;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Mobile != null)
            {
                textBoxMobile.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Mobile;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Telephone != null)
            {
                textBoxTelephone.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Telephone;
            }
            if (car.Owners[listBoxOwner.SelectedIndex].DateSold.ToString() != null)
            {
                textBoxDateSold.Text = car.Owners[listBoxOwner.SelectedIndex].DateSold.ToString();
            }
            if (car.Owners[listBoxOwner.SelectedIndex].DateBought.ToString() != null)
            {
                textBoxDateBought.Text = car.Owners[listBoxOwner.SelectedIndex].DateBought.ToString();
            }
            if (car.Owners[listBoxOwner.SelectedIndex].Owner.Email != null)
            {
            textBoxEmail.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Email;
            }
             if (car.Owners[listBoxOwner.SelectedIndex].Owner.Address.Postcode != null)
            {
            textBoxPostalCode.Text = car.Owners[listBoxOwner.SelectedIndex].Owner.Address.Postcode;
             }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            registrationNumber = textBox1.Text;
            getcar();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
            car.Owners[listBoxOwner.SelectedIndex].Owner.Forename = textBoxForName.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Surname = textBoxSurname.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Title = textBoxTitle.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine1_HouseNameNumber = textBoxAddress1.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine2_Street = textBoxAddress2.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Address.AddressLine3_Locality = textBoxAddress3.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Address.Postcode = textBoxPostalCode.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Address.Country = textBoxCountry.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Address.County_Region = textBoxCounty.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Email = textBoxEmail.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Telephone = textBoxTelephone.Text;
            car.Owners[listBoxOwner.SelectedIndex].Owner.Mobile = textBoxMobile.Text;
            //car.Owners[listBoxOwner.SelectedIndex].Owner.Address.City = textB
            //car.Owners[listBoxOwner.SelectedIndex].DateBought = DateTime.Parse(textBoxDateBought.Text);
            //car.Owners[listBoxOwner.SelectedIndex].DateSold = DateTime.Parse(textBoxDateSold.Text);
            UpdateCar();
        }

    }
}