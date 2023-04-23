using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxoparkMobile
{
    public partial class AvtorizKlientPage : ContentPage
    {
        public AvtorizKlientPage()
        {
            InitializeComponent();

            image1.Source = ImageSource.FromResource("TaxoparkMobile.image.logo.png");
            imagebutton.Source = ImageSource.FromResource("TaxoparkMobile.image.reg.png");

        }

        private async void OpenReg_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegKlientPage());
        }
        private async void adminButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AvtorizAdmintPage());
        }
    }
}
