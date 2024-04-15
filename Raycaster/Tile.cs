using SFML.Graphics;
using SFML.System;

namespace Raycaster
{
    public class Tile
    {
        public static int TILESIZE_X = 64;
        public static int TILESIZE_Y = 64;
        public RectangleShape Shape { get; private set; }
        Vector2f position;
        Vector2f Position {
            get { return this.position; }
            set
            {
                if (this.Shape != null)
                    this.Shape.Position = value * TILESIZE_X;
                this.position = value * TILESIZE_X;
            }
        }
        
        public Tile(Vector2f position, Color fillColor)
        {
            this.Shape = new RectangleShape(new Vector2f(TILESIZE_X, TILESIZE_Y));
            this.Position = position;
            this.Shape.Position = this.Position;
            this.Shape.FillColor = fillColor;
        }
    }
}
