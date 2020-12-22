using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldWordApp.DB
{
    class DataBaseConnector
    {
        private static MySqlConnection connection;
        private static MySqlCommand cmd = null;
        private static DataTable dt;
        private static MySqlDataAdapter sda;

        /*
         * Establishing connection to the MySql DataBase
         */
        public static void EstablishConnection(string ip, string userName, string password, string dbName)
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = ip;
                builder.UserID = userName;
                builder.Password = password;
                builder.Database = dbName;
                builder.SslMode = MySqlSslMode.None;
                connection = new MySqlConnection(builder.ToString());
                connection.Open();
                // if the connection succeeded it will get here. else - Exception
                MessageBox.Show("Database is connected", "Connection Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Application could not connect to DB ", "Connection Failed ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static MySqlCommand RunPlayerQuery(string query, string userId, string username, int? highScore)
        {
            try
            {
                if (connection != null)
                {
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@userid", userId);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@highscore", highScore);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception)
            {
                connection.Close();
            }
            return cmd;
        }
    }
}
