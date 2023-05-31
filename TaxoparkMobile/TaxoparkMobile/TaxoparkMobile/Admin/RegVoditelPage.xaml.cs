using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxoparkMobile
{
    public partial class RegVoditelPage : ContentPage
    {
        public RegVoditelPage()
        {
            InitializeComponent();

        }
        private void Add_Clicked(object sender, EventArgs e)
        {
            int idCar;
            DB db = new DB();
            db.openConnection();
            try
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO `car`(`Id_Car`, `Model_Car`, `Register_Number`, `Number_Body`, `Number_Engine`, `Year_Release`, `Probeg`, `Data_TO`)" +
                    " VALUES (default, @model, @register, @body, @engine, @release, @probeg, @to)", db.getConnection());
                command.Parameters.Add("@model", MySqlDbType.VarChar).Value = model.Text;
                command.Parameters.Add("@register", MySqlDbType.VarChar).Value = register.Text;
                command.Parameters.Add("@body", MySqlDbType.VarChar).Value = body.Text;
                command.Parameters.Add("@engine", MySqlDbType.VarChar).Value = engine.Text;
                command.Parameters.Add("@release", MySqlDbType.Date).Value = release.Date;
                command.Parameters.Add("@probeg", MySqlDbType.Double).Value = Convert.ToDouble(probeg.Text);
                command.Parameters.Add("@to", MySqlDbType.Date).Value = to.Date;
                command.ExecuteNonQuery();

                MySqlCommand command2 = new MySqlCommand("SELECT max(`Id_Car`) FROM `car`", db.getConnection());
                idCar = Convert.ToInt32(command2.ExecuteScalar());

                MySqlCommand command3 = new MySqlCommand("INSERT INTO `driver`(`Id_Driver`, `FIO_Driver`, `Phone_Driver`, `PassportData_Driver`, `Password_Driver`, `Driver_License`, `Car_Id_Car`, `Tarif_Id_Tarif`) " +
                    "VALUES (default, @fio, @telephone, @passport, @password, @license, @car, @tarif)", db.getConnection());
                command3.Parameters.Add("@fio", MySqlDbType.VarChar).Value = fio.Text;
                command3.Parameters.Add("@telephone", MySqlDbType.VarChar).Value = telephone.Text;
                command3.Parameters.Add("@passport", MySqlDbType.VarChar).Value = passport.Text;
                command3.Parameters.Add("@password", MySqlDbType.VarChar).Value = password.Text;
                command3.Parameters.Add("@license", MySqlDbType.VarChar).Value = license.Text;
                command3.Parameters.Add("@car", MySqlDbType.Int32).Value = idCar;
                command3.Parameters.Add("@tarif", MySqlDbType.Int32).Value = tarif.SelectedIndex + 1;
                command3.ExecuteNonQuery();
            }
            catch
            {
                DisplayAlert("Ошибка", "Введены неверные данные или заполненны не все поля", "ок");
            }
            DisplayAlert("Успех", "Вы добавили нового водителя", "ок");
            db.closeConnection();
        }

        private void release_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
        public string GetHashMD5(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
    }
}
