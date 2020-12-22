using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldWordApp.DB;
using WorldWordApp.Objects;

namespace WorldWordApp.Data_Access_Layer
{
    class PlayersDA
    {
        private static MySqlCommand cmd = null;
        private static DataTable dt;
        private static MySqlDataAdapter sda;


        public static List<Player> RetrieveUser(string username)
        {
            string query = "SELECT * FROM world_word_db.players where name = (@username)";
            cmd = DataBaseConnector.RunPlayerQuery(query, null, username, null);
            List<Player> aUser = new List<Player>();

            //Player[] aUser = new Player[3];
            if (cmd != null)
            {
                dt = new DataTable();
                sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string uName = dr["name"].ToString();
                    string userId = dr["userid"].ToString();
                    aUser.Add(new Player(uName, userId));
                    i++;
                }
            }
            return aUser;
        }

        public static void InsertNewUser(string username, string userId)
        {
            string query = "INSERT INTO `world_word_db`.`players` (`userid`, `name`, `high_score`) VALUES ((@userid), (@username), '0')";
            cmd = DataBaseConnector.RunPlayerQuery(query, userId, username, null);

        }
        public static void DeleteUser(string userId)
        {
            string query = "DELETE FROM `world_word_db`.`players` WHERE(`userid` = (@userid))";
            cmd = DataBaseConnector.RunPlayerQuery(query, userId, null, null);
        }
        public static void UpdateHighScore(string userId, int newScore)
        {
            string query = "UPDATE `world_word_db`.`players` SET `high_score` = (@highscore) WHERE(`userid` = (@userid))";
            cmd = DataBaseConnector.RunPlayerQuery(query, userId, null, newScore);
        }

    }
}
