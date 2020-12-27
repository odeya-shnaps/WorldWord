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
        private DataBaseConnector dbConnector;


        public PlayersDA(DataBaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
        }


        public List<Player> RetrieveUser(string userName)
        {
            string query = "SELECT * FROM world_word_db.players where name = (@username)";
            dt = dbConnector.RunPlayerQuery(query, userName, null, "SELECT");
            List<Player> aUser = new List<Player>();
            
            if (dt.Rows.Count != 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string uName = dr["name"].ToString();
                    int userScore = Convert.ToInt32(dr["high_score"]);
                    aUser.Add(new Player(uName, userScore));
                    i++;
                }
            }
            return aUser;
        }

        public void InsertNewUser(string userName)
        {
            string query = "INSERT INTO `world_word_db`.`players` (`name`, `high_score`) VALUES ((@username), '0')";
            dbConnector.RunPlayerQuery(query, userName, null, "INSERT");

        }
        public void DeleteUser(string userName)
        {
            string query = "DELETE FROM `world_word_db`.`players` WHERE(`name` = (@username))";
            dbConnector.RunPlayerQuery(query, userName, null, "DELETE");
        }
        public void UpdateHighScore(string userName, int newScore)
        {
            string query = "UPDATE `world_word_db`.`players` SET `high_score` = (@highscore) WHERE(`name` = (@username))";
            dbConnector.RunPlayerQuery(query, userName, newScore, "UPDATE");
        }

    }
}
