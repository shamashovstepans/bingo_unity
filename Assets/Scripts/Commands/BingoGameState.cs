using System;

namespace BingoGame.Commands
{
    internal class BingoGameState
    {
        public event Action Updated; 
        public BingoCard Card;
        public Bingo[] Bingo = new Bingo[12];

        public void OnUpdated()
        {
            Updated?.Invoke();
        }
    }
}