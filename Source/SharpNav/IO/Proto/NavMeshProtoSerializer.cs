#if IO_PROTO
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;

using SharpNav.Collections;
using SharpNav.Geometry;
using SharpNav.Pathfinding;
using System.Reflection;

namespace SharpNav.IO.Proto
{
    /// <summary>
    /// Subclass of NavMeshSerializer that implements 
    /// serialization/deserializtion in binary files with proto format
    /// </summary>
    public class NavMeshProtoSerializer : NavMeshSerializer
    {
        private static readonly string FileHeader = "SNMB";

        //increase this once every time the file format changes.
        private static readonly int FormatVersion = 1;

        public override void Serialize(Stream stream, TiledNavMesh mesh)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                //
                byte[] HEADER = System.Text.Encoding.UTF8.GetBytes(FileHeader);
                writer.Write(HEADER);
                writer.Write(FormatVersion);

                //
                NavMeshData data = new NavMeshData();
                data.Origin = mesh.Origin;
                data.TileWidth = mesh.TileWidth;
                data.TileHeight = mesh.TileHeight;
                data.MaxTiles = mesh.MaxTiles;
                data.MaxPolys = mesh.MaxPolys;

                foreach (NavTile tile in mesh.Tiles)
                {
                    NavPolyId id = mesh.GetTileRef(tile);
                    data.Tiles.Add(NavTileData.ToData(tile, id));
                }

                ProtoBuf.Serializer.Serialize<NavMeshData>(stream, data);
            }
        }

        public override TiledNavMesh Deserialize(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                //
                byte[] HEADER = System.Text.Encoding.UTF8.GetBytes(FileHeader);
                byte[] head = reader.ReadBytes(HEADER.Length);
                if (head.SequenceEqual(HEADER) == false)
                {
                    return null;
                }

                if (reader.ReadInt32() != FormatVersion)
                {
                    return null;
                }

                //
                NavMeshData data = ProtoBuf.Serializer.Deserialize<NavMeshData>(stream);

                TiledNavMesh mesh = new TiledNavMesh(data.Origin, data.TileWidth, data.TileHeight, data.MaxTiles, data.MaxPolys);
                foreach(NavTileData tileData in data.Tiles)
                {
                    NavPolyId tileRef;
                    NavTile tile = tileData.ToObject(mesh.IdManager, out tileRef);
                    mesh.AddTileAt(tile, tileRef);
                }

                return mesh;
            }

            return null;
        }
    }
}
#endif
