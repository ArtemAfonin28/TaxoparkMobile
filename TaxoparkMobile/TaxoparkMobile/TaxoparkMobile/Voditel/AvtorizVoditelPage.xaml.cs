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
    public partial class AvtorizVoditelPage : ContentPage
    {
        List<string> voditelData = new List<string>();
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
        private async void vhod_Clicked(object sender, EventArgs e)
        {
            
            string phoneDriver = nameInput1.Text;
            string passwordDriver = nameInput2.Text;
            if (phoneDriver == "" || passwordDriver == "")
            {
                await DisplayAlert("Ошибка", "Заполненны не все поля", "OK");
            }
            else
            {
                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT * FROM `driver` WHERE `Phone_Driver`=@phoneDriver AND `Password_Driver`=@passwordDriver", db.getConnection());
                command.Parameters.Add("@phoneDriver", MySqlDbType.VarChar).Value = phoneDriver;
                command.Parameters.Add("@passwordDriver", MySqlDbType.VarChar).Value = passwordDriver;

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    voditelData.Clear();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            voditelData.Add(reader[i].ToString());
                        }
                    }
                    await Navigation.PushAsync(new MainVoditelPage(voditelData));
                }
                else
                {
                    await DisplayAlert("Ошибка", "Такого пользавателя не существует", "OK");
                }
                db.closeConnection();
            }
            
        }

    }
}
