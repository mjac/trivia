using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia
{
    public class GameRunner
    {
        private static bool hasNotWon;

        public static void Main(String[] args)
        {
            TriviaGame triviaGame = new TriviaGame();

            triviaGame.AddPlayer("Chet");
            triviaGame.AddPlayer("Pat");
            triviaGame.AddPlayer("Sue");
            
            Random randomDieRoll = (args.Length == 0 ? new Random() : new Random(args[0].GetHashCode()));

            do
            {

                triviaGame.TakeTurn(randomDieRoll.Next(5) + 1);

                if (randomDieRoll.Next(9) == 7)
                {
                    hasNotWon = triviaGame.SendPlayerToPenaltyBoxAndEndTurn();
                }
                else
                {
                    hasNotWon = triviaGame.HandleCorrectAnswer();
                }



            } while (hasNotWon);

        }


    }

}

