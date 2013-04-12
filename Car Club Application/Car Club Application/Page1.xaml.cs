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

namespace Car_Club_Application
{
    public partial class Page1 : PhoneApplicationPage
    {
        string id_number;
        string regNumber;
        public Page1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchRegister.xaml",UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UploadPhoto.xaml", UriKind.Relative));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SpotCar.xaml", UriKind.Relative));
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Update_Member.xaml?text=" + id_number, UriKind.Relative));
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("text"))
            {
                id_number = NavigationContext.QueryString["text"];        
            }

            base.OnNavigatedFrom(e);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MemberDetail.xaml?text=" + id_number, UriKind.Relative));
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Update_CarOwner.xaml" , UriKind.Relative));
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UpdateOwner.xaml", UriKind.Relative));
        }
    }
}