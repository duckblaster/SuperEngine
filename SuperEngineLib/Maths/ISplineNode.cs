using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperEngineLib.Maths {
    public interface ISplineNode<T> {
        T Sub(T a);
        T Add(T a);
        T Mult(double a);
        double Length {
            get;
        }
        double LengthFast {
            get;
        }
        double LengthSquared {
            get;
        }
    }
}
