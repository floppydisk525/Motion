using System;
using System.Collections.Generic;
using System.Drawing;

namespace Motion {
    public struct Vector : IEquatable<Vector> {
        public static readonly Vector Zero = new Vector(0, 0);
        public static readonly Vector XAxis = new Vector(1, 0);
        public static readonly Vector YAxis = new Vector(0, 1);

        public Double X;
        public Double Y;

        public Vector(Double x, Double y) {
            X = x;
            Y = y;
        }

        public static Vector Multiply(Vector a, Double b) {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(Vector a, Double b) {
            return Multiply(a, b);
        }

        public static Vector operator *(Double a, Vector b) {
            return Multiply(b, a);
        }

        public static Vector Divide(Vector a, Double b) {
            return (1 / b) * a;
        }

        public static Vector operator /(Vector a, Double b) {
            return Divide(a, b);
        }

        public static Vector Add(Vector a, Vector b) {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator +(Vector a, Vector b) {
            return Add(a, b);
        }

        public static Vector Subtract(Vector a, Vector b) {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator -(Vector a, Vector b) {
            return Subtract(a, b);
        }

        public static Vector Negate(Vector a) {
            return new Vector(-a.X, -a.Y);
        }

        public static Vector operator -(Vector a) {
            return Negate(a); ;
        }

        public Boolean Equals(Vector a) {
            return X == a.X && Y == a.Y;
        }

        public static Boolean operator ==(Vector a, Vector b) {
            return Vector.Equals(a, b);
        }

        public static Boolean operator !=(Vector a, Vector b) {
            return !Vector.Equals(a, b);
        }

        public static Double Dot(Vector a, Vector b) {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Double CrossScalar(Vector a, Vector b) {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Double Angle(Vector a, Vector b) {
            return Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        public static Double Distance(Vector a, Vector b) {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public static Vector Projection(Vector a, Vector b) {
            return (Dot(a, b) / Dot(b, b)) * b;
        }

        public static Vector Rejection(Vector a, Vector b) {
            return a - Projection(a, b);
        }

        public Vector Rotate(Double angle) {
            Double m = Magnitude();
            Double a = Math.Atan2(Y, X) + angle;
            X = m * Math.Cos(a);
            Y = m * Math.Sin(a);
            return this;
        }

        public Vector To(Vector a) {
            return a - this;
        }

        public Vector Unit() {
            return this / Magnitude();
        }

        public Double Magnitude() {
            return Math.Sqrt(X * X + Y * Y);
        }

        public static Vector Sum(ICollection<Vector> vectors) {
            Vector r = new Vector();
            foreach (Vector p in vectors)
                r += p;
            return r;
        }

        public static Vector Average(ICollection<Vector> vectors) {
            return Sum(vectors) / vectors.Count;
        }

        public override String ToString() {
            return "[" + X + " " + Y + "]";
        }

        public Point ToPoint() {
            return new Point((Int32)X, (Int32)Y);
        }

        public override Boolean Equals(Object a) {
            return a is Vector && Equals((Vector)a);
        }

        public override Int32 GetHashCode() {
            Int32 code = 17;
            code += X.GetHashCode();
            code *= 31;
            code += Y.GetHashCode();
            return code;
        }
    }
}