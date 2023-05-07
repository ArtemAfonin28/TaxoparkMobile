using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace TaxoparkMobile
{
    public partial class ZakazKlientPage : ContentPage
    {
        public ZakazKlientPage()
        {
            InitializeComponent();

            icon.Source = ImageSource.FromResource("TaxoparkMobile.image.iconTaxi.png");
        }


    }
}
