using System;

namespace Trivia
{
    public class WrappedRandom : IRandom
    {
        private Random rand;

        public WrappedRandom()
        {
            rand = new Random();
        }

        public WrappedRandom(int seed)
        {
            rand = new Random(seed);
        }

        public int Next(int upperLimit)
        {
            return rand.Next(upperLimit);
        }
    }
}