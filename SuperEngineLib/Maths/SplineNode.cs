using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperEngineLib.Maths {
    struct SplineNode {
        double[] _values;
        double _length;
        double _lengthSquared;
        public double this[int i] {
            set {
                _values[i] = value;
                calcLength();
            }
            get {
                return _values[i];
            }
        }

        public double[] Values {
            set {
                _values = value;
                calcLength();
            }
            get {
                return _values;
            }
        }

        public double Length {
            get {
                return _length;
            }
        }

        /*public SplineNode() {
            _values = new double[1];
            _length = 0;
            _lengthSquared = 0;
        }*/

        public SplineNode(int size) {
            _values = new double[size];
            _length = 0;
            _lengthSquared = 0;
        }

        public SplineNode(IEnumerable<double> values) {
            _values = values.ToArray();
            _length = 0;
            _lengthSquared = 0;
            calcLength();
        }

        private void calcLength() {
            double lengthSquared = 0;
            foreach(double value in _values) {
                lengthSquared += value * value;
            }
            _lengthSquared = lengthSquared;
            _length = Math.Sqrt(_lengthSquared);
        }

        public static implicit operator SplineNode(Vector4d vec) {
            return new SplineNode();
        }
    }
}
