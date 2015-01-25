using OpenTK;
using SuperEngineLib.Misc;

namespace SuperEngineLib.Maths {
    public abstract class SplineNodeBase<TSplineNode> : NotifyPropertyChanged, ISplineNode<TSplineNode> {
        double tension = -0.5;
        double bias;
        double continuity = -0.05;

        /// <summary>
        /// The tension at this node.
        /// Tight = +1.0
        /// Round = -1.0
        /// When Tension=Bias=Continuity=0 this is Catmul-Rom.
        /// When Tension=1 & Bias=Continuity=0 this is Simple Cubic.
        /// When Tension=Bias=0 & Continuity=-1 this is linear interp.
        /// </summary>
        public double Tension {
            get {
                return tension;
            }
            set {
                SetProperty(ref tension, value);
            }
        }

        /// <summary>
        /// The bias at this node.
        /// Post Shoot = +1.0
        /// Pre Shoot = -1.0
        /// When Tension=Bias=Continuity=0 this is Catmul-Rom.
        /// When Tension=1 & Bias=Continuity=0 this is Simple Cubic.
        /// When Tension=Bias=0 & Continuity=-1 this is linear interp.
        /// </summary>
        public double Bias {
            get {
                return bias;
            }
            set {
                SetProperty(ref bias, value);
            }
        }

        /// <summary>
        /// The continuity at this node.
        /// Inverted Corners = +1.0
        /// Box Corners = -1.0
        /// When Tension=Bias=Continuity=0 this is Catmul-Rom.
        /// When Tension=1 & Bias=Continuity=0 this is Simple Cubic.
        /// When Tension=Bias=0 & Continuity=-1 this is linear interp.
        /// </summary>
        public double Continuity {
            get {
                return continuity;
            }
            set {
                SetProperty(ref continuity, value);
            }
        }

        static SplineNodeBase() {
            PropertyDependsOn<SplineNodeBase<TSplineNode>>("TBC", "Tension");
            PropertyDependsOn<SplineNodeBase<TSplineNode>>("TBC", "Bias");
            PropertyDependsOn<SplineNodeBase<TSplineNode>>("TBC", "Continuity");
        }

        #region ISplineNode implementation

        public abstract TSplineNode Subtract(TSplineNode a);

        public abstract TSplineNode Add(TSplineNode a);

        public abstract TSplineNode Multiply(double a);

        public abstract bool Eq(TSplineNode a);

        public abstract double Length {
            get;
        }

        public abstract double LengthFast {
            get;
        }

        public abstract double LengthSquared {
            get;
        }

        #endregion
    }

}
