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
using System.Text;

namespace Car_Club_Application
{
    public partial class Update_CarOwner : PhoneApplicationPage
    {
        Car car = new Car();
        string registrationNumber;
        public Update_CarOwner()
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

                }
            }
            , null);
           
        }
        public void updateview()
        {
            textBoxClolour.Text = car.Colour;
            textBoxModel.Text = car.Model;
            textBoxYear.Text =car.Year.ToString();
            textBoxCondition.Text = car.Condition;
            textBoxBodystyle.Text = car.BodyStyle;
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
                    Dispatcher.BeginInvoke(() => showresult(false));
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
                            Dispatcher.BeginInvoke(() => showresult(true));
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

        public void showresult(bool ok)
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            registrationNumber = textBox1.Text;
            getcar();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            car.BodyStyle = textBoxBodystyle.Text;
            car.Colour = textBoxClolour.Text;
            car.Condition = textBoxCondition.Text;
            car.Model = textBoxModel.Text;
            car.Year = short.Parse(textBoxYear.Text);
            UpdateCar();
            
        }
    }
}