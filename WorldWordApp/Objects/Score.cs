using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    public class Score : IEquatable<Score>, IComparable<Score>
    {
        public Score(string name, int score, int id)
        {
            Name = name;
            HighScore = score;
            Id = Id;
        }
        
        public String Name { get; set; }

        public int HighScore { get; set; }
        public int Id { get; set; }

        public int CompareTo(Score compareScore)
        {
            // A null value means that this object is greater.
            if (compareScore == null)
                return 1;
            else
                return this.HighScore.CompareTo(compareScore.HighScore);

        }

        public bool Equals(Score other)
        {
            if (other == null) return false;
            return this.HighScore.Equals(other.HighScore);
        }

    }
}
