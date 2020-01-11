using System;

namespace World.Map.Tiles
{
    /**
 * A TileLoc is an immutable tile location (x, y) in game represented by integers.
 */
    public class TileLoc
    {
        public readonly int x;
        public readonly int y;

        public TileLoc(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public TileLoc(TileLoc tileLoc)
        {
            x = tileLoc.x;
            y = tileLoc.y;
        }

        public TileLoc RelativeTo(TileLoc tileLoc)
        {
            if (tileLoc == null)
            {
                throw new ArgumentNullException();
            }

            return new TileLoc(tileLoc.x + x, tileLoc.y + y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TileLoc item))
            {
                return false;
            }

            return (x == item.x) && (y == item.y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }

        public override string ToString()
        {
            return "x : '" + x + ", y : '" + y + "'";
        }
    }
}