using System;

namespace DualNumbers
{    
    public struct Vector
    {
        public static Vector Zero => default(Vector);

        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector Vec(double x, double y, double z) => new Vector(x, y, z);

        public override string ToString() => $"[{X}, {Y}, {Z}]";

        public double Norm => Math.Sqrt(X * X + Y * Y + Z * Z);

        public Vector Normalize() => this / Norm;

        #region Arithmetic Operators

        public static Vector operator +(Vector a) => a;
        public static Vector operator -(Vector a) => new Vector(-a.X, -a.Y, -a.Z);

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector operator *(double a, Vector b) => new Vector(a * b.X, a * b.Y, a * b.Z);
        public static Vector operator *(Vector a, double b) => new Vector(a.X * b, a.Y * b, a.Z * b);

        public static Vector operator /(Vector a, double b) => new Vector(a.X / b, a.Y / b, a.Z / b);

        public static double operator *(Vector a, Vector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static Vector operator ^(Vector a, Vector b) => new Vector(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        #endregion
    }
}
