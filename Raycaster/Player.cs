using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    public class Player
    {
        private static Vector2f Size = new Vector2f(16, 16);
        private Vector2f position;
        Sprite Sprite { get; set; }
        public Vector2f Velocity { get; set; }
        public Vector2i MousePrevious { get; set; }
        public float Speed { get; set; }
        public bool WPressed { get; set; }
        public bool SPressed { get; set; }
        public bool APressed { get; set; }
        public bool DPressed { get; set; }
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
            this.Speed = 50f;
        }
        public void Draw(RenderWindow window)
        {
            window.Draw(this.Sprite);
        }
        public void UpdatePosition(float deltaTime)
        {

            Vector2f directionVector = new Vector2f();
            if (SPressed)
            {
                directionVector += new Vector2f(-MathF.Cos(Angle), -MathF.Sin(Angle));
                //player.Velocity = new Vector2f(-MathF.Cos(player.Angle), -MathF.Sin(player.Angle));
            }
            if (WPressed)
            {
                directionVector += new Vector2f(MathF.Cos(Angle), MathF.Sin(Angle));
                //player.Velocity = new Vector2f(MathF.Cos(player.Angle), MathF.Sin(player.Angle));
            }
            if (APressed)
            {
                directionVector += new Vector2f(MathF.Sin(Angle), -MathF.Cos(Angle));
                //player.Velocity = new Vector2f(MathF.Sin(player.Angle), -MathF.Cos(player.Angle));
            }
            if (DPressed)
            {
                directionVector += new Vector2f(-MathF.Sin(Angle), MathF.Cos(Angle));
                //player.Velocity = new Vector2f(-MathF.Sin(player.Angle), MathF.Cos(player.Angle));
            }
            if (directionVector != new Vector2f(0, 0))
            {
                directionVector = directionVector / MathF.Sqrt(directionVector.X * directionVector.X + directionVector.Y * directionVector.Y);
            }
            Velocity = directionVector;
            Position += Velocity * Speed * deltaTime;
        }
    }
}
