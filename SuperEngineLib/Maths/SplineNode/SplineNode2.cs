using OpenTK;

namespace SuperEngineLib.Maths {
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
}
