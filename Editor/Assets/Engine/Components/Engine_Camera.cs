using System;
using Editor.Assets.Engine.Data;
using Editor.Assets.Engine.Utilities;
using SharpDX;

namespace Editor.Assets.Engine.Components
{
    class Engine_Camera
    {
        Engine_Renderer m_d3d;

        internal SViewConstantsBuffer m_viewConstants;

        internal Vector3 m_cameraPosition = Vector3.Zero;
        internal Vector3 m_cameraMouseRot = Vector3.Zero;
        internal Vector2 m_mouseAxis = Vector2.Zero;
        internal Vector3 m_front = Vector3.ForwardLH;
        internal Vector3 m_right = Vector3.Right;
        internal Vector3 m_up = Vector3.Up;
        internal Vector3 m_localUp = Vector3.Up;

        Windows.Foundation.Point? m_tmpPoint = new Windows.Foundation.Point();
        Windows.Foundation.Point m_pointerPosition = new Windows.Foundation.Point();

        internal Engine_Camera()
        {
            #region //Get Instances
            m_d3d = Engine_Renderer.Instance;
            #endregion
        }

        void RecreateViewConstants()
        {
            #region //Get PointerDelta
            m_pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;

            m_mouseAxis.X = (float)(m_pointerPosition.X - m_tmpPoint.Value.X);
            m_mouseAxis.Y = (float)(m_pointerPosition.Y - m_tmpPoint.Value.Y);

            m_tmpPoint = m_pointerPosition;
            #endregion

            #region //Set Transform
            var front = new Vector3(
                MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_cameraMouseRot.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)));
            m_front = Vector3.Normalize(front);
            m_right = Vector3.Normalize(Vector3.Cross(m_front, m_up));
            m_localUp = Vector3.Normalize(Vector3.Cross(m_right, m_front));
            #endregion

            #region //Set ViewConstantBuffer
            var view = Matrix.LookAtLH(
                m_cameraPosition,
                m_cameraPosition + m_front,
                m_up);

            var aspect = (float)(m_d3d.m_swapChainPanel.ActualWidth / m_d3d.m_swapChainPanel.ActualHeight);

            var proj = Matrix.PerspectiveFovLH(
                MathUtil.DegreesToRadians(110 / aspect),
                aspect,
                0.1f, 1000);

            var viewProj = Matrix.Transpose(view * proj);
            m_viewConstants = new SViewConstantsBuffer() { VP = viewProj, WCP = m_cameraPosition };
            #endregion
        }
    }
}
