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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Car_Club_Application
{
    public partial class UploadPhoto : PhoneApplicationPage
    {
        PhotoChooserTask chooser = new PhotoChooserTask();
        CarPhoto carphoto = new CarPhoto();

        public UploadPhoto()
        {
            InitializeComponent();

            chooser.Completed += new EventHandler<PhotoResult>(chooser_Completed);
        }

        void chooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(e.ChosenPhoto);
                image1.Source = bitmap;

                e.ChosenPhoto.Position = 0;

                carphoto.Photo = new byte[e.ChosenPhoto.Length];
                e.ChosenPhoto.Read(carphoto.Photo, 0, carphoto.Photo.Length);
            }
        }

        public void UploadCarPhoto()
        {
            WebRequest webRequest = WebRequest.Create("http://08309web.net.dcs.hull.ac.uk/borre/service.svc/images");
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

                    ser = new DataContractJsonSerializer(typeof(CarPhoto));
                    ser.WriteObject(ms, carphoto);

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
                        else
                        {

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
            MessageBox.Show("Photo Uploaded");
        }

        private void browsebutton_Click(object sender, RoutedEventArgs e)
        {
            chooser.ShowCamera = true;
            chooser.Show();
        }

        private void uploadbutton_Click(object sender, RoutedEventArgs e)
        {
            carphoto.RegistrationNumber = textBox1.Text;

            UploadCarPhoto();
        }
    }
}