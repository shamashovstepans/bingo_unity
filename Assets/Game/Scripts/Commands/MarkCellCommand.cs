namespace BingoGame.Commands
{
    internal class MarkCellCommand
    {
        public BingoGameState Execute(BingoGameState gameState, int row, int column)
        {
            gameState.Card.checkedCells[row][column] = !gameState.Card.checkedCells[row][column];
            return gameState;
        }
    }
}