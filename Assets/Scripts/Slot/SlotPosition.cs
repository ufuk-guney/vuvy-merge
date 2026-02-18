using System;
using UnityEngine;

namespace VuvyMerge.Grid
{
    public readonly struct SlotPosition : IEquatable<SlotPosition>
    {
        public int X { get; }
        public int Y { get; }

        public SlotPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector3 ToWorldPosition() => new(X, Y, 0f);

        public bool Equals(SlotPosition other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj)
        {
            return obj is SlotPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(SlotPosition a, SlotPosition b) => a.Equals(b);
        public static bool operator !=(SlotPosition a, SlotPosition b) => !a.Equals(b);
    }
}
