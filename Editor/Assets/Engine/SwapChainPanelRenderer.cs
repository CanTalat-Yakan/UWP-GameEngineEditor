namespace Editor.Assets.Engine
{
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Mathematics.Interop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using D3D11 = SharpDX.Direct3D11;
    using DXGI = SharpDX.DXGI;
    using Quaternion = SharpDX.Quaternion;
    using Vector2 = SharpDX.Vector2;
    using Vector3 = SharpDX.Vector3;
    using Vector4 = SharpDX.Vector4;

    internal class SwapChainPanelRenderer
    {
        D3D11.Device2 m_device;
        D3D11.DeviceContext m_deviceContext;
        DXGI.SwapChain2 m_swapChain;
        D3D11.Texture2D m_backBufferTexture;
        D3D11.RenderTargetView m_backBufferView;
        D3D11.VertexShader m_vertexShader;
        D3D11.PixelShader m_pixelShader;
        SwapChainPanel m_swapChainPanel;
        bool m_isDXInitialized;


        Dictionary<Guid, MeshBufferInfo> m_bufferMap;
        Vector3 m_cameraPosition;
        Vector3 m_cameraMouseRot = new Vector3(0, 0, 0);
        Vector3 m_front = new Vector3(0, 0, 10);
        Vector3 m_right = new Vector3(1, 0, 0);
        Vector3 m_up = Vector3.Up;
        D3D11.Buffer m_viewConstantsBuffer;
        ViewConstantsBuffer m_viewConstants;

        static readonly string VERTEX_SHADER_FILE = @"Assets//Engine//Shader.hlsl";
        static readonly string PIXEL_SHADER_FILE = @"Assets//Engine//Shader.hlsl";



        internal SwapChainPanelRenderer(SwapChainPanel swapChainPanel)
        {
            m_swapChainPanel = swapChainPanel;
            m_bufferMap = new Dictionary<Guid, MeshBufferInfo>();
        }

        public void Initialise()
        {
            m_swapChainPanel.SizeChanged += this.OnSwapChainPanelSizeChanged;

            InitialiseDirect3D();

            InitialiseScene();
        }
        void InitialiseDirect3D()
        {
            using (var defaultDevice = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.Debug))
            {
                m_device = defaultDevice.QueryInterface<D3D11.Device2>();
            }

            var rasterizerDesc = new D3D11.RasterizerStateDescription()
            {
                CullMode = D3D11.CullMode.None,
                FillMode = D3D11.FillMode.Solid
            };

            m_deviceContext = m_device.ImmediateContext2;
            m_deviceContext.Rasterizer.State = new D3D11.RasterizerState(m_device, rasterizerDesc);

            var swapChainDescription = new DXGI.SwapChainDescription1()
            {
                AlphaMode = DXGI.AlphaMode.Ignore,
                BufferCount = 2,
                Format = DXGI.Format.R8G8B8A8_UNorm,
                Height = (int)(m_swapChainPanel.RenderSize.Height),
                Width = (int)(m_swapChainPanel.RenderSize.Width),
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Scaling = DXGI.Scaling.Stretch,
                Stereo = false,
                SwapEffect = DXGI.SwapEffect.FlipSequential,
                Usage = DXGI.Usage.RenderTargetOutput
            };

            using (var dxgiDevice3 = m_device.QueryInterface<DXGI.Device3>())
            using (var dxgiFactory3 = dxgiDevice3.Adapter.GetParent<DXGI.Factory3>())
            {
                var swapChain1 = new DXGI.SwapChain1(dxgiFactory3, m_device, ref swapChainDescription);
                m_swapChain = swapChain1.QueryInterface<DXGI.SwapChain2>();
            }

            using (var nativeObject = ComObject.As<DXGI.ISwapChainPanelNative>(m_swapChainPanel))
            {
                nativeObject.SwapChain = m_swapChain;
            }

            m_backBufferTexture = m_swapChain.GetBackBuffer<D3D11.Texture2D>(0);
            m_backBufferView = new D3D11.RenderTargetView(m_device, m_backBufferTexture);

            m_deviceContext.Rasterizer.SetViewport(0, 0, (int)m_swapChainPanel.ActualWidth, (int)m_swapChainPanel.ActualHeight);

            Windows.UI.Xaml.Media.CompositionTarget.Rendering += (s, e) => { this.RenderScene(); };


            m_isDXInitialized = true;
        }

        Windows.Foundation.Point m_tmpPoint = new Windows.Foundation.Point();
        void RecreateViewConstants()
        {
            //    //360 degree horizontal rotation
            //    XMFLOAT3 front = {front.x = cos(XMConvertToRadians(m_mouseRot.x)) * cos(XMConvertToRadians(m_mouseRot.y)),
            //                      front.y = sin(XMConvertToRadians(m_mouseRot.y)),
            //                      front.z = sin(XMConvertToRadians(m_mouseRot.x)) * cos(XMConvertToRadians(m_mouseRot.y)) };
            //    //update frontVector
            //    m_front = XMVector3Normalize(XMVectorSet(front.x, front.y, front.z, 0));
            //    //update rightVector
            //    m_right = XMVector3Normalize(XMVector3Cross(m_front, m_up));

            //    //update viewMatrix
            //    m_view = XMMatrixLookAtLH(m_position, m_position + m_front, m_up);

            {
                var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
                if (m_tmpPoint != pointerPosition)
                {
                    m_cameraMouseRot.X -= (float)(pointerPosition.X - m_tmpPoint.X) * 0.2f;
                    m_cameraMouseRot.Y -= (float)(pointerPosition.Y - m_tmpPoint.Y) * 0.2f;
                    m_tmpPoint = pointerPosition;
                }
            }

            Vector3 front = new Vector3(
                MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_cameraMouseRot.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)));
            m_front = Vector3.Normalize(front);
            m_right = Vector3.Normalize(Vector3.Cross(m_front, m_up));

            var view = Matrix.LookAtLH(
                m_cameraPosition,
                m_cameraPosition + m_front,
                m_up);

            var proj = Matrix.PerspectiveFovLH(
                MathUtil.DegreesToRadians(80),
                (float)(m_swapChainPanel.ActualWidth / m_swapChainPanel.ActualHeight),
                0.1f, 1000);

            var viewProj = Matrix.Multiply(view, proj);

            m_viewConstants = new ViewConstantsBuffer() { VP = viewProj, WCP = m_cameraPosition };
        }
        void InitialiseScene()
        {
            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32B32_Float, 0, 0),
                new D3D11.InputElement("TEXCOORD", 0, DXGI.Format.R32G32_Float, D3D11.InputElement.AppendAligned, 0),
                new D3D11.InputElement("NORMAL", 0, DXGI.Format.R32G32B32_Float, D3D11.InputElement.AppendAligned, 0)};

            D3D11.InputLayout vertexLayout = null;


            using (var vsResult = ShaderBytecode.CompileFromFile(
              VERTEX_SHADER_FILE, "VS", "vs_4_0", ShaderFlags.Debug))
            {
                m_vertexShader = new D3D11.VertexShader(m_device, vsResult.Bytecode.Data);

                vertexLayout = new D3D11.InputLayout(
                  m_device, vsResult.Bytecode, inputElements);
            }
            m_deviceContext.VertexShader.Set(m_vertexShader);
            m_deviceContext.InputAssembler.InputLayout = vertexLayout;

            using (var psResult = ShaderBytecode.CompileFromFile(
              PIXEL_SHADER_FILE, "PS", "ps_4_0", ShaderFlags.Debug))
            {
                m_pixelShader = new D3D11.PixelShader(m_device, psResult.Bytecode.Data);
            }
            m_deviceContext.PixelShader.Set(m_pixelShader);


            m_deviceContext.InputAssembler.PrimitiveTopology =
              SharpDX.Direct3D.PrimitiveTopology.TriangleList;

            m_cameraPosition = new Vector3(0, 0.5f, -10);

            RecreateViewConstants();

            m_viewConstantsBuffer = D3D11.Buffer.Create(
              m_device,
              D3D11.BindFlags.ConstantBuffer,
              ref m_viewConstants);

            m_deviceContext.VertexShader.SetConstantBuffer(0, m_viewConstantsBuffer);

            CreateCube();
        }
        void RenderScene()
        {
            RecreateViewConstants();

            m_deviceContext.OutputMerger.SetRenderTargets(this.m_backBufferView);

            m_deviceContext.ClearRenderTargetView(
            m_backBufferView, new RawColor4(0.5f, 0.2f, 0.2f, 1));

            m_deviceContext.UpdateSubresource<ViewConstantsBuffer>(
              ref m_viewConstants,
              m_viewConstantsBuffer);

            lock (m_bufferMap)
            {
                foreach (var entry in m_bufferMap)
                {
                    m_deviceContext.VertexShader.SetConstantBuffer(1, entry.Value.constantsBuffer);

                    m_deviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(
                        entry.Value.vertexBuffer, (int)entry.Value.vertexStride, 0));

                    m_deviceContext.InputAssembler.SetIndexBuffer(
                      entry.Value.indexBuffer, (DXGI.Format)entry.Value.indexFormat, 0);

                    m_deviceContext.DrawIndexed(entry.Value.indexCount, 0, 0);
                }
            }
            m_swapChain.Present(0, DXGI.PresentFlags.None);
        }

        void CreateCube()
        {
            Obj obj = new Obj();
            float hs = 0.5f;


            // quad - with pos, uv & normals
            obj.Vertices = new List<CVertex>{
                // Front Face
                new CVertex(-hs, -hs, -hs, 0, 1, 0, 0, 1),	//Bottom	Left
			    new CVertex(-hs, +hs, -hs, 0, 0, 0, 0, 1),	//Top		Left
			    new CVertex(+hs, +hs, -hs, 1, 0, 0, 0, 1),	//Top		Right
			    new CVertex(+hs, -hs, -hs, 1, 1, 0, 0, 1),	//Bottom	Right

			    // Left Face
			    new CVertex(-hs, -hs, +hs, 0, 1, 1, 0, 0),	//Bottom	Left
			    new CVertex(-hs, +hs, +hs, 0, 0, 1, 0, 0),	//Top		Left
			    new CVertex(-hs, +hs, -hs, 1, 0, 1, 0, 0),	//Top		Right
			    new CVertex(-hs, -hs, -hs, 1, 1, 1, 0, 0),	//Bottom	Right

			    // Back Face
			    new CVertex(+hs, -hs, +hs, 0, 1, 0, 0, -1),	//Bottom	Left
			    new CVertex(+hs, +hs, +hs, 0, 0, 0, 0, -1),	//Top		Left
			    new CVertex(-hs, +hs, +hs, 1, 0, 0, 0, -1),	//Top		Right
			    new CVertex(-hs, -hs, +hs, 1, 1, 0, 0, -1),	//Bottom	Right

			    // Right Face
			    new CVertex(+hs, -hs, -hs, 0, 1, -1, 0, 0),	//Bottom	Left
			    new CVertex(+hs, +hs, -hs, 0, 0, -1, 0, 0),	//Top		Left
			    new CVertex(+hs, +hs, +hs, 1, 0, -1, 0, 0),	//Top		Right
			    new CVertex(+hs, -hs, +hs, 1, 1, -1, 0, 0),	//Bottom	Right

			    // Top Face
			    new CVertex(-hs, +hs, -hs, 0, 0, 0, -1, 0),	//Top		Right
			    new CVertex(-hs, +hs, +hs, 1, 0, 0, -1, 0),	//Bottom	Right
			    new CVertex(+hs, +hs, +hs, 1, 1, 0, -1, 0),	//Bottom	Left
			    new CVertex(+hs, +hs, -hs, 0, 1, 0, -1, 0),	//Top		Left

			    // Base Face					 	
			    new CVertex(-hs, -hs, +hs, 1, 1, 0, 1, 0),	//Bottom	Left
			    new CVertex(-hs, -hs, -hs, 0, 1, 0, 1, 0),	//Top		Left
			    new CVertex(+hs, -hs, -hs, 0, 0, 0, 1, 0),	//Top		Right
			    new CVertex(+hs, -hs, +hs, 1, 0, 0, 1, 0)};	//Bottom	Right

            obj.Indices = new List<ushort>{
                // Front Face
                0, 1, 2,
                0, 2, 3,
			    // Left Face
			    4, 5, 6,
                4, 6, 7,
			    // Back Face
			    8, 9, 10,
                8, 10, 11,
			    // Right Face
			    12, 13, 14,
                12, 14, 15,
			    // Top Face
			    16, 17, 18,
                16, 18, 19,
			    // Bottom Face
			    20, 21, 22,
                20, 22, 23 };

            obj.VertexStride = (uint)Utilities.SizeOf<CVertex>();
            obj.IndexStride = (uint)sizeof(int);



            var bufferInfo = new MeshBufferInfo();

            unsafe
            {
                fixed (CVertex* p = obj.Vertices.ToArray())
                {
                    IntPtr ptr = (IntPtr)p;
                    int byteWidth = (int)(obj.Vertices.Count * obj.VertexStride);
                    bufferInfo.vertexBuffer = new D3D11.Buffer(
                      m_device,
                      ptr,
                      new D3D11.BufferDescription(byteWidth, D3D11.BindFlags.VertexBuffer, D3D11.ResourceUsage.Default));
                }
            }
            bufferInfo.vertexCount = obj.Vertices.Count;
            bufferInfo.vertexStride = obj.VertexStride;

            unsafe
            {
                fixed (ushort* p = obj.Indices.ToArray())
                {
                    IntPtr ptr = (IntPtr)p;
                    int byteWidth = (int)(obj.Indices.Count * obj.IndexStride);
                    bufferInfo.indexBuffer = new D3D11.Buffer(
                      m_device,
                      ptr,
                      new D3D11.BufferDescription(byteWidth, D3D11.BindFlags.IndexBuffer, D3D11.ResourceUsage.Default));
                }
            }
            bufferInfo.indexFormat = Windows.Graphics.DirectX.DirectXPixelFormat.R16UInt;
            bufferInfo.indexCount = obj.Indices.Count;



            Matrix scale = Matrix.Scaling(new Vector3(1, 1, 1));
            Matrix rotation;
            Matrix translation = Matrix.Translation(new Vector3(0, 0.5f, 10));

            Quaternion quaternion = new Quaternion(new Vector4(0, 0, 0, 1));
            Matrix.RotationQuaternion(ref quaternion, out rotation);

            Matrix worldMatrix = scale * rotation * translation;

            var world = Matrix.Transpose(worldMatrix);
            PerModelConstantBuffer cb = new PerModelConstantBuffer() { World = world };

            bufferInfo.constantsBuffer = D3D11.Buffer.Create(
              m_device,
              D3D11.BindFlags.ConstantBuffer,
              ref cb);

            lock (m_bufferMap)
            {
                m_bufferMap.Add(obj.Id, bufferInfo);
            }
        }


        void OnSwapChainPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.m_isDXInitialized)
            {
                var newSize = new Size2((int)e.NewSize.Width, (int)e.NewSize.Height);

                Utilities.Dispose(ref this.m_backBufferView);
                Utilities.Dispose(ref this.m_backBufferTexture);

                m_swapChain.ResizeBuffers(
                  m_swapChain.Description.BufferCount,
                  (int)e.NewSize.Width,
                  (int)e.NewSize.Height,
                  m_swapChain.Description1.Format,
                  m_swapChain.Description1.Flags);

                this.m_backBufferTexture =
                  D3D11.Resource.FromSwapChain<D3D11.Texture2D>(this.m_swapChain, 0);

                this.m_backBufferView = new D3D11.RenderTargetView(this.m_device, this.m_backBufferTexture);

                m_swapChain.SourceSize = newSize;

                m_deviceContext.Rasterizer.SetViewport(
                  0, 0, (int)e.NewSize.Width, (int)e.NewSize.Height);
            }
        }

    }


}

