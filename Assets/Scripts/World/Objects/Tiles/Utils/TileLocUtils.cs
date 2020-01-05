using Map.Objects.Tiles;

namespace World.Objects.Tiles.ConnectedTiles
{
    public class TilesUtils
    {
        /**
	
	Defines the relative x,y for the following locations :
	
	[NorthWest, North,   NorthEast]
	[West,      Center,  East] 
	[SouthWest, South,   SouthEast]
	*/
        public static readonly TileLoc NorthWest = new TileLoc(-1, 1);

        public static readonly TileLoc North = new TileLoc(0, 1);
        public static readonly TileLoc NorthEast = new TileLoc(1, 1);

        public static readonly TileLoc West = new TileLoc(-1, 0);
        public static readonly TileLoc Center = new TileLoc(0, 0);
        public static readonly TileLoc East = new TileLoc(1, 0);

        public static readonly TileLoc SouthWest = new TileLoc(-1, -1);
        public static readonly TileLoc South = new TileLoc(0, -1);
        public static readonly TileLoc SouthEast = new TileLoc(1, -1);


        public static TileLoc[] Cardinal =
        {
            NorthWest, North, NorthEast,
            West, Center, East,
            SouthWest, South, SouthEast
        };

        public static TileLoc[] ImmediateCardinal =
        {
            North, West, East, South
        };
    }
}