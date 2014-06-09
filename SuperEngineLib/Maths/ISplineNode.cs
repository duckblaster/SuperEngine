
namespace SuperEngineLib.Maths {
	public interface ISplineNode<TSplineNode> {
		//double Tension;

		TSplineNode Subtract(TSplineNode a);
		TSplineNode Add(TSplineNode a);
		TSplineNode Multiply(double a);
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
