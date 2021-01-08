using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using WorldWordApp.Data_Access_Layer;
using WorldWordApp.DB;
using WorldWordApp.Objects;

namespace WorldWordApp.Game_Logic
{
    public class GameLogic : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DataBaseConnector dbConnector;
        private PlayersDA playerDA;
        private QueryDA queryDA;
        private List<Question> questionsList;
        private Player player1;
        private Player player2;
        private string question;
        private string answer;
        private string status;
        private int numQue;
        private int numLetters;
        private string turn_of;
        private bool tie;
        private string answerWithoutDelimiters;
        private char[] delimiterChars = { ' ', ',', ':', '\n', '\t','-', '\''};

        public GameLogic()
        {
            this.dbConnector = new DataBaseConnector();
            this.queryDA = new QueryDA(dbConnector);
            this.playerDA = new PlayersDA(dbConnector);
        }

        // properties
        public string Name1
        {
            get { return player1.PlayerName; }
            set
            {
                player1.PlayerName = value;
                NotifyProperyChanged("Name1");
            }
        }
        public string Name2
        {
            get { return player2.PlayerName; }
            set
            {
                player2.PlayerName = value;
                NotifyProperyChanged("Name2");
            }
        }
        public string CurrentQuestion 
        {
            get { return question; }
            set 
            {
                question = value;
                NotifyProperyChanged("CurrentQuestion");
                //timer = new DispatcherTimer();
            } 
        }
        public string CurrentAnswer
        {
            get { return answer; }
            set
            {
                answer = value;
                NotifyProperyChanged("CurrentAnswer");
            }
        }
        public int NumLetters
        {
            get { return numLetters; }
            set
            {
                numLetters = value;
                NotifyProperyChanged("NumLetters");
            }
        }
        public int Score1
        {
            get { return player1.CurrentScore; }
            set
            {
                player1.CurrentScore = value;
                NotifyProperyChanged("Score1");
            }
        }
        public int Score2
        {
            get { return player2.CurrentScore; }
            set
            {
                player2.CurrentScore = value;
                NotifyProperyChanged("Score2");
            }
        }
        public int Life
        {
            get
            {
                if (turn_of.Equals(player1.PlayerName))
                {
                    return player1.Life;
                }
                else
                {
                    return player2.Life;
                }
            }
            set
            {
                if (turn_of == player1.PlayerName)
                {
                    player1.Life = value;
                }
                else
                {
                    player2.Life  = value;
                }
                NotifyProperyChanged("Life");
            }
        }
        public int NumQuestion 
        {
            get { return numQue; }
            set
            {
                numQue = value;
                NotifyProperyChanged("NumQuestion");
            } 
        }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyProperyChanged("Status");
            }
        }
        public string Turn
        {
            get { return "Turn of:\n" + turn_of; }
            set
            {
                turn_of = value;
                NotifyProperyChanged("Turn");
            }
        }

        // connect to the database according to the details in app.config
        public void Connect()
        {
            string ip = Properties.Settings.Default["Ip"].ToString();
            string userName = Properties.Settings.Default["UserName"].ToString();
            string password = Properties.Settings.Default["Password"].ToString();
            string dbName = Properties.Settings.Default["NameOfDB"].ToString();
            dbConnector.EstablishConnection(ip, userName, password, dbName);
        }

        // disconnect from the db
        public void CloseConnection()
        {
            dbConnector.CloseConnectionToDB();
        }

        // get 2 names of the players and create 2 players.
        private void AddPlayers(string name1, string name2) 
        {
            player1 = new Player(name1);
            player2 = new Player(name2);
        }

        // get the question to the game from the db.
        private void GetQuestions(string[] categories)
        {
           questionsList = queryDA.QuestionsGeneration(categories);
           //questionsList = new List<Question>() {new Question("what is your name?", "Odeya"), new Question("how are you today?", "fine"), new Question("how old are you?","23"), new Question("tired?","yes"), new Question("what is your last name?","shnaps"),new Question("how long?","forever"), new Question("time?","15:30")};
        }

        // At the end of each game we insert the new player to the players table
        // and update the score of old players if the current score they achieve in the game is higher.
        public void UpdateOrInsertPlayers()
        {
            List<Player> dbPlayers = playerDA.RetrieveUser(player1.PlayerName, player2.PlayerName);
            // the two players are new
            if (dbPlayers.Count == 0)
            {
                playerDA.InsertNewUser(player1.PlayerName, player1.CurrentScore);
                playerDA.InsertNewUser(player2.PlayerName, player2.CurrentScore);
            }
            // at least one old player
            else
            {
                Player oldPlayer1 = null;
                Player oldPlayer2 = null;
                if (player1.PlayerName.Equals(dbPlayers[0].PlayerName))
                {
                    oldPlayer1 = dbPlayers[0];
                }
                else
                {
                    oldPlayer2 = dbPlayers[0];
                }

                // only one player is new
                if (dbPlayers.Count == 1)
                {
                    // player 1 is old and player 2 is new
                    if (oldPlayer1 != null)
                    {
                        // insert player 2
                        playerDA.InsertNewUser(player2.PlayerName, player2.CurrentScore);
                        // check if we need to update player 1 score
                        if (player1.CurrentScore > oldPlayer1.UserHighScore)
                        {
                            playerDA.UpdateHighScore(player1.PlayerName, player1.CurrentScore);
                        }

                    }
                    // player 2 is old and player 1 is new
                    else
                    {
                        // insert player 1
                        playerDA.InsertNewUser(player1.PlayerName, player1.CurrentScore);
                        // check if we need to update player 2 score
                        if (player2.CurrentScore > oldPlayer2.UserHighScore)
                        {
                            playerDA.UpdateHighScore(player2.PlayerName, player2.CurrentScore);
                        }
                    }
                }
                // none of the players are new
                else
                {
                    if (oldPlayer1 == null)
                    {
                        oldPlayer1 = dbPlayers[1];
                    }
                    else
                    {
                        oldPlayer2 = dbPlayers[1];
                    }
                    // check if we need to update player 1 score
                    if (player1.CurrentScore > oldPlayer1.UserHighScore)
                    {
                        playerDA.UpdateHighScore(player1.PlayerName, player1.CurrentScore);
                    }
                    // check if we need to update player 2 score
                    if (player2.CurrentScore > oldPlayer2.UserHighScore)
                    {
                        playerDA.UpdateHighScore(player2.PlayerName, player2.CurrentScore);
                    }
                }
            } 
        }

        // At the end of each game we enter the new scores to high score table
        // if they are part of the 10th highest score that were achieved. 
        public void UpdateHighScores()
        {
            List<Score> highScores = playerDA.GetHighScoresList();
            if (highScores.Count <= 8)
            {
                playerDA.AddToHighScoresList(player1.PlayerName, player1.CurrentScore);
                playerDA.AddToHighScoresList(player2.PlayerName, player2.CurrentScore);
            }
            else
            {
                Player winner, loser;
                int lowestIndx;
                if (player1.CurrentScore >= player2.CurrentScore)
                {
                    winner = player1;
                    loser = player2;
                }
                else
                {
                    winner = player2;
                    loser = player1;
                }
                if (highScores.Count == 9)
                {
                    lowestIndx = 8;
                    playerDA.AddToHighScoresList(winner.PlayerName, winner.CurrentScore);
                    if (highScores[lowestIndx].HighScore <= loser.CurrentScore)
                    {
                        playerDA.DeleteFromHighScoreList(highScores[lowestIndx].Id);
                        playerDA.AddToHighScoresList(loser.PlayerName, loser.CurrentScore);
                    }
                }
                else
                {
                    lowestIndx = 9;
                    if (highScores[lowestIndx].HighScore <= winner.CurrentScore)
                    {
                        playerDA.DeleteFromHighScoreList(highScores[lowestIndx].Id);
                        playerDA.AddToHighScoresList(winner.PlayerName, winner.CurrentScore);
                        lowestIndx = 8;
                        if (highScores[lowestIndx].HighScore <= loser.CurrentScore)
                        {
                            playerDA.DeleteFromHighScoreList(highScores[lowestIndx].Id);
                            playerDA.AddToHighScoresList(loser.PlayerName, loser.CurrentScore);
                        }
                    }
                }
            }
        }

        // get all the scores in high score table in descending order.
        public List<Score> GetHighScores()
        {
            return playerDA.GetHighScoresList();
        }

        // get all the players fron players table
        public List<Player> GetAllPlayers()
        {
            return playerDA.GetPlayersList();
        }

        // preparations for the game.
        public void StartGame(string name1, string name2, string[] categories)
        {
            AddPlayers(name1, name2);
            GetQuestions(categories);
            numQue = 0;
            Score1 = 0;
            Score2 = 0;
            status = "";
            question = "";
            turn_of = player1.PlayerName;
        }

        // checking if the player answer is correct
        public bool IsCorrectAnswer(string playerAnswer, int seconds)
        {
            string splittedPlayerAns = "";
            string[] words = playerAnswer.Split(delimiterChars);
            foreach (string word in words)
            {
                splittedPlayerAns += word;
            }
            if (splittedPlayerAns.ToUpper().Equals(answerWithoutDelimiters.ToUpper()))
            {
                Status = "Good job!";
                if (turn_of.Equals(player1.PlayerName))
                {
                    Score1 = Score1 + 500 + seconds * 10;
                }
                else
                {
                    Score2 = Score2 + 500 + seconds * 10;
                }
                return true;
            }
            Status = "Try Again...";
            return false;
        }

        // ask the next question
        public void AskQuestion()
        {
            answerWithoutDelimiters = "";
            Status = "";
            NumQuestion += 1;
            Question q = questionsList[NumQuestion - 1];
            CurrentQuestion = q.QuestionString + "?";
            CurrentAnswer = q.AnswerString;
            string ans = CurrentAnswer;
            // remove the unimportant punctuation and spaces from answer.
            string[] words = ans.Split(delimiterChars);
            foreach (string word in words)
            {
                answerWithoutDelimiters += word;
            }
            NumLetters = answer.Length;
        }

        // change to the next question, it's one of the life saver the player can use.
        public void ChangeQuestion()
        {
            Life = Life - 1;
            AskQuestion();
        }

        //chang the turns of the players.
        public void ChangeTurn()
        {
            if (turn_of.Equals(player1.PlayerName))
            {
                Turn = player2.PlayerName;
            }
            else
            {
                Turn = player1.PlayerName;
            }
            NotifyProperyChanged("Life");
        }

        // At the end of each game we update high scores if needed and insert new players 
        // or update old players score if needed in the db.
        public void EndeGame()
        {
            UpdateOrInsertPlayers();
            UpdateHighScores();
        }

        // get the winner and loser or tie.
        public List<Player> GetWinnerAndLoser()
        {
            tie = false;
            List<Player> order = new List<Player>();
            if (player1.CurrentScore > player2.CurrentScore)
            {
                order.Add(player1);
                order.Add(player2);
            }
            else if (player1.CurrentScore < player2.CurrentScore){
                order.Add(player2);
                order.Add(player1);
            }
            else
            {
                order.Add(player1);
                order.Add(player2);
                tie = true;
            }
            return order;
        }

        public bool IsTie()
        {
            return tie;
        }

        // we bind the properties with WPF controls in the view, so when one of the properties change we inform the view.
        public void NotifyProperyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
