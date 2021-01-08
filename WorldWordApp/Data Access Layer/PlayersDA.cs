using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldWordApp.DB;
using WorldWordApp.Objects;

namespace WorldWordApp.Data_Access_Layer
{
    class PlayersDA
    {
        private static DataTable dt;
        private DataBaseConnector dbConnector;
        private string db_name;


        public PlayersDA(DataBaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
            db_name = Properties.Settings.Default["NameOfDB"].ToString();
        }


        public List<Player> RetrieveUser(string userName1, string userName2)
        {
            string query = "SELECT * FROM " + db_name + ".players WHERE name = (@username1) OR name = (@username2)";
            dt = dbConnector.RunPlayersQuery(query, userName1, userName2, "SELECT");
            List<Player> aUser = new List<Player>();

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string uName = dr["name"].ToString();
                    int userScore = Convert.ToInt32(dr["high_score"]);
                    aUser.Add(new Player(uName, userScore));
                }
            }
            return aUser;
        }

        public void InsertNewUser(string userName, int score)
        {
            string query = "INSERT INTO `" + db_name + "`.`players` (`name`, `high_score`) VALUES ((@username), (@highscore))";
            dbConnector.RunPlayerQuery(query, userName, score, "INSERT");
        }

        public void DeleteUser(string userName)
        {
            string query = "DELETE FROM `" + db_name + "`.`players` WHERE(`name` = (@username))";
            dbConnector.RunPlayerQuery(query, userName, null, "DELETE");
        } 
        public void UpdateHighScore(string userName, int newScore)
        {
            string query = "UPDATE `" + db_name + "`.`players` SET `high_score` = (@highscore) WHERE(`name` = (@username))";
            dbConnector.RunPlayerQuery(query, userName, newScore, "UPDATE");
        }

        // adding new score record to high_scores table
        public void AddToHighScoresList(string userName, int score)
        {
            string query = "INSERT INTO `" + db_name + "`.`high_scores` (`player`, `score`) VALUES ((@username), (@score))";
            dbConnector.RunHighScoresQuery(query, null, userName, score, "INSERT");
        }

        public List<Score> GetHighScoresList()
        {
            List<Score> scores = new List<Score>();

            string query = "SELECT * FROM " + db_name + ".high_scores;";
            dt = dbConnector.CreateCommandForDB(query, true);

            // there are scores records in db
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    string uName = dr["player"].ToString();
                    int userScore = Convert.ToInt32(dr["score"]);
                    scores.Add(new Score(uName, userScore, id));
                }
            }
            else
            {
               // MessageBox.Show("no previous games", "no data in db", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            scores.Sort();
            scores.Reverse();
            return scores;
        }

        // delete high_score from high_score list
        public void DeleteFromHighScoreList(int id)
        {
            string query = "DELETE FROM " + db_name + ".high_scores WHERE id = (@id);";
            dbConnector.RunHighScoresQuery(query, id, null, null, "DELETE");
        }


        public List<Player> GetPlayersList()
        {
            List<Player> players = new List<Player>();

            string query = "SELECT * FROM " + db_name + ".players;";
            dt = dbConnector.CreateCommandForDB(query, true);

            // there are players in db
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string uName = dr["name"].ToString();
                    int userHighScore = Convert.ToInt32(dr["high_score"]);
                    players.Add(new Player(uName, userHighScore));
                }
            }
            else
            {
                MessageBox.Show("There are no previous players", "no data in db", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return players;

        }


    }
}
