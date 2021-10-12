using SharpDX.Mathematics.Interop;

namespace Editor.Assets.Engine.Data
{
    public struct SViewConstantsBuffer
    {
        public RawMatrix VP;
        public RawVector3 WCP;
        public float pad;
    }

    public struct SPerModelConstantBuffer
    {
        public RawMatrix World;
    }
}

