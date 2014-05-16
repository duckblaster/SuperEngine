using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperEngineLib.Maths {
    public class SplineNode1 : ISplineNode<SplineNode1> {
        private float vec;
        public SplineNode1(float vector) {
            vec = vector;
        }
        public static implicit operator float(SplineNode1 spline) {
            return spline.vec;
        }
        public static implicit operator SplineNode1(float vector) {
            return new SplineNode1(vector);
        }
        public SplineNode1 Sub(SplineNode1 a) {
            return (vec - a.vec);
        }
        public SplineNode1 Add(SplineNode1 a) {
            return (vec + a.vec);
        }
        public SplineNode1 Mult(SplineNode1 a) {
            return (vec * a.vec);
        }
        public SplineNode1 Mult(float a) {
            return vec * a;
        }
        public bool Eq(SplineNode1 a) {
            return vec == a.vec;
        }
        public double Length {
            get {
                return vec;
            }
        }
        public double LengthFast {
            get {
                return vec;
            }
        }
        public double LengthSquared {
            get {
                return vec * vec;
            }
        }
    }
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
        public SplineNode2 Mult(float a) {
            return Vector2.Multiply(vec, a);
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
        public SplineNode3 Mult(float a) {
            return Vector3.Multiply(vec, a);
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
        public SplineNode4 Mult(float a) {
            return Vector4.Multiply(vec, a);
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
