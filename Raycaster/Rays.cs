using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    public static class Rays
    {
        private static float fov = 0f;
        private static float halfFov = 0f;
        private static float increment = 0f;
        private static int halfScreen = 0;
        static Rays()
        {
            fov = 1.0472f;
            halfFov = fov / 2f;
            increment = fov / Program.SCREEN_WIDTH;
            halfScreen = Program.SCREEN_HEIGHT / 2;
        }
        public static void DrawTestRays(Player player, RenderWindow window, Map map)
        {
            float angle = player.Angle - halfFov;

            for (int rays = 0; rays < Program.SCREEN_WIDTH; rays++)
            {
                float rayCos = MathF.Cos(angle) / 64f;
                float raySin = MathF.Sin(angle) / 64f;

                Vector2f rayPos = player.Position;
                int wall = 0;
                while (wall == 0)
                {
                    rayPos.X += rayCos;
                    rayPos.Y += raySin;
                    int indexX = (int)rayPos.X;
                    int indexY = (int)rayPos.Y;
                    if (indexX < 0 || indexX >= Program.SCREEN_WIDTH || indexY < 0 || indexY >= Program.SCREEN_HEIGHT)
                        break;
                    wall = map.WorldMap[indexX / 32, indexY / 32];
                }
                float distance = MathF.Sqrt(MathF.Pow(player.Position.X - rayPos.X, 2) + MathF.Pow(player.Position.Y - rayPos.Y, 2));
                distance = distance * MathF.Cos(angle - player.Angle);

                float wallHeight = MathF.Floor(halfScreen / distance * 25f);

                Vertex[] wallStrip = { 
                    new Vertex(new Vector2f(rays, halfScreen - wallHeight), Color.Blue), 
                    new Vertex(new Vector2f(rays, halfScreen + wallHeight), Color.Blue) 
                };
                Vertex[] floor = {
                    new Vertex(new Vector2f(rays, halfScreen + wallHeight), Color.Green),
                    new Vertex(new Vector2f(rays, Program.SCREEN_HEIGHT), Color.Green)
                    };
                window.Draw(wallStrip, PrimitiveType.Lines);
                window.Draw(floor, PrimitiveType.Lines);
                Vertex[] line = { new Vertex(player.Position, Color.White), new Vertex(rayPos, Color.White) };
                window.Draw(line, PrimitiveType.Lines);

                angle += increment;
            }
        }
        public static void DrawTestRaysSide(Player player, RenderWindow window, Map map)
        {
            float angle = player.Angle - halfFov;

            for (int rays = 0; rays < Program.SCREEN_WIDTH; rays++)
            {
                float rayCos = MathF.Cos(angle) / 64f;
                float raySin = MathF.Sin(angle) / 64f;

                Vector2f rayPos = player.Position;
                int wall = 0;
                int side = 0;
                Color color = Color.Blue;
                while (wall == 0)
                {
                    rayPos.X += rayCos;
                    rayPos.Y += raySin;

                    int indexX = (int)rayPos.X;
                    int indexY = (int)rayPos.Y;
                    if (indexX < 0 || indexX >= Program.SCREEN_WIDTH || indexY < 0 || indexY >= Program.SCREEN_HEIGHT)
                        break;
                    wall = map.WorldMap[indexX / 32, indexY / 32];
                    if (wall == 1 && rayPos.X > rayPos.Y)
                        color -= new Color(125, 125, 125, 0);
                }
                float distance = MathF.Sqrt(MathF.Pow(player.Position.X - rayPos.X, 2) + MathF.Pow(player.Position.Y - rayPos.Y, 2));
                distance = distance * MathF.Cos(angle - player.Angle);

                float wallHeight = MathF.Floor(halfScreen / distance * 25f);

                Vertex[] wallStrip = {
                    new Vertex(new Vector2f(rays, halfScreen - wallHeight), color),
                    new Vertex(new Vector2f(rays, halfScreen + wallHeight), color)
                };
                Vertex[] floor = {
                    new Vertex(new Vector2f(rays, halfScreen + wallHeight), Color.Green),
                    new Vertex(new Vector2f(rays, Program.SCREEN_HEIGHT), Color.Green)
                    };
                window.Draw(wallStrip, PrimitiveType.Lines);
                window.Draw(floor, PrimitiveType.Lines);
                Vertex[] line = { new Vertex(player.Position, Color.White), new Vertex(rayPos, Color.White) };
                window.Draw(line, PrimitiveType.Lines);

                angle += increment;
            }
        }

        public static void DrawRays(Player player, RenderWindow window, Map map)
        {
            float fov = MathF.PI / 2;
            float rayAngle = player.Angle - (MathF.PI);
            float maxRayDistance = 1000f;
            for (int i = 0; i < 1; i++)
            {
                float rayDirectionX = MathF.Cos(rayAngle);
                float rayDirectionY = MathF.Sin(rayAngle);


                Vector2f rayEnd = player.Position + new Vector2f(rayDirectionX * maxRayDistance, rayDirectionY * maxRayDistance);
                Vertex[] line = { new Vertex(player.Position, Color.White), new Vertex(rayEnd, Color.White) };
                window.Draw(line, PrimitiveType.Lines);
            }
        }
        public static float RadToDeg(float rad)
        {
            return (180 / MathF.PI) * rad;
        }
        public static float DegToRad(float rad)
        {
            return MathF.PI / 180 * rad;
        }
    }
}
