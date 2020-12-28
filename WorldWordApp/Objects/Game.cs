using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    class Game
    {

        public Game(Player player1, Player player2, List<Question> questions)
        {
            Player1 = player1;
            Player2 = player2;
            Questions = questions;
        }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public List<Question> Questions { get; set; }

    }
}
