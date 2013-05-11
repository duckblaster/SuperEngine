//    Copyright 2008 James P Michels III
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Affero General Public License as published by
//    the Free Software Foundation.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Affero General Public License for more details.
//
//    You should have received a copy of the GNU Affero General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

using OpenTK.Graphics;

namespace OOGL.Textures
{
	public class Texture : IDisposable
	{
		public readonly uint handle;
		
		public Texture(uint handle)
		{
			this.handle = handle;
		}
		
		public void Dispose()
		{
			uint handle = this.handle;
		
			GL.DeleteTextures(1, ref handle);
		
			GC.SuppressFinalize(this);
		}
	}
}