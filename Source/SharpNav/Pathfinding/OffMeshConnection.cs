// Copyright (c) 2014-2015 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System;

using SharpNav.Geometry;

namespace SharpNav.Pathfinding
{
	/// <summary>
	/// A set of flags that define properties about an off-mesh connection.
	/// </summary>
	[Flags]
	public enum OffMeshConnectionFlags : byte
	{
		/// <summary>
		/// No flags.
		/// </summary>
		None = 0x0,

		/// <summary>
		/// The connection is bi-directional.
		/// </summary>
		Bidirectional = 0x1
	}

    /// <summary>
    /// An offmesh connection links two polygons, which are not directly adjacent, but are accessibly through
    /// other means (jumping, climbing, etc...).
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class OffMeshConnection
	{
        /// <summary>
        /// Gets or sets the first endpoint of the connection
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public Vector3 Pos0 { get; set; }

        /// <summary>
        /// Gets or sets the second endpoint of the connection
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public Vector3 Pos1 { get; set; }

        /// <summary>
        /// Gets or sets the radius
        /// </summary>
        [ProtoBuf.ProtoMember(3, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets the polygon's index
        /// </summary>
        [ProtoBuf.ProtoMember(4, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public int Poly { get; set; }

        /// <summary>
        /// Gets or sets the polygon flag
        /// </summary>
        [ProtoBuf.ProtoMember(5, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public OffMeshConnectionFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the endpoint's side
        /// </summary>
        [ProtoBuf.ProtoMember(6, DataFormat = ProtoBuf.DataFormat.TwosComplement)]
        public BoundarySide Side { get; set; } 

		/// <summary>
		/// Gets or sets user data for this connection.
		/// </summary>
		public object Tag { get; set; }
	}
}
