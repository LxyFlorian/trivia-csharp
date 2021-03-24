using System;
using System.Collections.Generic;
using System.Text;

namespace Trivia
{
    class Player
    {
        public string nom;
        public int purse;
        public int place;
        public bool joker;
        public bool penalty;

        public Player (string nom, int place)
        {
            this.nom = nom;
            this.purse = 0;
            this.place = place;
            this.joker = false;
            this.penalty = false;
        }
    }
}
