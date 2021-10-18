using Editor.Assets.Engine.Data;
using System.IO;
using System.Collections.Generic;
using Vector3 = SharpDX.Vector3;
using Vector2 = SharpDX.Vector2;

namespace Editor.Assets.Engine.Helper
{
    enum EPrimitives { Cube, Plane, Cylinder, Pyramid, Sphere, }
    class Engine_ObjLoader
    {
        internal static Engine_MeshInfo Load(EPrimitives _type)
        {
            Engine_MeshInfo obj = new Engine_MeshInfo();

            switch (_type)
            {
                case EPrimitives.Cube:
                    {
                        float hs = 0.5f;
                        obj.vertices = new List<Engine_Vertex>{
                        // Front Face
                        new Engine_Vertex(-hs, -hs, -hs, 0, 1, 0, 0, 1),	//Bottom	Left
			            new Engine_Vertex(-hs, +hs, -hs, 0, 0, 0, 0, 1),	//Top		Left
			            new Engine_Vertex(+hs, +hs, -hs, 1, 0, 0, 0, 1),	//Top		Right
			            new Engine_Vertex(+hs, -hs, -hs, 1, 1, 0, 0, 1),	//Bottom	Right

			            // Left Face
			            new Engine_Vertex(-hs, -hs, +hs, 0, 1, 1, 0, 0),	//Bottom	Left
			            new Engine_Vertex(-hs, +hs, +hs, 0, 0, 1, 0, 0),	//Top		Left
			            new Engine_Vertex(-hs, +hs, -hs, 1, 0, 1, 0, 0),	//Top		Right
			            new Engine_Vertex(-hs, -hs, -hs, 1, 1, 1, 0, 0),	//Bottom	Right

			            // Back Face
			            new Engine_Vertex(+hs, -hs, +hs, 0, 1, 0, 0, -1),	//Bottom	Left
			            new Engine_Vertex(+hs, +hs, +hs, 0, 0, 0, 0, -1),	//Top		Left
			            new Engine_Vertex(-hs, +hs, +hs, 1, 0, 0, 0, -1),	//Top		Right
			            new Engine_Vertex(-hs, -hs, +hs, 1, 1, 0, 0, -1),	//Bottom	Right

			            // Right Face
			            new Engine_Vertex(+hs, -hs, -hs, 0, 1, -1, 0, 0),	//Bottom	Left
			            new Engine_Vertex(+hs, +hs, -hs, 0, 0, -1, 0, 0),	//Top		Left
			            new Engine_Vertex(+hs, +hs, +hs, 1, 0, -1, 0, 0),	//Top		Right
			            new Engine_Vertex(+hs, -hs, +hs, 1, 1, -1, 0, 0),	//Bottom	Right

			            // Top Face
			            new Engine_Vertex(-hs, +hs, -hs, 0, 0, 0, -1, 0),	//Top		Right
			            new Engine_Vertex(-hs, +hs, +hs, 1, 0, 0, -1, 0),	//Bottom	Right
			            new Engine_Vertex(+hs, +hs, +hs, 1, 1, 0, -1, 0),	//Bottom	Left
			            new Engine_Vertex(+hs, +hs, -hs, 0, 1, 0, -1, 0),	//Top		Left

			            // Base Face					 	
			            new Engine_Vertex(-hs, -hs, +hs, 1, 1, 0, 1, 0),	//Bottom	Left
			            new Engine_Vertex(-hs, -hs, -hs, 0, 1, 0, 1, 0),	//Top		Left
			            new Engine_Vertex(+hs, -hs, -hs, 0, 0, 0, 1, 0),	//Top		Right
			            new Engine_Vertex(+hs, -hs, +hs, 1, 0, 0, 1, 0)};   //Bottom	Right

                        obj.indices = new List<ushort>{
                        // Front Face
                        0, 1, 2,
                        0, 2, 3,
			            // Left Face
			            4, 5, 6,
                        4, 6, 7,
			            // Back Face
			            8, 9, 10,
                        8, 10, 11,
			            // Right Face
			            12, 13, 14,
                        12, 14, 15,
			            // Top Face
			            16, 17, 18,
                        16, 18, 19,
			            // Bottom Face
			            20, 21, 22,
                        20, 22, 23 };
                    }
                    break;
                case EPrimitives.Plane:
                    break;
                case EPrimitives.Cylinder:
                    break;
                case EPrimitives.Pyramid:
                    break;
                case EPrimitives.Sphere:
                    break;
                default:
                    break;
            }

            return obj;
        }

        internal static Engine_MeshInfo LoadFile(string _fileName)
        {
            string[] data = File.ReadAllLines(_fileName);

            Engine_MeshInfo obj = new Engine_MeshInfo();
            obj.vertices = new List<Engine_Vertex>();
            obj.indices = new List<ushort>();
            List<Vector3> position = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector3> normal = new List<Vector3>();
            int faceCount = 0;

            foreach (string line in data)
            {
                string[] split = line.Split(' ');
                var ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.NumberDecimalSeparator = ".";

                if (split[0] == "v")
                {
                    int iOffset = split[1] == "" ? 1 : 0;
                    position.Add(new Vector3(float.Parse(split[1 + iOffset], ci), float.Parse(split[2 + iOffset], ci), float.Parse(split[3 + iOffset], ci)));
                }
                else if (split[0] == "vt")
                    uv.Add(new Vector2(float.Parse(split[1], ci), float.Parse(split[2], ci)));
                else if (split[0] == "vn")
                    normal.Add(new Vector3(float.Parse(split[1], ci), float.Parse(split[2], ci), float.Parse(split[3], ci)));
                else if (split[0] == "f")
                {
                    int vertexCount = split.Length - 1;

                    for (int i = 1; i <= vertexCount; i++)
                    {
                        if (string.IsNullOrEmpty(split[i]))
                        {
                            vertexCount--;
                            continue;
                        }

                        string[] face = split[i].Split('/');
                        Engine_Vertex vertex = new Engine_Vertex(
                            position[int.Parse(face[0]) - 1].X,
                            position[int.Parse(face[0]) - 1].Y,
                            position[int.Parse(face[0]) - 1].Z,
                            uv[int.Parse(face[1]) - 1].X,
                            uv[int.Parse(face[1]) - 1].Y,
                            normal[int.Parse(face[2]) - 1].X,
                            normal[int.Parse(face[2]) - 1].Y,
                            normal[int.Parse(face[2]) - 1].Z);
                        obj.vertices.Add(vertex);
                    }

                    var offSet = faceCount * vertexCount;

                    var rangeIndices = new ushort[] {
                        (ushort)(0 + offSet),
                        (ushort)(1 + offSet),
                        (ushort)(2 + offSet)};
                    obj.indices.AddRange(rangeIndices);
                    if (vertexCount == 4)
                    {
                        rangeIndices = new ushort[] {
                        (ushort)(0 + offSet),
                        (ushort)(2 + offSet),
                        (ushort)(3 + offSet)};
                        obj.indices.AddRange(rangeIndices);
                    }

                    faceCount++;
                }
            }

            return obj;
        }
    }
}
