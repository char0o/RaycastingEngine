using SFML.System;
using SFML.Graphics;
namespace Raycaster
{
    public class Map
    {
        List<Tile> Tiles { get; set; }
        public int[,] WorldMap = new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 1, 1, 0, 0, 1 },
            {1, 0, 0, 0, 0, 0, 0, 1 },
            {1, 0, 0, 0, 0, 1, 0, 1 },
            {1, 0, 0, 1, 0, 0, 0, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1 }
        };
        public Map()
        {
            this.Tiles = new List<Tile>();
            CreateMap();
        }
        public void CreateMap()
        {
            for (int i = 0; i < Program.MAP_SIZE_X; i++)
            {
                for (int j = 0; j < Program.MAP_SIZE_Y; j++)
                {
                    Color color = Color.Black;
                    if (WorldMap[i, j] == 1)
                        color = Color.Red;
                    Tile tile = new Tile(new Vector2f(i, j), color);
                    Tiles.Add(tile);
                }
            }
        }
        public void DrawTiles(RenderWindow window)
        {
            foreach (Tile tile in Tiles)
            {
                window.Draw(tile.Shape);
            }
        }
    }
}
