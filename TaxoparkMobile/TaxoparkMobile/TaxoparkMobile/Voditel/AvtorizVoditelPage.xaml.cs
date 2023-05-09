using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxoparkMobile
{
    public partial class AvtorizVoditelPage : ContentPage
    {
        public AvtorizVoditelPage()
        {
            InitializeComponent();

            image1.Source = ImageSource.FromResource("TaxoparkMobile.image.logo.png");
            back.Source = ImageSource.FromResource("TaxoparkMobile.image.arrow.png");
        }
        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
