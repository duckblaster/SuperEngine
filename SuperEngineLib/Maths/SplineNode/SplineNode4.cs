using OpenTK;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
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
	
}
