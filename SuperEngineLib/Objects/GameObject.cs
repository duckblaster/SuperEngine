using System;
using OpenTK;
using SuperEngine.Editors;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenTK.Graphics.OpenGL;

namespace SuperEngine.Objects {
	/// <summary>
	/// Description of GameObject.
	/// </summary>
	public class GameObject {
		#region Position/Orientation
		#region Events
		public class PropertyChangedEventArgs<T> : EventArgs {
			public T OldValue { get; private set; }

			public T Value { get; private set; }

			public PropertyChangedEventArgs(T oldValue, T newValue) {
				this.OldValue = oldValue;
				this.Value = newValue;
			}
		}

		public class PositionChangedEventArgs : EventArgs {
			public Vector3 OldPosition { get; private set; }

			public Vector3 Position { get; private set; }

			public PositionChangedEventArgs(Vector3 oldPosition, Vector3 position) {
				this.OldPosition = oldPosition;
				this.Position = position;
			}
		}

		public class OrientationChangedEventArgs : EventArgs {
			public Quaternion OldOrientation { get; private set; }

			public Quaternion Orientation { get; private set; }

			public OrientationChangedEventArgs(Quaternion oldOrientation, Quaternion orientation) {
				this.OldOrientation = oldOrientation;
				this.Orientation = orientation;
			}
		}

		public event EventHandler<PositionChangedEventArgs> PositionChanged;
		public event EventHandler<OrientationChangedEventArgs> OrientationChanged;

		protected virtual void OnPositionChanged(PositionChangedEventArgs e) {
			if(PositionChanged != null) {
				PositionChanged(this, e);
			}
		}

		protected virtual void OnOrientationChanged(OrientationChangedEventArgs e) {
			if(OrientationChanged != null) {
				OrientationChanged(this, e);
			}
		}
		#endregion
		Vector3 position;
		Quaternion orientation;

		public Vector3 Position {
			get { return position; }
			set {
				if(position != value) {
					PositionChangedEventArgs e = new PositionChangedEventArgs(position, value);
					position = value;
					OnPositionChanged(e);
				}
			}
		}

		public Quaternion Orientation {
			get { return orientation; }
			set {
				if(orientation != value) {
					OrientationChangedEventArgs e = new OrientationChangedEventArgs(orientation, value);
					orientation = value;
					OnOrientationChanged(e);
				}
			}
		}
		#endregion
		#region Parent/Child
		List<GameObject> children = new List<GameObject>();
		GameObject parent;

		public IEnumerable<GameObject> Children {
			get {
				return children;
			}
			set {
				foreach(GameObject child in children) {
					RemoveChild(child);
				}
				foreach(GameObject child in value) {
					AddChild(child);
				}
			}
		}

		public GameObject Parent {
			get {
				return parent;
			}
			set {
				if(parent != value) {
					if(parent != null) {
						parent.children.Remove(this);
					}
					parent = value;
					if(parent != null) {
						parent.children.Add(this);
					}
				}
			}
		}

		public void AddChild(GameObject child) {
			child.Parent = this;
			children.Add(child);
		}

		public void RemoveChild(GameObject child) {
			if(child.Parent != this) {
				throw new InvalidOperationException();
			}
			children.Remove(child);
			child.Parent = null;
		}
		#endregion
		#region Update/Draw
		public virtual void Update() {
			
		}

		public virtual void FixedUpdate() {
			
		}

		public virtual void Draw() {
			
		}
		#endregion
		#region Editor
		public virtual Type Editor {
			get {
				return typeof(Editor);
			}
		}
		#endregion
		public GameObject(Vector3 position, Quaternion orientation) {
			this.position = position;
			this.orientation = orientation;
		}
	}
}

