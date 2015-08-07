using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class TriviaGame
    {
        private class Players
        {
            public List<Player> players = new List<Player>();
        }

        class Player
        {
            public string Name { get; set; }
            public int Coins { get; set; }
            public int Position { get; set; }
            public bool Penalty { get; set; }
        }

        private Players players = new Players();

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayerIndex = 0;
        bool isGettingOutOfPenaltyBox;
        private Player CurrentPlayer
        {
            get { return players.players[currentPlayerIndex]; }
        }

        private bool CurrentPlayerInPenaltyBox
        {
            get { return CurrentPlayer.Penalty; }
            set { CurrentPlayer.Penalty = value; }
        }

        private int CurrentPlayerCoins
        {
            get { return CurrentPlayer.Coins; }
            set { CurrentPlayer.Coins = value; }
        }

        private string CurrentPlayerName
        {
            get { return CurrentPlayer.Name; }
        }

        private int NumberOfPlayers
        {
            get { return players.players.Count; }
        }

        private int CurrentBoardPosition
        {
            get { return CurrentPlayer.Position; }
            set { CurrentPlayer.Position = value; }
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
            players.players.Add(new Player{Name = playerName});
            var newPlayerIndex = NumberOfPlayers;

            GameNotifications.NotifyAboutNewPlayer(playerName, newPlayerIndex);
            return true;
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
