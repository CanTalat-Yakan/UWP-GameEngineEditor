using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Scene
    {
        Engine_Camera m_camera = new Engine_Camera();
        Engine_Object m_gameObject = new Engine_Object();

        static readonly string SHADER_FILE = @"Assets//Engine//Resources//Shader.hlsl";

        internal void Start()
        {
            m_gameObject.m_mesh = new Engine_Mesh(Engine_ObjLoader.Load(EPrimitives.Cube));
            m_gameObject.m_material = new Engine_Material(SHADER_FILE);
        }
        internal void Update()
        {

        }
        internal void LateUpdate()
        {

        }
        internal void Render()
        {
            m_gameObject.Update_Render(m_camera.m_viewConstants);
        }
    }
}
