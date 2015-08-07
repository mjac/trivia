using System;

namespace Trivia
{
    public class QuestionAnswerer
    {
        private IRandom random;

        public QuestionAnswerer(IRandom random)
        {
            this.random = random;
        }

        public bool GetIsCorrectAnswer()
        {
            return random.Next(9) != 7;
        }
    }
}