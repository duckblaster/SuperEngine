using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SuperEngineLib.Maths {
	public interface ISplineNode {

	}
    public interface ISplineNode<TSplineNode> : ISplineNode {
		/*SplineList<TSplineNode> Splines {
            get;
            set;
        }*/// TODO: Something?
		TSplineNode Sub(TSplineNode a);
		TSplineNode Add(TSplineNode a);
		TSplineNode Mult(double a);
		bool Eq(TSplineNode a);
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
