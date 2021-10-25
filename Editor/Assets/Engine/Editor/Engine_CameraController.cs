using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Utilities;
using System;
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

        public static float m_transformSpeed = 2;
        float m_cameraSpeed = 25;
        Vector3 m_direction;
        internal void Update()
        {
            MovementSpeedCalc();

            if (m_input.GetButton(Engine_Input.Mouse_Input.IsMiddleButtonPressed))
            {
                TransformMovement();
                ScreenMovement();
                HeightTransformMovement();
            }

            if (m_input.GetButton(Engine_Input.Mouse_Input.IsRightButtonPressed))
            {
                if (m_input.GetButton(Engine_Input.Mouse_Input.IsLeftButtonPressed))
                    ScreenMovement();
                else
                {
                    TransformMovement();
                    CameraMovement();
                }
                HeightTransformMovement();
            }

            ZoomMovement();
            if (!m_input.GetKey(VirtualKey.X))
                m_camera.m_transform.m_position += m_direction * (float)Engine_Time.m_delta;
            m_direction = new Vector3();
        }

        void MovementSpeedCalc()
        {
            if (m_input.GetButton(Engine_Input.Mouse_Input.IsLeftButtonPressed)
                || m_input.GetButton(Engine_Input.Mouse_Input.IsRightButtonPressed))
                m_transformSpeed += m_input.GetMouseWheel();

            m_transformSpeed = Math.Clamp(m_transformSpeed, 0.1f, 10);
        }

        void ZoomMovement()
        {
            if (!m_input.GetButton(Engine_Input.Mouse_Input.IsLeftButtonPressed)
                && !m_input.GetButton(Engine_Input.Mouse_Input.IsMiddleButtonPressed)
                && !m_input.GetButton(Engine_Input.Mouse_Input.IsRightButtonPressed))
                m_direction += 50 * m_camera.m_front * m_input.GetMouseWheel();
        }

        void CameraMovement(int _horizontalFactor = 1, int _verticalFactor = 1)
        {
            m_camera.m_transform.m_rotation.X += m_cameraSpeed * m_input.GetMouseAxis().X * (float)Engine_Time.m_delta * _horizontalFactor;
            m_camera.m_transform.m_rotation.Y += m_cameraSpeed * m_input.GetMouseAxis().Y * (float)Engine_Time.m_delta * _verticalFactor;
        }

        void TransformMovement()
        {
            m_direction = new Vector3();
            if (m_input.GetKey(VirtualKey.W)) m_direction += m_transformSpeed * m_camera.m_front;
            if (m_input.GetKey(VirtualKey.S)) m_direction -= m_transformSpeed * m_camera.m_front;
            if (m_input.GetKey(VirtualKey.A)) m_direction += m_transformSpeed * m_camera.m_right;
            if (m_input.GetKey(VirtualKey.D)) m_direction -= m_transformSpeed * m_camera.m_right;
        }
        void HeightTransformMovement()
        {
            if (m_input.GetKey(VirtualKey.LeftShift)) m_transformSpeed *= 4;
            if (m_input.GetKey(VirtualKey.LeftControl)) m_transformSpeed *= 0.25f;

            if (m_input.GetKey(VirtualKey.E)) m_direction += m_transformSpeed * m_camera.m_up;
            if (m_input.GetKey(VirtualKey.Q)) m_direction -= m_transformSpeed * m_camera.m_up;
            if (m_input.GetKey(VirtualKey.E) && m_input.GetKey(VirtualKey.W)) m_direction += m_transformSpeed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.Q) && m_input.GetKey(VirtualKey.W)) m_direction -= m_transformSpeed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.E) && m_input.GetKey(VirtualKey.S)) m_direction += m_transformSpeed * m_camera.m_localUp;
            if (m_input.GetKey(VirtualKey.Q) && m_input.GetKey(VirtualKey.S)) m_direction -= m_transformSpeed * m_camera.m_localUp;
        }
        void ScreenMovement()
        {
            m_direction -= m_camera.m_right * m_input.GetMouseAxis().X;
            m_direction -= m_camera.m_localUp * m_input.GetMouseAxis().Y;
        }
        void HorizontalScreenMovement()
        {
            m_direction += 0.1f * Vector3.Normalize(m_camera.m_front * new Vector3(1, 0, 1)) * m_input.GetMouseAxis().Y;
        }
    }
}
