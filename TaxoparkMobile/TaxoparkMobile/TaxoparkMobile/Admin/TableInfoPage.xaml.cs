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
    public partial class TableInfoPage : ContentPage
    {
        string[] nameColumn;
        string sql;
        public TableInfoPage()
        {
            InitializeComponent();
        }

        private void Services_Clicked(object sender, EventArgs e)
        {
            sql = "SELECT * FROM `add_services`";
            nameColumn = new string[] { "Код услуги", "Название", "Описание", "Цена" };
            LoadData();
        }
        private void Call_Clicked(object sender, EventArgs e)
        {
            sql = "SELECT `Id_Call`, `DataTime_Call`, `Otkuda`, `Kuda`, `Accepted`, `Accepted_DataTime`, `Alerts` FROM `call` ";
            nameColumn = new string[] { "Код заказа", "Время заказа", "Откуда", "Куда", "Принят ли", "Время принятия","Подьехал ли водитель" };
            LoadData();
        }
        private void Car_Clicked(object sender, EventArgs e)
        {
            sql = "SELECT * FROM `car`";
            nameColumn = new string[] { "Код автомобиля", "Марка авто", "Регистрационный номер", "Номер кузова", "Номер двигателя", "Год выпуска", "Пробег", "Дата ТО" };
            LoadData();
        }
        private void Client_Clicked(object sender, EventArgs e)
        {
            sql = "SELECT `Id_Client`, `FIO_Client`, `Phone_Client` FROM `client`";
            nameColumn = new string[] {"Код клиента","ФИО","Номер телефона" };
            LoadData();
        }
        private void Driver_Clicked(object sender, EventArgs e)
        {
            sql = "SELECT `Id_Driver`, `FIO_Driver`, `Phone_Driver`, `PassportData_Driver`, `Driver_License`, `Car_Id_Car`, `Tarif_Id_Tarif` FROM `driver`";
            nameColumn = new string[] { "Код водителя","ФИО водителя", "Телефон", "Паспортные данные","Водительская лицензия","Код автомобиля","Код тарифа" };
            LoadData();
        }
        private void Tarif_Clicked(object sender, EventArgs e)
        {
            sql = "SELECT * FROM `tarif`";
            nameColumn = new string[] { "Код тарифа", "Название", "Описание", "Цена" };
            LoadData();
        }


        private void LoadData()
        {
            callView.Children.Clear();
            NameColumn.Children.Clear();
            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand(sql, db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                Grid grid2 = new Grid
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
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    }
                };
                for (int i = 0; i < nameColumn.Length; i++)
                {
                    string textcall2 = nameColumn[i];
                    Label label2 = new Label
                    {
                        FontSize = 12,
                        Text = textcall2,
                    };
                    label2.VerticalTextAlignment = TextAlignment.Center;
                    label2.HorizontalTextAlignment = TextAlignment.Center;
                    label2.TextColor = Color.Black;
                    label2.BackgroundColor = Color.White;
                    grid2.Children.Add(label2, i, 0);
                }
                BoxView box2 = new BoxView
                {
                    Margin = new Thickness(0, -7, 0,0),
                    HeightRequest = 4,
                };
                box2.BackgroundColor = Color.DarkOrange;
                NameColumn.Children.Add(grid2);
                NameColumn.Children.Add(box2);

                while (reader.Read())
                {

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
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        }
                    };
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string textcall = reader[i].ToString();
                        if (textcall == "")
                        {
                            textcall = "Нет";
                        }
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
