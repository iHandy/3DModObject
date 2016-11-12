using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Soloviev3DModKurs.Geometry
{
    class GeometryUtils
    {
        public static List<Point3D> approximationCircle(int n, double y, double radius)
        {
            double alpha = 2.0 * Math.PI / (double)n;
            // 2 = 360/180
            List<Point3D> points = new List<Point3D>(n);
            double currentAlpha = -alpha;
            for (int i = 0; i < n; i++)
            {
                currentAlpha = currentAlpha + alpha;

                double x = radius * Math.Cos(currentAlpha);
                double z = radius * Math.Sin(currentAlpha);

                points.Add(new Point3D(x, y, z));
            }

            return points;
        }

        public static int[] matrixMultiplication(int[] a, int[,] b)
        {
            int[] r = new int[a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(0); j++)
                    r[i] += b[j, i] * a[j];
            return r;
        }

        public static double[] matrixMultiplication(double[] a, double[,] b)
        {
            double[] r = new double[a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(0); j++)
                    r[i] += b[j, i] * a[j];
            return r;
        }

        private class LightSource
        {
            public double X = 0;
            public double Y = 0;
            public double Z = 0;

            public double A = 0;
            public double B = 0;
            public double C = 0;
        }

        public static double CalculatingEquation(Face face, Point3D viewPoint)
        {
            double LX = viewPoint.X, LY = viewPoint.Y, LZ = viewPoint.Z;

            LightSource light = new LightSource();
            List<Point3D> workPoints = new List<Point3D>(3);
            foreach (var edge in face.getEdges())
            {
                var point = edge.getPoints()[0];
                light.X += point.X;
                light.Y += point.Y;
                light.Z += point.Z;

                if (workPoints.Count < 3)
                {
                    workPoints.Add(point);
                }
            }

            double X1 = workPoints[2].X;
            double Y1 = workPoints[2].Y;
            double Z1 = workPoints[2].Z;

            double X2 = workPoints[1].X;
            double Y2 = workPoints[1].Y;
            double Z2 = workPoints[1].Z;

            double X3 = workPoints[0].X;
            double Y3 = workPoints[0].Y;
            double Z3 = workPoints[0].Z;

            double A = (Y1 * Z2) + (Y2 * Z3) + (Y3 * Z1) - (Y2 * Z1) - (Y3 * Z2) - (Y1 * Z3);
            double B = (Z1 * X2) + (Z2 * X3) + (Z3 * X1) - (Z2 * X1) - (Z3 * X2) - (Z1 * X3);
            double C = (X1 * Y2) + (X2 * Y3) + (X3 * Y1) - (X2 * Y1) - (X3 * Y2) - (X1 * Y3);
            double D = (-1) * (A * X1 + B * Y1 + C * Z1);

            int edgeCount = face.getEdges().Count;
            light.A = LX - (light.X / edgeCount);
            light.B = LY - (light.Y / edgeCount);
            light.C = LZ - (light.Z / edgeCount);

            return ((A * light.A) + (B * light.B) + (C * light.C)) /
                (Math.Pow(Math.Pow(A, 2) + Math.Pow(B, 2) + Math.Pow(C, 2), 0.5) *
                 Math.Pow(Math.Pow(light.A, 2) + Math.Pow(light.B, 2) + Math.Pow(light.C, 2), 0.5));

        }
    }
}
