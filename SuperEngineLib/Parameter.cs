using System;
namespace FPRPG.Commands {
	public class Parameter {
		public string Name { get; private set; }
		public bool Locked { get; private set; }
		private object val;
		public object Value {
			get {
				return val;
			}
			set {
				if(ValueType.IsAssignableFrom(value.GetType()) && !(value is ParameterGroup && Locked)) {
					val = value;
				} else {
					throw new InvalidCastException();
				}
			}
		}
		public Type ValueType { get; private set; }
		public Parameter(string name, object val, Type type) {
			this.Name = name;
			this.Value = val;
			this.ValueType = type;
			this.Locked = false;
		}
		public Parameter(string name, object val, Type type, bool locked) : this(name, val, type) {
			this.Locked = locked;
		}
		public void Lock() {
			Locked = true;
		}
		internal void Unlock() {
			Locked = false;
		}
	}
}

