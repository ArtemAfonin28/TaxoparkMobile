using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxoparkMobile
{
    internal class DB
    {
        //MySqlConnection connection = new MySqlConnection("server=triniti.ru-hoster.com; uid=taxop4ps;port=3306;pwd=o1rlbE452J;database=taxop4ps; convert zero datetime=True");
        MySqlConnection connection = new MySqlConnection("server=server113.hosting.reg.ru;port=3306;username=u2077712_Savan28;password=Savan282; database=u2077712_Taxopark; charset=utf8");
        
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        public MySqlConnection getConnection()
        {
            return connection;
        }

    }
}
