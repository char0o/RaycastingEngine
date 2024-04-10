using SFML.Graphics;
using SFML.System;

namespace Raycaster
{
    public class Tile
    {
        public RectangleShape Shape { get; private set; }
        Vector2f position;
        Vector2f Position {
            get { return this.position; }
            set
            {
                if (this.Shape != null)
                    this.Shape.Position = value * 32;
                this.position = value * 32;
            }
        }
        
        public Tile(Vector2f position, Color fillColor)
        {
            this.Shape = new RectangleShape(new Vector2f(32, 32));
            this.Position = position;
            this.Shape.Position = this.Position;
            this.Shape.FillColor = fillColor;
        }
    }
}
