using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Vec2
    {
        private double x;
        private double y;

        public Vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2() : this(0, 0)
        { }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(x * x + y * y);
            }
        }

        public Vec2 Clone()
        {
            return new Vec2
            (
                this.x,
                this.y
            );
        }

        public double X
        {
            get => this.x;
            set => this.x = value;
        }
        public double Y
        {
            get => this.y;
            set => this.y = value;
        }
        
        public static Vec2 operator+(Vec2 first, Vec2 second)
        {
            return new Vec2
            (
                first.X + second.X,
                first.Y + second.Y
            );
        }

        public static Vec2 operator-(Vec2 first, Vec2 second)
        {
            return new Vec2
            (
                first.X - second.X,
                first.Y - second.Y
            );
        }

        public static Vec2 operator-(Vec2 vec)
        {
            return new Vec2
            (
                -vec.X,
                -vec.Y
            );
        }

        public static Vec2 operator*(Vec2 vec, double a)
        {
            return new Vec2
            (
                vec.X * a,
                vec.Y * a
            );
        }

        public static Vec2 operator*(double a, Vec2 vec)
        {
            return new Vec2
            (
                vec.X * a,
                vec.Y * a
            );
        }

        public static Vec2 operator*(Vec2 a, Vec2 b)
        {
            return new Vec2
            (
                a.X * b.X,
                a.Y * b.Y
            );
        }

        public static Vec2 operator/(Vec2 vec, double a)
        {
            return new Vec2
            (
                vec.X / a,
                vec.Y / a
            );
        }

        public static bool yIsInt(Vec2 vec)
        {
            return vec.Y == Convert.ToInt32(vec.Y);
        }
    }
}
