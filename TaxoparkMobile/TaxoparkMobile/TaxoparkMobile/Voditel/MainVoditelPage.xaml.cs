using MySqlConnector;
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
    public partial class MainVoditelPage : ContentPage
    {

        List<string> voditelData = new List<string>();
        public string newcall;
        List<string> acceptedCallData = new List<string>();

        public MainVoditelPage(List<string> voditelData2)
        {
            InitializeComponent();

            voditelData.AddRange(voditelData2);
            VoditelName.Text= voditelData2[1];

            Alerts.IsEnabled = false;
            Finished.IsEnabled = false;
            CancelButton.IsEnabled = false;

            UpdateCallView();

        }

        private void AcceptedCall_Clicked(object sender, EventArgs e)
        {
            picker.IsEnabled = false;
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `call` WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(picker.SelectedItem);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        acceptedCallData.Add(reader[i].ToString());
                    }
                }
            }
            reader.Close();
            db.closeConnection();
            
            DateTime dateTime = new DateTime();
            dateTime = DateTime.UtcNow;

            db.openConnection();
            MySqlCommand command2 = new MySqlCommand("UPDATE `call` SET `Accepted`= 1,`Accepted_DataTime`=@DataTime_Call, `Driver_Id_Driver`=@idDriver WHERE `Id_Call`= @id", db.getConnection());
            command2.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command2.Parameters.Add("@DataTime_Call", MySqlDbType.DateTime).Value = dateTime;
            command2.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[0]);
            command2.ExecuteNonQuery();
            DisplayAlert("Отлично", "Вы приняли заказ", "Ок");
            db.closeConnection();
            Accepted.IsEnabled= false;
            Alerts.IsEnabled = true;
            CancelButton.IsEnabled = true;

            Otkuda.Text = acceptedCallData[2];
            Kuda.Text = acceptedCallData[3];
        }

        private void Alerts_Clicked(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE `call` SET `Alerts`= 1 WHERE `Id_Call` =@id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command.ExecuteNonQuery();
            DisplayAlert("Отлично", "Оповещение отправленно", "Ок");
            Alerts.IsEnabled = false;
            Finished.IsEnabled = true;
            db.closeConnection();
        }
        private void Finished_Clicked(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE `call` SET `Finished`= 1 WHERE `Id_Call` =@id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command.ExecuteNonQuery();
            DisplayAlert("Отлично", "Заказ завершен", "Ок");
            db.closeConnection();
            Finished.IsEnabled = false;
            DeleteFinishedCall();
            UpdateCallView();

        }
        private async void DeleteFinishedCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("DELETE FROM `call` WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command.ExecuteNonQuery();
            db.closeConnection();
        }
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE `call` SET `Accepted`=null,`Accepted_DataTime`=null,`Alerts`=null,`Finished`=null,`Driver_Id_Driver`=null WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command.ExecuteNonQuery();
            DisplayAlert("Оповещение", "Вы отменили свой заказ", "Ок");
            db.closeConnection();
            UpdateCallView();
        }

        private async void UpdateCallView()
        {
            Otkuda.Text = "";
            Kuda.Text = "";
            picker.IsEnabled = true;
            Accepted.IsEnabled = true;
            Alerts.IsEnabled = false;
            Finished.IsEnabled = false;
            CancelButton.IsEnabled = false;

            acceptedCallData.Clear();
            callView.Children.Clear();
            picker.Items.Clear();

            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `Id_Call`,`DataTime_Call`,`Otkuda`,`Kuda`,`Add_Services_Id_Services` " +
                "FROM `call` WHERE `Accepted` IS NULL and `Tarif_Id_Tarif`=@tarif", db.getConnection());
            command.Parameters.Add("@tarif", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[7]);
            MySqlDataReader reader = command.ExecuteReader();

            
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    picker.Items.Add(reader[0].ToString());

                    string addservices;
                    switch (Convert.ToInt32(reader[4]))
                    {
                        case (1):
                            addservices = "Доп услуга1";
                            break;
                        case (2):
                            addservices = "Доп услуга2";
                            break;
                        case (3):
                            addservices = "Доп услуга3";
                            break;
                        default:
                            addservices = "Без Доп услуг";
                            break;
                    }

                    Grid grid = new Grid
                    {
                        RowSpacing = -30,
                        ColumnSpacing = 1,
                        RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        }
                    };
                    for (int i = 0; i < 5; i++)
                    {
                        string textcall = reader[i].ToString();
                        if (i == 4)
                        {
                            textcall = addservices;
                        }
                        Label label1 = new Label
                        {

                            FontSize = 12,
                            Text = textcall
                        };
                        label1.VerticalTextAlignment = TextAlignment.Center;
                        label1.HorizontalTextAlignment = TextAlignment.Center;
                        label1.TextColor = Color.Black;
                        grid.Children.Add(label1, i, j);
                    }
                    j++;
                    BoxView box = new BoxView
                    {
                        Margin = -5,
                        HeightRequest = 2
                    };
                    box.BackgroundColor = Color.DarkOrange;
                    callView.Children.Add(grid);
                    callView.Children.Add(box);
                }
            }
        }
    }
}
