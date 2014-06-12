using OpenTK;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
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
	
}
