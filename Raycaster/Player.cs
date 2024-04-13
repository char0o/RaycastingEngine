using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    public class Player
    {
        private static Vector2f Size = new Vector2f(16, 16);
        private Vector2f position;
        Sprite Sprite { get; set; }
        Vector2f Velocity { get; set; }
        public Vector2i MousePrevious { get; set; }
        public Vector2f Position
        {
            get { return position; }
            set
            {
                this.position = value;
                if (this.Sprite != null)
                    this.Sprite.Position = value;
            }
        }
        public Vector2f Direction { get; set; }
        private float angle;
        public float Angle
        {
            get { return this.angle; }
            set
            {
                /*if (value > (MathF.PI * 2f) || value < 0)
                    throw new ArgumentOutOfRangeException();*/
                if (this.Sprite != null)
                    this.Sprite.Rotation = Rays.RadToDeg(value - MathF.PI / 2);
                this.angle = value;
                this.Direction = new Vector2f(MathF.Cos(this.Angle), MathF.Sin(this.Angle));
            }
        }

        public Player(Vector2f position)
        {
            this.Sprite = new Sprite(new Texture("resources/player.png"));
            this.Sprite.Origin = new Vector2f(Player.Size.X / 2, Player.Size.Y / 2);
            this.Position = position;
            this.MousePrevious = new Vector2i(Program.SCREEN_WIDTH / 2, Program.SCREEN_HEIGHT / 2);
            this.Angle = 0;
        }
        public void Draw(RenderWindow window)
        {
            window.Draw(this.Sprite);
        }

    }
}
