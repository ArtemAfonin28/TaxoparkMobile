using MySqlConnector;
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
    public partial class VoditelAccountPage : ContentPage
    {
        int id;
        public VoditelAccountPage(int idVoditel)
        {
            InitializeComponent();
            id = idVoditel;
            LoadData();
        }
        private void LoadData()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `FIO_Driver`,`Phone_Driver`,`Model_Car`,`Register_Number`,`Year_Release`,`Data_TO`, `CountCall`, `CountCompletedCall` FROM `driver`,`car` WHERE `Car_Id_Car`=`Id_Car` and `Id_Driver`=@id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Name.Text = reader[0].ToString();
                    Phone.Text = reader[1].ToString();
                    Marka.Text = reader[2].ToString();
                    Number.Text = reader[3].ToString();
                    Year.Text = reader[4].ToString();
                    YearTO.Text = reader[5].ToString();
                    CountCall.Text = reader[6].ToString();
                    CountCompletedCall.Text = reader[7].ToString();
                }
            }
        }
    }
}
