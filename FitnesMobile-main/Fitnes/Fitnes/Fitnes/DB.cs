using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxoparkMobile
{
    internal class DB
    {
        MySqlConnection connection = new MySqlConnection("server=triniti.ru-hoster.com; uid=v6FZ;port=3306;pwd=n2m066nOvK;database=v6FZ; convert zero datetime=True");

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
