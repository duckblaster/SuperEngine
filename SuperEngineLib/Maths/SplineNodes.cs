using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SuperEngineLib.Maths {
    public class SplineNode2 : ISplineNode<SplineNode2> {
        private Vector2 vec;
        public SplineNode2(Vector2 vector) {
            vec = vector;
        }
        public static implicit operator Vector2(SplineNode2 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode2(Vector2 vector) {
            return new SplineNode2(vector);
        }
        public SplineNode2 Sub(SplineNode2 a) {
            return Vector2.Subtract(vec, a.vec);
        }
        public SplineNode2 Add(SplineNode2 a) {
            return Vector2.Add(vec, a.vec);
        }
        public SplineNode2 Mult(SplineNode2 a) {
            return Vector2.Multiply(vec, a.vec);
        }
        public SplineNode2 Mult(double a) {
            return Vector2.Multiply(vec, (float)a);
        }
        public bool Eq(SplineNode2 a) {
            return vec == a.vec;
        }
        public double Length {
            get { return vec.Length; }
        }
        public double LengthFast {
            get {
                return vec.LengthFast;
            }
        }
        public double LengthSquared {
            get { return vec.LengthSquared; }
        }
    }
    public class SplineNode2d : ISplineNode<SplineNode2d> {
        private Vector2d vec;
        public SplineNode2d(Vector2d vector) {
            vec = vector;
        }
        public static implicit operator Vector2d(SplineNode2d spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode2d(Vector2d vector) {
            return new SplineNode2d(vector);
        }
        public SplineNode2d Sub(SplineNode2d a) {
            return Vector2d.Subtract(vec, a.vec);
        }
        public SplineNode2d Add(SplineNode2d a) {
            return Vector2d.Add(vec, a.vec);
        }
        public SplineNode2d Mult(SplineNode2d a) {
            return Vector2d.Multiply(vec, a.vec);
        }
        public SplineNode2d Mult(double a) {
            return Vector2d.Multiply(vec, a);
        }
		public bool Eq(SplineNode2d a) {
			return vec == a.vec;
		}
        public double Length {
            get { return vec.Length; }
        }
        public double LengthFast {
            get {
                return 1.0 / MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
            }
        }
        public double LengthSquared {
            get { return vec.LengthSquared; }
        }
    }
    public class SplineNode3 : ISplineNode<SplineNode3> {
        private Vector3 vec;
        public SplineNode3(Vector3 vector) {
            vec = vector;
        }
        public static implicit operator Vector3(SplineNode3 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode3(Vector3 vector) {
            return new SplineNode3(vector);
        }
        public SplineNode3 Sub(SplineNode3 a) {
            return Vector3.Subtract(vec, a.vec);
        }
        public SplineNode3 Add(SplineNode3 a) {
            return Vector3.Add(vec, a.vec);
        }
        public SplineNode3 Mult(SplineNode3 a) {
            return Vector3.Multiply(vec, a.vec);
        }
        public SplineNode3 Mult(double a) {
            return Vector3.Multiply(vec, (float)a);
		}
		public bool Eq(SplineNode3 a) {
			return vec == a.vec;
		}
        public double Length {
            get {
                return vec.Length;
            }
        }
        public double LengthFast {
            get {
                return vec.LengthFast;
            }
        }
        public double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
    public class SplineNode3d : ISplineNode<SplineNode3d> {
        private Vector3d vec;
        public SplineNode3d(Vector3d vector) {
            vec = vector;
        }
        public static implicit operator Vector3d(SplineNode3d spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode3d(Vector3d vector) {
            return new SplineNode3d(vector);
        }
        public SplineNode3d Sub(SplineNode3d a) {
            return Vector3d.Subtract(vec, a.vec);
        }
        public SplineNode3d Add(SplineNode3d a) {
            return Vector3d.Add(vec, a.vec);
        }
        public SplineNode3d Mult(SplineNode3d a) {
            return Vector3d.Multiply(vec, a.vec);
        }
        public SplineNode3d Mult(double a) {
            return Vector3d.Multiply(vec, a);
		}
		public bool Eq(SplineNode3d a) {
			return vec == a.vec;
		}
        public double Length {
            get {
                return vec.Length;
            }
        }
        public double LengthFast {
            get {
                return vec.LengthFast;
            }
        }
        public double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
    public class SplineNode4 : ISplineNode<SplineNode4> {
        private Vector4 vec;
        public SplineNode4(Vector4 vector) {
            vec = vector;
        }
        public static implicit operator Vector4(SplineNode4 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode4(Vector4 vector) {
            return new SplineNode4(vector);
        }
        public SplineNode4 Sub(SplineNode4 a) {
            return Vector4.Subtract(vec, a.vec);
        }
        public SplineNode4 Add(SplineNode4 a) {
            return Vector4.Add(vec, a.vec);
        }
        public SplineNode4 Mult(SplineNode4 a) {
            return Vector4.Multiply(vec, a.vec);
        }
        public SplineNode4 Mult(double a) {
            return Vector4.Multiply(vec, (float)a);
		}
		public bool Eq(SplineNode4 a) {
			return vec == a.vec;
		}
        public double Length {
            get {
                return vec.Length;
            }
        }
        public double LengthFast {
            get {
                return vec.LengthFast;
            }
        }
        public double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
    public class SplineNode4d : ISplineNode<SplineNode4d> {
        private Vector4d vec;
        public SplineNode4d(Vector4d vector) {
            vec = vector;
        }
        public static implicit operator Vector4d(SplineNode4d spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode4d(Vector4d vector) {
            return new SplineNode4d(vector);
        }
        public SplineNode4d Sub(SplineNode4d a) {
            return Vector4d.Subtract(vec, a.vec);
        }
        public SplineNode4d Add(SplineNode4d a) {
            return Vector4d.Add(vec, a.vec);
        }
        public SplineNode4d Mult(SplineNode4d a) {
            return Vector4d.Multiply(vec, a.vec);
        }
        public SplineNode4d Mult(double a) {
            return Vector4d.Multiply(vec, a);
		}
		public bool Eq(SplineNode4d a) {
			return vec == a.vec;
		}
        public double Length {
            get {
                return vec.Length;
            }
        }
        public double LengthFast {
            get {
                return vec.LengthFast;
            }
        }
        public double LengthSquared {
            get {
                return vec.LengthSquared;
            }
        }
    }
}
