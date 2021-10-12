using Editor.Assets.Engine.Data;
using SharpDX;

namespace Editor.Assets.Engine.Components
{
    class Engine_Transform
    {
        internal SPerModelConstantBuffer m_ConstantsBuffer { get => new SPerModelConstantBuffer() { World = m_WorldMatrix }; }
        internal Matrix m_WorldMatrix = new Matrix();
        internal Vector3 m_Position = new Vector3();
        internal Vector4 m_Rotation = new Vector4(0, 0, 0, 1);
        internal Vector3 m_Scale = new Vector3(1, 1, 1);

        internal void Udate()
        {
            Matrix translation = Matrix.Translation(m_Position);
            Matrix rotation = Matrix.RotationYawPitchRoll(m_Rotation.X, m_Rotation.Y, m_Rotation.Z);
            Matrix scale = Matrix.Scaling(m_Scale);

            m_WorldMatrix = Matrix.Transpose(scale * rotation * translation);
        }
    }
}
