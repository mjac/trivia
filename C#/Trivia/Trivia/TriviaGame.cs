using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class TriviaGame
    {
        class Player
        {
            public string Name { get; set; }
        }

        List<Player> playerNames = new List<Player>();

        int[] boardPositionsByPlayerIndex = new int[6];
        int[] coinsByPlayerIndex = new int[6];

        bool[] isInPenaltyBoxByPlayerIndex = new bool[6];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayerIndex = 0;
        bool isGettingOutOfPenaltyBox;
        private bool CurrentPlayerInPenaltyBox
        {
            get { return isInPenaltyBoxByPlayerIndex[currentPlayerIndex]; }
            set { isInPenaltyBoxByPlayerIndex[currentPlayerIndex] = value; }
        }

        private int CurrentPlayerCoins
        {
            get { return coinsByPlayerIndex[currentPlayerIndex]; }
            set { coinsByPlayerIndex[currentPlayerIndex] = value; }
        }

        private string CurrentPlayerName
        {
            get { return playerNames[currentPlayerIndex].Name; }
        }

        private int NumberOfPlayers
        {
            get { return playerNames.Count; }
        }

        private int CurrentBoardPosition
        {
            get { return boardPositionsByPlayerIndex[currentPlayerIndex]; }
            set { boardPositionsByPlayerIndex[currentPlayerIndex] = value; }
        }

        private const int TotalBoardPositions = 12;

        public TriviaGame()
        {
            for (int questionNumber = 0; questionNumber < 50; questionNumber++)
            {
                popQuestions.AddLast("Pop Question " + questionNumber);
                scienceQuestions.AddLast("Science Question " + questionNumber);
                sportsQuestions.AddLast("Sports Question " + questionNumber);
                rockQuestions.AddLast("Rock Question " + questionNumber);
            }
        }

        public bool AddPlayer(String playerName)
        {
            playerNames.Add(new Player{Name = playerName});
            var newPlayerIndex = NumberOfPlayers;

            InitializePlayer(newPlayerIndex);

            GameNotifications.NotifyAboutNewPlayer(playerName, newPlayerIndex);
            return true;
        }

        private void InitializePlayer(int newPlayerIndex)
        {
            boardPositionsByPlayerIndex[newPlayerIndex] = 0;
            coinsByPlayerIndex[newPlayerIndex] = 0;
            isInPenaltyBoxByPlayerIndex[newPlayerIndex] = false;
        }

        public void TakeTurn(int standardDieRoll)
        {
            var currentPlayerName = CurrentPlayerName;

            GameNotifications.NotifyAboutCurrentPlayer(currentPlayerName);
            GameNotifications.NotifyAboutDieRoll(standardDieRoll);

            var isInPenaltyBox = CurrentPlayerInPenaltyBox;
            if (isInPenaltyBox)
            {
                if (!TryGetOutOfPenaltyBox(standardDieRoll))
                {
                    GameNotifications.NotifyStuckInTheBox(currentPlayerName);
                    return;
                }

                GameNotifications.NotifyGotOutOfBox(currentPlayerName);
            }

            AdvancePosition(standardDieRoll);

            WriteLocationAndCategory();

            AskQuestion();
        }

        private bool TryGetOutOfPenaltyBox(int standardDieRoll)
        {
            isGettingOutOfPenaltyBox = standardDieRoll % 2 != 0;
            return isGettingOutOfPenaltyBox;
        }

        private void WriteLocationAndCategory()
        {
            var boardPosition = CurrentBoardPosition;

            var currentPlayerName = CurrentPlayerName;
            GameNotifications.NotifyNewLocation(currentPlayerName, boardPosition);

            var category = Board.GetCategoryForPosition(boardPosition);
            GameNotifications.NotifyCategory(category);
        }

        private void AdvancePosition(int standardDieRoll)
        {
            var nextPosition = (CurrentBoardPosition + standardDieRoll) % TotalBoardPositions;
            CurrentBoardPosition = nextPosition;
        }

        private void AskQuestion()
        {
            var boardPosition = CurrentBoardPosition;

            var questionCategory = Board.GetCategoryForPosition(boardPosition);

            var questions = GetQuestionsForCategory(questionCategory);

            WriteQuestionAndDiscard(questions);
        }

        private LinkedList<string> GetQuestionsForCategory(string questionCategory)
        {
            switch (questionCategory)
            {
                case "Pop":
                    return popQuestions;
                case "Science":
                    return scienceQuestions;
                case "Sports":
                    return sportsQuestions;
                case "Rock":
                    return rockQuestions;
                default:
                    throw new ArgumentException();
            }
        }

        private static void WriteQuestionAndDiscard(LinkedList<string> questions)
        {
            var question = questions.First();
            GameNotifications.NotifyQuestion(question);
            questions.RemoveFirst();
        }


        public bool HandleCorrectAnswer()
        {
            var isStayingInPenaltyBox = CurrentPlayerInPenaltyBox && !isGettingOutOfPenaltyBox;
            if (isStayingInPenaltyBox)
            {
                AdvancePlayer();
                return true;
            }

            GameNotifications.NotifyCorrectAnswer();
            CurrentPlayerCoins++;

            var currentPlayerName = CurrentPlayerName;
            var currentPlayerCoins = CurrentPlayerCoins;
            GameNotifications.NotifyPlayerCoins(currentPlayerName, currentPlayerCoins);

            bool winner = HasCurrentPlayerWon();

            AdvancePlayer();

            return winner;
        }

        private void AdvancePlayer()
        {
            currentPlayerIndex = AdvancePlayer(currentPlayerIndex);
        }

        private int AdvancePlayer(int currentPlayerIndex)
        {
            currentPlayerIndex++;

            if (currentPlayerIndex == NumberOfPlayers)
            {
                currentPlayerIndex = 0;
            }

            return currentPlayerIndex;
        }


        public bool SendPlayerToPenaltyBoxAndEndTurn()
        {
            GameNotifications.NotifyIncorrectAnswer();
            var currentPlayerName = CurrentPlayerName;

            GameNotifications.NotifyPenaltyAdded(currentPlayerName);
            CurrentPlayerInPenaltyBox = true;

            AdvancePlayer();

            return true;
        }

        private bool HasCurrentPlayerWon()
        {
            return !(CurrentPlayerCoins == 6);
        }
    }

}
