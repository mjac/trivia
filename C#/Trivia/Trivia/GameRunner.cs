using System;

namespace Trivia
{
    public class GameRunner
    {
        public void PlayGame(TriviaGame triviaGame, NSidedDie turnDie, QuestionAnswerer questionAnswerer)
        {
            bool hasNotWon;

            do
            {
                triviaGame.TakeTurn(turnDie.RollDie());

                var isCorrectAnswer = questionAnswerer.GetIsCorrectAnswer();
                if (isCorrectAnswer)
                {
                    hasNotWon = triviaGame.HandleCorrectAnswer();
                }
                else
                {
                    hasNotWon = triviaGame.SendPlayerToPenaltyBoxAndEndTurn();
                }
            } while (hasNotWon);
        }
    }
}

