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
    public partial class ModerationCallPage : ContentPage
    {
        string pickerCallText;
        public ModerationCallPage()
        {
            InitializeComponent();

            LoadData();
            FillDriverPicker();
        }
        private void LoadData()
        {
            Call.Items.Clear();
            callView.Children.Clear();
            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `Id_Call`,`DataTime_Call`,`Otkuda`,`Kuda`,`FIO_Client`,`FIO_Driver`,`Name_Services` " +
                "FROM `call`,`client`,`driver`,`add_services` " +
                "WHERE `Client_Id_Client`=`Id_Client` and `Add_Services_Id_Services`=`Id_Services` and `Driver_Id_Driver`=`Id_Driver` and `Accepted` = 1", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pickerCallText = "";

                    Grid grid = new Grid
                    {
                        HeightRequest = 45,
                        RowSpacing = -45,
                        ColumnSpacing = 4,
                        BackgroundColor = Color.DarkOrange,
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
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        }
                    };
                    for (int i = 0; i < 6; i++)
                    {
                        string textcall = reader[i+1].ToString();
                        pickerCallText = reader[0].ToString() + " " + reader[2].ToString() + "  -->  " + reader[3].ToString();
                        Label label1 = new Label
                        {
                            FontSize = 12,
                            Text = textcall,
                        };
                        label1.VerticalTextAlignment = TextAlignment.Center;
                        label1.HorizontalTextAlignment = TextAlignment.Center;
                        label1.TextColor = Color.Black;
                        label1.BackgroundColor = Color.LightGray;
                        grid.Children.Add(label1, i, j);
                    }
                    Call.Items.Add(pickerCallText);
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
            db.closeConnection();
        }
        private void ReplaceDriver_Clicked(object sender, EventArgs e)
        {
            string id = "";

            DB db = new DB();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("SELECT `Id_Driver` FROM `driver` WHERE `FIO_Driver`=@FIO", db.getConnection());
            command.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = Driver.SelectedItem.ToString();
            int idDriver = Convert.ToInt32(command.ExecuteScalar());


            MySqlCommand command2 = new MySqlCommand("UPDATE `call` SET `Driver_Id_Driver`=@idDriver WHERE `Id_Call`=@idCall", db.getConnection());
            command2.Parameters.Add("@idDriver", MySqlDbType.Int32).Value = idDriver;

            string CallPicker = Call.SelectedItem.ToString();
            for (int i = 0; i < CallPicker.Length; i++)
            {
                if (CallPicker[i].ToString() == " ")
                    break;
                id += CallPicker[i].ToString();
            }

            command2.Parameters.Add("@idCall", MySqlDbType.VarChar).Value = Convert.ToInt32(id);
            try
            {
                command2.ExecuteNonQuery();
            }
            catch
            {
                DisplayAlert("Ошибка", "Этот заказ уже завершен", "ок");
            }
            db.closeConnection();
            LoadData();
        }

        private void FillDriverPicker()
        {
            Driver.Items.Clear();
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `FIO_Driver` FROM `driver`", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Driver.Items.Add(reader[0].ToString());
                }
            }
        }
    }
}
