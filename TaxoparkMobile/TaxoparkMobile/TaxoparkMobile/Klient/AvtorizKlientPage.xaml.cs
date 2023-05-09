
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            buttonVodAvtoriz.Source = ImageSource.FromResource("TaxoparkMobile.image.voditelLogo.png");
            buttonAdminAvtoriz.Source = ImageSource.FromResource("TaxoparkMobile.image.adminLogo.png");


        }

        private async void OpenReg_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegKlientPage());
        }
        private async void AdminButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AvtorizAdmintPage());
        }
        private async void VoditelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AvtorizVoditelPage());
        }
        private async void AvtorizButton_Clicked(object sender, EventArgs e)
        {
            string phoneUser = nameInput1.Text;
            string passwordUser = nameInput2.Text;
            
            DB db = new DB();
            db.openConnection();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `klient` WHERE `Phone_Klient`=@phoneUser AND `Password_Klient`=@passwordUser");
            command.Parameters.Add("@phoneUser", MySqlDbType.VarChar).Value = phoneUser;
            command.Parameters.Add("@passwordUser", MySqlDbType.VarChar).Value = passwordUser;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            db.closeConnection();
            if (table.Rows.Count > 0)
            {
                await Navigation.PushAsync(new ZakazKlientPage());
            }
        }
    }
}
