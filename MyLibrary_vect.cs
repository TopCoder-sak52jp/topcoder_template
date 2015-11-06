using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    /// <summary>
    /// Pont/Vector class for 2D 
    /// </summary>
    public class Pt
    {
        public double X;
        public double Y;

        public Pt(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Pt operator +(Pt v1, Pt v2)
        {
            return new Pt(DoubleUtil.Add(v1.X, v2.X), DoubleUtil.Add(v1.Y, v2.Y));
        }
        public static Pt operator -(Pt v1, Pt v2)
        {
            return new Pt(DoubleUtil.Add(v1.X, -v2.X), DoubleUtil.Add(v1.Y, -v2.Y));
        }
        public static Pt operator *(Pt v, double d)
        {
            return new Pt(v.X * d, v.Y * d);
        }

        /// <summary>
        /// Dot product of two vectors: O -> this and O -> other
        ///  a dot b = |a| |b| cos(theta) = ax bx + ax by
        ///  zero if two vectors run orthogonally
        /// </summary>
        public double Dot(Pt other)
        {
            return DoubleUtil.Add(this.X * other.X, this.Y * other.Y);
        }

        /// <summary>
        /// Cross(det) product of two vectors: O -> this and O -> other
        ///  a x b = |a| |b| sin(theta) = ax by - ay bx
        ///  zero if two vectors run parallelly
        /// </summary>
        public double Cross(Pt other)
        {
            return DoubleUtil.Add(this.X * other.Y, -this.Y * other.X);
        }

        /// <summary>
        /// point q exists on line p1-p2?
        /// </summary>
        public static bool OnSeg(Pt p1, Pt p2, Pt q)
        {
            return (p1 - q).Cross(p2 - q) == 0 && (p1 - q).Dot(p2 - q) <= 0;
        }

        /// <summary>
        /// crosssing point of line p1-p2 and q1-q2
        /// </summary>
        public static Pt Intersect(Pt p1, Pt p2, Pt q1, Pt q2)
        {
            return p1 + (p2 - p1) * ((q2 - q1).Cross(q1 - p1) / (q2 - q1).Cross(p2 - p1));
        }
        public static bool HasIntersect(Pt p1, Pt p2, Pt q1, Pt q2)
        {
            if ((p1 - q1).Cross(p2 - q2) == 0)
            {
                return OnSeg(p1, q1, p2) || OnSeg(p1, q1, q2) || OnSeg(p2, q2, p1) || OnSeg(p2, q2, q1);
            }
            else
            {
                var r = Intersect(p1, q1, p2, q2);
                return OnSeg(p1, q1, r) && OnSeg(p2, q2, r);
            }
        }

        public static bool operator ==(Pt x, Pt y)
        {
            return (DoubleUtil.Eq(x.X, y.X) && DoubleUtil.Eq(x.Y, y.Y));
        }

        public static bool operator !=(Pt x, Pt y)
        {
            return (!DoubleUtil.Eq(x.X, y.X) || !DoubleUtil.Eq(x.Y, y.Y));
        }
        public double Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public double Dist(Pt other)
        {
            return (this - other).Norm();
        }
        public static double Dist(Pt v1, Pt v2)
        {
            return v1.Dist(v2);
        }
    }

    public class Edge
    {
        public Pt p1, p2, vect;    //vector p1 -> p2
        public double norm;

        public Edge(Pt p1n, Pt p2n)
        {
            p1 = p1n;
            p2 = p2n;
            vect = p2 - p1;
            norm = vect.Norm();
        }        
        public bool HasIntersect(Edge other)
        {
            ////do edges "this" and "other" intersect?
            if (Math.Min(p1.X, p2.X) > Math.Max(other.p1.X, other.p2.X)) return false;
            if (Math.Max(p1.X, p2.X) < Math.Min(other.p1.X, other.p2.X)) return false;
            if (Math.Min(p1.Y, p2.Y) > Math.Max(other.p1.Y, other.p2.Y)) return false;
            if (Math.Max(p1.Y, p2.Y) < Math.Min(other.p1.Y, other.p2.Y)) return false;

            int den = (int)(other.vect.Y * vect.X - other.vect.X * vect.Y);
            int num1 = (int)(other.vect.X * (p1.Y - other.p1.Y) - other.vect.Y * (p1.X - other.p1.X));
            int num2 = (int)(vect.X * (p1.Y - other.p1.Y) - vect.Y * (p1.X - other.p1.X));

            //parallel edges
            if (den == 0)
            {
                if (Math.Min(other.dist2(this), dist2(other)) > 0)
                    return false;
                //on the same line - "not intersect" only if one of the vertices is common,
                //and the other doesn't belong to the line
                if ((this.p1 == other.p1 && DoubleUtil.Eq(Pt.Dist(this.p2, other.p2), this.norm + other.norm)) ||
                    (this.p1 == other.p2 && DoubleUtil.Eq(Pt.Dist(this.p2, other.p1), this.norm + other.norm)) ||
                    (this.p2 == other.p1 && DoubleUtil.Eq(Pt.Dist(this.p1, other.p2), this.norm + other.norm)) ||
                    (this.p2 == other.p2 && DoubleUtil.Eq(Pt.Dist(this.p1, other.p1), this.norm + other.norm)))
                    return false;
                return true;
            }

            //common vertices
            if (this.p1 == other.p1 || this.p1 == other.p2 || this.p2 == other.p1 || this.p2 == other.p2)
                return false;

            double u1 = (double)num1 / den;
            double u2 = (double)num2 / den;
            if (u1 < 0 || u1 > 1 || u2 < 0 || u2 > 1)
                return false;
            return true;
        }
        public double Dist(Pt p)
        {
            //distance from p to the edge
            if (vect.Dot(p - p1) <= 0)
                return p.Dist(p1);         //from p to p1
            if (vect.Dot(p - p2) >= 0)
                return p.Dist(p2);         //from p to p2
            //distance to the line itself
            return Math.Abs(-vect.Y * p.X + vect.X * p.Y + p1.X * p2.Y - p1.Y * p2.X) / norm;
        }
        private double dist2(Edge other)
        {
            //distance from the closest of the endpoints of "other" to "this"
            return Math.Min(Dist(other.p1), Dist(other.p2));
        }
        public Pt Intersect(Edge other)
        {
            return Pt.Intersect(this.p1, this.p2, other.p1, other.p2);
        }
    }

}