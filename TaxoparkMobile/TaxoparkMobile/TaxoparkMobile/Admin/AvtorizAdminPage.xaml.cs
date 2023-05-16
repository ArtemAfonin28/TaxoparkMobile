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
    public partial class AvtorizAdmintPage : ContentPage
    {
        public AvtorizAdmintPage()
        {
            InitializeComponent();

            image1.Source = ImageSource.FromResource("TaxoparkMobile.image.logo.png");
            back.Source = ImageSource.FromResource("TaxoparkMobile.image.arrow.png");
        }
        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void vhod_Clicked(object sender, EventArgs e)
        {
            string phoneAdmin = nameInput1.Text;
            string passwordAdmin = nameInput2.Text;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `admin` WHERE `Phone_Admin`=@phoneAdmin AND `Password_Admin`=@passwordAdmin", db.getConnection());
            command.Parameters.Add("@phoneAdmin", MySqlDbType.VarChar).Value = phoneAdmin;
            command.Parameters.Add("@passwordAdmin", MySqlDbType.VarChar).Value = passwordAdmin;

            MySqlDataReader reader = command.ExecuteReader();


            if (reader.HasRows)
            {
                await DisplayAlert("Ништяк", "Реально ништяк", "OK");
            }
            else
            {
                await DisplayAlert("Не Ништяк", "Реально не ништяк", "OK");
            }
            db.closeConnection();
        }

    }
}
