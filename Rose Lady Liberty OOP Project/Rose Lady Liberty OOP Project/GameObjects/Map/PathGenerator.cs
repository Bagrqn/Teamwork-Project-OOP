﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace RoseLadyLibertyOOPProject.GameObjects.Map
{
    public class PathGenerator
    {
        public enum Direction
        {
            Down = 1,
            Up = 2,
            Right = 4
        }

        public class Path : List<Direction>
        {
            public static Direction GetNewDirection(Direction allowed, Random rnd)
            {
                Direction newd;
                int maxd = Enum.GetValues(typeof(Direction)).Length;
                int[] vals = (int[])Enum.GetValues(typeof(Direction));
                do
                {
                    var t = rnd.Next(0, maxd);
                    newd = (Direction)vals[t];
                }
                while ((newd & allowed) == 0);
                return newd;
            }

            public static Path GenerateRandomPath(int startx, int starty, int endx, int endy)
            {
                Path newpath = new Path();
                Random rnd = new Random();

                int curx = startx;
                int cury = starty;
                Direction prevd = Direction.Down;
                Direction curd = Direction.Right;
                Direction newd = curd;

                while (curx != endx)
                {
                    do
                    {
                        if (curd == prevd)
                        {
                            if (cury >= (endy - 2)) newd = GetNewDirection(Direction.Up | Direction.Right, rnd);
                            else if (cury <= 2) newd = GetNewDirection(Direction.Down | Direction.Right, rnd);
                            else if (curx <= 0) newd = GetNewDirection(Direction.Right | Direction.Down | Direction.Up, rnd);
                            else newd = GetNewDirection(Direction.Right | Direction.Down | Direction.Up, rnd);
                        }

                    }
                    while ((newd | curd) == (Direction.Up | Direction.Down)); // excluding going back

                    newpath.Add(newd);
                    prevd = curd;
                    curd = newd;
                    switch (newd)
                    {
                        case Direction.Up:
                            cury--;
                            break;
                        case Direction.Right:
                            curx++;
                            break;
                        case Direction.Down:
                            cury++;
                            break;
                    }
                }
                return newpath;
            }
        }

        public static void InitializeStartEndPoints(int maxHeight, int maxWidth, out Tuple<int, int> startPoint, out Tuple<int, int> endPoint)
        {
            Random rand = new Random();

            startPoint = new Tuple<int, int>(0, rand.Next(2, maxHeight - 1)); // startpoint = leftside
            endPoint = new Tuple<int, int>(maxWidth, rand.Next(2, maxHeight - 1));  // startpoint = rightside
        }

        public static List<Tuple<int, int>> GeneratePath(int maxWidth, int maxHeight)
        {
            Tuple<int, int> startPoint;
            Tuple<int, int> endPoint;

            PathGenerator.InitializeStartEndPoints(maxHeight, maxWidth, out startPoint, out endPoint);
            Path path = Path.GenerateRandomPath(startPoint.Item1, startPoint.Item2, maxWidth -1, maxHeight - 1);
            List<Tuple<int, int>> nodeCordinates = new List<Tuple<int, int>>();
            nodeCordinates.Add(startPoint);

            for (int i = 0; i < path.Count; i++)
            {
                var lastCordinate = nodeCordinates.Last();
                switch (path[i])
                {
                    case Direction.Down:
                        nodeCordinates.Add(new Tuple<int, int>(lastCordinate.Item1, lastCordinate.Item2 + 1));
                        break;
                    case Direction.Up:
                        nodeCordinates.Add(new Tuple<int, int>(lastCordinate.Item1, lastCordinate.Item2 - 1));
                        break;
                    case Direction.Right:
                        nodeCordinates.Add(new Tuple<int, int>(lastCordinate.Item1 + 1, lastCordinate.Item2));
                        break;
                }
            }
            return nodeCordinates;
        }
    }
}
