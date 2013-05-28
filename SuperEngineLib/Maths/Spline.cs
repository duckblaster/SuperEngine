using System;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;

namespace SuperEngineLib.Maths {
    public class SplineList<T> : List<Spline<T>> where T : ISplineNode<T> {
        // TODO: Something?
    }
    public class Spline<T> where T : ISplineNode<T> {
        private struct SplineNodeWrapper<T> where T : ISplineNode<T> {
            private T node;
            public SplineNodeWrapper(T splineNode) {
                this.node = splineNode;
            }
            public static implicit operator SplineNodeWrapper<T>(T splineNode) {
                return new SplineNodeWrapper<T>(splineNode);
            }
            public static implicit operator T(SplineNodeWrapper<T> wrapper) {
                return wrapper.node;
            }
            public static SplineNodeWrapper<T> operator +(SplineNodeWrapper<T> a, SplineNodeWrapper<T> b) {
                return new SplineNodeWrapper<T>(a.node.Add(b.node));
            }
            public static SplineNodeWrapper<T> operator -(SplineNodeWrapper<T> a, SplineNodeWrapper<T> b) {
                return new SplineNodeWrapper<T>(a.node.Sub(b.node));
            }
            public static SplineNodeWrapper<T> operator *(SplineNodeWrapper<T> a, double b) {
                return new SplineNodeWrapper<T>(a.node.Mult(b));
            }
            public static SplineNodeWrapper<T> operator *(double b, SplineNodeWrapper<T> a) {
                return new SplineNodeWrapper<T>(a.node.Mult(b));
            }
            public static bool operator ==(SplineNodeWrapper<T> a, SplineNodeWrapper<T> b) {
                return false;//a.node.Eq(b);
            }
            public static bool operator !=(SplineNodeWrapper<T> a, SplineNodeWrapper<T> b) {
                return true;//!a.node.Eq(b);
            }
            public bool Equals(SplineNodeWrapper<T> a) {
                return false;//this.node.Eq(a);
            }
            public double Length {
                get {
                    return node.Length;
                }
            }
            public double LengthFast {
                get {
                    return node.LengthFast;
                }
            }
            public double LengthSquared {
                get {
                    return node.LengthSquared;
                }
            }
        }
        private Spline<T> next;
        private Spline<T> prev;
        private SplineNodeWrapper<T> next_end;
        private SplineNodeWrapper<T> prev_start;
        private SplineNodeWrapper<T> start;
        private SplineNodeWrapper<T> end;
        public Spline<T> Next {
            get {
                return next;
            }
            set {
                if (next != null) {
                    next.Prev = null;
                }
                next = value;
                if (next != null) {
                    if (next.Prev != this) {
                        next.Prev = this;
                    }
                    next_end = next.end;
                } else {
                    next_end = end + (end - start);
                }
            }
        }
        public Spline<T> Prev {
            get {
                return prev;
            }
            set {
                if (prev != null) {
                    prev.Next = null;
                }
                prev = value;
                if (prev != null) {
                    if (prev.Next != this) {
                        prev.Next = this;
                    }
                    prev_start = prev.start;
                } else {
                    prev_start = start - (end - start);
                }
            }
        }
        public T Start {
            get {
                return start;
            }
            set {
                if(start != value) {
                    start = value;
                    if(prev != null) {
                        prev.End = start;
                    }
                }
            }
        }
        public T End {
            get {
                return end;
            }
            set {
                if(end != value) {
                    end = value;
                    if(next != null) {
                        next.Start = end;
                    }
                }
            }
        }

        public double Length {
            get {
                return CalcLength();
            }
        }

        public T Point(double s) {
            double h1 = 2 * Math.Pow(s, 3) - 3 * Math.Pow(s, 2) + 1;
            double h2 = -2 * Math.Pow(s, 3) + 3 * Math.Pow(s, 2);
            double h3 = Math.Pow(s, 3) - 2 * Math.Pow(s, 2) + s;
            double h4 = Math.Pow(s, 3) - Math.Pow(s, 2);
            double tension = 0.75;
            SplineNodeWrapper<T> p = (h1 * start + h2 * end + h3 * (tension * (end - prev_start)) + h4 * (tension * (next_end - start)));
            return p;
        }

        public delegate void DrawPoint(Spline<T> spline, double t, T p, object data);

        public void Draw(Bitmap bmp, DrawPoint func, object data = null, double quality = 1) {
            double len = CalcLength(1);
            int iters = (int)Math.Ceiling(len * quality);
            double inv_iters = 1.0 / iters;
            for (int i = 0; i < iters; i++) {
                double t = inv_iters * i;
                func(this, t, Point(t), data);
            }
        }

        public double CalcLength() {
            return CalcLength((start - end).Length / 100);
        }

        public double CalcLength(double len) {
            return CalcLength(0, 1, len);
        }

        public double CalcLength(double u0, double u1, double len) {
            double umid = (u0 + u1) / 2;
            SplineNodeWrapper<T> P0 = Point(u0);
            SplineNodeWrapper<T> P1 = Point(u1);
            if((P0 - P1).LengthSquared > len * len) {
                double len0 = CalcLength(u0, umid, len);
                double len1 = CalcLength(umid, u1, len);
                return len0 + len1;
            } else {
                return (P0 - P1).Length;
            }
        }

        public double CalcLength(double u0, double u1, int maxDepth) {
            return CalcLength(u0, u1, maxDepth, 0);
        }
        private double CalcLength(double u0, double u1, int maxDepth, int depth) {
            double umid = (u0 + u1) / 2;
            if (depth < maxDepth) {
                double len0 = CalcLength(u0, umid, maxDepth, depth + 1);
                double len1 = CalcLength(umid, u1, maxDepth, depth + 1);
                return len0 + len1;
            } else {
                SplineNodeWrapper<T> P0 = Point(u0);
                SplineNodeWrapper<T> P1 = Point(u1);
                return (P0 - P1).Length;
            }
        }

        public Spline(T _start, T _end) {
            Start = _start;
            End = _end;
            Next = null;
            Prev = null;
        }

        public Spline(T _start, T _end, T _prev_start, T _next_end) {
            Start = _start;
            End = _end;
            prev_start = _prev_start;
            next_end = _next_end;
        }

        public Spline(T _start, T _end, Spline<T> _prev, Spline<T> _next) {
            Start = _start;
            End = _end;
            Prev = _prev;
            Next = _next;
        }
    }
}