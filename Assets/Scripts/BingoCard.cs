namespace BingoGame
{
    internal class BingoCard
    {
        public const int SIZE = 5;

        public readonly string[][] cells;
        public readonly bool[][] checkedCells;

        public BingoCard()
        {
            cells = new string[SIZE][];
            checkedCells = new bool[SIZE][];

            for (var i = 0; i < SIZE; i++)
            {
                cells[i] = new string[SIZE];
                checkedCells[i] = new bool[SIZE];
            }
        }

        public void Fill(string[][] sourceCells)
        {
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    cells[i][j] = sourceCells[i][j];
                }
            }
        }
        
        public void MarkCell(int row, int column)
        {
            checkedCells[row][column] = true;
        }
    }
}