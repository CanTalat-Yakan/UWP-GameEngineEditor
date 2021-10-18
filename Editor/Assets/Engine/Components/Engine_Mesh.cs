using System;
using System.Linq;
using Editor.Assets.Engine.Data;
using Editor.Assets.Engine.Utilities;
using D3D11 = SharpDX.Direct3D11;


namespace Editor.Assets.Engine.Components
{
    public class Engine_Mesh
    {
        Engine_Renderer m_d3d;

        public D3D11.Buffer m_vertexBuffer;
        public D3D11.Buffer m_indexBuffer;

        public int m_vertexCount;
        public int m_vertexStride;
        public int m_indexCount;
        public int m_indexStride;


        internal Engine_Mesh(Engine_MeshInfo _obj)
        {
            #region //Get Instance of DirectX
            m_d3d = Engine_Renderer.Instance;
            #endregion

            #region //Set Variables
            m_vertexCount = _obj.vertices.Count();
            m_vertexStride = SharpDX.Utilities.SizeOf<Engine_Vertex>();

            m_indexCount = _obj.indices.Count();
            m_indexStride = sizeof(int);
            #endregion

            #region //Create VertexBuffer
            unsafe
            {
                fixed (Engine_Vertex* p = _obj.vertices.ToArray()) //Array
                {
                    IntPtr ptr = (IntPtr)p;
                    m_vertexBuffer = new D3D11.Buffer(
                      m_d3d.m_device,
                      ptr,
                      new D3D11.BufferDescription(
                          m_vertexCount * m_vertexStride, //ByteWidth
                          D3D11.BindFlags.VertexBuffer,
                          D3D11.ResourceUsage.Default));
                }
            }
            #endregion

            #region //Create IndexBuffer
            unsafe
            {
                fixed (ushort* p = _obj.indices.ToArray())
                {
                    IntPtr ptr = (IntPtr)p;
                    m_indexBuffer = new D3D11.Buffer(
                      m_d3d.m_device,
                      ptr,
                      new D3D11.BufferDescription(
                          m_indexCount * m_indexStride,
                          D3D11.BindFlags.IndexBuffer,
                          D3D11.ResourceUsage.Default));
                }
            }
            #endregion
        }

        internal void Render()
        {
            m_d3d.RenderMesh(
                m_vertexBuffer, m_vertexStride, 
                m_indexBuffer, m_indexCount);
        }
    }
}
