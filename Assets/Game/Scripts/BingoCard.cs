using System;
using System.Collections.Generic;
using Game.Scenes;
using UnityEngine;

namespace BingoGame
{
    internal class BingoCard
    {
        public const int SIZE = 5;

        public readonly BingoCallDto[][] cells;
        public readonly bool[][] checkedCells;

        public BingoCard()
        {
            cells = new BingoCallDto[SIZE][];
            checkedCells = new bool[SIZE][];

            for (var i = 0; i < SIZE; i++)
            {
                cells[i] = new BingoCallDto[SIZE];
                checkedCells[i] = new bool[SIZE];
            }
        }

        public void Fill(BingoCallDto[][] sourceCells)
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

        public int[] GetBitmask()
        {
            var result = new List<int>();
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (checkedCells[i][j])
                    {
                        result.Add(cells[i][j].id);
                    }
                }
            }

            return result.ToArray();
        }
    }
}