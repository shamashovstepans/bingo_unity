using System;
using System.Collections.Generic;

namespace BingoGame.Commands
{
    internal class FillBingoCardCommand
    {
        private readonly BingoCardsConfigProvider _configProvider;

        private Random _random;

        public FillBingoCardCommand(BingoCardsConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public BingoGameState Execute(BingoGameState gameState, int seed)
        {
            _random = new Random(seed);
            var cardConfig = _configProvider.GetConfig();
            var randomCells = GetRandomCells(cardConfig);
            gameState.Card.Fill(randomCells);
            return gameState;
        }

        private string[][] GetRandomCells(BingoCardConfig cardConfig)
        {
            // reorder elements in random way
            var elements = cardConfig.Elements;

            var randomElements = Shuffle(elements);

            var cells = new string[BingoCard.SIZE][];
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                cells[i] = new string[BingoCard.SIZE];
                for (var j = 0; j < BingoCard.SIZE; j++)
                {
                    cells[i][j] = randomElements[i * BingoCard.SIZE + j];
                }
            }

            return cells;
        }

        private List<string> Shuffle(List<string> list)
        {
            var randomElements = new List<string>(list);
            return Shuffle<string>(randomElements);
        }

        private List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1); // Random index from 0 to i
                (list[i], list[j]) = (list[j], list[i]); // Swap elements
            }

            return list;
        }
    }
}