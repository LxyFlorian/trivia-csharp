using System;

namespace Trivia
{
    public class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            var aGame = new Game();

            aGame.Add("Chet");

            var rand = new Random();

            //Check if the game is playable.
            if (!aGame.IsPlayable())
            {
                Console.WriteLine("The game is unplayable because there is less than 2 players or more than 6 players");
                Environment.Exit(0);
            }

            do
            {
                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    _notAWinner = aGame.WrongAnswer();
                }
                else
                {
                    _notAWinner = aGame.WasCorrectlyAnswered();
                }
            } while (_notAWinner);
        }
    }
}