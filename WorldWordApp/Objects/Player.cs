using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    public class Player
    {

        public Player(string name)
        {
            PlayerName = name;
            CurrentScore = 0;
            Life = 2;
        }

        public Player(string name, int highScore)
        {
            PlayerName = name;
            UserHighScore = highScore;
        }

        public String PlayerName { get; set; }

        // the last high score
        public int UserHighScore { get; set; }

        public String UserId { get; set; }
        // the score that the player achieve in the game
        public int CurrentScore { get; set; }

        public int Life { get; set; }
    }
}
