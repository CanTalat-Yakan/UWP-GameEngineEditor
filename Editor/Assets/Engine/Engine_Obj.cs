using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Editor.Assets.Engine
{    
    public class Engine_Obj
    {
        public Guid Id;

        public List<Vertex> Vertices;
        public uint VertexStride;
        public List<ushort> Indices;
        public uint IndexStride;

        public Engine_Obj()
        {
            Id = Guid.NewGuid();
        }
    }
}
