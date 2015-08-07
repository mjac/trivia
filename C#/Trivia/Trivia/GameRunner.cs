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
            Game game = new Game();

            game.AddPlayer("Chet");
            game.AddPlayer("Pat");
            game.AddPlayer("Sue");
            
            Random randomDieRoll = (args.Length == 0 ? new Random() : new Random(args[0].GetHashCode()));

            do
            {

                game.TakeTurn(randomDieRoll.Next(5) + 1);

                if (randomDieRoll.Next(9) == 7)
                {
                    hasNotWon = game.SendPlayerToPenaltyBoxAndEndTurn();
                }
                else
                {
                    hasNotWon = game.HandleCorrectAnswer();
                }



            } while (hasNotWon);

        }


    }

}

