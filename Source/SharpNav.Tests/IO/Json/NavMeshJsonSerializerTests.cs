// Copyright (c) 2015-2016 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
// Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE

using System.IO;

using NUnit.Framework;

using SharpNav.IO.Json;

namespace SharpNav.Tests.IO.Json
{
	[TestFixture]
	class NavMeshSerializationTests
	{
		[Test]
		public void JsonSerializationTest()
		{
			string objPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "nav_test.obj");
			string snjPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "mesh.snj");

			var objModel = new ObjModel(objPath);
			TiledNavMesh mesh = NavMesh.Generate(objModel.GetTriangles(), NavMeshGenerationSettings.Default);
			new NavMeshJsonSerializer().Serialize(snjPath, mesh);

			TiledNavMesh deserializedMesh = new NavMeshJsonSerializer().Deserialize(snjPath);
		}
	}
}
