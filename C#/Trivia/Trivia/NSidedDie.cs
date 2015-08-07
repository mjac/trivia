namespace Trivia
{
    public class NSidedDie
    {
        private readonly int _numberOfSides;
        private readonly IRandom _random;

        public NSidedDie(int numberOfSides, IRandom random)
        {
            _numberOfSides = numberOfSides;
            _random = random;
        }

        public int RollDie()
        {
            return _random.Next(_numberOfSides - 1) + 1;
        }
    }
}