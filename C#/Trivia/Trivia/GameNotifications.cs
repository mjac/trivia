using System;

namespace Trivia
{
    public static class GameNotifications
    {
        public static void NotifyPenaltyAdded(string currentPlayerName)
        {
            Console.WriteLine(currentPlayerName + " was sent to the penalty box");
        }

        public static void NotifyIncorrectAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
        }

        public static void NotifyPlayerCoins(string currentPlayerName, int currentPlayerCoins)
        {
            Console.WriteLine(currentPlayerName + " now has " + currentPlayerCoins + " Gold Coins.");
        }

        public static void NotifyCorrectAnswer()
        {
            Console.WriteLine("Answer was correct!!!!");
        }

        public static void NotifyQuestion(string question)
        {
            Console.WriteLine(question);
        }

        public static void NotifyAboutNewPlayer(string playerName, int newPlayerIndex)
        {
            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + newPlayerIndex);
        }

        public static void NotifyGotOutOfBox(string currentPlayerName)
        {
            Console.WriteLine(currentPlayerName + " is getting out of the penalty box");
        }

        public static void NotifyStuckInTheBox(string currentPlayerName)
        {
            Console.WriteLine(currentPlayerName + " is not getting out of the penalty box");
        }

        public static void NotifyAboutDieRoll(int standardDieRoll)
        {
            Console.WriteLine("They have rolled a " + standardDieRoll);
        }

        public static void NotifyAboutCurrentPlayer(string currentPlayerName)
        {
            Console.WriteLine(currentPlayerName + " is the current player");
        }

        public static void NotifyCategory(string category)
        {
            Console.WriteLine("The category is " + category);
        }

        public static void NotifyNewLocation(string currentPlayerName, int boardPosition)
        {
            Console.WriteLine(currentPlayerName + "'s new location is " + boardPosition);
        }
    }
}