// Copyright (c) 2015-2016 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System.IO;

using NUnit.Framework;

using SharpNav.IO.Json;

namespace SharpNav.Tests.IO.Proto
{
	[TestFixture]
	class NavMeshSerializationTests
	{
		[Test]
		public void ProtoSerializationTest()
		{
            string path = TestContext.CurrentContext.TestDirectory;

            //
            string snjPath = System.IO.Path.Combine(path, "nav_test.snj");
            TiledNavMesh deserializedMesh = new NavMeshJsonSerializer().Deserialize(snjPath);

            //
            string snbPath = System.IO.Path.Combine(path, "nav_test.snb");
            new SharpNav.IO.Proto.NavMeshProtoSerializer().Serialize(snbPath, deserializedMesh);
            TiledNavMesh deserializedMesh2 = new SharpNav.IO.Proto.NavMeshProtoSerializer().Deserialize(snbPath);

            //
            string snjPath2 = System.IO.Path.Combine(path, "nav_test2.snj");
            new NavMeshJsonSerializer().Serialize(snjPath2, deserializedMesh2);
        }
	}
}
