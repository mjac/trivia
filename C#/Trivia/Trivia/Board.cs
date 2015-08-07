namespace Trivia
{
    public static class Board
    {
        public static string GetCategoryForPosition(int boardPosition)
        {
            if (boardPosition == 0) return "Pop";
            if (boardPosition == 4) return "Pop";
            if (boardPosition == 8) return "Pop";
            if (boardPosition == 1) return "Science";
            if (boardPosition == 5) return "Science";
            if (boardPosition == 9) return "Science";
            if (boardPosition == 2) return "Sports";
            if (boardPosition == 6) return "Sports";
            if (boardPosition == 10) return "Sports";
            return "Rock";
        }
    }
}