using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Raycaster;

class Program
{
    public const int SCREEN_WIDTH = 800;
    public const int SCREEN_HEIGHT = 600;
    public const int MAP_SIZE_X = 8;
    public const int MAP_SIZE_Y = 8;
    const float ROTATION_SPEED = 0.1f;
    public static void Main(string[] args)
    {


        Player player = new Player(new Vector2f(32, 32));
        RenderWindow window = new RenderWindow(new VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Raycaster");
        Map map = new Map();
        window.Closed += (sender, args) => window.Close();
        window.KeyPressed += (sender, args) => KeyPressed(sender, args, player);
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Black);
           
            
            map.DrawTiles(window);
            player.Draw(window);
            Rays.DrawTestRaysSide(player, window, map);
            window.Display();
        }
    }

    private static void KeyPressed(object? sender, KeyEventArgs e, Player player)
    {
        RenderWindow window = (RenderWindow)sender;
        if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }
        if (e.Code == Keyboard.Key.S)
        {
            player.Position += player.Direction * 5f;
        }
        if (e.Code == Keyboard.Key.W)
        {
            player.Position -= player.Direction * 5f;
        }
        if (e.Code == Keyboard.Key.A)
        {
            if (player.Angle - ROTATION_SPEED < 0)
            {
                float remaining = player.Angle - ROTATION_SPEED;
                player.Angle = (2f * MathF.PI);
            }
            else
            {
                player.Angle -= ROTATION_SPEED;
            }
        }
        if (e.Code == Keyboard.Key.D)
        {
            if (player.Angle + ROTATION_SPEED > (2 * MathF.PI))
            {
                float remaining = (player.Angle + ROTATION_SPEED) - player.Angle;
                player.Angle = 0f + remaining;
            }
            else
            {
                player.Angle += ROTATION_SPEED;
            }
        }

    }
    public static Vector2f Normalize(Vector2f vector)
    {
        float magnitude = MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        if (magnitude > 0)
        {
            return new Vector2f(vector.X / magnitude, vector.Y / magnitude);
        }
        else
        {
            return new Vector2f(); // Return zero vector if magnitude is zero to avoid division by zero
        }
    }
}