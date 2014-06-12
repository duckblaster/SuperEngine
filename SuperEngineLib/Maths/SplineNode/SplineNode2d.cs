using OpenTK;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
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
	
}
