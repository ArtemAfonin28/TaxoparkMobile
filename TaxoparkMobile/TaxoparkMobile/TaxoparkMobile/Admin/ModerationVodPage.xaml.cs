using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TaxoparkMobile
{
    public partial class ModerationVodPage : ContentPage
    {
        public ModerationVodPage()
        {
            InitializeComponent();

            LoadData();
        }
        private void DeleteVod_Clicked(object sender, EventArgs e)
        {
           try
           {
               DB db = new DB();
               db.openConnection();
               MySqlCommand command = new MySqlCommand("SELECT `Car_Id_Car` FROM `driver` WHERE `FIO_Driver` like @NameVod", db.getConnection());
               command.Parameters.Add("@NameVod", MySqlDbType.VarChar).Value = pickerVod.SelectedItem.ToString();
               int indexAvto = Convert.ToInt32(command.ExecuteScalar());
              
               MySqlCommand command2 = new MySqlCommand("DELETE FROM `driver` WHERE `FIO_Driver` like @NameVod", db.getConnection());
               command2.Parameters.Add("@NameVod", MySqlDbType.VarChar).Value = pickerVod.SelectedItem.ToString();
               command2.ExecuteNonQuery();
              
               MySqlCommand command3 = new MySqlCommand("DELETE FROM `car` WHERE `Id_Car`=@idAvto", db.getConnection());
               command3.Parameters.Add("@idAvto", MySqlDbType.Int32).Value = indexAvto;
               command3.ExecuteNonQuery();
              
               DisplayAlert("Успех", "Вы удалили водителя", "ОК");
               LoadData();
           }
           catch
           {
               DisplayAlert("Ошибка", "Выберите водителя", "ОК");
           }
        }

        private void LoadData()
        {
            callView.Children.Clear();
            pickerVod.Items.Clear();
            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `FIO_Driver`,`Phone_Driver`,`PassportData_Driver`,`Driver_License`,`Model_Car`,`Name_Tarif` " +
                "FROM `driver`,`tarif`,`car` WHERE `Tarif_Id_Tarif`=`Id_Tarif` and `Car_Id_Car`=`Id_Car`", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pickerVod.Items.Add(reader[0].ToString());

                    Grid grid = new Grid
                    {
                        HeightRequest= 45,
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
                        string textcall = reader[i].ToString();
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

    }
}
