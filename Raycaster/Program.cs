using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Raycaster;
using System.Diagnostics;
class Program
{
    public const int SCREEN_WIDTH = 800;
    public const int SCREEN_HEIGHT = 600;
    public const int MAP_SIZE_X = 8;
    public const int MAP_SIZE_Y = 8;
    const float ROTATION_SPEED = 0.1f;
    public static void Main(string[] args)
    {


        Player player = new Player(new Vector2f(64, 64));
        Font font = new Font("resources/arial.ttf");
        Text text = new Text("", font);
        text.Position = new Vector2f(500, 32);
        RenderWindow window = new RenderWindow(new VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Raycaster");
        Map map = new Map();
        window.Closed += (sender, args) => window.Close();
        window.KeyPressed += (sender, args) => KeyPressed(sender, args, player);
        window.MouseMoved += (sender, args) => MouseMoved(sender, args, player);
        window.SetMouseCursorVisible(false);
        Stopwatch clock = new Stopwatch();
        while (window.IsOpen)
        {
            
            window.DispatchEvents();
            clock.Restart();
            window.Clear(Color.Black);
                      
            player.Draw(window);
            Rays.Draw3DWorld(player, window, map);
            map.DrawTiles(window);
            Rays.DrawRays(player, window, map);
            double fps = 1.0 / clock.Elapsed.TotalSeconds;
            text.DisplayedString = $"Fps: {fps: 0.00}";
            window.Draw(text);
            window.Display();
        }
    }

    public static void MouseMoved(object sender, MouseMoveEventArgs args, Player player)
    {
        RenderWindow window = (RenderWindow)sender;
        float sensitivity = 0.005f;
        Vector2i center = new Vector2i(Program.SCREEN_WIDTH / 2, Program.SCREEN_HEIGHT / 2);
        Vector2i mousePos = Mouse.GetPosition(window);
        Vector2f deltaMouse = new Vector2f(mousePos.X - center.X, mousePos.Y - center.Y);

        Mouse.SetPosition(center, window);

        player.Angle += deltaMouse.X * sensitivity;

        if (player.Angle < 0)
            player.Angle += 2 * MathF.PI;
        else if (player.Angle >= 2 * MathF.PI)
            player.Angle -= 2 * MathF.PI;
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
            player.Position -= player.Direction * 5f;
        }
        if (e.Code == Keyboard.Key.W)
        {
            player.Position += player.Direction * 5f;
        }
        if (e.Code == Keyboard.Key.A)
        {
            Vector2f newDirection = new Vector2f(-player.Direction.Y, player.Direction.X);
            player.Position -= newDirection * 5f;
        }
        if (e.Code == Keyboard.Key.D)
        {
            Vector2f newDirection = new Vector2f(-player.Direction.Y, player.Direction.X);
            player.Position += newDirection * 5f;
        }

    }
}