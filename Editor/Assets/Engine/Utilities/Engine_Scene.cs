using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Editor;
using Editor.Assets.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = SharpDX.Vector2;
using Vector3 = SharpDX.Vector3;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Scene
    {
        public string m_profile;

        Engine_Camera m_camera = new Engine_Camera();
        Engine_CameraController m_cameraController;
        internal Engine_ObjectManager m_objectManager = new Engine_ObjectManager();


        internal void Awake()
        {
            m_cameraController = new Engine_CameraController(m_camera);
        }

        Engine_Object parent, subParent;
        internal void Start()
        {
            parent = m_objectManager.CreateEmpty("Content");
            subParent = m_objectManager.CreateEmpty("Cubes");
            subParent.m_parent = parent;

            m_objectManager.CreatePrimitive(EPrimitiveTypes.SPHERE, parent).m_transform.m_position = new Vector3(0, 0, 2);
            m_objectManager.CreatePrimitive(EPrimitiveTypes.SPHERE, parent).m_transform.m_position = new Vector3(0, 0, -2);
            m_objectManager.CreatePrimitive(EPrimitiveTypes.SPHERE, parent).m_transform.m_position = new Vector3(0, 2, 0);
            m_objectManager.CreatePrimitive(EPrimitiveTypes.SPHERE, parent).m_transform.m_position = new Vector3(0, -2, 0);
            m_objectManager.CreatePrimitive(EPrimitiveTypes.SPHERE, parent).m_transform.m_position = new Vector3(2, 0, 0);
            m_objectManager.CreatePrimitive(EPrimitiveTypes.SPHERE, parent).m_transform.m_position = new Vector3(-2, 0, 0);
            m_objectManager.CreatePrimitive(EPrimitiveTypes.CUBE, subParent);
        }

        internal void Update()
        {
            m_camera.RecreateViewConstants();
            m_cameraController.Update();

            if (Engine_Input.Instance.GetKey(Windows.System.VirtualKey.C, Engine_Input.Input_State.DOWN))
                m_objectManager.CreatePrimitive(EPrimitiveTypes.CUBE, subParent).m_transform = new Engine_Transform
                {
                    m_rotation = new Vector3(new Random().Next(1, 360), new Random().Next(1, 360), new Random().Next(1, 360)),
                    m_scale = new Vector3(new Random().Next(1, 3), new Random().Next(1, 3), new Random().Next(1, 3))
                };
        }

        internal void LateUpdate()
        {
            m_profile = "Objects: " + m_objectManager.m_list.Count().ToString();

            int vertexCount = 0;
            foreach (var item in m_objectManager.m_list)
                if (item.m_enabled && item.m_mesh != null)
                    vertexCount += item.m_mesh.m_vertexCount;
            m_profile += "\n" + "Vertices: " + vertexCount;
        }

        internal void Render()
        {
            foreach (var item in m_objectManager.m_list)
                if (item.m_enabled && item.m_mesh != null)
                    item.Update_Render();
        }
    }
}
