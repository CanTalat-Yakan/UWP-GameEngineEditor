namespace Editor.Assets.Engine.Helper
{
    using SharpDX.Mathematics.Interop;
    using Vector2 = SharpDX.Vector2;
    using Vector3 = SharpDX.Vector3;

    public struct ViewConstantsBuffer
    {
        public RawMatrix VP;
        public RawVector3 WCP;
        public float pad;
    }

    public struct PerModelConstantBuffer
    {
        public RawMatrix World;
    }

    public struct Vertex
    {
        public Vector3 pos;
        public Vector2 texCoord;
        public Vector3 normal;

        public Vertex(
            float x, float y, float z,
            float u, float v,
            float nx, float ny, float nz)
        {
            pos = new Vector3(x, y, z);
            texCoord = new Vector2(u, v);
            normal = new Vector3(nx, ny, nz);
        }

    };
}

