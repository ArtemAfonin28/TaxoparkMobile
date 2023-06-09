﻿using MySqlConnector;
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
        List<string> userData = new List<string>();
        List<string> call = new List<string>();

        public int tarif = 0;
        public string otkuda;
        public string kuda;
        string result;
        public ZakazKlientPage(List<string> userData2)
        {
            InitializeComponent();
            Comment.Source = ImageSource.FromResource("TaxoparkMobile.image.coment.png");
            icon.Source = ImageSource.FromResource("TaxoparkMobile.image.iconTaxi.png");

            KlientName.Text = userData2[1];
            userData.AddRange(userData2);
            picker.SelectedIndex = 0;
            HaveCall();
        }

        private async void HaveCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `call` WHERE `Client_Id_Client` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(userData[0]);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                await DisplayAlert("Оповещение", "У вас есть не завершенный заказ", "Ок");
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        call.Add(reader[i].ToString());
                    }
                }
                db.closeConnection();
                await Navigation.PushAsync(new TrackingPage(userData, call));
            }
            else
            {
                db.closeConnection();
            }
        }



		private async void Tracking_Clicked(object sender, EventArgs e)
        {
            if (OtkudaEntry.Text == "" || KudaEntry.Text == "" || tarif == 0)
            {
                await DisplayAlert("Ошибка", "Заполните все поля", "OK");
            } 
            else
            {
                
                int addServices = 0;

                DateTime dateTime = new DateTime();
                dateTime = DateTime.UtcNow;

                otkuda = OtkudaEntry.Text;
                kuda = KudaEntry.Text;
                addServices = picker.SelectedIndex;

                if (result == "")
                {
                    result = "null";
                }

                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("INSERT INTO `call`" +
                    "(`Id_Call`, `DataTime_Call`, `Otkuda`, `Kuda`, `Accepted`, `Accepted_DataTime`, `Alerts`,`Coment`, `Client_Id_Client`, `Add_Services_Id_Services`, `Driver_Id_Driver`,`Tarif_Id_Tarif`)" +
                    " VALUES (default,@DataTime_Call,@Otkuda,@Kuda,null,null,null,@comment,@id,@Add_Services,null,@Tarif)", db.getConnection());
                command.Parameters.Add("@DataTime_Call", MySqlDbType.DateTime).Value = dateTime;
                command.Parameters.Add("@Otkuda", MySqlDbType.VarChar).Value = otkuda;
                command.Parameters.Add("@Kuda", MySqlDbType.VarChar).Value = kuda;
                command.Parameters.Add("@comment", MySqlDbType.VarChar).Value = result;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(userData[0]);
                if (addServices == 0)
                {
                    command.Parameters.Add("@Add_Services", MySqlDbType.Int32).Value = null;
                }
                else command.Parameters.Add("@Add_Services", MySqlDbType.Int32).Value = addServices;
                command.Parameters.Add("@Tarif", MySqlDbType.Int32).Value = tarif;
                if (command.ExecuteNonQuery() == 1)
                {
                    db.closeConnection();
                    OpenTrackingPage();
                    await Navigation.PushAsync(new TrackingPage(userData, call));
                }
            }
        }
        private void OpenTrackingPage()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `call` WHERE `Client_Id_Client`=@id AND `Otkuda`=@Otkuda AND `Kuda`=@Kuda", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(userData[0]);
            command.Parameters.Add("@Otkuda", MySqlDbType.VarChar).Value = otkuda;
            command.Parameters.Add("@Kuda", MySqlDbType.VarChar).Value = kuda;
            MySqlDataReader reader = command.ExecuteReader();
            call.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0;i < reader.FieldCount; i++)
                    {
                        call.Add(reader[i].ToString());
                    }
                }
            }

            //await Navigation.PushAsync(new TrackingPage());
        }

        private async void Comment_Clicked(object sender, EventArgs e)
        {
            result = await DisplayPromptAsync("Комментарий", "Напишите особенности поездки водителю");
        }


        private void Tarif1_Clicked(object sender, EventArgs e)
        {
            tarif = 1;
            Tarif1.BorderColor= Color.Green;
            Tarif2.BorderColor = Color.Orange;
            Tarif3.BorderColor = Color.Orange;
        }
        private void Tarif2_Clicked(object sender, EventArgs e)
        {
            tarif = 2;
            Tarif2.BorderColor = Color.Green;
            Tarif1.BorderColor = Color.Orange;
            Tarif3.BorderColor = Color.Orange;
        }
        private void Tarif3_Clicked(object sender, EventArgs e)
        {
            tarif = 3;
            Tarif3.BorderColor = Color.Green;
            Tarif1.BorderColor = Color.Orange;
            Tarif2.BorderColor = Color.Orange;
        }

    }
}
