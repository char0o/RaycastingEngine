using SFML.System;
using SFML.Graphics;

namespace Raycaster
{
    public class Map
    {
        List<Tile> Tiles { get; set; }
        public int[,] WorldMap = new int[,]
        {
            {3, 2, 2, 3, 3, 3, 3, 3 },
            {3, 0, 0, 3, 0, 0, 0, 3 },
            {3, 0, 0, 0, 0, 0, 0, 3 },
            {3, 0, 0, 3, 0, 0, 0, 3 },
            {3, 3, 3, 3, 0, 0, 0, 2 },
            {3, 0, 0, 0, 0, 3, 0, 2 },
            {3, 0, 0, 3, 0, 0, 0, 3 },
            {3, 3, 3, 3, 2, 2, 3, 3 }
        };
        private Vector2i size;
        public Vector2i Size
        {
            get { return size; }
            set { size = value; }
        }
        public Map()
        {
            this.Tiles = new List<Tile>();
            Size = new Vector2i(WorldMap.GetLength(0), WorldMap.GetLength(1));
            CreateMinimap();
            
        }
        public void CreateMinimap()
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    Color color = Color.Black;
                    if (WorldMap[i, j] != 0)
                        color = Color.Red;
                    Tile tile = new Tile(new Vector2f(i, j), color);
                    Tiles.Add(tile);
                }
            }
        }
        public void DrawMinimap(RenderWindow window)
        {
            foreach (Tile tile in Tiles)
            {
                window.Draw(tile.Shape);
            }
            for (int i = 0; i < 8; i++)
            {
                Vertex[] line = {
                    new Vertex(new Vector2f(0, i * Tile.TILESIZE_Y), new Color(64, 64, 64, 255)),
                    new Vertex(new Vector2f(512, i * Tile.TILESIZE_Y), new Color(64, 64, 64, 255))
                };
                Vertex[] column = {
                    new Vertex(new Vector2f(i * Tile.TILESIZE_X, 0), new Color(64, 64, 64, 255)),
                    new Vertex(new Vector2f(i * Tile.TILESIZE_X, 512), new Color(64, 64, 64, 255))
                };
                window.Draw(line, PrimitiveType.Lines);
                window.Draw(column, PrimitiveType.Lines);
            }
        }
    }
}
