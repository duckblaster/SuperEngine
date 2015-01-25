using System;
using OpenTK;
using SuperEngine.Editors;
using System.Collections.Generic;

namespace SuperEngine.Objects {
	public class Spline : MeshObject {
		public sealed class SplineNode : GameObject {
			private float roll;
			
			public float Roll {
				get {
					return roll;
				}
				set {
					if(roll != value) {
						roll = value;
					}
				}
			}
			
			public SplineNode(Vector4 vec) : base(vec.Xyz, Quaternion.Identity) {
				Roll = vec.W;
			}
			
			public static implicit operator Vector4(SplineNode node) {
				return new Vector4(node.Position, node.Roll);
			}
            public static implicit operator Vector4d(SplineNode node)
            {
                return new Vector4d((Vector3d)node.Position, (double)node.Roll);
            }
            public static Vector4 operator-(SplineNode n1, SplineNode n2) {
                return new Vector4();
            }
		}
		
		private Spline next;
		private Spline prev;
		
		public Spline Next {
			get {
				return next;
			}
			set {
				if(next != value) {
					next = value;
					if(next != null) {
						next_start = next.Start;
					} else {
						Vector4 tmp = end + (end - start);
						next_start = new SplineNode(tmp);
					}
				}
			}
		}

		public Spline Prev {
			get { return prev; }
			set {
				if(prev != value) {
					prev = value;
					if(prev != null) {
						prev_end = prev.End;
						if(start != null && end!= null) {
							Prev.Next = this;
						}
					} else if(start != null && end != null) {
						Vector4 tmp = start + (start - end);
						prev_end = new SplineNode(tmp);
					}
				}
			}
		}

        public SplineNode Start
        {
            get { return start; }
            set
            {
                if (start != value)
                {
                    start = value;
                    /*if (start != null)
                    {
                        prev_end = prev.End;
                        if (start != null && end != null)
                        {
                            Prev.Next = this;
                        }
                    }
                    else if (start != null && end != null)
                    {
                        Vector4 tmp = start + (start - end);
                        prev_end = new SplineNode(tmp);
                    }*/
                }
            }
        }

        public SplineNode End
        {
            get { return end; }
            set
            {
                if (end != value)
                {
                    end = value;
                    /*if (start != null)
                    {
                        prev_end = prev.End;
                        if (start != null && end != null)
                        {
                            Prev.Next = this;
                        }
                    }
                    else if (start != null && end != null)
                    {
                        Vector4 tmp = start + (start - end);
                        prev_end = new SplineNode(tmp);
                    }*/
                }
            }
        }
		
		private SplineNode prev_end;
		private SplineNode start;
		private SplineNode end;
		private SplineNode next_start;
		
		private float length;
		
		public float Length {
			get {
				return length;
			}
		}
		
		public Spline(Vector3 position, Quaternion orientation, SplineNode _start, SplineNode _end, Spline _next, Spline _prev) : base(position, orientation) {
			if(_start == null) {
				throw new ArgumentNullException("_start");
			}
			if(_end == null) {
				throw new ArgumentNullException("_end");
			}
			Start = _start;
			End = _end;
			Next = _next;
			Prev = _prev;
            length = 0.0f;//CalcLength();
		}
		
		/*public float CalcLength() {
			return CalcLength((P1 - P2).Length/100);
		}*/
		
		public float CalcLength(float len) {
			return CalcLength(0, 1, len);
		}
		
		public float CalcLength(float u0, float u1, float len) {
			return CalcLength(u0, u1, len * len, 2);
		}

		public float CalcLength(float u0, float u1, float len, int maxDepth) {
			return CalcLength(u0, u1, len * len, maxDepth, 0);
		}
		
		private float CalcLength(float u0, float u1, float lenSquared, int maxDepth, int depth) {
			float umid = (u0 + u1) / 2;
			Vector4 P0 = Point(u0);
			Vector4 P1 = Point(u1);
			if(((P0 - P1).LengthSquared > lenSquared) || depth < maxDepth) {
				float len0 = CalcLength(u0, umid, lenSquared, maxDepth, depth + 1);
				float len1 = CalcLength(umid, u1, lenSquared, maxDepth, depth + 1);
				return len0 + len1;
			} else {
				return (P0 - P1).Length;
			}
		}

		public Vector4 Point(float s) {
			double h1 = 2 * Math.Pow(s, 3) - 3 * Math.Pow(s, 2) + 1;
            double h2 = -2 * Math.Pow(s, 3) + 3 * Math.Pow(s, 2);
            double h3 = Math.Pow(s, 3) - 2 * Math.Pow(s, 2) + s;
            double h4 = Math.Pow(s, 3) - Math.Pow(s, 2);
            Vector4d p = h1 * (Vector4d)start + h2 * (Vector4d)end + h3 * (0.5 * ((Vector4d)end - (Vector4d)prev_end)) + h4 * (0.5 * ((Vector4d)next_start - (Vector4d)start));
			return (Vector4)p;
		}

        public Spline(Vector3 position, Quaternion orientation) :base(position, orientation)
        {
		}
	}
}

