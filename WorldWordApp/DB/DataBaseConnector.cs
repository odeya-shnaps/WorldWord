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
        //private static MySqlDataAdapter sda;

        /*
         * Establishing connection to the MySql DataBase
         */
        public void EstablishConnection(string ip, string userName, string password, string dbName)
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

        public DataTable RunPlayerQuery(string query, string userName, int? highScore, string type)
        {
            CreateCommandForDB(query, false);

            cmd.Parameters.AddWithValue("@username", userName);
            cmd.Parameters.AddWithValue("@highscore", highScore);

            RunQuery(cmd, type);
            return dt;
        }

        public DataTable RunPlayersQuery(string query, string userName1, string userName2, string type)
        {
            CreateCommandForDB(query, false);

            cmd.Parameters.AddWithValue("@username1", userName1);
            cmd.Parameters.AddWithValue("@username2", userName2);

            RunQuery(cmd, type);
            return dt;
        }

        public DataTable RetrieveQueryByCategory(string query, int category)
        {
            CreateCommandForDB(query, false);
            cmd.Parameters.AddWithValue("@category", category);

            RunQuery(cmd, "SELECT");
            return dt;
        }

        public DataTable CreateCommandForDB(string query, bool run)
        {
            cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            MySqlDataReader result = null;
            // run without change anything
            if (run)
            {
                RunQuery(cmd, "SELECT");
            }

            return dt;
        }

        private void RunQuery(MySqlCommand cmd, string type)
        {
            dt = null;
            try
            {
                if (connection != null)
                {
                    connection.Open();
                    switch (type)
                    {
                        case "SELECT":
                            MySqlDataReader result = cmd.ExecuteReader();
                            dt = new DataTable();
                            dt.Load(result);
                            result.Close();
                            break;
                        default: // INSERT, UPDATE, DELETE
                            cmd.ExecuteNonQuery();
                            break;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                connection.Close();
            }
        }

    }
}
