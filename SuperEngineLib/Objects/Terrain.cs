using System;
using OpenTK;
using SuperEngine.Editors;

namespace SuperEngine.Objects {
	public class Terrain : GameObject {
		#region Terrain
		private uint width;
		private uint height;

		private float[,] heights;

		private string[] textures;
		private byte[,,] textureAlpha;
		#endregion

        public Terrain(Vector3 position, Quaternion orientation) : base(position, orientation)
        {
		}
	}
}

