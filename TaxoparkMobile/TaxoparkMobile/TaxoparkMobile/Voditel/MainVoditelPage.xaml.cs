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
        public string newcall, comment;
        List<string> acceptedCallData = new List<string>();
        int idVoditel;

        public MainVoditelPage(List<string> voditelData2)
        {
            InitializeComponent();
            updateButton.Source = ImageSource.FromResource("TaxoparkMobile.image.update.png");
            Comment.Source = ImageSource.FromResource("TaxoparkMobile.image.coment.png");
            Profile.Source = ImageSource.FromResource("TaxoparkMobile.image.profile.png");

            voditelData.AddRange(voditelData2);
            idVoditel = Convert.ToInt32(voditelData2[0]);
            VoditelName.Text= voditelData2[1];

            Alerts.IsEnabled = false;
            Finished.IsEnabled = false;
            CancelButton.IsEnabled = false;
            Comment.IsEnabled = false;
            Comment.IsVisible = false;

            
            if (HaveCall())
            {
                DisplayAlert("Оповещение", "У вас есть не завершенный заказ", "Ок");
            }
            else UpdateCallView();
        }

        private bool HaveCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `call` WHERE `Driver_Id_Driver` = @idDriver and `Accepted` = 1 ", db.getConnection());
            command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[0]);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                
                while (reader.Read())
                {
                    string field = reader[7].ToString();
                    if (field != "")
                    {
                        Comment.IsEnabled = true;
                        Comment.IsVisible = true;
                        comment = reader[7].ToString();
                    }

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        acceptedCallData.Add(reader[i].ToString());
                    }
                }
                if (acceptedCallData[4] == "1" && acceptedCallData[6] == "1")
                {
                    picker.IsEnabled = false;
                    Accepted.IsEnabled = false;
                    Alerts.IsEnabled = false;
                    Finished.IsEnabled = true;
                    CancelButton.IsEnabled = true;

                    Otkuda.Text = acceptedCallData[2];
                    Kuda.Text = acceptedCallData[3];
                    db.closeConnection();
                    return true;
                }
                if (acceptedCallData[4] == "1")
                {
                    picker.IsEnabled = false;
                    Accepted.IsEnabled = false;
                    Alerts.IsEnabled = true;
                    CancelButton.IsEnabled = true;

                    Otkuda.Text = acceptedCallData[2];
                    Kuda.Text = acceptedCallData[3];
                    db.closeConnection();
                    return true;
                }
                db.closeConnection();
                return true;
            }
            else 
            {
                db.closeConnection();
                return false;
            }
            
        }

        private void AcceptedCall_Clicked(object sender, EventArgs e)
        {
            if (picker.SelectedItem != null)
            {
                acceptedCallData.Clear();

                updateButton.IsEnabled = true;
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
                        string field = reader[7].ToString();
                        if (field != "")
                        {
                            Comment.IsEnabled = true;
                            Comment.IsVisible = true;
                            comment = reader[7].ToString();
                        }

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
                Accepted.IsEnabled = false;
                Alerts.IsEnabled = true;
                CancelButton.IsEnabled = true;

                Otkuda.Text = acceptedCallData[2];
                Kuda.Text = acceptedCallData[3];
                IncrementCall();
            }
            else DisplayAlert("Ошибка", "Выберите код заказа", "Ок");
        }

        private void IncrementCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `CountCall` FROM `driver` WHERE `Id_Driver`=@idDriver", db.getConnection());
            command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[0]);
            int count = Convert.ToInt32(command.ExecuteScalar())+1;

            MySqlCommand command2 = new MySqlCommand("UPDATE `driver` SET `CountCall`=@inc WHERE `Id_Driver`=@idDriver", db.getConnection());
            command2.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[0]);
            command2.Parameters.Add("@inc", MySqlDbType.Int32).Value = count;
            command2.ExecuteNonQuery();
        }
        private void IncrementCompletedCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `CountCompletedCall` FROM `driver` WHERE `Id_Driver`=@idDriver", db.getConnection());
            command.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[0]);
            int count = Convert.ToInt32(command.ExecuteScalar()) + 1;

            MySqlCommand command2 = new MySqlCommand("UPDATE `driver` SET `CountCompletedCall`=@inc WHERE `Id_Driver`=@idDriver", db.getConnection());
            command2.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[0]);
            command2.Parameters.Add("@inc", MySqlDbType.Int32).Value = count;
            command2.ExecuteNonQuery();
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
            Comment.IsEnabled = false;
            Comment.IsVisible = false;
            Finished.IsEnabled = false;
            DeleteFinishedCall();
            UpdateCallView();

        }
        private void DeleteFinishedCall()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("DELETE FROM `call` WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command.ExecuteNonQuery();
            db.closeConnection();
            DisplayAlert("Отлично", "Заказ завершен", "Ок");
            IncrementCompletedCall();
        }
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Comment.IsEnabled = false;
            Comment.IsVisible = false;
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE `call` SET `Accepted`=null,`Accepted_DataTime`=null,`Alerts`=null,`Finished`=null,`Driver_Id_Driver`=null WHERE `Id_Call` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(acceptedCallData[0]);
            command.ExecuteNonQuery();
            DisplayAlert("Оповещение", "Вы отменили свой заказ", "Ок");
            db.closeConnection();
            UpdateCallView();
        }
        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            UpdateCallView();
        }

        private void UpdateCallView()
        {
            updateButton.IsEnabled= true;
            Otkuda.Text = "";
            Kuda.Text = "";
            picker.IsEnabled = true;
            Accepted.IsEnabled = true;
            Alerts.IsEnabled = false;
            Finished.IsEnabled = false;
            CancelButton.IsEnabled = false;

            callView.Children.Clear();
            picker.Items.Clear();

            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `Id_Call`,`DataTime_Call`,`Otkuda`,`Kuda`,`Add_Services_Id_Services` " +
                "FROM `call` WHERE `Accepted` IS NULL and `Tarif_Id_Tarif`=@tarif", db.getConnection());
            command.Parameters.Add("@tarif", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[9]);
            MySqlDataReader reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    picker.Items.Add(reader[0].ToString());

                    string addservices;
                    switch (reader[4].ToString())
                    {
                        case ("1"):
                            addservices = "Десткое кресло";
                            break;
                        case ("2"):
                            addservices = "Зоотакси";
                            break;
                        case ("3"):
                            addservices = "Пустой багажник";
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
        private void Comment_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Коментарий от клиента", comment, "Ок");
        }
        private void Profile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VoditelAccountPage(idVoditel));
        }
    }
}
