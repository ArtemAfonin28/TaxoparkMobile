﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Shapes;

namespace TaxoparkMobile
{
    public partial class TrackingPage : ContentPage
    {
        List<string> userData = new List<string>();
        List<string> call = new List<string>();
        List<string> dataDriver = new List<string>();
        int idVoditel;

        bool timer = true;

        bool accepted = false;
        bool alert = false;

        public int i = 0;

        public TrackingPage(List<string> userData2, List<string> call2)
        {
            InitializeComponent();
            Profile.Source = ImageSource.FromResource("TaxoparkMobile.image.profile.png");
            Profile.IsVisible = false;
            ProfileLabel.IsVisible = false;

            userData.AddRange(userData2);
            call.AddRange(call2);

            KlientName.Text = userData2[1];
            Label1.Text = call[2].ToString();
            Label2.Text = call[3].ToString();

            
            OnTimerTick();
        }


        private async void OnTimerTick()
        {
            try
            {
                if (timer)
                {
                    await Task.Delay(2000);
                    if (timer)
                    {
                        FillTable();
                    }
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            catch
            {
                await DisplayAlert("Оповещение", "Ваш заказ завершен", "ОК");
                await Navigation.PopAsync();
            }
            
        }

		protected override bool OnBackButtonPressed()
		{
			timer = false;
            Navigation.PopAsync();
            Navigation.PopAsync();
            return true;
		}

		private void FillTable()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `call` WHERE `Id_Call`=@id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(call[0]);
            MySqlDataReader reader = command.ExecuteReader();
            call.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        call.Add(reader[i].ToString());
                    }
                }
            }
            else
            {
                timer = false;
                OnTimerTick();
            } 

            

            if (call[4] != "" && accepted == false)
            {
                Profile.IsVisible = true;
                accepted = true;
                ProfileLabel.IsVisible = true;
                stateCall.Text = "Ваш заказ был принят";
                UpdateTable();
            }
            else if (call[6] != "" && alert == false)
            {
                Profile.IsVisible = true;
                ProfileLabel.IsVisible = true;
                alert = true;
                stateCall.Text = "Водитель ожидает вас";
            }
            else if (accepted == true && call[4] == "")
            {
                stateCall.Text = "Ваш заказ еще никто не принял";
                Label3.Text = "Неизвестно";
                Label4.Text = "Неизвестно";
                Label5.Text = "Неизвестно";
                Profile.IsVisible = false;
                ProfileLabel.IsVisible = false;

                alert = false;
                accepted = false;
            }
            db.closeConnection();
            OnTimerTick();
            

        }
        private async void DeleteCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("DELETE FROM `call` WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(call[0]);
            command.ExecuteNonQuery();
            db.closeConnection();
            await DisplayAlert("Отлично", "Ваш заказ завершен", "ОК");
            await Navigation.PopAsync();
        }
        private void UpdateTable()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `Id_Driver`,`FIO_Driver`,`Model_Car`,`Register_Number` FROM `driver`,`car` WHERE `Car_Id_Car`=`Id_Car` AND `Id_Driver` =  @Id_Driver", db.getConnection());
            command.Parameters.Add("@Id_Driver", MySqlDbType.Int32).Value = Convert.ToInt32(call[10]);
            MySqlDataReader reader = command.ExecuteReader();
            dataDriver.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataDriver.Add(reader[i].ToString());
                    }
                    idVoditel = Convert.ToInt32(reader[0]);
                }
            }
            Label3.Text = dataDriver[1].ToString();
            Label4.Text = dataDriver[2].ToString();
            Label5.Text = dataDriver[3].ToString();
        }
        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            timer = false;
            DB db = new DB();
            await DisplayAlert("Оповещение", "Вы отменили свой заказ", "ОК");
            db.openConnection();
            MySqlCommand command = new MySqlCommand("DELETE FROM `call` WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(call[0]);
            command.ExecuteNonQuery();
            db.closeConnection();
            await Navigation.PopAsync();
        }
        private void Profile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VoditelAccountPage(idVoditel));
        }
    }
}
