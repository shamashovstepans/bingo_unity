using System;

namespace BingoGame
{
    internal struct Bingo : IEquatable<Bingo>
    {
        public bool IsBingo;
        public BingoDirection Direction;
        public int Index;

        public Bingo(bool isBingo, BingoDirection direction, int index)
        {
            IsBingo = isBingo;
            Direction = direction;
            Index = index;
        }

        public bool Equals(Bingo other)
        {
            return Direction == other.Direction && Index == other.Index;        
        }

        public override bool Equals(object obj)
        {
            return obj is Bingo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Direction, Index);
        }
    }
}