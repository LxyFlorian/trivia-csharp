using System;
using System.Collections.Generic;

namespace Trivia
{
    public class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            int count = 0;

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

                if (!_notAWinner)
                {
                    Console.WriteLine("Un joueur vient de monter sur le podium.");
                    count++;
                }

            } while (count < 3);
            aGame.Statistiques();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Il y a maintenant 3 gagnants");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}