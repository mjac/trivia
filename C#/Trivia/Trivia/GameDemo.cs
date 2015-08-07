using System;

namespace Trivia
{
    public class GameDemo
    {
        public static void Main(String[] args)
        {
            var triviaGame = new TriviaGame();

            triviaGame.AddPlayer("Chet");
            triviaGame.AddPlayer("Pat");
            triviaGame.AddPlayer("Sue");
            
            var random = (args.Length == 0 ? new WrappedRandom() : new WrappedRandom(args[0].GetHashCode()));
            var turnDie = new NSidedDie(6, random);

            var gameRunner = new GameRunner();

            gameRunner.PlayGame(triviaGame, turnDie, new QuestionAnswerer(random));

        }
    }
}