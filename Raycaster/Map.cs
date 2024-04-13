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
            for (int i = 0; i < 8; i++)
            {
                Vertex[] line = {
                    new Vertex(new Vector2f(0, i * 32), new Color(64, 64, 64, 255)),
                    new Vertex(new Vector2f(256, i * 32), new Color(64, 64, 64, 255))
                };
                Vertex[] column = {
                    new Vertex(new Vector2f(i * 32f, 0), new Color(64, 64, 64, 255)),
                    new Vertex(new Vector2f(i * 32f, 256), new Color(64, 64, 64, 255))
                };
                window.Draw(line, PrimitiveType.Lines);
                window.Draw(column, PrimitiveType.Lines);
            }
        }
    }
}
