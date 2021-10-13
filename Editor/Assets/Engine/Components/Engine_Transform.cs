using Editor.Assets.Engine.Data;
using SharpDX;

namespace Editor.Assets.Engine.Components
{
    class Engine_Transform
    {
        internal SPerModelConstantBuffer m_constantsBuffer { get => new SPerModelConstantBuffer() { World = m_worldMatrix }; }
        internal Matrix m_worldMatrix = new Matrix();
        internal Vector3 m_position = new Vector3();
        internal Vector3 m_rotation = new Vector3(0, 0, 0);
        internal Vector4 m_quaternion = new Vector4(0, 0, 0, 1);
        internal Vector3 m_scale = new Vector3(1, 1, 1);

        internal void Udate()
        {
            Matrix translation = Matrix.Translation(m_position);
            Matrix rotation = Matrix.RotationYawPitchRoll(m_rotation.X, m_rotation.Y, m_rotation.Z);
            Matrix scale = Matrix.Scaling(m_scale);

            m_worldMatrix = Matrix.Transpose(scale * rotation * translation);
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
