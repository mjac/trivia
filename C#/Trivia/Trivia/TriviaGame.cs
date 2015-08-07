using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class TriviaGame
    {
        List<string> playerNames = new List<string>();

        int[] boardPositionsByPlayerIndex = new int[6];
        int[] coinsByPlayerIndex = new int[6];

        bool[] isInPenaltyBoxByPlayerIndex = new bool[6];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayerIndex = 0;
        bool isGettingOutOfPenaltyBox;

        private const int TotalBoardPositions = 12;

        public TriviaGame()
        {
            for (int questionNumber = 0; questionNumber < 50; questionNumber++)
            {
                popQuestions.AddLast("Pop Question " + questionNumber);
                scienceQuestions.AddLast(("Science Question " + questionNumber));
                sportsQuestions.AddLast(("Sports Question " + questionNumber));
                rockQuestions.AddLast(createRockQuestion(questionNumber));
            }
        }

        public String createRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool CanStartGame()
        {
            return (GetPlayerCount() >= 2);
        }

        public bool AddPlayer(String playerName)
        {
            playerNames.Add(playerName);
            boardPositionsByPlayerIndex[GetPlayerCount()] = 0;
            coinsByPlayerIndex[GetPlayerCount()] = 0;
            isInPenaltyBoxByPlayerIndex[GetPlayerCount()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + playerNames.Count);
            return true;
        }

        public int GetPlayerCount()
        {
            return playerNames.Count;
        }

        public void TakeTurn(int standardDieRoll)
        {
            Console.WriteLine(playerNames[currentPlayerIndex] + " is the current player");
            Console.WriteLine("They have rolled a " + standardDieRoll);

            var isInPenaltyBox = isInPenaltyBoxByPlayerIndex[currentPlayerIndex];
            if (isInPenaltyBox)
            {
                isGettingOutOfPenaltyBox = standardDieRoll%2 != 0;
                
                if (!isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine(playerNames[currentPlayerIndex] + " is not getting out of the penalty box");
                    return;
                }

                Console.WriteLine(playerNames[currentPlayerIndex] + " is getting out of the penalty box");
            }

            AdvancePositionAndAskQuestion(standardDieRoll);
        }

        private void AdvancePositionAndAskQuestion(int standardDieRoll)
        {
            var nextPosition = (boardPositionsByPlayerIndex[currentPlayerIndex] + standardDieRoll) % TotalBoardPositions;
            boardPositionsByPlayerIndex[currentPlayerIndex] = nextPosition;

            Console.WriteLine(playerNames[currentPlayerIndex]
                              + "'s new location is "
                              + boardPositionsByPlayerIndex[currentPlayerIndex]);
            Console.WriteLine("The category is " + GetQuestionCategoryFromCurrentPlayerBoardPosition());
            askQuestion();
        }

        private void askQuestion()
        {
            if (GetQuestionCategoryFromCurrentPlayerBoardPosition() == "Pop")
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (GetQuestionCategoryFromCurrentPlayerBoardPosition() == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (GetQuestionCategoryFromCurrentPlayerBoardPosition() == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (GetQuestionCategoryFromCurrentPlayerBoardPosition() == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }


        private String GetQuestionCategoryFromCurrentPlayerBoardPosition()
        {
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 0) return "Pop";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 4) return "Pop";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 8) return "Pop";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 1) return "Science";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 5) return "Science";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 9) return "Science";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 2) return "Sports";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 6) return "Sports";
            if (boardPositionsByPlayerIndex[currentPlayerIndex] == 10) return "Sports";
            return "Rock";
        }

        public bool HandleCorrectAnswer()
        {
            var isStayingInPenaltyBox = isInPenaltyBoxByPlayerIndex[currentPlayerIndex] && !isGettingOutOfPenaltyBox;
            if (isStayingInPenaltyBox)
            {
                currentPlayerIndex++;
                if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;
                return true;
            }

            Console.WriteLine("Answer was correct!!!!");
            coinsByPlayerIndex[currentPlayerIndex]++;

            Console.WriteLine(playerNames[currentPlayerIndex]
                              + " now has "
                              + coinsByPlayerIndex[currentPlayerIndex]
                              + " Gold Coins.");

            bool winner = HasCurrentPlayerWon();

            currentPlayerIndex++;
            if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;

            return winner;
        }

        public bool SendPlayerToPenaltyBoxAndEndTurn()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(playerNames[currentPlayerIndex] + " was sent to the penalty box");
            isInPenaltyBoxByPlayerIndex[currentPlayerIndex] = true;

            currentPlayerIndex++;
            if (currentPlayerIndex == playerNames.Count) currentPlayerIndex = 0;
            return true;
        }


        private bool HasCurrentPlayerWon()
        {
            return !(coinsByPlayerIndex[currentPlayerIndex] == 6);
        }
    }

}
