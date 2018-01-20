using System;
using System.Collections.Generic;
using System.Text;

using SharpNav.Collections;
using SharpNav.Geometry;
using SharpNav.Pathfinding;

namespace SharpNav.IO.Proto
{
    [ProtoBuf.ProtoContract]
    class NavMeshData
    {
        [ProtoBuf.ProtoMember(1)]
        public Vector3 Origin;
        [ProtoBuf.ProtoMember(2, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        public float TileWidth;
        [ProtoBuf.ProtoMember(3, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        public float TileHeight;
        [ProtoBuf.ProtoMember(4, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int MaxTiles;
        [ProtoBuf.ProtoMember(5, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int MaxPolys;

        [ProtoBuf.ProtoMember(20)]
        public List<NavTileData> Tiles = new List<NavTileData>();

        public TiledNavMesh ToObject()
        {
            TiledNavMesh result = new TiledNavMesh(Origin, TileWidth, TileHeight, MaxTiles, MaxPolys);
            foreach (NavTileData tileData in Tiles)
            {
                NavPolyId tileRef;
                NavTile tile = tileData.ToObject(result.IdManager, out tileRef);
                result.AddTileAt(tile, tileRef);
            }

            return result;
        }
    }

    [ProtoBuf.ProtoContract]
    class NavTileData
    {
        [ProtoBuf.ProtoMember(1)]
        public NavPolyId id;

        [ProtoBuf.ProtoMember(2)]
        public Vector2i Location;

        [ProtoBuf.ProtoMember(3, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int Layer;

        [ProtoBuf.ProtoMember(4, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int Salt;

        [ProtoBuf.ProtoMember(5)]
        public List<NavPolyData> Polys = new List<NavPolyData>();

        [ProtoBuf.ProtoMember(7)]
        public List<Vector3> Verts = new List<Vector3>();

        [ProtoBuf.ProtoMember(8)]
        public List<PolyMeshDetail.MeshData> DetailMeshes = new List<PolyMeshDetail.MeshData>();

        [ProtoBuf.ProtoMember(9)]
        public List<Vector3> DetailVerts = new List<Vector3>();

        [ProtoBuf.ProtoMember(10)]
        public List<PolyMeshDetail.TriangleData> DetailTris = new List<PolyMeshDetail.TriangleData>();

        [ProtoBuf.ProtoMember(11)]
        public List<OffMeshConnection> OffMeshConnections = new List<OffMeshConnection>();

        [ProtoBuf.ProtoMember(13)]
        public BVTreeData BVTree;

        [ProtoBuf.ProtoMember(14, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        public float BvQuantFactor;

        [ProtoBuf.ProtoMember(15, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int BvNodeCount;

        [ProtoBuf.ProtoMember(16)]
        public BBox3 Bounds;

        [ProtoBuf.ProtoMember(17, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        public float WalkableClimb;

        static public NavTileData ToData(NavTile tile, NavPolyId id)
        {
            NavTileData data = new NavTileData();
            data.id = id;
            data.Location = tile.Location;
            data.Layer = tile.Layer;
            data.Salt = tile.Salt;
            foreach(NavPoly poly in tile.Polys) data.Polys.Add(NavPolyData.ToData(poly));
            data.Verts.AddRange(tile.Verts);
            data.DetailMeshes.AddRange(tile.DetailMeshes);
            data.DetailVerts.AddRange(tile.DetailVerts);
            data.DetailTris.AddRange(tile.DetailTris);
            data.OffMeshConnections.AddRange(tile.OffMeshConnections);

            data.BVTree = BVTreeData.ToData(tile.BVTree);

            data.BvQuantFactor = tile.BvQuantFactor;
            data.BvNodeCount = tile.BvNodeCount;
            data.Bounds = tile.Bounds;
            data.WalkableClimb = tile.WalkableClimb;

            return data;
        }

        public NavTile ToObject(NavPolyIdManager manager, out NavPolyId refId)
        {
            refId = id;
            NavTile result = new NavTile(Location, Layer, manager, refId);

            result.Salt = Salt;
            result.Bounds = Bounds;
            result.Polys = new NavPoly[Polys.Count];
            for (int i = 0; i < Polys.Count; ++i) result.Polys[i] = Polys[i].ToObject();
            result.PolyCount = Polys.Count;
            result.Verts = Verts.ToArray();
            result.DetailMeshes = DetailMeshes.ToArray();
            result.DetailVerts = DetailVerts.ToArray();
            result.DetailTris = DetailTris.ToArray();
            result.OffMeshConnections = OffMeshConnections.ToArray();
            result.OffMeshConnectionCount = result.OffMeshConnections.Length;
            result.BvNodeCount = BvNodeCount;
            result.BvQuantFactor = BvQuantFactor;
            result.WalkableClimb = WalkableClimb;
            result.BVTree = new BVTree(BVTree.Nodes);
            return result;
        }
    }

    [ProtoBuf.ProtoContract]
    public class NavPolyData
    {
        [ProtoBuf.ProtoMember(1, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public NavPolyType PolyType;

        [ProtoBuf.ProtoMember(2)]
        public List<Link> Links = new List<Link>();

        [ProtoBuf.ProtoMember(3, IsPacked = true)]
        public List<int> Verts = new List<int>();

        [ProtoBuf.ProtoMember(4, IsPacked = true)]
        public List<int> Neis = new List<int>();

        [ProtoBuf.ProtoMember(5, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int VertCount;

        [ProtoBuf.ProtoMember(6)]
        public Area Area;

        static public NavPolyData ToData(NavPoly poly)
        {
            NavPolyData data = new NavPolyData();
            data.PolyType = poly.PolyType;
            data.Links = poly.Links;
            data.Verts.AddRange(poly.Verts);
            data.Neis.AddRange(poly.Neis);
            data.VertCount = poly.VertCount;
            data.Area = poly.Area;

            return data;
        }

        public NavPoly ToObject()
        {
            NavPoly result = new NavPoly();
            result.PolyType = PolyType;
            result.Links = Links;
            result.Verts = Verts.ToArray();
            result.Neis = Neis.ToArray();
            result.VertCount = VertCount;
            result.Area = Area;

            return result;
        }
    }

    [ProtoBuf.ProtoContract]
    public class BVTreeData
    {
        [ProtoBuf.ProtoMember(1)]
        public List<BVTree.Node> Nodes = new List<BVTree.Node>();

        static public BVTreeData ToData(BVTree tree)
        {
            BVTreeData data = new BVTreeData();
            for (int i = 0; i < tree.Count; i++)
                data.Nodes.Add(tree[i]);

            return data;
        }
    }
}
