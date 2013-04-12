using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Car_Club_Application
{
    public class Car
    {
        public string BodyStyle;
        public short? ChassisNumber;
        public string Colour;
        public string Condition;
        public string Model;
        public List<CarOwnershipRecord> Owners;
        public string RegistrationNumber;
        public short Year;
    }
}
