using System;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;

namespace SuperEngineLib.Maths {
    public class SplineList<T> : List<Spline<T>> where T : ISplineNode<T> {
        // TODO: Something?
    }
    public class SplineBase {
        public static float tension = 0.75F;
    }
    public class Spline<T> : SplineBase where T : ISplineNode<T> {
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
            public static SplineNodeWrapper<T> operator *(SplineNodeWrapper<T> a, float b) {
                return new SplineNodeWrapper<T>(a.node.Mult(b));
            }
            public static SplineNodeWrapper<T> operator *(float b, SplineNodeWrapper<T> a) {
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
            public float Length {
                get {
                    return node.Length;
                }
            }
            public float LengthFast {
                get {
                    return node.LengthFast;
                }
            }
            public float LengthSquared {
                get {
                    return node.LengthSquared;
                }
            }
        }
        private Spline<T> next;
        private Spline<T> prev;
        private T next_end;
        private T prev_start;
        private T start;
        private T end;

        private SplineNodeWrapper<T> next_end_w {
            get {
                return next_end;
            }
            set {
                next_end = value;
            }
        }
        private SplineNodeWrapper<T> prev_start_w {
            get {
                return prev_start;
            }
            set {
                prev_start = value;
            }
        }
        private SplineNodeWrapper<T> end_w {
            get {
                return end;
            }
            set {
                end = value;
            }
        }
        private SplineNodeWrapper<T> start_w {
            get {
                return start;
            }
            set {
                start = value;
            }
        }

        public Spline<T> Next {
            get {
                return next;
            }
            set {
                if(next_end != null) {
                    next_end.Splines.Remove(this);
                }
                if (next != null) {
                    Spline<T> tmp = next;
                    next = null;
                    tmp.Prev = null;
                }
                next = value;
                if (next != null) {
                    if (next.Prev != this) {
                        next.Prev = this;
                    }
                    next_end = next.end;
                } else {
                    next_end = end + (end_w - start);
                }
                if(next_end != null) {
                    next_end.Splines.Add(this);
                }
            }
        }
        public Spline<T> Prev {
            get {
                return prev;
            }
            set {
                if(prev_start != null) {
                    prev_start.Splines.Remove(this);
                }
                if(prev != null) {
                    Spline<T> tmp = prev;
                    prev = null;
                    tmp.Next = null;
                }
                prev = value;
                if (prev != null) {
                    if (prev.Next != this) {
                        prev.Next = this;
                    }
                    prev_start = prev.start;
                } else {
                    prev_start = start - (end_w - start);
                }
                if(prev_start != null) {
                    prev_start.Splines.Add(this);
                }
            }
        }
        public T Start {
            get {
                return start;
            }
            set {
                if(start_w != value) {
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
                if(end_w != value) {
                    end = value;
                    if(next != null) {
                        next.Start = end;
                    }
                }
            }
        }

        public float Length {
            get {
                return CalcLength();
            }
        }

        public T Point(float s) {
            float h1 = 2 * s * s * s - 3 * s * s + 1;
            float h2 = -2 * s * s * s + 3 * s * s;
            float h3 = s * s * s - 2 * s * s + s;
            float h4 = s * s * s - s * s;
            SplineNodeWrapper<T> p = (h1 * start_w + h2 * end_w + h3 * (tension * (end_w - prev_start_w)) + h4 * (tension * (next_end_w - start_w)));
            return p;
            /*float h00 = 2 * Math.Pow(s, 3) - 3 * Math.Pow(s, 2) + 1;
            float h10 = Math.Pow(s, 3) - 2 * Math.Pow(s, 2) + s;
            float h01 = -2 * Math.Pow(s, 3) + 3 * Math.Pow(s, 2);
            float h11 = Math.Pow(s, 3) - Math.Pow(s, 2);
            float c = 0.75;
            SplineNodeWrapper<T> point = (h1 * start + h2 * end + h3 * (tension * (end - prev_start)) + h4 * (tension * (next_end - start)));
            return point;*/
        }

        public delegate void DrawPoint(Spline<T> spline, float t, T p, object data);

        public void Draw(Bitmap bmp, DrawPoint func, object data = null, float quality = 1) {
            float len = CalcLength(1);
            int iters = (int)Math.Ceiling(len * quality);
            float inv_iters = 1.0F / iters;
            for (int i = 0; i < iters; i++) {
                float t = inv_iters * i;
                func(this, t, Point(t), data);
            }
        }

        public float CalcLength() {
            return CalcLength((start_w - end_w).LengthFast / 100);
        }

        public float CalcLength(float len) {
            return CalcLength(0, 1, len);
        }

        public float CalcLength(float u0, float u1, float len) {
            float umid = (u0 + u1) / 2;
            SplineNodeWrapper<T> P0 = Point(u0);
            SplineNodeWrapper<T> P1 = Point(u1);
            if((P0 - P1).LengthSquared > len * len) {
                float len0 = CalcLength(u0, umid, len);
                float len1 = CalcLength(umid, u1, len);
                return len0 + len1;
            } else {
                return (P0 - P1).Length;
            }
        }

        public float CalcLength(float u0, float u1, int maxDepth) {
            return CalcLength(u0, u1, maxDepth, 0);
        }
        private float CalcLength(float u0, float u1, int maxDepth, int depth) {
            float umid = (u0 + u1) / 2;
            if (depth < maxDepth) {
                float len0 = CalcLength(u0, umid, maxDepth, depth + 1);
                float len1 = CalcLength(umid, u1, maxDepth, depth + 1);
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