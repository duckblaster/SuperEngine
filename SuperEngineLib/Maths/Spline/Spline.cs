using System;
using System.Collections.Generic;

namespace SuperEngineLib.Maths.Spline {
	public class SplineNode {
		ICollection<SplineSegment> splines;
		public ICollection<SplineSegment> Splines {
			get {
				return splines;
			}
			set {
				splines = value;
			}
		}
	}
	public class SplineNode<TWrapped> : SplineNode {
	}
	public class SplineSegment {
		SplineNode start;
		public SplineNode Start {
			get {
				return start;
			}
			set {
				start = value;
			}
		}
		SplineNode end;
		public SplineNode End {
			get {
				return end;
			}
			set {
				end = value;
			}
		}
		ICollection<SplineSegment> next;
		public ICollection<SplineSegment> Next {
			get {
				return next;
			}
			set {
				next = value;
			}
		}
		ICollection<SplineSegment> previous;
		public ICollection<SplineSegment> Previous {
			get {
				return previous;
			}
			set {
				previous = value;
			}
		}
	}
	public class SplineSegment<TSplineNode> : SplineSegment where TSplineNode : SplineNode {
		TSplineNode start;
		public new TSplineNode Start {
			get {
				return start;
			}
			set {
				start = value;
			}
		}
		TSplineNode end;
		public new TSplineNode End {
			get {
				return end;
			}
			set {
				end = value;
			}
		}
		ICollection<SplineSegment<TSplineNode>> next;
		public new ICollection<SplineSegment<TSplineNode>> Next {
			get {
				return next;
			}
			set {
				next = value;
			}
		}
		ICollection<SplineSegment<TSplineNode>> previous;
		public new ICollection<SplineSegment<TSplineNode>> Previous {
			get {
				return previous;
			}
			set {
				previous = value;
			}
		}
	}
}

