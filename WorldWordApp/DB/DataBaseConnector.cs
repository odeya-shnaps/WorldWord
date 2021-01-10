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
            }
            catch (Exception)
            {
                MessageBox.Show("Application could not connect to DB ", "Connection Failed ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // closing the DB connection
        public void CloseConnectionToDB()
        {
            connection.Close();
        }

        // sending a query related to player schema - one player
        public DataTable RunPlayerQuery(string query, string userName, int? highScore, string type)
        {
            CreateCommandForDB(query, false);

            cmd.Parameters.AddWithValue("@username", userName);
            cmd.Parameters.AddWithValue("@highscore", highScore);

            RunQuery(cmd, type);
            return dt;
        }

        // sending a query related to player schema - two users
        public DataTable RunPlayersQuery(string query, string userName1, string userName2, string type)
        {
            CreateCommandForDB(query, false);

            cmd.Parameters.AddWithValue("@username1", userName1);
            cmd.Parameters.AddWithValue("@username2", userName2);

            RunQuery(cmd, type);
            return dt;
        }
        // sending a query related to high_scores schema
        public DataTable RunHighScoresQuery(string query, int? id, string userName, int? score, string type)
        {
            CreateCommandForDB(query, false);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@username", userName);
            cmd.Parameters.AddWithValue("@score", score);
            RunQuery(cmd, type);
            return dt;
        }
        // getting all queries from 'queries schema' in DB belongs to 'category'
        public DataTable RetrieveQueryByCategory(string query, int category)
        {
            CreateCommandForDB(query, false);
            cmd.Parameters.AddWithValue("@category", category);
            RunQuery(cmd, "SELECT");
            return dt;
        }
        // creating the command will be sent to DB
        public DataTable CreateCommandForDB(string query, bool run)
        {
            cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            // run without change anything
            if (run)
            {
                RunQuery(cmd, "SELECT");
            }

            return dt;
        }
        // sending the query to DB
        private void RunQuery(MySqlCommand cmd, string type)
        {
            dt = null;
            try
            {
                if (connection != null)
                {
                    switch (type)
                    {
                        // query we want to use the data returned
                        case "SELECT":
                            // openning a reader
                            MySqlDataReader result = cmd.ExecuteReader();
                            dt = new DataTable();
                            // loading the data returned to dataTable
                            dt.Load(result);
                            // closing the reader
                            result.Close();
                            break;
                        // query we do not want to use the data returned
                        default: // INSERT, UPDATE, DELETE
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                CloseConnectionToDB();
            }
        }

    }
}
