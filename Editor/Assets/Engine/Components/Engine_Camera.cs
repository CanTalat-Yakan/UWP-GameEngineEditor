using System;
using Editor.Assets.Engine.Data;
using Editor.Assets.Engine.Utilities;
using SharpDX;
using D3D11 = SharpDX.Direct3D11;

namespace Editor.Assets.Engine.Components
{
    class Engine_Camera
    {
        Engine_Renderer m_d3d;

        internal SViewConstantsBuffer m_viewConstants;
        D3D11.Buffer m_view;

        internal Engine_Transform m_transform = new Engine_Transform();
        internal Vector3 m_front = Vector3.ForwardLH;
        internal Vector3 m_right = Vector3.Right;
        internal Vector3 m_up = Vector3.Up;
        internal Vector3 m_localUp = Vector3.Up;

        internal Engine_Camera()
        {
            #region //Get Instances
            m_d3d = Engine_Renderer.Instance;
            #endregion

            SViewConstantsBuffer cbView = new SViewConstantsBuffer();
            m_view = D3D11.Buffer.Create(m_d3d.m_device, D3D11.BindFlags.ConstantBuffer, ref cbView);

            RecreateViewConstants();
        }

        internal void RecreateViewConstants()
        {
            #region //Set Transform
            m_transform.m_rotation.X %= 360;
            m_transform.m_rotation.Y = Math.Clamp(m_transform.m_rotation.Y, -89, 89);

            var front = new Vector3(
                MathF.Cos(MathUtil.DegreesToRadians(m_transform.m_rotation.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_transform.m_rotation.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_transform.m_rotation.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_transform.m_rotation.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_transform.m_rotation.Y)));
            m_front = Vector3.Normalize(front);
            m_right = Vector3.Normalize(Vector3.Cross(m_front, m_up));
            m_localUp = Vector3.Normalize(Vector3.Cross(m_right, m_front));
            #endregion

            #region //Set ViewConstantBuffer
            var view = Matrix.LookAtLH(
                m_transform.m_position,
                m_transform.m_position + m_front,
                m_up);

            var aspect = (float)(m_d3d.m_swapChainPanel.ActualWidth / m_d3d.m_swapChainPanel.ActualHeight);

            var proj = Matrix.PerspectiveFovLH(
                MathUtil.DegreesToRadians(110 / aspect),
                aspect,
                0.1f, 1000);

            var viewProj = Matrix.Transpose(view * proj);
            m_viewConstants = new SViewConstantsBuffer() { VP = viewProj, WCP = m_transform.m_position };
            #endregion

            m_d3d.m_deviceContext.UpdateSubresource(ref m_viewConstants, m_view);
            m_d3d.m_deviceContext.VertexShader.SetConstantBuffer(0, m_view);
        }
    }
}
