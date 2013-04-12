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
    public partial class SpotCar : PhoneApplicationPage
    {
        Car car = new Car();
        public SpotCar()
        {
            InitializeComponent();
        }
        public void AddCar()
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
            MessageBox.Show("Car Added");
        }

        private void updatebutton_Click(object sender, RoutedEventArgs e)
        {
            car.RegistrationNumber = textBox1.Text;
            car.Colour = textBox2.Text;
            car.Model = textBox3.Text;

            try
            {
                car.Year = short.Parse(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("Incorrect format");
                return;
            }
            car.Condition = textBox5.Text;
            car.Model = textBox6.Text;
            car.Owners = new List<CarOwnershipRecord>();
            car.BodyStyle = textBox7.Text;
            AddCar();
        }
    }
}