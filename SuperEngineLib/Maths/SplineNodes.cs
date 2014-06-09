using OpenTK;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
	public abstract class SplineNodeBase<TSplineNode> : NotifyPropertyChanged,  ISplineNode<TSplineNode> {
		double tension = -0.5;
		double bias;
		double continuity = -0.05;

		/// <summary>
		/// The tension at this node.
		/// Tight = +1.0
		/// Round = -1.0
		/// When Tension=Bias=Continuity=0 this is Catmul-Rom.
		/// When Tension=1 & Bias=Continuity=0 this is Simple Cubic.
		/// When Tension=Bias=0 & Continuity=-1 this is linear interp.
		/// </summary>
		public double Tension {
			get {
				return tension;
			}
			set {
				SetProperty(ref tension, value);
			}
		}
		
		/// <summary>
		/// The bias at this node.
		/// Post Shoot = +1.0
		/// Pre Shoot = -1.0
		/// When Tension=Bias=Continuity=0 this is Catmul-Rom.
		/// When Tension=1 & Bias=Continuity=0 this is Simple Cubic.
		/// When Tension=Bias=0 & Continuity=-1 this is linear interp.
		/// </summary>
		public double Bias {
			get {
				return bias;
			}
			set {
				SetProperty(ref bias, value);
			}
		}
		
		/// <summary>
		/// The continuity at this node.
		/// Inverted Corners = +1.0
		/// Box Corners = -1.0
		/// When Tension=Bias=Continuity=0 this is Catmul-Rom.
		/// When Tension=1 & Bias=Continuity=0 this is Simple Cubic.
		/// When Tension=Bias=0 & Continuity=-1 this is linear interp.
		/// </summary>
		public double Continuity {
			get {
				return continuity;
			}
			set {
				SetProperty(ref continuity, value);
			}
		}

		static SplineNodeBase() {
			PropertyDependsOn<SplineNodeBase<TSplineNode>>("TBC", "Tension");
			PropertyDependsOn<SplineNodeBase<TSplineNode>>("TBC", "Bias");
			PropertyDependsOn<SplineNodeBase<TSplineNode>>("TBC", "Continuity");
		}
		
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
	}
	public class SplineNode2 : SplineNodeBase<SplineNode2> {
        Vector2 vec;
        public SplineNode2(Vector2 vector) {
            vec = vector;
		}
        public static implicit operator Vector2(SplineNode2 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode2(Vector2 vector) {
            return new SplineNode2(vector);
        }
		public override SplineNode2 Subtract(SplineNode2 a) {
            return Vector2.Subtract(vec, a.vec);
        }
		public override SplineNode2 Add(SplineNode2 a) {
			return Vector2.Add(vec, a.vec);
        }
		public SplineNode2 Multiply(SplineNode2 a) {
			return Vector2.Multiply(vec, a.vec);
        }
		public override SplineNode2 Multiply(double a) {
			return Vector2.Multiply(vec, (float)a);
        }
		public override bool Eq(SplineNode2 a) {
			return vec == a.vec;
        }
		public override double Length {
            get { return vec.Length; }
        }
		public override double LengthFast {
            get {
				return vec.LengthFast;
            }
        }
		public override double LengthSquared {
            get { return vec.LengthSquared; }
        }
    }
	public class SplineNode2d : SplineNodeBase<SplineNode2d> {
        Vector2d vec;
        public SplineNode2d(Vector2d vector) {
            vec = vector;
        }
        public static implicit operator Vector2d(SplineNode2d spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode2d(Vector2d vector) {
            return new SplineNode2d(vector);
		}
		public override SplineNode2d Subtract(SplineNode2d a) {
			return Vector2d.Subtract(vec, a.vec);
		}
		public override SplineNode2d Add(SplineNode2d a) {
			return Vector2d.Add(vec, a.vec);
		}
		public SplineNode2d Multiply(SplineNode2d a) {
			return Vector2d.Multiply(vec, a.vec);
		}
		public override SplineNode2d Multiply(double a) {
			return Vector2d.Multiply(vec, a);
		}
		public override bool Eq(SplineNode2d a) {
			return vec == a.vec;
		}
		public override double Length {
            get { return vec.Length; }
        }
		public override double LengthFast {
            get {
				return 1.0 / MathHelper.InverseSqrtFast(vec.LengthSquared);
            }
        }
		public override double LengthSquared {
            get { return vec.LengthSquared; }
        }
    }
	public class SplineNode3 : SplineNodeBase<SplineNode3> {
        Vector3 vec;
        public SplineNode3(Vector3 vector) {
            vec = vector;
        }
        public static implicit operator Vector3(SplineNode3 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode3(Vector3 vector) {
            return new SplineNode3(vector);
        }
		public override SplineNode3 Subtract(SplineNode3 a) {
			return Vector3.Subtract(vec, a.vec);
        }
		public override SplineNode3 Add(SplineNode3 a) {
			return Vector3.Add(vec, a.vec);
        }
		public SplineNode3 Multiply(SplineNode3 a) {
			return Vector3.Multiply(vec, a.vec);
        }
		public override SplineNode3 Multiply(double a) {
			return Vector3.Multiply(vec, (float)a);
		}
		public override bool Eq(SplineNode3 a) {
			return vec == a.vec;
		}
		public override double Length {
            get {
                return vec.Length;
            }
        }
		public override double LengthFast {
            get {
				return vec.LengthFast;
            }
        }
		public override double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
	public class SplineNode3d : SplineNodeBase<SplineNode3d> {
        Vector3d vec;
        public SplineNode3d(Vector3d vector) {
            vec = vector;
        }
        public static implicit operator Vector3d(SplineNode3d spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode3d(Vector3d vector) {
            return new SplineNode3d(vector);
        }
		public override SplineNode3d Subtract(SplineNode3d a) {
            return Vector3d.Subtract(vec, a.vec);
        }
		public override SplineNode3d Add(SplineNode3d a) {
            return Vector3d.Add(vec, a.vec);
        }
		public SplineNode3d Multiply(SplineNode3d a) {
            return Vector3d.Multiply(vec, a.vec);
        }
		public override SplineNode3d Multiply(double a) {
            return Vector3d.Multiply(vec, a);
		}
		public override bool Eq(SplineNode3d a) {
			return vec == a.vec;
		}
		public override double Length {
            get {
                return vec.Length;
            }
        }
		public override double LengthFast {
            get {
				return vec.LengthFast;
            }
        }
		public override double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
	public class SplineNode4 : SplineNodeBase<SplineNode4> {
        Vector4 vec;
        public SplineNode4(Vector4 vector) {
            vec = vector;
        }
        public static implicit operator Vector4(SplineNode4 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode4(Vector4 vector) {
            return new SplineNode4(vector);
        }
		public override SplineNode4 Subtract(SplineNode4 a) {
            return Vector4.Subtract(vec, a.vec);
        }
		public override SplineNode4 Add(SplineNode4 a) {
            return Vector4.Add(vec, a.vec);
        }
		public SplineNode4 Multiply(SplineNode4 a) {
            return Vector4.Multiply(vec, a.vec);
        }
		public override SplineNode4 Multiply(double a) {
            return Vector4.Multiply(vec, (float)a);
		}
		public override bool Eq(SplineNode4 a) {
			return vec == a.vec;
		}
		public override double Length {
            get {
                return vec.Length;
            }
        }
		public override double LengthFast {
            get {
				return vec.LengthFast;
            }
        }
		public override double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
	public class SplineNode4d : SplineNodeBase<SplineNode4d> {
        Vector4d vec;
        public SplineNode4d(Vector4d vector) {
            vec = vector;
        }
        public static implicit operator Vector4d(SplineNode4d spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode4d(Vector4d vector) {
            return new SplineNode4d(vector);
        }
		public override SplineNode4d Subtract(SplineNode4d a) {
            return Vector4d.Subtract(vec, a.vec);
        }
		public override SplineNode4d Add(SplineNode4d a) {
            return Vector4d.Add(vec, a.vec);
        }
		public SplineNode4d Multiply(SplineNode4d a) {
            return Vector4d.Multiply(vec, a.vec);
        }
		public override SplineNode4d Multiply(double a) {
            return Vector4d.Multiply(vec, a);
		}
		public override bool Eq(SplineNode4d a) {
			return vec == a.vec;
		}
		public override double Length {
            get {
                return vec.Length;
            }
        }
		public override double LengthFast {
            get {
				return vec.LengthFast;
            }
        }
		public override double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
}
