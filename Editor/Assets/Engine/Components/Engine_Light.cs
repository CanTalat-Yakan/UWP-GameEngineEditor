using SharpDX.Mathematics.Interop;
using Quaternion = SharpDX.Quaternion;
using Vector2 = SharpDX.Vector2;
using Vector3 = SharpDX.Vector3;
using Vector4 = SharpDX.Vector4;

namespace Editor.Assets.Engine.Data
{
    public struct SDirectionalLight
    {
        public Vector3 direction;
        public float pad;
        public Vector4 diffuse;
        public Vector4 ambient;
        public float intensity;
        public Vector3 pad2;
    }

    public struct SPointLight
    {
        public Vector3 position;
        public float pad;
        public Vector4 diffuse;
        public float intensity;
        public float radius;
        public Vector2 pad2;
    }
}
