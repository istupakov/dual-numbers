using System;

namespace DualNumbers
{
    public class DualNumber
    {
        public double Value { get; }
        public double Diff { get; }
        public double Diff2 { get; }

        public DualNumber(double value, double diff = 0, double diff2 = 0)
        {
            Value = value;
            Diff = diff;
            Diff2 = diff2;
        }

        public static DualNumber Variable(double value) => new DualNumber(value, 1);

        public static implicit operator DualNumber(double v) => new DualNumber(v);

        public override string ToString() => $"f(t) = {Value}, f'(t) = {Diff}, f\"(t) = {Diff2}";

        #region Arithmetic Operators

        public static DualNumber operator +(DualNumber a) => a;
        public static DualNumber operator -(DualNumber a) => new DualNumber(-a.Value, -a.Diff, -a.Diff2);

        public static DualNumber operator +(DualNumber a, DualNumber b) => new DualNumber(a.Value + b.Value, a.Diff + b.Diff, a.Diff2 + b.Diff2);
        public static DualNumber operator +(double a, DualNumber b) => new DualNumber(a + b.Value, b.Diff, b.Diff2);
        public static DualNumber operator +(DualNumber a, double b) => new DualNumber(a.Value + b, a.Diff, a.Diff2);

        public static DualNumber operator -(DualNumber a, DualNumber b) => new DualNumber(a.Value - b.Value, a.Diff - b.Diff, a.Diff2 - b.Diff2);
        public static DualNumber operator -(double a, DualNumber b) => new DualNumber(a - b.Value, -b.Diff, -b.Diff2);
        public static DualNumber operator -(DualNumber a, double b) => new DualNumber(a.Value - b, a.Diff, a.Diff2);

        public static DualNumber operator *(DualNumber a, DualNumber b)
            => new DualNumber(a.Value * b.Value, a.Diff * b.Value + b.Diff * a.Value, a.Value * b.Diff2 + 2 * a.Diff * b.Diff + a.Diff2 * b.Value);
        public static DualNumber operator *(double a, DualNumber b) => new DualNumber(a * b.Value, a * b.Diff, a * b.Diff2);
        public static DualNumber operator *(DualNumber a, double b) => new DualNumber(a.Value * b, a.Diff * b, a.Diff2 * b);

        public static DualVector operator *(DualNumber a, Vector b) => new DualVector(a * b.X, a * b.Y, a * b.Z);
        public static DualVector operator *(Vector a, DualNumber b) => new DualVector(a.X * b, a.Y * b, a.Z * b);
        public static DualVector operator /(Vector a, DualNumber b) => new DualVector(a.X / b, a.Y / b, a.Z / b);

        public static DualNumber operator /(DualNumber a, DualNumber b) => a * Inverse(b);
        public static DualNumber operator /(double a, DualNumber b) => a * Inverse(b);
        public static DualNumber operator /(DualNumber a, double b) => a * (1 / b);

        #endregion

        #region Power Functions

        public static DualNumber Inverse(DualNumber a)
        {
            return new DualNumber(1 / a.Value, -a.Diff / (a.Value * a.Value), (2 * a.Diff * a.Diff / a.Value - a.Diff2) / (a.Value * a.Value));
        }

        public static DualNumber Sqrt(DualNumber a)
        {
            var r = Math.Sqrt(a.Value);
            return new DualNumber(r, 0.5 * a.Diff / r, 0.5 * a.Diff2 / r - 0.25 * a.Diff * a.Diff / (a.Value * r));
        }

        public static DualNumber Exp(DualNumber a)
        {
            var exp = Math.Exp(a.Value);
            return new DualNumber(exp, exp * a.Diff, exp * (a.Diff2 + a.Diff * a.Diff));
        }

        public static DualNumber Log(DualNumber a)
        {
            return new DualNumber(Math.Log(a.Value), a.Diff / a.Value, (a.Diff2 - a.Diff * a.Diff / a.Value) / a.Value);
        }

        public static DualNumber Pow(DualNumber a, DualNumber b) => Exp(b * Log(a));

        public static DualNumber Pow(DualNumber a, double b)
        {
            var pow = Math.Pow(a.Value, b);
            var frac = a.Diff / a.Value;
            return new DualNumber(pow, b * pow * frac, b * pow * (a.Diff2 / a.Value + (b - 1) * frac * frac));
        }

        #endregion

        #region Trigonometric Functions

        public static DualNumber Sin(DualNumber a)
        {
            var sin = Math.Sin(a.Value);
            var cos = Math.Cos(a.Value);
            return new DualNumber(sin, cos * a.Diff, cos * a.Diff2 - sin * a.Diff * a.Diff);
        }

        public static DualNumber Cos(DualNumber a)
        {
            var sin = Math.Sin(a.Value);
            var cos = Math.Cos(a.Value);
            return new DualNumber(cos, -sin * a.Diff, -sin * a.Diff2 - cos * a.Diff * a.Diff);
        }

        public static DualNumber Tan(DualNumber a)
        {
            var tan = Math.Tan(a.Value);
            return new DualNumber(tan, (1 + tan * tan) * a.Diff, (1 + tan * tan) * (a.Diff2 + 2 * tan * a.Diff * a.Diff));
        }

        public static DualNumber Asin(DualNumber a)
        {
            var tmp = 1 / Math.Sqrt(1 - a.Value * a.Value);
            var tmpDiff = a.Diff * tmp;
            return new DualNumber(Math.Asin(a.Value), tmpDiff, (a.Diff2 + a.Value * tmpDiff * tmpDiff) * tmp);
        }

        public static DualNumber Acos(DualNumber a)
        {
            var tmp = 1 / Math.Sqrt(1 - a.Value * a.Value);
            var tmpDiff = a.Diff * tmp;
            return new DualNumber(Math.Acos(a.Value), -tmpDiff, -(a.Diff2 + a.Value * tmpDiff * tmpDiff) * tmp);
        }

        public static DualNumber Atan(DualNumber a)
        {
            var tmp = 1 / (1 + a.Value * a.Value);
            var tmpDiff = a.Diff * tmp;
            return new DualNumber(Math.Atan(a.Value), tmpDiff, a.Diff2 * tmp - 2 * a.Value * tmpDiff * tmpDiff);
        }

        #endregion

        #region Hyperbolic Functions

        public static DualNumber Sinh(DualNumber a)
        {
            var sin = Math.Sinh(a.Value);
            var cos = Math.Cosh(a.Value);
            return new DualNumber(sin, cos * a.Diff, cos * a.Diff2 + sin * a.Diff * a.Diff);
        }

        public static DualNumber Cosh(DualNumber a)
        {
            var sin = Math.Sinh(a.Value);
            var cos = Math.Cosh(a.Value);
            return new DualNumber(cos, sin * a.Diff, sin * a.Diff2 + cos * a.Diff * a.Diff);
        }

        public static DualNumber Tanh(DualNumber a)
        {
            var tan = Math.Tanh(a.Value);
            return new DualNumber(tan, (1 - tan * tan) * a.Diff, (1 - tan * tan) * (a.Diff2 - 2 * tan * a.Diff * a.Diff));
        }

        #endregion

        #region Other Function

        public static DualNumber Abs(DualNumber a) => a.Value >= 0 ? a : -a;

        #endregion
    }

    public class DualVector
    {
        public DualNumber X { get; }
        public DualNumber Y { get; }
        public DualNumber Z { get; }

        public Vector Value => new Vector(X.Value, Y.Value, Z.Value);
        public Vector Diff => new Vector(X.Diff, Y.Diff, Z.Diff);
        public Vector Diff2 => new Vector(X.Diff2, Y.Diff2, Z.Diff2);

        public DualVector(DualNumber x, DualNumber y, DualNumber z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public DualVector(Vector v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static DualVector Vec(DualNumber x, DualNumber y, DualNumber z) => new DualVector(x, y, z);

        public static implicit operator DualVector(Vector v) => new DualVector(v);

        public override string ToString() => $"f(t) = {Value}, f'(t) = {Diff}, f\"(t) = {Diff2}";

        public DualNumber Norm => DualNumber.Sqrt(X * X + Y * Y + Z * Z);

        public DualVector Normalize() => this / Norm;

        #region Arithmetic Operators

        public static DualVector operator +(DualVector a) => a;
        public static DualVector operator -(DualVector a) => new DualVector(-a.X, -a.Y, -a.Z);

        public static DualVector operator +(DualVector a, DualVector b) => new DualVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static DualVector operator +(Vector a, DualVector b) => new DualVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static DualVector operator +(DualVector a, Vector b) => new DualVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static DualVector operator -(DualVector a, DualVector b) => new DualVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static DualVector operator -(Vector a, DualVector b) => new DualVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static DualVector operator -(DualVector a, Vector b) => new DualVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static DualVector operator *(DualNumber a, DualVector b) => new DualVector(a * b.X, a * b.Y, a * b.Z);
        public static DualVector operator *(double a, DualVector b) => new DualVector(a * b.X, a * b.Y, a * b.Z);
        public static DualVector operator *(DualVector a, DualNumber b) => new DualVector(a.X * b, a.Y * b, a.Z * b);
        public static DualVector operator *(DualVector a, double b) => new DualVector(a.X * b, a.Y * b, a.Z * b);

        public static DualVector operator /(DualVector a, DualNumber b) => new DualVector(a.X / b, a.Y / b, a.Z / b);
        public static DualVector operator /(DualVector a, double b) => new DualVector(a.X / b, a.Y / b, a.Z / b);

        public static DualNumber operator *(DualVector a, DualVector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static DualNumber operator *(Vector a, DualVector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static DualNumber operator *(DualVector a, Vector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static DualVector operator ^(DualVector a, DualVector b) => new DualVector(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        public static DualVector operator ^(Vector a, DualVector b) => new DualVector(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        public static DualVector operator ^(DualVector a, Vector b) => new DualVector(a.Y * b.Z - b.Y * a.Z, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        #endregion
    }
}
