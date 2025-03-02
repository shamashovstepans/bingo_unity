using UnityEngine;

namespace BingoGame
{
    internal class BingoCardsConfigProvider
    {
        public BingoCardConfig GetConfig()
        {
            return Resources.Load<BingoCardConfig>("BingoCardConfig");
        }
    }
}