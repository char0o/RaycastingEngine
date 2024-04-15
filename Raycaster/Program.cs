using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Raycaster;
using System.Diagnostics;

class Program
{
    public const int SCREEN_WIDTH = 1920;
    public const int SCREEN_HEIGHT = 1080;
    public const int MAP_SIZE_X = 8;
    public const int MAP_SIZE_Y = 8;
    const float ROTATION_SPEED = 0.1f;
    private static bool ToggleMinimap = false;

    public static void Main(string[] args)
    {
        Player player = new Player(new Vector2f(64, 64));
        Font font = new Font("resources/arial.ttf");
        Text text = new Text("", font);
        text.Position = new Vector2f(SCREEN_WIDTH - 200, 20);
        ContextSettings context = new ContextSettings() { AntialiasingLevel = 16 };
        RenderWindow window = new RenderWindow(new VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Raycaster", Styles.Default, new ContextSettings() { AntialiasingLevel = 4 });
        Map map = new Map();
        Texture textures = GetCombinedTexture();
        VertexArray skyAndGroundVertexArray = new VertexArray(PrimitiveType.Lines, SCREEN_WIDTH * 18);
        VertexArray wallVertexArray = new VertexArray(PrimitiveType.Lines, SCREEN_WIDTH * 18);


        window.Closed += (sender, args) => window.Close();
        window.KeyPressed += (sender, args) => KeyPressed(sender, args, player);
        window.KeyReleased += (sender, args) => KeyReleased(sender, args, player);
        window.MouseMoved += (sender, args) => MouseMoved(sender, args, player);
        window.SetMouseCursorVisible(false);

        Sprite textureDebug = new Sprite(textures);

        Clock clock = new Clock();
        Clock fpsClock = new Clock();

        RenderStates states = new RenderStates(textures);
        while (window.IsOpen)
        {
            fpsClock.Restart();
            window.DispatchEvents();
            float deltaTime = clock.Restart().AsSeconds();
            player.UpdatePosition(deltaTime);
            window.Clear(Color.Black);
           
            wallVertexArray.Clear();
            skyAndGroundVertexArray.Clear();
            Rays.Draw3DWorldTextured(player, window, map, wallVertexArray, skyAndGroundVertexArray);

            window.Draw(skyAndGroundVertexArray, RenderStates.Default);
            window.Draw(wallVertexArray, states);
            if (ToggleMinimap)
            {
                player.Draw(window);
                map.DrawMinimap(window);
                Rays.DrawMinimapRays(player, window, map);
                
            }
            double fps = 1.0 / fpsClock.ElapsedTime.AsSeconds();
            text.DisplayedString = $"Fps: {fps: 0.00}";
            //window.Draw(textureDebug);
            window.Draw(text);
            window.Display();
        }
    }

    public static Texture GetCombinedTexture()
    {
        Texture wallTexture = new Texture("resources/hl22.png");
        Texture windowTexture = new Texture("resources/window.png");
        Texture wallTexture2 = new Texture("resources/brickwall2.png");
        List<Texture> textures = new List<Texture>();
        textures.Add(wallTexture);
        textures.Add(windowTexture);
        textures.Add(wallTexture2);
        RenderTexture textureAtlas = new RenderTexture(2048, 2048);

        textureAtlas.Display();
        textureAtlas.Smooth = true;
        textureAtlas.Clear(Color.Transparent);
        int index = 0;
        foreach (Texture texture in textures)
        {
            Sprite sprite = new Sprite(texture);
            sprite.Position = new Vector2f(0 + index * 512f, 0);
            textureAtlas.Draw(sprite);
            index++;
        }
        return new Texture(textureAtlas.Texture);
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
        Vector2f directionVector = new Vector2f();
        if (e.Code == Keyboard.Key.S)
        {
            player.SPressed = true;
        }
        if (e.Code == Keyboard.Key.W)
        {
            player.WPressed = true;
        }
        if (e.Code == Keyboard.Key.A)
        {
            player.APressed = true;
        }
        if (e.Code == Keyboard.Key.D)
        {
            player.DPressed = true;
        }
        if (e.Code == Keyboard.Key.LShift)
        {
            player.Speed = 100f;
        }
        if (e.Code == Keyboard.Key.M)
        {
            ToggleMinimap = !ToggleMinimap;
        }
    }
    private static void KeyReleased(object sender, KeyEventArgs e, Player player)
    {
        if (e.Code == Keyboard.Key.S)
        {
            player.SPressed = false;
        }
        if (e.Code == Keyboard.Key.W)
        {
            player.WPressed = false;
        }
        if (e.Code == Keyboard.Key.A)
        {
            player.APressed = false;
        }
        if (e.Code == Keyboard.Key.D)
        {
            player.DPressed = false;
        }
        if (e.Code == Keyboard.Key.LShift)
        {
            player.Speed = 50f;
        }
    }
}