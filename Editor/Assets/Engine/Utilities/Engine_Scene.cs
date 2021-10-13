using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Editor;
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
        public string m_profile;

        Engine_Camera m_camera = new Engine_Camera();
        Engine_CameraController m_cameraController;
        List<Engine_Object> m_gameObject = new List<Engine_Object>();

        static readonly string SHADER_FILE = @"Assets//Engine//Resources//Shader.hlsl";
        static readonly string IMAGE_FILE = @"Assets//Engine//Resources//UVMap.jpg";


        void CreateCube(float _x, float _y, float _z)
        {
            Engine_Object gObject = new Engine_Object();
            gObject.m_mesh = new Engine_Mesh(Engine_ObjLoader.Load(EPrimitives.Cube));
            gObject.m_material = new Engine_Material(SHADER_FILE, IMAGE_FILE);
            gObject.m_transform.m_position = new SharpDX.Vector3(_x, _y, _z);
            gObject.m_transform.m_scale = new SharpDX.Vector3(new Random().Next(1, 3), new Random().Next(1, 3), new Random().Next(1, 3));
            //gObject.m_transform.m_Rotation = new SharpDX.Vector4(new Random().Next(1, 360), new Random().Next(1, 360), new Random().Next(1, 360), 1);
            m_gameObject.Add(gObject);
        }

        internal void Awake()
        {
            m_cameraController = new Engine_CameraController(m_camera);
        }
        internal void Start()
        {
            CreateCube(0, 0, 2);
            CreateCube(0, 0, -2);
            CreateCube(0, 2, 0);
            CreateCube(0, -2, 0);
            CreateCube(2, 0, 0);
            CreateCube(-2, 0, 0);
            CreateCube(0, -1, -1);
            CreateCube(-2, 0, -2);
            CreateCube(1, 0, 0);
            CreateCube(1, 1, 1);
            CreateCube(2, 0, 0);
            CreateCube(0, 2, 4);
        }
        internal void Update()
        {
            m_camera.RecreateViewConstants();
            m_cameraController.Update();
            m_profile = m_cameraController.m_profile;
        }
        internal void LateUpdate()
        {

        }
        internal void Render()
        {
            foreach (var item in m_gameObject)
                item.Update_Render(m_camera.m_viewConstants);
        }
    }
}
