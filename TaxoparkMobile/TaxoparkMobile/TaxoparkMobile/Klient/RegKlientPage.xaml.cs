using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TaxoparkMobile
{
    public partial class RegKlientPage : ContentPage
    {

        public RegKlientPage()
        {
            

            InitializeComponent();

            image1.Source = ImageSource.FromResource("TaxoparkMobile.image.logo.png");
            back.Source = ImageSource.FromResource("TaxoparkMobile.image.arrow.png");

        }

        private async void button1_Clicked(object sender, EventArgs e)
        {
            string fioUser = FIO.Text;
            string phoneUser = Phone.Text;
            string passwordUser = Password.Text;
            string passwordUser2 = Password2.Text;
            if (fioUser == "" || phoneUser == "" || passwordUser == "" || passwordUser2 == "")
            {
                await DisplayAlert("Ошибка", "Заполнены не все поля", "OK");
            }
            else
            {
                if (isUserExist() == false)
                {
                    if (passwordUser == passwordUser2)
                    {
                        DB db = new DB();
                        db.openConnection();
                        MySqlCommand command = new MySqlCommand("INSERT INTO `client`(`Id_Client`,`FIO_Client`, `Phone_Client`, `Password_Client`)" +
                            "values (default, @fio, @phone, @password)", db.getConnection());
                        command.Parameters.Add("@fio", MySqlDbType.VarChar).Value = fioUser;
                        command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phoneUser;
                        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = GetHashMD5(passwordUser);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            await DisplayAlert("Успех", "Вы успешно зарегистрировались", "OK");
                            db.closeConnection();
                            await Navigation.PopAsync();
                        }
                        else await DisplayAlert("Ошибка", "Не верные данные", "OK");
                        db.closeConnection();
                    }
                    else await DisplayAlert("Ошибка", "Не совпадают пароли", "OK");
                }
            }

        }

        public Boolean isUserExist()
        {
            string phoneUser = Phone.Text;


            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `client` WHERE `Phone_Client`=@phone", db.getConnection());
            command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phoneUser;
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                DisplayAlert("Ошибка", "Пользователь с этим номером уже зарегистрирован", "OK");
                return true;
            }
            else return false;
        }

        public string GetHashMD5(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
