using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    public class Score
    {
        public Score(string name, int score)
        {
            Name = name;
            HighScore = score;
        }
        
        public String Name { get; set; }

        public int HighScore { get; set; }
    }
}
