using System;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
	/*public class SplineList<TSplineNode> : List<Spline<TSplineNode>> where TSplineNode : ISplineNode {
        // TODO: Something?
    }*/
	public class Spline<TSplineNode> where TSplineNode : SplineNodeBase<TSplineNode> {

		/*private struct SplineNodeWrapper : IEquatable<TSplineNode> {
			public TSplineNode node;

			public SplineNodeWrapper(TSplineNode splineNode) {
				this.node = splineNode;
			}

			public static implicit operator SplineNodeWrapper(TSplineNode splineNode) {
				return new SplineNodeWrapper(splineNode);
			}

			public static implicit operator TSplineNode(SplineNodeWrapper wrapper) {
				return wrapper.node;
			}

			public static SplineNodeWrapper operator +(SplineNodeWrapper a, SplineNodeWrapper b) {
				return new SplineNodeWrapper(a.node.Add(b.node));
			}

			public static SplineNodeWrapper operator -(SplineNodeWrapper a, SplineNodeWrapper b) {
				return new SplineNodeWrapper(a.node.Subtract(b.node));
			}

			public static SplineNodeWrapper operator *(SplineNodeWrapper a, double b) {
				return new SplineNodeWrapper(a.node.Multiply(b));
			}

			public static SplineNodeWrapper operator *(double b, SplineNodeWrapper a) {
				return new SplineNodeWrapper(a.node.Multiply(b));
			}

			public static bool operator ==(SplineNodeWrapper a, SplineNodeWrapper b) {
				if((object)a == null)
					return false;
				return a.Equals(b);
			}

			public static bool operator !=(SplineNodeWrapper a, SplineNodeWrapper b) {
				return !(a==b);
			}

			public bool Equals(SplineNodeWrapper a) {
				if((object)a == null)
					return false;
				if((object)node == null)
					return false;
				if(ReferenceEquals(this, a))
					return true;
				return node.Eq(a.node);
			}

			public bool Equals(TSplineNode a) {
				return node.Eq(a);
			}
			public override bool Equals(object obj) {
				if(obj == null)
					return false;
				if(ReferenceEquals(this, obj))
					return true;
				if(obj.GetType() != typeof(SplineNodeWrapper))
					return false;
				SplineNodeWrapper other = (SplineNodeWrapper)obj;
				return node.Eq(other.node);
			}
			

			public override int GetHashCode() {
				unchecked {
					return (node != null ? node.GetHashCode() : 0);
				}
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
		}*/

		/*public abstract class SplineNodeBase : ISplineNode<TSplineNode> {
			public double Tension = 0.0;
			public double Continuity = 0.0;
			public double Bias = 0.0;

			#region ISplineNode implementation

			public abstract TSplineNode Subtract(TSplineNode a);

			public abstract TSplineNode Add(TSplineNode a);

			public abstract TSplineNode Multiply(double a);

			public abstract bool Eq(TSplineNode a);

			public abstract double Length {
				get;
			}

			public abstract double LengthFast {
				get;
			}

			public abstract double LengthSquared {
				get;
			}
			#endregion
		}*/

		public class SplineSegment : NotifyPropertyChanged {
			SplineSegment next;
			SplineSegment prev;
			TSplineNode next_end;
			TSplineNode prev_start;
			TSplineNode start;
			TSplineNode end;

			#region Point calculation cache
			bool recalcMatrix = true;
			bool recalcCoefficients = true;

			double PointCalc_aPrevStart;

			double PointCalc_aStart;
			double PointCalc_bStart;
			double PointCalc_cStart;

			double PointCalc_bEnd;
			double PointCalc_cEnd;
			double PointCalc_dEnd;

			double PointCalc_dNextEnd;
			
			const double PointCalc_divisor = 1.0/2.0;

			double PointCalc_t1;
			double PointCalc_t2;
			double PointCalc_t3;
			double PointCalc_t4;

			double PointCalc_u1;
			double PointCalc_u2;
			double PointCalc_u3;
			double PointCalc_u4;

			double PointCalc_v1;
			double PointCalc_v2;
			double PointCalc_v3;
			const double PointCalc_v4=0;

			const double PointCalc_w1=0;
			const double PointCalc_w2=1;
			const double PointCalc_w3=0;
			const double PointCalc_w4=0;

			TSplineNode PointCalc_a;
			TSplineNode PointCalc_b;
			TSplineNode PointCalc_c;
			TSplineNode PointCalc_d;
			#endregion

			public SplineSegment Next {
				get {
					return next;
				}
				set {
					if(next != null) {
						next.Prev = null;
					}
					next = value;
					if(next != null) {
						if(next.Prev != this) {
							next.Prev = this;
						}
						next_end = next.end;
					} else {
						next_end = end.Add(end.Subtract(start));
						//next_end = end + (end - start);
					}
				}
			}

			public SplineSegment Prev {
				get {
					return prev;
				}
				set {
					if(prev != null) {
						prev.Next = null;
					}
					prev = value;
					if(prev != null) {
						if(prev.Next != this) {
							prev.Next = this;
						}
						prev_start = prev.start;
					} else {
						prev_start = start.Subtract(end.Subtract(start));
						//prev_start = start - (end - start);
					}
				}
			}

			public TSplineNode Start {
				get {
					return start;
				}
				set {
					if(start == null || !start.Eq(value)) {
						start = value;
						if(prev != null) {
							prev.End = start;
						}
					}
				}
			}

			public TSplineNode End {
				get {
					return end;
				}
				set {
					if(end == null || !end.Eq(value)) {
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

			public TSplineNode Point(double t) {
				if(recalcMatrix) {
					#region a-d
					PointCalc_aPrevStart = (1 - prev_start.Tension) * (1 + prev_start.Continuity) * (1 + prev_start.Bias);

					PointCalc_aStart = (1 - start.Tension) * (1 + start.Continuity) * (1 + start.Bias);
					PointCalc_bStart = (1 - start.Tension) * (1 - start.Continuity) * (1 - start.Bias);
					PointCalc_cStart = (1 - start.Tension) * (1 - start.Continuity) * (1 + start.Bias);

					PointCalc_bEnd = (1 - end.Tension) * (1 - end.Continuity) * (1 - end.Bias);
					PointCalc_cEnd = (1 - end.Tension) * (1 - end.Continuity) * (1 + end.Bias);
					PointCalc_dEnd = (1 - end.Tension) * (1 + end.Continuity) * (1 - end.Bias);

					PointCalc_dNextEnd = (1 - next_end.Tension) * (1 + next_end.Continuity) * (1 - next_end.Bias);
					#endregion

					#region matrix
					PointCalc_t1 = (-PointCalc_aPrevStart) * PointCalc_divisor;
					PointCalc_t2 = (4 + PointCalc_aStart - PointCalc_bStart - PointCalc_cStart) * PointCalc_divisor;
					PointCalc_t3 = (-4 + PointCalc_bEnd + PointCalc_cEnd - PointCalc_dEnd) * PointCalc_divisor;
					PointCalc_t4 = (PointCalc_dNextEnd) * PointCalc_divisor;

					PointCalc_u1 = (2 * PointCalc_aPrevStart) * PointCalc_divisor;
					PointCalc_u2 = (-6 - 2 * PointCalc_aStart + 2 * PointCalc_bStart + PointCalc_cStart) * PointCalc_divisor;
					PointCalc_u3 = (6 - 2 * PointCalc_bEnd - PointCalc_cEnd + PointCalc_dEnd) * PointCalc_divisor;
					PointCalc_u4 = (-PointCalc_dNextEnd) * PointCalc_divisor;

					PointCalc_v1 = (-PointCalc_aPrevStart) * PointCalc_divisor;
					PointCalc_v2 = (PointCalc_aStart - PointCalc_bStart) * PointCalc_divisor;
					PointCalc_v3 = (PointCalc_bEnd) * PointCalc_divisor;
					#endregion

					recalcCoefficients = true;
				}

				if(recalcCoefficients) {
					PointCalc_a = prev_start.Multiply(PointCalc_t1).Add(start.Multiply(PointCalc_t2)).Add(end.Multiply(PointCalc_t3)).Add(next_end.Multiply(PointCalc_t4));
					PointCalc_b = prev_start.Multiply(PointCalc_u1).Add(start.Multiply(PointCalc_u2)).Add(end.Multiply(PointCalc_u3)).Add(next_end.Multiply(PointCalc_u4));
					PointCalc_c = prev_start.Multiply(PointCalc_v1).Add(start.Multiply(PointCalc_v2)).Add(end.Multiply(PointCalc_v3)).Add(next_end.Multiply(PointCalc_v4));
					PointCalc_d = prev_start.Multiply(PointCalc_w1).Add(start.Multiply(PointCalc_w2)).Add(end.Multiply(PointCalc_w3)).Add(next_end.Multiply(PointCalc_w4));
				}

				TSplineNode p = PointCalc_a.Multiply(t).Add(PointCalc_b).Multiply(t).Add(PointCalc_c).Multiply(t).Add(PointCalc_d);
				return p;
			}

			/*public TSplineNode PointCubicHermite(double s) {
				double h1 = 2 * Math.Pow(s, 3) - 3 * Math.Pow(s, 2) + 1;
				double h2 = -2 * Math.Pow(s, 3) + 3 * Math.Pow(s, 2);
				double h3 = Math.Pow(s, 3) - 2 * Math.Pow(s, 2) + s;
				double h4 = Math.Pow(s, 3) - Math.Pow(s, 2);
				double tension = 0.95;
				TSplineNode p = (h1 * start + h2 * end + h3 * (tension * (end - prev_start)) + h4 * (tension * (next_end - start)));
				return p;
			}*/

			public delegate void DrawPoint(SplineSegment spline, double t, TSplineNode p, object data);
			
			public void DrawPoints(DrawPoint func, object data = null, double accuracy = 1, double u0 = 0.0, double u1 = 1.0) {
				double umid = (u0 + u1) / 2;
				TSplineNode P0 = Point(u0);
				TSplineNode P1 = Point(u1);
				if(P0.Subtract(P1).LengthSquared > accuracy * accuracy) {
					DrawPoints(func, data, accuracy, u0, umid);
					DrawPoints(func, data, accuracy, umid, u1);
				} else {
					func(this, u0, P0, data);
					func(this, u1, P1, data);
				}
			}

			public void DrawPointsOld(DrawPoint func, object data = null, double quality = 10) {
				double len = CalcLength(1);
				int iters = (int)Math.Ceiling(len * quality);
				double inv_iters = 1.0 / iters;
				for(int i = 0; i < iters; i++) {
					double t = inv_iters * i;
					func(this, t, Point(t), data);
				}
			}

			public double CalcLength() {
				return CalcLength(2);
			}

			public double CalcLength(double accuracy) {
				return CalcLength(0, 1, accuracy);
			}

			public double CalcLength(int depth) {
				return CalcLength(0, 1, depth);
			}

			public double CalcLength(double u0, double u1, double accuracy) {
				double umid = (u0 + u1) / 2;
				TSplineNode P0 = Point(u0);
				TSplineNode P1 = Point(u1);
				if(P0.Subtract(P1).LengthSquared > accuracy * accuracy) {
					double len0 = CalcLength(u0, umid, accuracy);
					double len1 = CalcLength(umid, u1, accuracy);
					return len0 + len1;
				}
				return P0.Subtract(P1).Length;
			}

			public double CalcLength(double u0, double u1, int maxDepth) {
				return CalcLength(u0, u1, maxDepth, 0);
			}

			double CalcLength(double u0, double u1, int maxDepth, int depth) {
				double umid = (u0 + u1) / 2;
				if(depth < maxDepth) {
					double len0 = CalcLength(u0, umid, maxDepth, depth + 1);
					double len1 = CalcLength(umid, u1, maxDepth, depth + 1);
					return len0 + len1;
				}
				TSplineNode P0 = Point(u0);
				TSplineNode P1 = Point(u1);
				return P0.Subtract(P1).Length;
			}

			public SplineSegment(TSplineNode start, TSplineNode end) {
				Start = start;
				End = end;
				Next = null;
				Prev = null;
			}

			public SplineSegment(TSplineNode start, TSplineNode end, TSplineNode prevStart, TSplineNode nextEnd) {
				Start = start;
				End = end;
				prev_start = prevStart;
				next_end = nextEnd;
			}

			public SplineSegment(TSplineNode start, TSplineNode end, SplineSegment prev, SplineSegment next) {
				Start = start;
				End = end;
				Prev = prev;
				Next = next;
			}
		}
	}
}