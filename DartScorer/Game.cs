﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DartScorer
{
    public class Game
    {
        private Player player1;
        private Player player2;
        private Player thrower;
        private int startScore;
        private int sets;
        private int legsPerSet;

        public int StartScore
        {
            get { return this.startScore; }
            set { this.startScore = value;  }
        }

        public int Sets
        {
            get { return this.sets; }
            set { this.sets = value; }
        }

        public int LegsPerSet
        {
            get { return this.legsPerSet; }
            set { this.legsPerSet = value; }
        }

        public Player Thrower
        {
            get { return this.thrower; }
            set { this.thrower = value; }
        }

        public Player Player1
        {
            get { return this.player1; }
            set { this.player1 = value;  }
        }

        public Player Player2
        {
            get { return this.player2; }
            set { this.player2 = value; }
        }

        //public Game() {
            
        //}
        [JsonConstructor]
        public Game(int startScore, Player player1, Player player2, int sets, int legsPerSet)
        {
            this.startScore = startScore;
            this.sets = sets;
            this.legsPerSet = legsPerSet;
            this.player1 = player1;
            this.player2 = player2;
            this.thrower = this.player1;
        }

        public Game(Player player1, Player player2)
        {
            this.startScore = 501;
            this.player1 = player1;
            this.player2 = player2;
            this.thrower = this.player1;
        }

        public void ChangeThrower()
        {
            if (this.thrower.Equals(this.player1))
            //if (this.thrower.Name == this.player1.Name)
            {
                this.thrower = this.player2;
            }
            else
            {
                this.thrower = this.player1;
            }
        }

        public Player GetThrower()
        {
            if (this.thrower.Equals(this.player1))
            //if (this.thrower.Name == this.player1.Name)
            {
                return player1;
            }
            else
            {
                return player2;
            }
        }

        public bool IsWon()
        {
            return ((player1.CurrentScore == 0) || (player2.CurrentScore == 0));
        }

        public Player Winner()
        {
            if (player1.CurrentScore == 0)
            {
                return player1;
            }
            else if (player2.CurrentScore == 0)
            {
                return player2;
            }
            return null;
        }

        public void ProcessThrow(Throw playerThrow)
        {
            if (this.thrower.Equals(this.player1))
            {
                this.player1.ThrowDarts(playerThrow);
            }
            else if (this.thrower.Equals(this.player2))
            {
                this.player2.ThrowDarts(playerThrow);
            }
            //this.thrower.ThrowDarts(playerThrow);
        }

    }
}
