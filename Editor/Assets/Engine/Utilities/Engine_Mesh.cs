using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Editor.Assets.Engine.Helper;

namespace Editor.Assets.Engine.Utilities
{    
    public class Engine_Mesh
    {
        public Guid Id;

        public List<Engine_Vertex> Vertices;
        public uint VertexStride;
        public List<ushort> Indices;
        public uint IndexStride;

        public Engine_Mesh()
        {
            Id = Guid.NewGuid();
        }
    }
}
