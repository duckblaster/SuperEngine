using OpenTK;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
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
