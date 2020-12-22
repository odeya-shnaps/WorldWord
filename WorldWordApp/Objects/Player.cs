using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldWordApp.Objects
{
    class Player
    {
        public Player(string name, string userId)
        {
            PlayerName = name;
            UserId = userId;
        }

        public String PlayerName { get; set; }

        public String UserId { get; set; }
    }
}
