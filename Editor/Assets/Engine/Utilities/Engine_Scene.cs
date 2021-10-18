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
        List<Engine_Object> m_objects = new List<Engine_Object>();

        static readonly string SHADER_FILE = @"Assets//Engine//Resources//Shader.hlsl";
        static readonly string IMAGE_FILE = @"Assets//Engine//Resources//UVMap.jpg";
        static readonly string OBJ_FILE = @"Assets//Engine//Resources//Sphere.obj";

        Engine_Material m_material;
        Engine_Mesh m_meshObj;
        Engine_Mesh m_meshCube;

        void CreateCube(float _x, float _y, float _z)
        {
            Engine_Object gObject = new Engine_Object();
            gObject.m_mesh = m_meshCube;
            gObject.m_material = m_material;
            gObject.m_transform.m_position = new Vector3(_x, _y, _z);
            gObject.m_transform.m_scale = new Vector3(new Random().Next(1, 3), new Random().Next(1, 3), new Random().Next(1, 3));
            gObject.m_transform.m_rotation = new Vector3(new Random().Next(1, 360), new Random().Next(1, 360), new Random().Next(1, 360));
            m_objects.Add(gObject);
        }
        void CreateObj(float _x, float _y, float _z)
        {
            Engine_Object gObject = new Engine_Object();
            gObject.m_mesh = m_meshObj;
            gObject.m_material = m_material;
            gObject.m_transform.m_position = new Vector3(_x, _y, _z);
            gObject.m_transform.m_rotation = new Vector3(new Random().Next(1, 360), new Random().Next(1, 360), new Random().Next(1, 360));
            m_objects.Add(gObject);
        }

        internal void Awake()
        {
            m_cameraController = new Engine_CameraController(m_camera);
            m_meshCube = new Engine_Mesh(Engine_ObjLoader.Load(EPrimitives.Cube));
            m_meshObj = new Engine_Mesh(Engine_ObjLoader.LoadFile(OBJ_FILE));
            m_material = new Engine_Material(SHADER_FILE, IMAGE_FILE);
        }
        internal void Start()
        {
            CreateObj(0, 0, 2);
            CreateObj(0, 0, -2);
            CreateObj(0, 2, 0);
            CreateObj(0, -2, 0);
            CreateObj(2, 0, 0);
            CreateObj(-2, 0, 0);
            CreateCube(0, 0, 0);
        }
        internal void Update()
        {
            m_camera.RecreateViewConstants();
            m_cameraController.Update();
            m_profile = m_cameraController.m_profile + "\n" + m_objects.Count();
            int vertexCount = 0;
            foreach (var item in m_objects)
                vertexCount += item.m_mesh.m_vertexCount;
            m_profile += "\n" + vertexCount;

            if (Engine_Input.Instance.GetKey(Windows.System.VirtualKey.C, Engine_Input.Input_State.DOWN))
                CreateCube(0, 0, 0);
        }
        internal void LateUpdate()
        {

        }
        internal void Render()
        {
            foreach (var item in m_objects)
                item.Update_Render(m_camera.m_viewConstants);
        }
    }
}
