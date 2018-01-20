// Copyright (c) 2014-2016 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

namespace SharpNav.Pathfinding
{
    /// <summary>
    /// A link is formed between two polygons in a TiledNavMesh
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class Link
	{
		/// <summary>
		/// Entity links to external entity.
		/// </summary>
		public const int External = unchecked((int)0x80000000);

		/// <summary>
		/// Doesn't link to anything.
		/// </summary>
		public const int Null = unchecked((int)0xffffffff);

        /// <summary>
        /// Gets or sets the neighbor reference (the one it's linked to)
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public NavPolyId Reference { get; set; }

        /// <summary>
        /// Gets or sets the index of polygon edge
        /// </summary>
        [ProtoBuf.ProtoMember(2, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int Edge { get; set; }

        /// <summary>
        /// Gets or sets the polygon side
        /// </summary>
        [ProtoBuf.ProtoMember(3, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public BoundarySide Side { get; set; }

        /// <summary>
        /// Gets or sets the minimum Vector3 of the bounding box
        /// </summary>
        [ProtoBuf.ProtoMember(4, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int BMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum Vector3 of the bounding box
        /// </summary>
        [ProtoBuf.ProtoMember(5, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int BMax { get; set; }

		public static bool IsExternal(int link)
		{
			return (link & Link.External) != 0;
		}
	}
}
