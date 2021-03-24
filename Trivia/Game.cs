﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];
        private readonly int[] bonus = new int[6];
        private readonly bool[] joker = new bool[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly int[] countQuestion = new int[] {0, 0, 0, 0, 0 };

        private String selectMode;
        private String selectNextCategory;
        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public int pointToWin = 0;

        public Game()
        {
            Console.WriteLine("How much point to win ?");
            Console.WriteLine("Minimum 6");
            do
            {
                string line = Console.ReadLine();
                if (Int32.TryParse(line,out pointToWin))
                {
                    if(pointToWin >= 6)
                    {
                        Console.WriteLine("You will play with win point to " + pointToWin + ".");
                    }
                    else
                    {
                        Console.WriteLine("Win point at " + pointToWin + " is not enough.");
                    }
                }
                else
                {
                    Console.WriteLine("Value is invalide.");
                }
            } while (pointToWin < 6);


            string rockortechno = "";
            do
            {
                Console.WriteLine("Rock ou Techno ?");
                rockortechno = Console.ReadLine();
            } while (rockortechno != "Rock" && rockortechno != "Techno");
            selectMode = rockortechno;

        }

        /// <summary>
        /// Check if the game is playable. Check if number of player is >= 2 and <= 6.
        /// </summary>
        /// <returns>Boolean</returns>

        public bool IsPlayable()
        {
            int count = HowManyPlayers();
            return (count >= 2 && count <= 6);
        }


        public bool Add(string playerName)
        {
            if (HowManyPlayers() < 5)
            {
                _players.Add(playerName);
                _places[HowManyPlayers()] = 0;
                _purses[HowManyPlayers()] = 0;
                bonus[HowManyPlayers()] = 0;
                joker[HowManyPlayers()] = false;
                _inPenaltyBox[HowManyPlayers()] = false;

                Console.WriteLine(playerName + " was added");
                Console.WriteLine("They are player number " + _players.Count);
            }
            else
            {
                Console.WriteLine("Cannot add player, only 6 players can be added to the game.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            
            return true;
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            Console.WriteLine("Voulez vous quitter ?");
            Console.WriteLine("Toucher ESC");

            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                Console.WriteLine(_players[_currentPlayer] + "est sorti");
                _players.RemoveAt(_currentPlayer);
                if (!IsPlayable())
                {
                    Console.WriteLine("La partie est terminé");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Le joueur continu à jouer");
            }

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;
                    _inPenaltyBox[_currentPlayer] = false;

                    Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;
                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                    Console.WriteLine(_players[_currentPlayer]
                            + "'s new location is "
                            + _places[_currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                Console.WriteLine(_players[_currentPlayer]
                        + "'s new location is "
                        + _places[_currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                int count = countQuestion[0];
                count++;
                countQuestion[0] = count;
                Console.WriteLine("Pop Question " + countQuestion[0]);
            }
            if (CurrentCategory() == "Science")
            {
                int count = countQuestion[1];
                count++;
                countQuestion[1] = count;
                Console.WriteLine("Science Question " + countQuestion[1]);
            }
            if (CurrentCategory() == "Sports")
            {
                int count = countQuestion[2];
                count++;
                countQuestion[2] = count;
                Console.WriteLine("Sports Question " + countQuestion[2]);
            }
            if (CurrentCategory() == "Rock")
            {
                int count = countQuestion[3];
                count++;
                countQuestion[3] = count;
                Console.WriteLine("Rock Question " + countQuestion[3]);
            }
            if (CurrentCategory() == "Techno")
            {
                int count = countQuestion[4];
                count++;
                countQuestion[4] = count;
                Console.WriteLine("Techno Question " + countQuestion[4]);
            }
        }

        private string CurrentCategory()
        {
            if (!String.IsNullOrEmpty(selectNextCategory) && (selectNextCategory == "Pop" || selectNextCategory == "Science" || selectNextCategory == "Sports" || selectNextCategory == selectMode))
            {
                return selectNextCategory;
            }
            if (_places[_currentPlayer] == 0) return "Pop";
            if (_places[_currentPlayer] == 4) return "Pop";
            if (_places[_currentPlayer] == 8) return "Pop";
            if (_places[_currentPlayer] == 1) return "Science";
            if (_places[_currentPlayer] == 5) return "Science";
            if (_places[_currentPlayer] == 9) return "Science";
            if (_places[_currentPlayer] == 2) return "Sports";
            if (_places[_currentPlayer] == 6) return "Sports";
            if (_places[_currentPlayer] == 10) return "Sports";
            if (selectMode == "Rock")
            {
                return "Rock";
            }
            else
            {
                return "Techno";
            }
        }

        public void Statistiques()
        {
            int count = 0;
            if (selectMode == "Rock")
            {
                count = countQuestion[0] + countQuestion[1] + countQuestion[2] + countQuestion[3];
            }
            else if(selectMode == "Techno")
            {
                count = countQuestion[0] + countQuestion[1] + countQuestion[2] + countQuestion[4];
            }

            Console.WriteLine("Pop Question : " + ((float)countQuestion[0] / count) * 100 + "%.");
            Console.WriteLine("Science Question : " + ((float)countQuestion[1] / count) * 100 + "%.");
            Console.WriteLine("Sports Question : " + (float)((float)countQuestion[2] / count) * 100 + "%.");

            if (selectMode == "Rock")
            {
                Console.WriteLine("Rock Question : " + ((float)countQuestion[3] / count) * 100 + "%.");
            }
            else if (selectMode == "Techno")
            {
                Console.WriteLine("Techno Question : " + ((float)countQuestion[4] / count) * 100 + "%.");
            }
        }

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    bonus[_currentPlayer]++;

                    if (bonus[_currentPlayer] != 1)
                    {
                        Console.WriteLine(_players[_currentPlayer]
                            + " gagne "
                            + bonus[_currentPlayer]
                            + " Gold Coins de bonus.");
                        _purses[_currentPlayer] += bonus[_currentPlayer];
                    }
                    Console.WriteLine(_players[_currentPlayer]
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");
                    
                    var winner = DidPlayerWin();
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;

                    return winner;
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was correct!!!!");
                _purses[_currentPlayer]++;
                bonus[_currentPlayer]++;

                if (bonus[_currentPlayer] != 1)
                {
                    Console.WriteLine(_players[_currentPlayer]
                        + " gagne "
                        + bonus[_currentPlayer]
                        + " Gold Coins de bonus.");
                    _purses[_currentPlayer] += bonus[_currentPlayer];
                }

                Console.WriteLine(_players[_currentPlayer]
                        + " now has "
                        + _purses[_currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " Doit choisir quelle catégorie le prochain joueur va avoir parmis : ");
            Console.WriteLine("Pop - Science - Sports - " + selectMode);
            string nextCategory = Console.ReadLine();
            if(nextCategory == "Rock" || nextCategory == "Techno")
            {
                while (nextCategory != selectMode)
                {
                    Console.WriteLine("Vous ne pouvez pas choisir une catégorie différente que celle du mode sélectionné. Merci de sélectionner " + selectMode);
                    nextCategory = Console.ReadLine();
                }
            }
           
                
            this.selectNextCategory = nextCategory;
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;
            bonus[_currentPlayer] = 0;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }

        public bool UseJoker()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (!_isGettingOutOfPenaltyBox)
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    return false;
                }
            }

            int useJoker = new Random().Next(9) + 1;
            if (!joker[_currentPlayer] && useJoker > 5)
            {
                joker[_currentPlayer] = true;
                Console.WriteLine(_players[_currentPlayer] + " use his joker.");
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;
                return false;
            }
            return true;
        }


        private bool DidPlayerWin()
        {
            return !(_purses[_currentPlayer] >= pointToWin);
        }
    }

}
