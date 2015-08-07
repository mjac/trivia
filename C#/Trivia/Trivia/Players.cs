using System.Collections.Generic;

namespace Trivia
{
    class Player
    {
        public string Name { get; set; }
        public int Coins { get; set; }
        public int Position { get; set; }
        public bool Penalty { get; set; }
    }
    internal class Players
    {
        

        int _currentPlayerIndex = 0;
        private readonly List<Player> _players = new List<Player>();

        public int Count { get { return _players.Count; }}

        public Player CurrentPlayer
        {
            get
            {
                return _players[_currentPlayerIndex];
            }
        }

        public void Add(Player player)
        {
            _players.Add(player);
        }

        public void AdvancePlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1)%Count;
        }
    }
}