using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Editor.Assets.Engine
{    
    public class Obj
    {
        public Guid Id;

        public IBuffer Data;
        public List<CVertex> Vertices;
        public uint VertexStride;
        public List<ushort> Indices;
        public uint IndexStride;

        public Obj()
        {
            Id = Guid.NewGuid();
        }
    }
}
