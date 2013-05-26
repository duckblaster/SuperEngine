using System;
using OpenTK;
using SuperEngine.Objects;

namespace SuperEngine.Editors {
	public class TerrainEditor : Editor {
		private Terrain terrain;
		
		public TerrainEditor(Terrain terrain) {
			this.terrain = terrain;
		}
	}
}

