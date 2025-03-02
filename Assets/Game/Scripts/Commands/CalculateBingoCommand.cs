namespace BingoGame.Commands
{
    internal class CalculateBingoCommand
    {
        public BingoGameState Execute(BingoGameState gameState)
        {
            var card = gameState.Card;
            var bingoArray = GetBingoArray(card);
            gameState.Bingo = bingoArray;
            gameState.OnUpdated();
            return gameState;
        }

        private Bingo[] GetBingoArray(BingoCard card)
        {
            var bingoArray = new Bingo[BingoCard.SIZE * 2 + 2];
            CheckRows(card, bingoArray);
            CheckColumns(card, bingoArray);
            CheckDiagonals(card, bingoArray);
            return bingoArray;
        }

        private void CheckRows(BingoCard card, Bingo[] bingoArray)
        {
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                var isBingo = true;
                for (var j = 0; j < BingoCard.SIZE; j++)
                {
                    if (!card.checkedCells[i][j])
                    {
                        isBingo = false;
                        break;
                    }
                }

                bingoArray[i] = new Bingo { Direction = BingoDirection.Horizontal, Index = i };

                if (isBingo)
                {
                    bingoArray[i].IsBingo = true;
                }
            }
        }

        private void CheckColumns(BingoCard card, Bingo[] bingoArray)
        {
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                var isBingo = true;
                for (var j = 0; j < BingoCard.SIZE; j++)
                {
                    if (!card.checkedCells[j][i])
                    {
                        isBingo = false;
                        break;
                    }
                }

                bingoArray[i + BingoCard.SIZE] = new Bingo { Direction = BingoDirection.Vertical, Index = i };

                if (isBingo)
                {
                    bingoArray[i + BingoCard.SIZE].IsBingo = true;
                }
            }
        }

        private void CheckDiagonals(BingoCard card, Bingo[] bingoArray)
        {
            var isBingo = true;
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                if (!card.checkedCells[i][i])
                {
                    isBingo = false;
                    break;
                }
            }

            bingoArray[BingoCard.SIZE * 2] = new Bingo { Direction = BingoDirection.Diagonal, Index = 0 };

            if (isBingo)
            {
                bingoArray[BingoCard.SIZE * 2].IsBingo = true;
            }

            isBingo = true;
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                if (!card.checkedCells[i][BingoCard.SIZE - i - 1])
                {
                    isBingo = false;
                    break;
                }
            }

            bingoArray[BingoCard.SIZE * 2 + 1] = new Bingo { Direction = BingoDirection.Diagonal, Index = 1 };

            if (isBingo)
            {
                bingoArray[BingoCard.SIZE * 2 + 1].IsBingo = true;
            }
        }
    }
}