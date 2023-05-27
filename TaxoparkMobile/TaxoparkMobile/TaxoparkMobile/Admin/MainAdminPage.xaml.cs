using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxoparkMobile
{
    public partial class MainAdminPage : ContentPage
    {
        List<string> adminData = new List<string>();
        public MainAdminPage(List<string> adminData2)
        {
            InitializeComponent();
            Image1.Source = ImageSource.FromResource("TaxoparkMobile.image.gear.png");
            adminData.AddRange(adminData2);
            AdminName.Text = adminData2[1];

        }

        private async void ButtonVod_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegVoditelPage());
        }

    }
}
