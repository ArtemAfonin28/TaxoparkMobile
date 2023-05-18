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
        public MainVoditelPage(List<string> voditelData2)
        {
            InitializeComponent();

            voditelData.AddRange(voditelData2);
            VoditelName.Text= voditelData2[1];
            UpdateCallView();

        }

        private async void UpdateCallView()
        {
            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `Id_Call`,`DataTime_Call`,`Otkuda`,`Kuda`,`Add_Services_Id_Services` " +
                "FROM `call` WHERE `Accepted` IS NULL and `Tarif_Id_Tarif`=@tarif", db.getConnection());
            command.Parameters.Add("@tarif", MySqlDbType.Int32).Value = Convert.ToInt32(voditelData[7]);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                await DisplayAlert("", "чето есть", "Прикол ебать");
                while (reader.Read())
                {
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
