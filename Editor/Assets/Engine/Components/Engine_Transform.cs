using Editor.Assets.Engine.Data;
using SharpDX;

namespace Editor.Assets.Engine.Components
{
    class Engine_Transform
    {
        internal Engine_Transform m_parent;

        internal SPerModelConstantBuffer m_constantsBuffer { get => new SPerModelConstantBuffer() { ModelView = m_worldMatrix }; }
        internal Matrix m_worldMatrix = Matrix.Identity;
        internal Matrix m_normalMatrix = Matrix.Identity;
        internal Vector3 m_position = Vector3.Zero;
        internal Vector3 m_rotation = Vector3.Zero;
        internal Vector3 m_scale = Vector3.One;

        internal void Udate()
        {
            Matrix translation = Matrix.Translation(m_position);
            Matrix rotation = Matrix.RotationYawPitchRoll(m_rotation.X, m_rotation.Y, m_rotation.Z);
            Matrix scale = Matrix.Scaling(m_scale);

            m_worldMatrix = Matrix.Transpose(scale * rotation * translation);
            if (m_parent != null) m_worldMatrix = m_worldMatrix * m_parent.m_worldMatrix;
        }

        public override string ToString()
        {
            string s = "";
            s += m_position.ToString() + "\n";
            s += m_rotation.ToString() + "\n";
            s += m_scale.ToString();
            return s;
        }
    }
}
