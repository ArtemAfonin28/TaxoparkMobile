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
    public partial class TrackingPage : ContentPage
    {
        List<string> userData = new List<string>();
        List<string> call = new List<string>();
        List<string> dataDriver = new List<string>();

        bool timer = true;

        bool accepted = false;
        bool alert = false;
        bool finished = false;

        public int i = 0;

        public TrackingPage(List<string> userData2, List<string> call2)
        {
            InitializeComponent();

            userData.AddRange(userData2);
            call.AddRange(call2);

            KlientName.Text = userData2[1];

            Label1.Text = call[2].ToString();
            Label2.Text = call[3].ToString();
            OnTimerTick();
        }

        private void Tracking_Clicked(object sender, EventArgs e)
        {

        }

        private async void OnTimerTick()
        {
            if (timer)
            {
                button1.Text = i.ToString();
                i++;
                await Task.Delay(1000);
                FillTable();
            }
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
            if (call[4] != "" && accepted == false)
            {
                accepted = true;
                stateCall.Text = "Ваш заказ был принят";
                UpdateTable();
            }
            if (call[6] != "" && alert == false)
            {
                alert = true;
                stateCall.Text = "Водитель ожидавет вас";
            }
            if (call[7] != "" && finished == false)
            {
                finished = true;
                stateCall.Text = "Заказ завершен";
                timer = false;
                DeleteCall();
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
            MySqlCommand command = new MySqlCommand("SELECT `FIO_Driver`,`Model_Car`,`Register_Number` FROM `driver`,`car` WHERE `Car_Id_Car`=`Id_Car` AND `Id_Driver` =  @Id_Driver", db.getConnection());
            command.Parameters.Add("@Id_Driver", MySqlDbType.Int32).Value = Convert.ToInt32(call[10]);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataDriver.Add(reader[i].ToString());
                    }
                }
            }
            Label3.Text = dataDriver[0].ToString();
            Label4.Text = dataDriver[1].ToString();
            Label5.Text = dataDriver[2].ToString();
        }
    }
}
