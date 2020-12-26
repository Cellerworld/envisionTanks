using Envision.Tanks.Math;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Envision.Tanks
{
    public class Terrain : GameObject
    {
        private int width;
        private int height;
        private int minHeight;
        private int maxHeight;
        private Vector2 tanksize;

        GraphicsPath path = new GraphicsPath(FillMode.Alternate);
        Pen pen = new Pen(Color.ForestGreen);
        Random rnd;

        private Vector2 startPoint;
        private Vector2 endPoint;

        private Vector2 tank1Foothold;
        private Vector2 tank2Foothold;
        private float tank1FootholdEnd;
        private float tank2FootholdEnd;

        private Point[] inBetweenPoints;
        private Vector2 LeftWallPoint;
        private Vector2 RightWallPoint;

        public Terrain(int width, int height, Vector2 tanksize) : base(Vector2.Zero)
        {
            this.isStatic = true;
            this.tag = "terrain";
            this.width = width;
            this.minHeight = height - 10;
            this.height = height;
            this.maxHeight = (int)(height * 0.75f);
            this.tanksize = tanksize;

            startPoint = new Vector2(-50, height);
            endPoint = new Vector2(width + 50, height);
            inBetweenPoints = new Point[40];
            GenerateTerrain();
        }

        //if I had made a tank list I could just ask for a tank index.
        public Vector2 GetTankSpawnPos(bool forP1)
        {
            if (forP1)
                return new Vector2(tank1Foothold.X, tank1Foothold.Y - tanksize.Y);
            else
                return new Vector2(tank2Foothold.X, tank2Foothold.Y - tanksize.Y);
        }

        public override void FixedUpdate()
        {
        }

        public override void GraphicsUpdate(object sender, PaintEventArgs e)
        {
            DrawTerrain(e);
        }

        private void GenerateTerrain()
        {
            rnd = new Random();
            GeneratePlayerFootings();
            ConnectFootings();
            ConnectToWalls();
            GeneratePath();
        }

        private void GeneratePlayerFootings()
        {
            tank1Foothold.X = rnd.Next(0, width / 3);
            tank1Foothold.Y = rnd.Next(maxHeight, minHeight);
            tank1FootholdEnd = tank1Foothold.X + tanksize.X;

            tank2Foothold.X = rnd.Next((int)(width * 0.66f), width - (int)tanksize.X);
            tank2Foothold.Y = rnd.Next(maxHeight, minHeight);
            tank2FootholdEnd = tank2Foothold.X + tanksize.X;
        }

        private void ConnectFootings()
        {
            //define some points and connect these randomly per lines and curves
            float x = tank1FootholdEnd;
            float y = tank1Foothold.Y;
            float xTankDistance = tank2Foothold.X - tank1FootholdEnd;
            float xDistanceOfInBetweenPoints = (xTankDistance - (xTankDistance / inBetweenPoints.Length)) / inBetweenPoints.Length;
            int yVarianz = minHeight / (12);

            for (int i = 0; i < inBetweenPoints.Length; i++)
            {
                x += xDistanceOfInBetweenPoints;
                y = y + rnd.Next(-yVarianz, yVarianz);
                y = System.Math.Min(y, minHeight);
                y = System.Math.Max(y, maxHeight);
                if (inBetweenPoints.Length - i < 5)
                {
                    y = System.Math.Max(y, tank2Foothold.Y - yVarianz / i);
                }
                inBetweenPoints[i] = new Point((int)x, (int)y);
            }
        }

        private void ConnectToWalls()
        {
            LeftWallPoint = new Vector2(0, tank1Foothold.Y);
            RightWallPoint = new Vector2(width, tank2Foothold.Y);
        }

        private void GeneratePath()
        {
            path.AddLine(startPoint.X, startPoint.Y, LeftWallPoint.X, LeftWallPoint.Y);
            path.AddLine(LeftWallPoint.X, LeftWallPoint.Y, tank1Foothold.X, tank1Foothold.Y);
            path.AddLine(tank1Foothold.X, tank1Foothold.Y, tank1FootholdEnd, tank1Foothold.Y);

            //path.AddLine(tank1FootholdEnd, tank1Foothold.Y, tank2Foothold.X, tank2Foothold.Y);//replace with funtion
            path.AddCurve(inBetweenPoints);
            //AddInBetweenPointsToPath();

            path.AddLine(tank2Foothold.X, tank2Foothold.Y, tank2FootholdEnd, tank2Foothold.Y);
            path.AddLine(tank2FootholdEnd, tank2Foothold.Y, RightWallPoint.X, RightWallPoint.Y);
            path.AddLine(RightWallPoint.X, RightWallPoint.Y, endPoint.X, endPoint.Y);

            GeneratePathCollision();
        }

        //I like the curve alternative better
        private void AddInBetweenPointsToPath()
        {
            float x = tank1FootholdEnd;
            float y = tank1Foothold.Y;
            for (int i = 0; i < inBetweenPoints.Length; i++)
            {
                path.AddLine(x, y, inBetweenPoints[i].X, inBetweenPoints[i].Y);
                x = inBetweenPoints[i].X;
                y = inBetweenPoints[i].Y;
            }
            path.AddLine(x, y, tank2Foothold.X, tank2Foothold.Y);
        }

        //If I think I have time maybe try alternating, but I think 
        private void AddCurveOrline(float x1, float y1, float x2, float y2)
        {
        }

        private void GeneratePathCollision()
        {
            for (int i = 0; i < path.PointCount - 1; i++)
            {
                AddComponent<Collider>();
                collider[i].localPosition = new Vector2(path.PathPoints[i].X, path.PathPoints[i].Y);
                collider[i].size = new Vector2(path.PathPoints[i + 1].X - path.PathPoints[i].X, height - path.PathPoints[i].Y);
            }
        }

        private void DrawTerrain(PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.DrawPath(pen, path);
            e.Graphics.FillPath(new SolidBrush(Color.ForestGreen), path);
        }
    }
}