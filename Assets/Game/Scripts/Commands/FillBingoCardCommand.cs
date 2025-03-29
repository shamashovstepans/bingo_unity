using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scenes;

namespace BingoGame.Commands
{
    internal class FillBingoCardCommand
    {
        private Random _random;
        
        public BingoGameState Execute(BingoGameState gameState, BingoCallDto[] cardConfig, int seed)
        {
            _random = new Random(seed);
            var randomCells = GetRandomCells(cardConfig);
            gameState.Card.Fill(randomCells);
            return gameState;
        }

        private BingoCallDto[][] GetRandomCells(BingoCallDto[] cardConfig)
        {
            // reorder elements in random way
            var elements = cardConfig;

            var randomElements = Shuffle(elements.ToList());

            var cells = new BingoCallDto[BingoCard.SIZE][];
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                cells[i] = new BingoCallDto[BingoCard.SIZE];
                for (var j = 0; j < BingoCard.SIZE; j++)
                {
                    cells[i][j] = randomElements[i * BingoCard.SIZE + j];
                }
            }

            return cells;
        }

        private List<BingoCallDto> Shuffle(List<BingoCallDto> list)
        {
            var randomElements = new List<BingoCallDto>(list);
            return Shuffle<BingoCallDto>(randomElements);
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