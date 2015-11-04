using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    public class Vertex
    {
        public double X;
        public double Y;

        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vertex operator +(Vertex v1, Vertex v2)
        {
            return new Vertex(DoubleUtil.Add(v1.X, v2.X), DoubleUtil.Add(v1.Y, v2.Y));
        }
        public static Vertex operator -(Vertex v1, Vertex v2)
        {
            return new Vertex(DoubleUtil.Add(v1.X, -v2.X), DoubleUtil.Add(v1.Y, -v2.Y));
        }
        public static Vertex operator *(Vertex v, double d)
        {
            return new Vertex(v.X * d, v.Y * d);
        }
        public double Dot(Vertex other)
        {
            return DoubleUtil.Add(this.X * other.X, this.Y * other.Y);
        }

        public double Cross(Vertex other)
        {
            return DoubleUtil.Add(this.X * other.Y, -this.Y * other.X);
        }

        //point q exists on line p1-p2?
        public static bool OnSeg(Vertex p1, Vertex p2, Vertex q)
        {
            return (p1 - q).Cross(p2 - q) == 0 && (p1 - q).Dot(p2 - q) <= 0;
        }

        //crosssing point of line p1-p2 and q1-q2
        public static Vertex Intersect(Vertex p1, Vertex p2, Vertex q1, Vertex q2)
        {
            return p1 + (p2 - p1) * ((q2 - q1).Cross(q1 - p1) / (q2 - q1).Cross(p2 - p1));
        }
        public static bool HasIntersect(Vertex p1, Vertex p2, Vertex q1, Vertex q2)
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

        public static bool operator ==(Vertex x, Vertex y)
        {
            return (DoubleUtil.Eq(x.X, y.X) && DoubleUtil.Eq(x.Y, y.Y));
        }

        public static bool operator !=(Vertex x, Vertex y)
        {
            return (!DoubleUtil.Eq(x.X, y.X) || !DoubleUtil.Eq(x.Y, y.Y));
        }
        public double Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public double Dist(Vertex other)
        {
            return (this - other).Norm();
        }
        public static double Dist(Vertex v1, Vertex v2)
        {
            return v1.Dist(v2);
        }
    }

    public class Edge
    {
        public Vertex p1, p2, vect;    //vector p1 -> p2
        public double norm;

        public Edge(Vertex p1n, Vertex p2n)
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
                if ((this.p1 == other.p1 && DoubleUtil.Eq(Vertex.Dist(this.p2, other.p2), this.norm + other.norm)) ||
                    (this.p1 == other.p2 && DoubleUtil.Eq(Vertex.Dist(this.p2, other.p1), this.norm + other.norm)) ||
                    (this.p2 == other.p1 && DoubleUtil.Eq(Vertex.Dist(this.p1, other.p2), this.norm + other.norm)) ||
                    (this.p2 == other.p2 && DoubleUtil.Eq(Vertex.Dist(this.p1, other.p1), this.norm + other.norm)))
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
        public double Dist(Vertex p)
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
        public Vertex Intersect(Edge other)
        {
            return Vertex.Intersect(this.p1, this.p2, other.p1, other.p2);
        }
    }

}