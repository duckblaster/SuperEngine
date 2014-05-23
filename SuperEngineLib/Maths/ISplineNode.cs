using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperEngineLib.Maths {
    public interface ISplineNode<T> {
        /*SplineList<T> Splines {
            get;
            set;
        } // TODO: Something?*/
        T Sub(T a);
        T Add(T a);
        T Mult(float a);
        //bool Eq(T a);
        float Length {
            get;
        }
        float LengthFast {
            get;
        }
        float LengthSquared {
            get;
        }
    }
}
