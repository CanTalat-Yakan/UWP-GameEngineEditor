using SharpDX.Mathematics.Interop;

namespace Editor.Assets.Engine.Data
{
    public struct SViewConstantsBuffer
    {
        public RawMatrix ViewProjection;
        public RawMatrix View;
        public RawMatrix Projection;
        public RawVector3 WorldCamPos;
        public float pad;
    }

    public struct SPerModelConstantBuffer
    {
        public RawMatrix ModelView;
    }
}

