using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;


namespace Editor.Assets.Engine.Editor
{
    class Engine_CameraController
    {
        public string m_profile { get => m_camera.m_transform.ToString(); }
        Engine_Camera m_camera;
        Engine_Input m_input;

        public Engine_CameraController(Engine_Camera _camera)
        {
            m_camera = _camera;
            m_input = Engine_Input.Instance;
        }

        internal void Update()
        {
            m_camera.m_transform.m_rotation.X -= m_input.GetMouseAxis().X;
            m_camera.m_transform.m_rotation.Y -= m_input.GetMouseAxis().Y;

            float speed = 2;
            if (m_input.GetKey(VirtualKey.LeftShift)) speed *= 4;
            if (m_input.GetKey(VirtualKey.LeftControl)) speed *= 0.25f;

            if (m_input.GetKey(VirtualKey.W)) m_camera.m_transform.m_position += speed * m_camera.m_front;
            if (m_input.GetKey(VirtualKey.S)) m_camera.m_transform.m_position -= speed * m_camera.m_front;
            if (m_input.GetKey(VirtualKey.A)) m_camera.m_transform.m_position += speed * m_camera.m_right;
            if (m_input.GetKey(VirtualKey.D)) m_camera.m_transform.m_position -= speed * m_camera.m_right;
            if (m_input.GetKey(VirtualKey.E)) m_camera.m_transform.m_position += speed * m_camera.m_up;
            if (m_input.GetKey(VirtualKey.Q)) m_camera.m_transform.m_position -= speed * m_camera.m_up;
            if (m_input.GetKey(VirtualKey.E) && m_input.GetKey(VirtualKey.W)) m_camera.m_transform.m_position += speed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.Q) && m_input.GetKey(VirtualKey.W)) m_camera.m_transform.m_position -= speed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.E) && m_input.GetKey(VirtualKey.S)) m_camera.m_transform.m_position += speed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.Q) && m_input.GetKey(VirtualKey.S)) m_camera.m_transform.m_position -= speed * m_camera.m_localUp;
        }
    }
}
