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
            aGame.Add("Foo");
            aGame.Add("Paul");
            aGame.Add("Florian");
            aGame.Add("Yoann");

            var rand = new Random();

            //Check if the game is playable.
            if (!aGame.IsPlayable())
            {
                Console.WriteLine("Il faut minimum 2 joueurs et maximum 6 joueurs pour jouer");
                Console.ReadLine();
                Environment.Exit(0);
            }

            do
            {
                aGame.Roll(rand.Next(5) + 1);
                _notAWinner = true;

                if (aGame.UseJoker())
                {
                    if (rand.Next(9) > 7)
                    {
                        _notAWinner = aGame.WrongAnswer();
                    }
                    else
                    {
                        _notAWinner = aGame.WasCorrectlyAnswered();
                    }
                }
                
            } while (_notAWinner);
            aGame.Statistiques();
            Console.ReadLine();
        }
    }
}