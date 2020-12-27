using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    class Player
    {
        public Player(string name, int highScore)
        {
            PlayerName = name;
            UserHighScore = highScore;
        }

        public String PlayerName { get; set; }

        public int UserHighScore { get; set; }
    }
}
