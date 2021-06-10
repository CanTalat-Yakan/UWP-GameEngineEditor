namespace Editor.Assets.Engine.Helper
{
    using SharpDX.Mathematics.Interop;

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
}

