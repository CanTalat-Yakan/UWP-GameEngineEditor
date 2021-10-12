using Editor.Assets.Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        obj.Vertices = new List<Engine_Vertex>{
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

                        obj.Indices = new List<ushort>{
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
    }
}
