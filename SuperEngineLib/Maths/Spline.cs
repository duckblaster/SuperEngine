using System;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;

namespace SuperEngine.Maths {
	/*public struct Spline {
		public struct Segment {
		}
		
		private List<Vector4> nodes;
		private List<Vector4> points;
		private List<Segment> segments;
		private float length;
		private bool continuous;
		
		public IEnumerable<Vector4> Nodes {
			get {
				return nodes.AsReadOnly();
			}
			set {
				nodes = new List<Vector4>(value);
				CalcSpline();
			}
		}
		
		public IEnumerable<Segment> Segments {
			get {
				return segments.AsReadOnly();
			}
		}
		
		public float Length {
			get {
				return length;
			}
		}
		
		public bool Continuous {
			get {
				return continuous;
			}
			set {
				if(continuous != value) {
					continuous = value;
					CalcSpline();
				}
			}
		}
		
		public void CalcSpline() {
			float tmpLen = 0.0f;
			int count = nodes.Count;
			if(count < 2) {
				return;
			}
			if(continuous) {
				points.Add(nodes[count - 1]);
				points.AddRange(nodes);
				points.Add(nodes[0]);
			} else {
				Vector4 diff;
				diff = nodes[0] + (nodes[1] - nodes[0]);
				points.Add(diff);
				points.AddRange(nodes);
				diff = nodes[count - 2] - nodes[count - 1];
				diff = nodes[count - 1] + diff;
				points.Add(diff);
			}
			for(int i = 1; i < count - 2; i++) {
				Segment tmp = new Segment(points[i - 1], points[i], points[i + 1], points[i + 2]);
				segments.Add(tmp);
				tmp.StartDistance = tmpLen;
				tmpLen += tmp.Length;
			}
			length = tmpLen;
		}
		
		public Vector4 Point(float distance) {
			Segment tmp = segments.Find(s => s.StartDistance <= distance && s.StartDistance + s.Length >= distance);
			float u = (distance - tmp.StartDistance) / tmp.Length;
			return tmp.Point(u);
		}
		
		public Spline() {
		}

		public Spline(IEnumerable<Vector4> n) {
			Nodes = n;
		}
	}*/
    public class Spline {
        public Spline next;
        public Spline prev;
        public Vector2d next_end;
        public Vector2d prev_start;
        public Vector2d start;
        public Vector2d end;
        public Spline Next {
            get {
                return next;
            }
            set {
                if (value == null && next != null) {
                    next = value;
                    next.Prev = null;
                } else {
                    next = value;
                }
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
        public Spline Prev {
            get {
                return prev;
            }
            set {
                if (value == null && prev != null) {
                    prev = value;
                    prev.Next = null;
                } else {
                    prev = value;
                }
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
        public Vector2d Start {
            get {
                return start;
            }
            set { start = value;
            }
        }
        public Vector2d End {
            get {
                return end;
            }
            set {
                end = value;
            }
        }

        public double Length {
            get {
                return CalcLength();
            }
        }

        public Vector2d Point(double s) {
            double h1 = 2 * Math.Pow(s, 3) - 3 * Math.Pow(s, 2) + 1;
            double h2 = -2 * Math.Pow(s, 3) + 3 * Math.Pow(s, 2);
            double h3 = Math.Pow(s, 3) - 2 * Math.Pow(s, 2) + s;
            double h4 = Math.Pow(s, 3) - Math.Pow(s, 2);
            Vector2d p = h1 * start + h2 * end + h3 * (0.75 * (end - prev_start)) + h4 * (0.75 * (next_end - start));
            return p;
        }

        public void Draw(Bitmap bmp, Color col) {
            double len = CalcLength(1);
            double width = (start - end).X;
            double height = (start - end).Y;
            int iters = (int)Math.Ceiling(Math.Max(len, Math.Max(Math.Abs(width), Math.Abs(height)))) * 100;
            double inv_iters = 1.0 / iters;
            for (int i = 0; i < iters; i++) {
                double t = inv_iters * i;
                Vector2d p = Point(t);
                int x = (int)Math.Round(p.X);
                int y = (int)Math.Round(p.Y);
                bmp.SetPixel(x, y, col);
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
            Vector2d P0 = Point(u0);
            Vector2d P1 = Point(u1);
            if (((P0 - P1).LengthSquared > len * len)) {
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
                Vector2d P0 = Point(u0);
                Vector2d P1 = Point(u1);
                return (P0 - P1).Length;
            }
        }

        public Spline(Vector2d _start, Vector2d _end) {
            Start = _start;
            End = _end;
            Next = null;
            Prev = null;
        }

        public Spline(Vector2d _start, Vector2d _end, Vector2d _prev_start, Vector2d _next_end) {
            Start = _start;
            End = _end;
            prev_start = _prev_start;
            next_end = _next_end;
        }

        public Spline(Vector2d _start, Vector2d _end, Spline _prev, Spline _next) {
            Start = _start;
            End = _end;
            Prev = _prev;
            Next = _next;
        }
    }
}

