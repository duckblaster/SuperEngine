using System;
using System.Collections.Generic;
namespace SuperEngine.Commands {
	public sealed class ParameterGroup {
		public bool Locked { get; private set; }
		
		private Dictionary<string, Parameter> parameters;
		public ReadOnlyDictionary<string, Parameter> Parameters {
			get {
				return new ReadOnlyDictionary<string, Parameter>(this.parameters);
			}
		}

		public ParameterGroup(ParameterGroup parameterGroup) {
			this.parameters = new Dictionary<string, Parameter>(parameterGroup.parameters);
			Locked = false;
		}
		
		public ParameterGroup(Dictionary<string, Parameter> parameters) {
			this.parameters = parameters;
			Locked = false;
		}

		private ParameterGroup() {
			this.parameters = new Dictionary<string, Parameter>();
			Locked = false;
		}
		
		public void Lock() {
			Locked = true;
		}
		private void Unlock() {
			Locked = false;
		}
		public void Lock(string name) {
			parameters[name].Lock();
		}
		public void Unlock(string name) {
			parameters[name].Unlock();
		}

		#region Indexers
		/*public object this[string name] {
			get { return parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public byte this[string name] {
			get { return (byte)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public sbyte this[string name] {
			get { return (sbyte)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public int this[string name] {
			get { return (int)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public uint this[string name] {
			get { return (uint)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public short this[string name] {
			get { return (short)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public ushort this[string name] {
			get { return (ushort)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public long this[string name] {
			get { return (long)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public ulong this[string name] {
			get { return (ulong)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public float this[string name] {
			get { return (float)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public double this[string name] {
			get { return (double)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public char this[string name] {
			get { return (char)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public bool this[string name] {
			get { return (bool)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public string this[string name] {
			get { return (string)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}
		public decimal this[string name] {
			get { return (decimal)parameters[name].Value; }
			set { parameters[name].Value = value; }
		}*/
		#endregion
		
		#region Add()
		private void Add(Parameter param) {
			if(Locked) {
				throw new AccessViolationException("ParameterGroup is Locked");
			} else {
				parameters.Add(param.Name, param);
			}
		}
        public void Add<T>(string name, T val)
        {
            Add(new Parameter(name, val, typeof(T)));
        }
		public void Add(string name, object val) {
			Add(new Parameter(name, val, val.GetType()));
		}
		public void Add(string name, object val, Type type) {
			Add(new Parameter(name, val, type));
		}
		public void Add(string name, ParameterGroup val) {
			Add(new Parameter(name, val, typeof(ParameterGroup)));
		}
		public void Add(string name, ParameterGroup val, bool locked) {
			Add(new Parameter(name, val, typeof(ParameterGroup), locked));
		}
		public void Add(string name, byte val) {
			Add(new Parameter(name, val, typeof(byte)));
		}
		public void Add(string name, sbyte val) {
			Add(new Parameter(name, val, typeof(sbyte)));
		}
		public void Add(string name, int val) {
			Add(new Parameter(name, val, typeof(int)));
		}
		public void Add(string name, uint val) {
			Add(new Parameter(name, val, typeof(uint)));
		}
		public void Add(string name, short val) {
			Add(new Parameter(name, val, typeof(short)));
		}
		public void Add(string name, ushort val) {
			Add(new Parameter(name, val, typeof(ushort)));
		}
		public void Add(string name, long val) {
			Add(new Parameter(name, val, typeof(long)));
		}
		public void Add(string name, ulong val) {
			Add(new Parameter(name, val, typeof(ulong)));
		}
		public void Add(string name, float val) {
			Add(new Parameter(name, val, typeof(float)));
		}
		public void Add(string name, double val) {
			Add(new Parameter(name, val, typeof(double)));
		}
		public void Add(string name, char val) {
			Add(new Parameter(name, val, typeof(char)));
		}
		public void Add(string name, bool val) {
			Add(new Parameter(name, val, typeof(bool)));
		}
		public void Add(string name, string val) {
			Add(new Parameter(name, val, typeof(string)));
		}
		public void Add(string name, decimal val) {
			Add(new Parameter(name, val, typeof(decimal)));
		}
		#endregion
		
		private static ParameterGroup terrainBrush;
		public static ParameterGroup TerrainBrush {
			get {
				if(terrainBrush == null) {
					terrainBrush = new ParameterGroup();
					terrainBrush.Add("radius", 10);
					terrainBrush.Add("sensitivity", 10);
					terrainBrush.Add("falloff", 10);
					terrainBrush.Lock();
				}
				return terrainBrush;
			}
		}
		
		private static ParameterGroup terrainTextureBrush;
		public static ParameterGroup TerrainTextureBrush {
			get {
				if(terrainTextureBrush == null) {
					terrainTextureBrush = new ParameterGroup(TerrainBrush);
					terrainTextureBrush.Add("texture", "(none)");
					//terrainTextureBrush.Add("scale", 1.0f);
					//terrainTextureBrush.Add("rotation", 0);
					terrainTextureBrush.Lock();
				}
				return terrainTextureBrush;
			}
		}
	}
}

