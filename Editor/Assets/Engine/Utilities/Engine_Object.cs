using Editor.Assets.Engine.Components;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Object
    {
        internal System.Guid ID = System.Guid.NewGuid();

        internal Engine_Object m_parent;

        internal Engine_Transform m_transform = new Engine_Transform();
        internal Engine_Material m_material;
        internal Engine_Mesh m_mesh;

        internal string m_name = "Object";
        internal bool m_enabled = true;
        internal bool m_static = false;

        
        internal void Update_Render()
        {
            if (!m_static)
            {
                if (m_parent != null)
                    m_transform.m_parent = m_parent.m_transform;
                m_transform.Udate();
            }
            m_material.Render(m_transform.m_constantsBuffer);
            m_mesh.Render();
        }
    }
}
