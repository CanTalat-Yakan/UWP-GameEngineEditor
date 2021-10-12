using Editor.Assets.Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Object
    {
        Engine_Transform m_transform;
        Engine_Material m_material;
        Engine_Mesh m_mesh;

        internal void Update_Render()
        {
            m_transform.Udate();
            m_material.Render();
            m_mesh.Render();
        }
    }
}
