using System;

namespace DualNumbers
{
    public class DualNumberGrad
    {
        public double Value { get; }
        public Vector Grad { get; }

        public DualNumberGrad(double value, Vector grad = default)
        {
            Value = value;
            Grad = grad;
        }

        public static implicit operator DualNumberGrad(double v) => new DualNumberGrad(v);

        public override string ToString() => $"f(p) = {Value}, grad f(p) = {Grad}";

        #region Arithmetic Operators

        public static DualNumberGrad operator +(DualNumberGrad a) => a;
        public static DualNumberGrad operator -(DualNumberGrad a) => new DualNumberGrad(-a.Value, -a.Grad);

        public static DualNumberGrad operator +(DualNumberGrad a, DualNumberGrad b) => new DualNumberGrad(a.Value + b.Value, a.Grad + b.Grad);
        public static DualNumberGrad operator +(double a, DualNumberGrad b) => new DualNumberGrad(a + b.Value, b.Grad);
        public static DualNumberGrad operator +(DualNumberGrad a, double b) => new DualNumberGrad(a.Value + b, a.Grad);

        public static DualNumberGrad operator -(DualNumberGrad a, DualNumberGrad b) => new DualNumberGrad(a.Value - b.Value, a.Grad - b.Grad);
        public static DualNumberGrad operator -(double a, DualNumberGrad b) => new DualNumberGrad(a - b.Value, -b.Grad);
        public static DualNumberGrad operator -(DualNumberGrad a, double b) => new DualNumberGrad(a.Value - b, a.Grad);

        public static DualNumberGrad operator *(DualNumberGrad a, DualNumberGrad b)
            => new DualNumberGrad(a.Value * b.Value, a.Grad * b.Value + b.Grad * a.Value);
        public static DualNumberGrad operator *(double a, DualNumberGrad b) => new DualNumberGrad(a * b.Value, a * b.Grad);
        public static DualNumberGrad operator *(DualNumberGrad a, double b) => new DualNumberGrad(a.Value * b, a.Grad * b);

        public static DualVectorGrad operator *(DualNumberGrad a, Vector b) => new DualVectorGrad(a * b.X, a * b.Y, a * b.Z);
        public static DualVectorGrad operator *(Vector a, DualNumberGrad b) => new DualVectorGrad(a.X * b, a.Y * b, a.Z * b);
        public static DualVectorGrad operator /(Vector a, DualNumberGrad b) => new DualVectorGrad(a.X / b, a.Y / b, a.Z / b);

        public static DualNumberGrad operator /(DualNumberGrad a, DualNumberGrad b) => a * Inverse(b);
        public static DualNumberGrad operator /(double a, DualNumberGrad b) => a * Inverse(b);
        public static DualNumberGrad operator /(DualNumberGrad a, double b) => a * (1 / b);

        #endregion

        #region Power Functions

        public static DualNumberGrad Inverse(DualNumberGrad a)
        {
            return new DualNumberGrad(1 / a.Value, -a.Grad / (a.Value * a.Value));
        }

        public static DualNumberGrad Sqrt(DualNumberGrad a)
        {
            var r = Math.Sqrt(a.Value);
            return new DualNumberGrad(r, 0.5 * a.Grad / r);
        }

        public static DualNumberGrad Exp(DualNumberGrad a)
        {
            var exp = Math.Exp(a.Value);
            return new DualNumberGrad(exp, exp * a.Grad);
        }

        public static DualNumberGrad Log(DualNumberGrad a)
        {
            return new DualNumberGrad(Math.Log(a.Value), a.Grad / a.Value);
        }

        public static DualNumberGrad Pow(DualNumberGrad a, DualNumberGrad b) => Exp(b * Log(a));

        public static DualNumberGrad Pow(DualNumberGrad a, double b)
        {
            var pow = Math.Pow(a.Value, b);
            return new DualNumberGrad(pow, b * pow / a.Value * a.Grad);
        }

        #endregion

        #region Trigonometric Functions

        public static DualNumberGrad Sin(DualNumberGrad a)
        {
            var sin = Math.Sin(a.Value);
            var cos = Math.Cos(a.Value);
            return new DualNumberGrad(sin, cos * a.Grad);
        }

        public static DualNumberGrad Cos(DualNumberGrad a)
        {
            var sin = Math.Sin(a.Value);
            var cos = Math.Cos(a.Value);
            return new DualNumberGrad(cos, -sin * a.Grad);
        }

        public static DualNumberGrad Tan(DualNumberGrad a)
        {
            var tan = Math.Tan(a.Value);
            return new DualNumberGrad(tan, (1 + tan * tan) * a.Grad);
        }

        public static DualNumberGrad Asin(DualNumberGrad a)
        {
            return new DualNumberGrad(Math.Asin(a.Value), a.Grad / Math.Sqrt(1 - a.Value * a.Value));
        }

        public static DualNumberGrad Acos(DualNumberGrad a)
        {
            return new DualNumberGrad(Math.Acos(a.Value), -a.Grad / Math.Sqrt(1 - a.Value * a.Value));
        }

        public static DualNumberGrad Atan(DualNumberGrad a)
        {
            return new DualNumberGrad(Math.Atan(a.Value), a.Grad / (1 + a.Value * a.Value));
        }

        #endregion

        #region Hyperbolic Functions

        public static DualNumberGrad Sinh(DualNumberGrad a)
        {
            var sin = Math.Sinh(a.Value);
            var cos = Math.Cosh(a.Value);
            return new DualNumberGrad(sin, cos * a.Grad);
        }

        public static DualNumberGrad Cosh(DualNumberGrad a)
        {
            var sin = Math.Sinh(a.Value);
            var cos = Math.Cosh(a.Value);
            return new DualNumberGrad(cos, sin * a.Grad);
        }

        public static DualNumberGrad Tanh(DualNumberGrad a)
        {
            var tan = Math.Tanh(a.Value);
            return new DualNumberGrad(tan, (1 - tan * tan) * a.Grad);
        }

        #endregion

        #region Other Function

        public static DualNumberGrad Abs(DualNumberGrad a) => a.Value >= 0 ? a : -a;

        #endregion
    }

    public class DualVectorGrad
    {
        public DualNumberGrad X { get; }
        public DualNumberGrad Y { get; }
        public DualNumberGrad Z { get; }

        public Vector Value => new Vector(X.Value, Y.Value, Z.Value);
        public double Div => X.Grad.X + Y.Grad.Y + Z.Grad.Z;
        public Vector Curl => new Vector(Z.Grad.Y - Y.Grad.Z, X.Grad.Z - Z.Grad.X, Y.Grad.X - X.Grad.Y);

        public DualVectorGrad(DualNumberGrad x, DualNumberGrad y, DualNumberGrad z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public DualVectorGrad(Vector v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static DualVectorGrad Variable(Vector value)
        {
            return new DualVectorGrad(new DualNumberGrad(value.X, new Vector(1, 0, 0)),
                new DualNumberGrad(value.Y, new Vector(0, 1, 0)), new DualNumberGrad(value.Z, new Vector(0, 0, 1)));
        }

        public static DualVectorGrad Vec(DualNumberGrad x, DualNumberGrad y, DualNumberGrad z) => new DualVectorGrad(x, y, z);

        public static implicit operator DualVectorGrad(Vector v) => new DualVectorGrad(v);

        public override string ToString() => $"f(p) = {Value}, div f(p) = {Div}, curl f(p) = {Curl}";

        public DualNumberGrad Norm => DualNumberGrad.Sqrt(X * X + Y * Y + Z * Z);

        public DualVectorGrad Normalize() => this / Norm;

        #region Arithmetic Operators

        public static DualVectorGrad operator +(DualVectorGrad a) => a;
        public static DualVectorGrad operator -(DualVectorGrad a) => new DualVectorGrad(-a.X, -a.Y, -a.Z);

        public static DualVectorGrad operator +(DualVectorGrad a, DualVectorGrad b) => new DualVectorGrad(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static DualVectorGrad operator +(Vector a, DualVectorGrad b) => new DualVectorGrad(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static DualVectorGrad operator +(DualVectorGrad a, Vector b) => new DualVectorGrad(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static DualVectorGrad operator -(DualVectorGrad a, DualVectorGrad b) => new DualVectorGrad(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static DualVectorGrad operator -(Vector a, DualVectorGrad b) => new DualVectorGrad(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static DualVectorGrad operator -(DualVectorGrad a, Vector b) => new DualVectorGrad(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static DualVectorGrad operator *(DualNumberGrad a, DualVectorGrad b) => new DualVectorGrad(a * b.X, a * b.Y, a * b.Z);
        public static DualVectorGrad operator *(double a, DualVectorGrad b) => new DualVectorGrad(a * b.X, a * b.Y, a * b.Z);
        public static DualVectorGrad operator *(DualVectorGrad a, DualNumberGrad b) => new DualVectorGrad(a.X * b, a.Y * b, a.Z * b);
        public static DualVectorGrad operator *(DualVectorGrad a, double b) => new DualVectorGrad(a.X * b, a.Y * b, a.Z * b);

        public static DualVectorGrad operator /(DualVectorGrad a, DualNumberGrad b) => new DualVectorGrad(a.X / b, a.Y / b, a.Z / b);
        public static DualVectorGrad operator /(DualVectorGrad a, double b) => new DualVectorGrad(a.X / b, a.Y / b, a.Z / b);

        public static DualNumberGrad operator *(DualVectorGrad a, DualVectorGrad b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static DualNumberGrad operator *(Vector a, DualVectorGrad b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static DualNumberGrad operator *(DualVectorGrad a, Vector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static DualVectorGrad operator ^(DualVectorGrad a, DualVectorGrad b) => new DualVectorGrad(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        public static DualVectorGrad operator ^(Vector a, DualVectorGrad b) => new DualVectorGrad(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        public static DualVectorGrad operator ^(DualVectorGrad a, Vector b) => new DualVectorGrad(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        #endregion
    }
}
