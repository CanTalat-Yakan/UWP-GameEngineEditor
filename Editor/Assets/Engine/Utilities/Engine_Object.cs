using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Data;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Object
    {
        internal Engine_Transform m_transform = new Engine_Transform();
        internal Engine_Material m_material;
        internal Engine_Mesh m_mesh;

        internal void Update_Render()
        {
            m_transform.Udate();
            m_material.Render(m_transform.m_constantsBuffer);
            m_mesh.Render();
        }
    }
}
