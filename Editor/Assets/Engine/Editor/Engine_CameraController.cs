using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Vector3 = SharpDX.Vector3;

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
            float speed = 50;
            speed *= (float)Engine_Time.m_delta;
            m_camera.m_transform.m_rotation.X -= m_input.GetMouseAxis().X * speed;
            m_camera.m_transform.m_rotation.Y -= m_input.GetMouseAxis().Y * speed;

            speed = 2;
            if (m_input.GetKey(VirtualKey.LeftShift)) speed *= 4;
            if (m_input.GetKey(VirtualKey.LeftControl)) speed *= 0.25f;
            speed *= (float)Engine_Time.m_delta;

            Vector3 direction = new Vector3();
            if (m_input.GetKey(VirtualKey.W)) direction += speed * m_camera.m_front;
            if (m_input.GetKey(VirtualKey.S)) direction -= speed * m_camera.m_front;
            if (m_input.GetKey(VirtualKey.A)) direction += speed * m_camera.m_right;
            if (m_input.GetKey(VirtualKey.D)) direction -= speed * m_camera.m_right;
            if (m_input.GetKey(VirtualKey.E)) direction += speed * m_camera.m_up;
            if (m_input.GetKey(VirtualKey.Q)) direction -= speed * m_camera.m_up;
            if (m_input.GetKey(VirtualKey.E) && m_input.GetKey(VirtualKey.W)) direction += speed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.Q) && m_input.GetKey(VirtualKey.W)) direction -= speed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.E) && m_input.GetKey(VirtualKey.S)) direction += speed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.Q) && m_input.GetKey(VirtualKey.S)) direction -= speed * m_camera.m_localUp;

            m_camera.m_transform.m_position += direction;
        }
    }
}
