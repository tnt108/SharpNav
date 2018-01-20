// Copyright (c) 2015 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System;
using System.Reflection;
using System.IO;
using SharpNav;

namespace SharpNav.IO
{
	//TODO make an interface if it doesn't need to be extended

	/// <summary>
	/// Abstract class for nav mesh serializers
	/// </summary>
	public abstract class NavMeshSerializer
	{
		/// <summary>
		/// Serialize navigation mesh into external file
		/// </summary>
		/// <param name="path">path of file to serialize into</param>
		/// <param name="mesh">mesh to serialize</param>
		public virtual void Serialize(string path, TiledNavMesh mesh)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                Serialize(stream, mesh);
            }
        }

        public abstract void Serialize(Stream stream, TiledNavMesh mesh);

        /// <summary>
        /// Deserialize navigation mesh from external file
        /// </summary>
        /// <param name="path">file to deserialize from</param>
        /// <returns>deserialized mesh</returns>
        public virtual TiledNavMesh Deserialize(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                TiledNavMesh mesh = Deserialize(stream);
                return mesh;
            }

            return null;
        }

        public abstract TiledNavMesh Deserialize(Stream stream);
    }
}
