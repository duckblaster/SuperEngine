using OpenTK;
using SuperEngine.Misc;

namespace SuperEngineLib.Maths {
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
	
}
