using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DartScorer
{
    public class Match
    {
        private int sets;
        private int legsPerSet;
        private int startScore;
        //List<Player> players;
        Player setThrower;
        Player legThrower;
        Game game;
        String statusMessage;
        String throwerMessage;

        public Game Game
        {
            get { return this.game; }
            set { this.game = value; }
        }

        public int LegsPerSet
        {
            get { return this.legsPerSet; }
            set { this.legsPerSet = value; }
        }

        public int Sets 
        {
            get { return this.sets; }
            set { this.sets = value; }
        }

        public Player LegThrower
        {
            get { return this.legThrower; }
            set { this.legThrower = value; }
        }

        public Player SetThrower
        {
            get { return this.setThrower; }
            set { this.setThrower = value; }
        }

        public int StartScore
        {
            get { return this.startScore; }
            set { this.startScore = value; }
        }

        //public List<Player> Players
        //{
        //    get { return this.players; }
        //}

        public Match()
        {
            //this.players = new List<Player>();
        }

        [JsonConstructor]
        public Match(Game game)
        {
            //this.players = new List<Player>();
            this.game = game;
            this.throwerMessage = this.game.Thrower.Name + " to throw";
        }

        public String ThrowerMessage
        {
            get { return this.throwerMessage; }
        }

        public String StatusMessage 
        {
            get { return this.statusMessage;  }    
        }

        //[JsonConstructor]
        public Match(String player1Name, String player2Name, int sets, int legsPerSet, int startScore)
        {
            Player player1 = new Player(player1Name, startScore);
            Player player2 = new Player(player2Name, startScore);
            this.sets = sets;
            this.legsPerSet = legsPerSet;
            this.startScore = startScore;
            //this.players = new List<Player>();
            //this.players.Add(player1);
            //this.players.Add(player2);

            //this.game = new Game(startScore, players, sets, legsPerSet);
            this.game = new Game(startScore, player1, player2, sets, legsPerSet);
            this.setThrower = game.Thrower;
            this.legThrower = game.Thrower;
            this.throwerMessage = legThrower.Name + " to throw";
        }

        public int SetsNeededToWinMatch()
        {
            return (this.sets / 2) + 1;
        }

        public int LegsNeededToWinSet()
        {
            //int x = this.legsPerSet / 2;
            //int y = x + 1;
            //return y;

            return (this.legsPerSet / 2) + 1;
        }

        public bool SetWon()
        {
            //if ((game.Player1.LegsWon == this.LegsNeededToWinSet()) ||
                    //(game.Player2.LegsWon == this.LegsNeededToWinSet()))
            if (game.Winner().LegsWon == this.LegsNeededToWinSet())
            {
                game.Winner().SetsWon++;
                return true;
            }
            return false;
        }

        public bool MatchWon()
        {
            return ((game.Player1.SetsWon == SetsNeededToWinMatch()) ||
                    (game.Player2.SetsWon == SetsNeededToWinMatch()));
        }

        private void NewSet()
        {
            game.Player1.LegsWon = 0;
            game.Player2.LegsWon = 0;
            this.NewGame();
            game.Thrower = this.SwitchThrower(this.setThrower);
            this.setThrower = game.Thrower;
            this.LegThrower = game.Thrower;
        }

        private void NewGame()
        {
            game.Player1.ResetScores(this.startScore);
            game.Player2.ResetScores(this.startScore);
            //game = new Game(startScore, players, sets, legsPerSet);
            game = new Game(startScore, game.Player1, game.Player2, sets, legsPerSet);
            this.throwerMessage = game.Thrower.Name + " to throw";
        }

        public void NewLeg()
        {
            this.NewGame();
            game.Thrower = this.SwitchThrower(this.legThrower);
            this.legThrower = game.Thrower;
            this.throwerMessage = "Next leg, " + game.Thrower.Name + " to throw";
        }

        private void Turn(Player player, int score)
        {

            Throw playerThrow = new Throw(score);
            if (playerThrow.IsValid())
            {
                if (player.IsBust(playerThrow))
                {
                    this.statusMessage = "Bust!";
                }
                else
                {
                    //player.ThrowDarts(playerThrow);
                    //game.Thrower.ThrowDarts(playerThrow);
                    game.ProcessThrow(playerThrow);
                }
                //this.game.ChangeThrower();
            } else {
                this.statusMessage = "Invalid Score Entered!";
            }
        }

        private Player SwitchThrower(Player thrower)
        {
            if (thrower.Equals(game.Player1))
            //if (thrower.Name == game.Player1.Name)
            {
                thrower = game.Player2;
            }
            else
            {
                thrower = game.Player1;
            }
            return thrower;
        }

        public void Play()
        {
            while (this.MatchWon() == false)
            {
                this.PlayLeg();
                this.LegWon();
            }
        }

        private void PlayLeg()
        {
            while (!game.IsWon())
            {
                //this.Turn(game.Thrower);
                this.game.ChangeThrower();
            }
        }

        private void LegWon()
        {
            game.Winner().LegsWon++;
            if (this.SetWon())
            {
                if (this.MatchWon())
                {
                    gameWon("match");
                    return;
                }
                else
                {
                    // TODO: game shot and the SET
                    gameWon("set");
                    this.NewSet();
                }
            }
            else
            {
                // TODO: game shot and the LEG
                gameWon("leg");
                this.NewLeg();
            }
        }

        public void ProcessScore(int score)
        {
            if (!MatchWon())
            {
                if (!game.IsWon())
                {
                    this.Turn(game.GetThrower(), score);
                    this.game.ChangeThrower();
                    if (game.IsWon())
                    {
                        LegWon();
                    }
                    else
                    {
                        //this.game.changeThrower();
                        if (game.Thrower.IsOnAFinish())
                        {
                            //TODO: set up '... you require ...'
                            this.statusMessage = game.Thrower.Name + ", you require " + game.Thrower.CurrentScore;
                        }
                        else
                        {
                            //TODO: set up '... to throw"
                            this.statusMessage = game.Thrower.Name + " to throw";
                        }
                    }
                }
            }
        }

        private void gameWon(String msg)
        {
            this.statusMessage = "Game Shot, and the " + msg + " to " + game.Winner().Name;
        }
    }
}
