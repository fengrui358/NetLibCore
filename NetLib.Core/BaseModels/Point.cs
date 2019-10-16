using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace FrHello.NetLib.Core.BaseModels
{
    /// <summary>
    /// 双精度浮点数Point
    /// </summary>
    public struct Point
    {
        /// <summary>Represents a <see cref="T:Point" /> that has <see cref="P:Point.X" /> and <see cref="P:Point.Y" /> values set to zero.</summary>
        // ReSharper disable once UnassignedReadonlyField
        public static readonly Point Empty;

        // ReSharper disable once InconsistentNaming
        private double x;
        // ReSharper disable once InconsistentNaming
        private double y;

        /// <summary>Initializes a new instance of the <see cref="T:Point" /> class with the specified coordinates.</summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>Initializes a new instance of the <see cref="T:Point" /> class from a <see cref="T:Size" />.</summary>
        /// <param name="sz">A <see cref="T:Size" /> that specifies the coordinates for the new <see cref="T:Point" />.</param>
        public Point(Size sz)
        {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>Gets a value indicating whether this <see cref="T:Point" /> is empty.</summary>
        /// <returns>
        /// <see langword="true" /> if both <see cref="P:Point.X" /> and <see cref="P:Point.Y" /> are 0; otherwise, <see langword="false" />.</returns>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (Math.Abs(x) < double.Epsilon)
                    return Math.Abs(y) < double.Epsilon;
                return false;
            }
        }

        /// <summary>Gets or sets the x-coordinate of this <see cref="T:Point" />.</summary>
        /// <returns>The x-coordinate of this <see cref="T:Point" />.</returns>
        public double X
        {
            get => x;
            set => x = value;
        }

        /// <summary>Gets or sets the y-coordinate of this <see cref="T:Point" />.</summary>
        /// <returns>The y-coordinate of this <see cref="T:Point" />.</returns>
        public double Y
        {
            get => y;
            set => y = value;
        }

        /// <summary>Converts the specified <see cref="T:Point" /> structure to a <see cref="T:PointF" /> structure.</summary>
        /// <param name="p">The <see cref="T:Point" /> to be converted.</param>
        /// <returns>The <see cref="T:PointF" /> that results from the conversion.</returns>
        public static implicit operator PointF(Point p)
        {
            return new PointF((float) p.X, (float) p.Y);
        }

        /// <summary>Converts the specified <see cref="T:Point" /> structure to a <see cref="T:PointF" /> structure.</summary>
        /// <param name="p">The <see cref="T:Point" /> to be converted.</param>
        /// <returns>The <see cref="T:PointF" /> that results from the conversion.</returns>
        public static implicit operator System.Drawing.Point(Point p)
        {
            return new System.Drawing.Point((int) p.X, (int) p.Y);
        }

        /// <summary>Converts the specified <see cref="T:Point" /> structure to a <see cref="T:Size" /> structure.</summary>
        /// <param name="p">The <see cref="T:Point" /> to be converted.</param>
        /// <returns>The <see cref="T:Size" /> that results from the conversion.</returns>
        public static explicit operator Size(Point p)
        {
            return new Size((int) p.X, (int) p.Y);
        }

        /// <summary>Translates a <see cref="T:Point" /> by a given <see cref="T:Size" />.</summary>
        /// <param name="pt">The <see cref="T:Point" /> to translate.</param>
        /// <param name="sz">A <see cref="T:Size" /> that specifies the pair of numbers to add to the coordinates of <paramref name="pt" />.</param>
        /// <returns>The translated <see cref="T:Point" />.</returns>
        public static Point operator +(Point pt, Size sz)
        {
            return Add(pt, sz);
        }

        /// <summary>Translates a <see cref="T:Point" /> by the negative of a given <see cref="T:Size" />.</summary>
        /// <param name="pt">The <see cref="T:Point" /> to translate.</param>
        /// <param name="sz">A <see cref="T:Size" /> that specifies the pair of numbers to subtract from the coordinates of <paramref name="pt" />.</param>
        /// <returns>A <see cref="T:Point" /> structure that is translated by the negative of a given <see cref="T:Size" /> structure.</returns>
        public static Point operator -(Point pt, Size sz)
        {
            return Subtract(pt, sz);
        }

        /// <summary>Compares two <see cref="T:Point" /> objects. The result specifies whether the values of the <see cref="P:Point.X" /> and <see cref="P:Point.Y" /> properties of the two <see cref="T:Point" /> objects are equal.</summary>
        /// <param name="left">A <see cref="T:Point" /> to compare.</param>
        /// <param name="right">A <see cref="T:Point" /> to compare.</param>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:Point.X" /> and <see cref="P:Point.Y" /> values of <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Point left, Point right)
        {
            if (Math.Abs(left.X - right.X) < double.Epsilon)
                return Math.Abs(left.Y - right.Y) < double.Epsilon;
            return false;
        }

        /// <summary>Compares two <see cref="T:Point" /> objects. The result specifies whether the values of the <see cref="P:Point.X" /> or <see cref="P:Point.Y" /> properties of the two <see cref="T:Point" /> objects are unequal.</summary>
        /// <param name="left">A <see cref="T:Point" /> to compare.</param>
        /// <param name="right">A <see cref="T:Point" /> to compare.</param>
        /// <returns>
        /// <see langword="true" /> if the values of either the <see cref="P:Point.X" /> properties or the <see cref="P:Point.Y" /> properties of <paramref name="left" /> and <paramref name="right" /> differ; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        /// <summary>Adds the specified <see cref="T:Size" /> to the specified <see cref="T:Point" />.</summary>
        /// <param name="pt">The <see cref="T:Point" /> to add.</param>
        /// <param name="sz">The <see cref="T:Size" /> to add</param>
        /// <returns>The <see cref="T:Point" /> that is the result of the addition operation.</returns>
        public static Point Add(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>Returns the result of subtracting specified <see cref="T:Size" /> from the specified <see cref="T:Point" />.</summary>
        /// <param name="pt">The <see cref="T:Point" /> to be subtracted from.</param>
        /// <param name="sz">The <see cref="T:Size" /> to subtract from the <see cref="T:Point" />.</param>
        /// <returns>The <see cref="T:Point" /> that is the result of the subtraction operation.</returns>
        public static Point Subtract(Point pt, Size sz)
        {
            return new Point(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>Specifies whether this <see cref="T:Point" /> contains the same coordinates as the specified <see cref="T:System.Object" />.</summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to test.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:Point" /> and has the same coordinates as this <see cref="T:Point" />.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            var point = (Point) obj;
            if (Math.Abs(point.X - X) < double.Epsilon)
                return Math.Abs(point.Y - Y) < double.Epsilon;
            return false;
        }

        /// <summary>Returns a hash code for this <see cref="T:Point" />.</summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="T:Point" />.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>Translates this <see cref="T:Point" /> by the specified amount.</summary>
        /// <param name="dx">The amount to offset the x-coordinate.</param>
        /// <param name="dy">The amount to offset the y-coordinate.</param>
        public void Offset(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }

        /// <summary>Translates this <see cref="T:Point" /> by the specified <see cref="T:Point" />.</summary>
        /// <param name="p">The <see cref="T:Point" /> used offset this <see cref="T:Point" />.</param>
        public void Offset(Point p)
        {
            Offset(p.X, p.Y);
        }

        /// <summary>Converts this <see cref="T:Point" /> to a human-readable string.</summary>
        /// <returns>A string that represents this <see cref="T:Point" />.</returns>
        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
                   "}";
        }
    }
}