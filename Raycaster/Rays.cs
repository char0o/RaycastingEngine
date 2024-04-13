using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Raycaster
{
    public static class Rays
    {
        private static float fov = 0f;
        private static float halfFov = 0f;
        private static float increment = 0f;
        private static int halfScreen = 0;
        private static float maxRayDistance = 500f;
        private static float twoPi = 0;
        private static float halfThreePi = 0;
        private static float halfPi = 0;
        static Rays()
        {
            
            fov = 1.0472f;
            halfFov = fov / 2f;
            increment = fov / Program.SCREEN_WIDTH;
            halfScreen = Program.SCREEN_HEIGHT / 2;
            twoPi = MathF.PI * 2f;
            halfPi = MathF.PI / 2f;
            halfThreePi = (3f * MathF.PI) / 2f;
        }
        public static void DrawTestRays(Player player, RenderWindow window, Map map)
        {
            float angle = player.Angle - halfFov;

            for (int rays = 0; rays < Program.SCREEN_WIDTH; rays++)
            {
                float rayCos = MathF.Cos(angle);
                float raySin = MathF.Sin(angle);

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
        public static float GetDistance(Vector2f initPos, Vector2f endPos)
        {
            return MathF.Sqrt(MathF.Pow(endPos.X - initPos.X, 2f) + MathF.Pow(endPos.Y - initPos.Y, 2f));
        }
        public static Vector2f GetRayXHit(Vector2f playerPos, float angle, Map map)
        {
            float rayAngle = angle;
            float perpendicularSlope = -1 / MathF.Tan(rayAngle);

            Vector2f rayPos = new Vector2f();
            Vector2f rayOffset = new Vector2f();
            Vector2i mapCoords = new Vector2i();

            if (rayAngle > MathF.PI)
            {
                rayPos.Y = (((int)(playerPos.Y) / 32) * 32f) - 0.0001f;
                rayPos.X = (playerPos.Y - rayPos.Y) * perpendicularSlope + playerPos.X;

                rayOffset.Y = -32f;
                rayOffset.X = -(rayOffset.Y) * perpendicularSlope;
            }
            else if (rayAngle < MathF.PI)
            {
                rayPos.Y = (((int)(playerPos.Y) / 32) * 32f) + 32f;
                rayPos.X = (playerPos.Y - rayPos.Y) * perpendicularSlope + playerPos.X;

                rayOffset.Y = 32f;
                rayOffset.X = -(rayOffset.Y) * perpendicularSlope;
            }
            else if (rayAngle == 0 || rayAngle == MathF.PI)
            {
                rayPos.X = playerPos.X;
                rayPos.Y = playerPos.Y;
            }
            int maxDistance = 8;
            while (maxDistance > 0)
            {
                mapCoords.X = (int)(rayPos.X) / 32;
                mapCoords.Y = ((int)(rayPos.Y) / 32);

                if (mapCoords.X < 0 || mapCoords.Y < 0 || mapCoords.X > 7 || mapCoords.Y > 7)
                    break;
                if (map.WorldMap[mapCoords.X, mapCoords.Y] == 1)
                {
                    break;
                }
                else
                {
                    rayPos.X += rayOffset.X;
                    rayPos.Y += rayOffset.Y;
                }
                maxDistance--;
            }
            return rayPos;
        }
        public static Vector2f GetRayYHit(Vector2f playerPos, float angle, Map map)
        {
            float rayAngle = angle;
            float perpendicularSlope = -MathF.Tan(rayAngle);

            Vector2f rayPos = new Vector2f();
            Vector2f rayOffset = new Vector2f();
            Vector2i mapCoords = new Vector2i();

            if (rayAngle > halfPi && rayAngle < halfThreePi)
            {
                rayPos.X = (((int)(playerPos.X) / 32) * 32f) - 0.0001f;
                rayPos.Y = (playerPos.X - rayPos.X) * perpendicularSlope + playerPos.Y;

                rayOffset.X = -32f;
                rayOffset.Y = -(rayOffset.X) * perpendicularSlope;
            }
            else if (rayAngle < halfPi || rayAngle  > halfThreePi)
            {
                rayPos.X = (((int)(playerPos.X) / 32) * 32f) + 32f;
                rayPos.Y = (playerPos.X - rayPos.X) * perpendicularSlope + playerPos.Y;

                rayOffset.X = 32f;
                rayOffset.Y = -(rayOffset.X) * perpendicularSlope;
            }
            else if (rayAngle == 0 || rayAngle == MathF.PI / 2)
            {
                rayPos.Y = playerPos.Y;
                rayPos.X = playerPos.Y;
            }
            int maxDistance = 8;
            while (maxDistance > 0)
            {
                mapCoords.X = (int)(rayPos.X) / 32;
                mapCoords.Y = ((int)(rayPos.Y) / 32);

                if (mapCoords.X < 0 || mapCoords.Y < 0 || mapCoords.X > 7 || mapCoords.Y > 7)
                    break;
                if (map.WorldMap[mapCoords.X, mapCoords.Y] == 1)
                {
                    break;
                }
                else
                {
                    rayPos.X += rayOffset.X;
                    rayPos.Y += rayOffset.Y;
                }
                maxDistance--;
            }
            return rayPos;

        }
        public static void Draw3DWorld(Player player, RenderWindow window, Map map)
        {
            Color blue = new Color(0, 0, 200, 255);
            for (int rayNum = 0; rayNum < Program.SCREEN_WIDTH; rayNum++)
            {
                float angle = player.Angle - halfFov + rayNum * increment;
                if (angle < 0)
                    angle += MathF.PI * 2f;
                if (angle > MathF.PI * 2f)
                    angle -= MathF.PI * 2f;

                Vector2f rayX = GetRayXHit(player.Position, angle, map);
                Vector2f rayY = GetRayYHit(player.Position, angle, map);
                float distanceX = GetDistance(player.Position, rayX);
                float distanceY = GetDistance(player.Position, rayY);

                float finalDistance = float.MaxValue;
               
                if (distanceY < distanceX)
                {
                    blue = new Color(0, 0, 100, 255);
                    finalDistance = distanceY;
                }
                if (distanceX < distanceY)
                {
                    finalDistance = distanceX;
                    blue = new Color(0, 0, 150, 255);
                }
                    
                finalDistance = finalDistance * MathF.Cos(angle - player.Angle);

                float wallHeight = MathF.Floor(halfScreen / finalDistance * 25f);

                Vertex[] wallStrip = {
                    new Vertex(new Vector2f(rayNum, halfScreen - wallHeight), blue),
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), blue)
                };
                Vertex[] sky =
{
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), new Color(135, 106, 125, 255)),
                    new Vertex(new Vector2f(rayNum, 0f), new Color(135, 106, 235, 255))
                };
                Vertex[] floor = {
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), new Color(64, 64, 64)),
                    new Vertex(new Vector2f(rayNum, Program.SCREEN_HEIGHT), new Color(32, 32, 32))
                    };
                
                window.Draw(sky, PrimitiveType.Lines);
                window.Draw(wallStrip, PrimitiveType.Lines);
                window.Draw(floor, PrimitiveType.Lines);
            }
        }
        public static Vector2f GetFinalRay(Player player, float angle, Map map)
        {
                Vector2f rayPosY = GetRayYHit(player.Position, angle, map);
                Vector2f rayPosX = GetRayXHit(player.Position, angle, map);

                float distanceX = GetDistance(player.Position, rayPosX);
                float distanceY = GetDistance(player.Position, rayPosY);

                if (distanceX < distanceY)
                {
                    return rayPosX;
                }
                else
                {
                    return rayPosY;
                }
        }
        public static void DrawRays(Player player, RenderWindow window, Map map)
        {
            for (int rayNum = 0; rayNum < Program.SCREEN_WIDTH; rayNum++)
            {
                float angle = player.Angle - halfFov + rayNum * increment;
                if (angle < 0)
                    angle += MathF.PI * 2f;
                if (angle > MathF.PI * 2f)
                    angle -= MathF.PI * 2f;
                Vector2f rayPos = GetFinalRay(player, angle, map);
                Vertex[] wallStrip = new Vertex[2];
                wallStrip[0] = new Vertex(new Vector2f(player.Position.X, player.Position.Y), Color.Blue);
                wallStrip[1] = new Vertex(new Vector2f(rayPos.X, rayPos.Y), Color.Blue);
                
                window.Draw(wallStrip, PrimitiveType.Lines);
            }

        }
        public static void DrawRaysDDA(Player player, RenderWindow window, Map map)
        {
            
            for (int rayNum = 0; rayNum < Program.SCREEN_WIDTH; rayNum++)
            {
                float angle = player.Angle - halfFov + rayNum * increment;
                float rayDirX = MathF.Cos(angle);
                float rayDirY = MathF.Sin(angle);

                Vector2f rayEnd = new Vector2f(player.Position.X + rayDirX * maxRayDistance,
                                            player.Position.Y + rayDirY * maxRayDistance);

                float deltaX = rayEnd.X - player.Position.X;
                float deltaY = rayEnd.Y - player.Position.Y;

                float steps = 0;
                if (MathF.Abs(deltaX) > MathF.Abs(deltaY))
                    steps = MathF.Floor(MathF.Abs(deltaX));
                else
                    steps = MathF.Floor(MathF.Abs(deltaY));

                float xInc = deltaX / steps;
                float yInc = deltaY / steps;

                float x = player.Position.X;
                float y = player.Position.Y;

                int wall = 0;
                int side = 0;
                for (int i = 0; i < steps; i++)
                {
                    x += xInc;
                    y += yInc;

                    if (x < 0 || x > Program.SCREEN_WIDTH || y < 0 || y > Program.SCREEN_HEIGHT)
                        break;
                    int indexX = (int)(x / 32f);
                    int indexY = (int)(y / 32f);
                    rayEnd = new Vector2f(x, y);
                    wall = map.WorldMap[indexX, indexY];
                    if (wall != 0)
                    {
                        int diffX = (int)(x + yInc) - indexX * 32;
                        int diffY = (int)(y + yInc) - indexY * 32;

                        if (MathF.Abs(diffX) < MathF.Abs(diffY))
                        {
                            side = xInc < 0 ? 2 : 3;
                        }
                        else
                        {
                            side = yInc < 0 ? 0 : 1;
                        }

                        break;
                    }
                }
                Color color = Color.Blue;
                switch (side)
                {
                    case 0:
                    case 1:
                        color = Color.Cyan;
                        break;
                    case 2:
                    case 3:
                        color = Color.Magenta;
                        break;
                }
                float distance = MathF.Sqrt(MathF.Pow(player.Position.X - x, 2) + MathF.Pow(player.Position.Y - y, 2));
                distance = distance * MathF.Cos(angle - player.Angle);

                float wallHeight = MathF.Floor(halfScreen / distance * 25f);


                Vertex[] wallStrip = {
                    new Vertex(new Vector2f(rayNum, halfScreen - wallHeight), color),
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), color)
                };
                Vertex[] line = { new Vertex(player.Position, color), new Vertex(rayEnd, color) };
                window.Draw(wallStrip, PrimitiveType.Lines);
                window.Draw(line, PrimitiveType.Lines);
            }
        }
        public static void DrawSingleRay(Player player, RenderWindow window, Map map)
        {
            float angle = player.Angle;

            for (int rayNum = 0; rayNum < 2; rayNum++)
            {
                float rayCos = MathF.Cos(angle) / 64f;
                float raySin = MathF.Sin(angle) / 64f;

                Vector2f rayPos = player.Position;
                int wall = 0;
                int side = 0;

                while (wall == 0)
                {
                    rayPos.X += rayCos;
                    rayPos.Y += raySin;

                    int indexX = (int)rayPos.X;
                    int indexY = (int)rayPos.Y;
                    if (indexX < 0 || indexX >= Program.SCREEN_WIDTH || indexY < 0 || indexY >= Program.SCREEN_HEIGHT)
                        break;
                    wall = map.WorldMap[indexX / 32, indexY / 32];
                    if (wall != 0)
                    {

                        float decX = rayPos.X - MathF.Floor(rayPos.X);
                        float decY = rayPos.Y - MathF.Floor(rayPos.Y);
                        if (decX < 0.5 && decY < 0.5)
                        {
                            side = 0;
                        }
                        else if (decX >= 0.5 && decY < 0.5)
                        {
                            side = 1;
                        }
                        else if (decX < 0.5 && rayPos.Y - decY >= 0.5)
                        {
                            side = 2;
                        }
                        else
                        {
                            side = 3;
                        }
                    }
                    Color color = Color.Black;

                    switch (side)
                    {
                        case 0:
                            color = Color.Blue;
                            break;
                        case 1:
                            color = Color.White;
                            break;
                        case 2:
                            color = Color.Red;
                            break;
                        case 3:
                            color = new Color(125, 125, 0, 255);
                            break;
                    }
                    Vertex[] line = { new Vertex(player.Position, color), new Vertex(rayPos, color) };
                    window.Draw(line, PrimitiveType.Lines);
                }
            }
        }
        public static void DrawTestRaysSide(Player player, RenderWindow window, Map map)
        {
            float angle = player.Angle - halfFov;

            for (int rayNum = 0; rayNum < Program.SCREEN_WIDTH; rayNum++)
            {
                float rayCos = MathF.Cos(angle) / 64f;
                float raySin = MathF.Sin(angle) / 64f;

                Vector2f rayEnd = new Vector2f(player.Position.X + rayCos * maxRayDistance,
                            player.Position.Y + raySin * maxRayDistance);

                float deltaX = rayEnd.X - player.Position.X;
                float deltaY = rayEnd.Y - player.Position.Y;

                float steps = 0;
                if (MathF.Abs(deltaX) > MathF.Abs(deltaY))
                    steps = MathF.Floor(MathF.Abs(deltaX));
                else
                    steps = MathF.Floor(MathF.Abs(deltaY));

                Vector2f rayPos = player.Position;
                int wall = 0;
                int side = 0;

                float xInc = deltaX / steps;
                float yInc = deltaY / steps;

                while (wall == 0)
                {
                    rayPos.X += rayCos;
                    rayPos.Y += raySin;

                    int indexX = (int)MathF.Round(rayPos.X);
                    int indexY = (int)MathF.Round(rayPos.Y);
                    rayEnd = new Vector2f(rayPos.X, rayPos.Y);
                    if (indexX < 0 || indexX >= Program.SCREEN_WIDTH || indexY < 0 || indexY >= Program.SCREEN_HEIGHT)
                        break;
                    wall = map.WorldMap[indexX / 32, indexY / 32];
                    if (wall != 0)
                    {
                        int diffX = (int)((rayPos.X + yInc) - indexX / 32);
                        int diffY = (int)((rayPos.Y + yInc) - indexY / 32);

                        if (MathF.Abs(diffX) < MathF.Abs(diffY))
                        {
                            side = xInc < 0 ? 2 : 3;
                        }
                        else
                        {
                            side = yInc < 0 ? 0 : 1;
                        }
                    }
                }
                Color color = Color.Black;

                switch (side)
                {
                    case 0:
                    case 1:
                        color = Color.Cyan;
                        break;
                    case 2:
                    case 3:
                        color = Color.Magenta;
                        break;
                }
                float distance = MathF.Sqrt(MathF.Pow(player.Position.X - rayPos.X, 2) + MathF.Pow(player.Position.Y - rayPos.Y, 2));
                distance = distance * MathF.Cos(angle - player.Angle);

                float wallHeight = MathF.Floor(halfScreen / distance * 25f);



                Vertex[] wallStrip = {
                    new Vertex(new Vector2f(rayNum, halfScreen - wallHeight), color),
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), color)
                };
                Vertex[] sky =
                {
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), new Color(135, 106, 125, 255)),
                    new Vertex(new Vector2f(rayNum, 0f), new Color(135, 106, 235, 255))
                };
                Vertex[] floor = {
                    new Vertex(new Vector2f(rayNum, halfScreen + wallHeight), new Color(64, 64, 64)),
                    new Vertex(new Vector2f(rayNum, Program.SCREEN_HEIGHT), new Color(32, 32, 32))
                    };
                //window.Draw(sky, PrimitiveType.Lines);
                window.Draw(wallStrip, PrimitiveType.Lines);
                window.Draw(floor, PrimitiveType.Lines);
                Vertex[] line = { new Vertex(player.Position, Color.White), new Vertex(rayPos, Color.White) };
                window.Draw(line, PrimitiveType.Lines);


                angle += increment;
            }
        }

        public static void DrawRaysOld(Player player, RenderWindow window, Map map)
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
