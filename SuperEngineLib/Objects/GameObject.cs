using System;
using OpenTK;
using SuperEngine.Editors;
using System.Collections.Generic;
using SuperEngine.Misc;

namespace SuperEngineLib.Objects {
	/// <summary>
	/// Description of GameObject.
	/// </summary>
	public class GameObject : NotifyPropertyChanged {
		#region Position/Orientation
		Vector3 position;
		Quaternion orientation;

		public Vector3 Position {
			get { return position; }
			set {
				SetProperty(ref position, value);
			}
		}

		public Quaternion Orientation {
			get { return orientation; }
			set {
				SetProperty(ref orientation, value);
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

