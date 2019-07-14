﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DartScorer
{
    public class Player
    {
        private String name;
        private int currentScore;
        private int legsWon;
        private int setsWon;
        private List<int> scores;

        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int CurrentScore
        {
            get { return this.currentScore; }
            set { this.currentScore = value; }
        }

        public int LegsWon
        {
            get { return this.legsWon; }
            set { this.legsWon = value; }
        }

        public int SetsWon
        {
            get { return this.setsWon; }
            set { this.setsWon = value; }
        }

        public Player() {
            this.scores = new List<int>();
        }

        [JsonConstructor]
        public Player(String name, int startScore)
        {
            this.name = name;
            this.legsWon = 0;
            this.setsWon = 0;
            this.ResetScores(startScore);
        }

        public void ResetScores(int startScore)
        {
            this.currentScore = startScore;
            this.scores = new List<int>();
        }


        public void ThrowDarts(Throw t)
        {
            if (t.IsValid() && !IsBust(t))
            {
                this.currentScore -= t.Score;
                this.scores.Add(this.currentScore);
            }
        }

        public bool IsBust(Throw t)
        {
            if ((t.Score > this.currentScore) || ((this.currentScore - t.Score) == 1))
            {
                return true;
            }
            return false;
        }

        public bool IsOnAFinish()
        {
            int[] highFinishes = { 170, 167, 164, 161, 160 };

            if ((Array.IndexOf(highFinishes, this.currentScore) > -1) || (this.currentScore < 159))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsWinningScore(Throw t)
        {
            if (t.Score == this.currentScore)
            {
                currentScore = 0;
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Player;
            if (other == null) {
                return false;
            }

            if (string.Compare(this.name,other.name) != 0)
            {
                return false;
            }
            return true;
        }

    }
}
