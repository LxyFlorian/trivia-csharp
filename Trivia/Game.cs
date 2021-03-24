using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        public readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];
        private readonly int[] bonus = new int[6];
        private readonly bool[] joker = new bool[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _technoQuestions = new LinkedList<string>();

        private String selectMode;
        private String selectNextCategory;
        public int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public int pointToWin = 0;

        public Game()
        {
            Console.WriteLine("Paramétrage : Entrez combien de points pour gagner :");
            Console.WriteLine("Minimum 6");
            do
            {
                string line = Console.ReadLine();
                if (Int32.TryParse(line,out pointToWin))
                {
                    if(pointToWin >= 6)
                    {
                        Console.WriteLine("Le jeu sera gagné quand un premier joueur arrivera à " + pointToWin + " points.");
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

            Console.WriteLine("Rock ou Techno ?");
            selectMode = Console.ReadLine();
            for (var i = 0; i < 50; i++)
            {
                _popQuestions.AddLast("[Pop] Question : " + i);
                _scienceQuestions.AddLast(("[Science] Question : " + i));
                _sportsQuestions.AddLast(("[Sports] Question : " + i));
                if (selectMode == "Rock")
                {
                    _rockQuestions.AddLast(CreateRockQuestion(i));
                }
                else
                {
                    _technoQuestions.AddLast(CreateTechnoQuestion(i));
                }
            }
        }

        public string CreateRockQuestion(int index)
        {
            return "[Rock] Question : " + index;
        }

        public string CreateTechnoQuestion(int index)
        {
            return "[Techno] Question : " + index;
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

                Console.WriteLine(playerName + " a rejoint la partie");
                //Console.WriteLine("They are player number " + _players.Count);
            }
            else
            {
                Console.WriteLine("Impossible d'ajouter le joueur. Seulement 6 personnes maximum par partie.");
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(_players[_currentPlayer] + " est le joueur actuel");
            Console.ResetColor();
            //Console.WriteLine("They have rolled a " + roll);

            //Console.WriteLine("Voulez vous quitter la partie ?");
            Console.WriteLine("Toucher la touche ESC si vous voulez quitter");

            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                Console.WriteLine(_players[_currentPlayer] + " est sorti");
                _players.RemoveAt(_currentPlayer);
                if (!IsPlayable())
                {
                    Console.WriteLine("La partie est terminée");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Le joueur continue à jouer");
            }

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;
                    _inPenaltyBox[_currentPlayer] = false;

                    Console.WriteLine(_players[_currentPlayer] + " va en prison");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;
                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                    //Console.WriteLine(_players[_currentPlayer]
                    //        + "'s new location is "
                    //        + _places[_currentPlayer]);
                    Console.WriteLine("La catégorie de la question est : " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine("Malheureusement " + _players[_currentPlayer] + " ne sors pas de la prison");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                //Console.WriteLine(_players[_currentPlayer]
                //        + "'s new location is "
                //        + _places[_currentPlayer]);
                Console.WriteLine("La catégorie de la question est : " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Console.WriteLine(_popQuestions.First());
                _popQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Science")
            {
                Console.WriteLine(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Sports")
            {
                Console.WriteLine(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Rock")
            {
                Console.WriteLine(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Techno")
            {
                Console.WriteLine(_technoQuestions.First());
                _technoQuestions.RemoveFirst();
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

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    ////Console.WriteLine("Bonne réponse !!!!");
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
                            + " à maintenant "
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
                Console.WriteLine("Bonne réponse !!!!");
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
                        + " à maintenant "
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
            Console.WriteLine("Mauvaise réponse");
            Console.WriteLine(_players[_currentPlayer] + " Doit choisir la catégorie de question que le prochain joueur va répondre, parmis : ");
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
            Console.WriteLine("Malheureusement " + _players[_currentPlayer] + " va en prison");
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
                Console.WriteLine(_players[_currentPlayer] + " utilise son joker.");
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
