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
        public ZakazKlientPage(List<string> userData2)
        {
            InitializeComponent();
            icon.Source = ImageSource.FromResource("TaxoparkMobile.image.iconTaxi.png");
            KlientName.Text = userData2[1];
        }
        private async void Tracking_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrackingPage());
        }

    }
}
