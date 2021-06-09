namespace Editor.Assets.Engine
{
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Mathematics.Interop;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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

    internal class Engine_SwapChainPanelRenderer
    {
        D3D11.Device2 m_device;
        D3D11.DeviceContext m_deviceContext;
        DXGI.SwapChain2 m_swapChain;
        D3D11.Texture2D m_backBufferTexture;
        D3D11.RenderTargetView m_backBufferView;
        D3D11.Texture2D m_depthStencilTexture;
        D3D11.Texture2DDescription m_depthStencilTextureDescription;
        D3D11.DepthStencilView m_depthStencilView;
        D3D11.VertexShader m_vertexShader;
        D3D11.PixelShader m_pixelShader;
        SwapChainPanel m_swapChainPanel;
        internal bool m_IsDXInitialized;

        Dictionary<Guid, Engine_MeshBufferInfo> m_bufferMap;

        Vector3 m_cameraPosition = Vector3.Zero;
        Vector3 m_cameraMouseRot = Vector3.Zero;
        Vector2 m_mouseAxis = Vector2.Zero;
        Vector3 m_front = Vector3.ForwardLH;
        Vector3 m_right = Vector3.Right;
        Vector3 m_up = Vector3.Up;
        Vector3 m_localUp = Vector3.Up;
        Windows.Foundation.Point? m_tmpPoint = new Windows.Foundation.Point();
        Windows.Foundation.Point m_pointerPosition = new Windows.Foundation.Point();
        D3D11.Buffer m_viewConstantsBuffer;
        ViewConstantsBuffer m_viewConstants;

        internal bool m_IsRightButtonPressed = false;
        internal float m_MouseSensitivity = 0.2f;
        internal float m_MovementSpeed = 0.01f;
        internal bool m_W = false;
        internal bool m_S = false;
        internal bool m_A = false;
        internal bool m_D = false;
        internal bool m_E = false;
        internal bool m_Q = false;
        internal bool m_C = false;

        internal int m_frames = 0;

        static readonly string VERTEX_SHADER_FILE = @"Assets//Engine//Shader.hlsl";
        static readonly string PIXEL_SHADER_FILE = @"Assets//Engine//Shader.hlsl";



        internal Engine_SwapChainPanelRenderer(SwapChainPanel swapChainPanel)
        {
            m_swapChainPanel = swapChainPanel;
            m_bufferMap = new Dictionary<Guid, Engine_MeshBufferInfo>();
        }
        View_Port m_view;
        public void Initialise(View_Port _view)
        {
            m_view = _view;

            m_swapChainPanel.SizeChanged += this.OnSwapChainPanelSizeChanged;

            InitialiseDirect3D();

            InitialiseScene();
        }
        void InitialiseDirect3D()
        {
            using (var defaultDevice = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.Debug))
                m_device = defaultDevice.QueryInterface<D3D11.Device2>();

            var rasterizerDesc = new D3D11.RasterizerStateDescription()
            {
                FillMode = D3D11.FillMode.Solid,
                CullMode = D3D11.CullMode.Back
            };

            m_deviceContext = m_device.ImmediateContext2;
            m_deviceContext.Rasterizer.State = new D3D11.RasterizerState(m_device, rasterizerDesc);

            var swapChainDescription = new DXGI.SwapChainDescription1()
            {
                AlphaMode = DXGI.AlphaMode.Ignore,
                BufferCount = 2,
                Format = DXGI.Format.R8G8B8A8_UNorm,
                Height = (int)m_swapChainPanel.RenderSize.Height,
                Width = (int)m_swapChainPanel.RenderSize.Width,
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
                nativeObject.SwapChain = m_swapChain;

            m_backBufferTexture = m_swapChain.GetBackBuffer<D3D11.Texture2D>(0);
            m_backBufferView = new D3D11.RenderTargetView(m_device, m_backBufferTexture);


            m_depthStencilTextureDescription = new D3D11.Texture2DDescription
            {
                Format = DXGI.Format.D16_UNorm,
                ArraySize = 1,
                MipLevels = 1,
                Width = swapChainDescription.Width,
                Height = swapChainDescription.Height,
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Usage = D3D11.ResourceUsage.Default,
                BindFlags = D3D11.BindFlags.DepthStencil,
                CpuAccessFlags = D3D11.CpuAccessFlags.None,
                OptionFlags = D3D11.ResourceOptionFlags.None
            };
            using (m_depthStencilTexture = new D3D11.Texture2D(m_device, m_depthStencilTextureDescription))
                m_depthStencilView = new D3D11.DepthStencilView(m_device, m_depthStencilTexture);

            D3D11.DepthStencilStateDescription desc = new D3D11.DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                DepthComparison = D3D11.Comparison.Less,
                DepthWriteMask = D3D11.DepthWriteMask.All,
            };
            D3D11.DepthStencilState state = new D3D11.DepthStencilState(m_device, desc);
            m_deviceContext.OutputMerger.SetDepthStencilState(state);
            m_deviceContext.OutputMerger.SetRenderTargets(m_depthStencilView, m_backBufferView);


            m_deviceContext.Rasterizer.SetViewport(0, 0, (int)m_swapChainPanel.ActualWidth, (int)m_swapChainPanel.ActualHeight);


            Windows.UI.Xaml.Media.CompositionTarget.Rendering += (s, e) => { this.RenderScene(); };

            Stopwatch watch = new Stopwatch();
            float time = 0;
            Windows.UI.Xaml.Media.CompositionTarget.Rendering += (s, e) =>
            {
                watch.Stop();

                time += watch.ElapsedMilliseconds;

                m_view.m_debugFrames.Text = "Time: " + time.ToString();
                m_view.m_debugFrames.Text += "\nFrames: " + m_frames++.ToString();
                m_view.m_debugFrames.Text += "\nFPS: " + m_view.m_Fps;
                m_view.m_debugFrames.Text += "\nDelta: " + watch.ElapsedMilliseconds.ToString();


                watch.Reset();
                watch.Start();
            };

            m_IsDXInitialized = true;
        }

        void RecreateViewConstants()
        {
            m_pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;

            m_mouseAxis.X = (float)(m_pointerPosition.X - m_tmpPoint.Value.X);
            m_mouseAxis.Y = (float)(m_pointerPosition.Y - m_tmpPoint.Value.Y);

            m_tmpPoint = m_pointerPosition;


            if (m_IsRightButtonPressed)
            {
                m_cameraMouseRot.X -= m_mouseAxis.X * m_MouseSensitivity;
                m_cameraMouseRot.Y -= m_mouseAxis.Y * m_MouseSensitivity;

                m_cameraMouseRot.Y = Math.Clamp(m_cameraMouseRot.Y, -89, 89);


                if (m_W) m_cameraPosition += m_MovementSpeed * m_front;
                if (m_S) m_cameraPosition -= m_MovementSpeed * m_front;
                if (m_A) m_cameraPosition += m_MovementSpeed * m_right;
                if (m_D) m_cameraPosition -= m_MovementSpeed * m_right;
                if (m_E) m_cameraPosition += m_MovementSpeed * m_up;
                if (m_Q) m_cameraPosition -= m_MovementSpeed * m_up;
                if (m_E && m_W) m_cameraPosition += m_MovementSpeed * m_localUp;
                if (m_Q && m_W) m_cameraPosition -= m_MovementSpeed * m_localUp;
                if (m_E && m_S) m_cameraPosition += m_MovementSpeed * m_localUp;
                if (m_Q && m_S) m_cameraPosition -= m_MovementSpeed * m_localUp;
            }
            m_view.m_main.m_Layout.m_ViewProperties.Pos = m_cameraPosition;
            m_view.m_main.m_Layout.m_ViewProperties.Rot = m_cameraMouseRot;


            var front = new Vector3(
                MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)),
                MathF.Sin(MathUtil.DegreesToRadians(m_cameraMouseRot.X)) * MathF.Cos(MathUtil.DegreesToRadians(m_cameraMouseRot.Y)));
            m_front = Vector3.Normalize(front);
            m_right = Vector3.Normalize(Vector3.Cross(m_front, m_up));
            m_localUp = Vector3.Normalize(Vector3.Cross(m_right, m_front));

            var view = Matrix.LookAtLH(
                m_cameraPosition,
                m_cameraPosition + m_front,
                m_up);

            var proj = Matrix.PerspectiveFovLH(
                MathUtil.DegreesToRadians(80),
                (float)(m_swapChainPanel.ActualWidth / m_swapChainPanel.ActualHeight),
                0.1f, 1000);

            var viewProj = Matrix.Transpose(view * proj);
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

            m_cameraPosition = new Vector3(-3, 0.5f, 0);

            RecreateViewConstants();

            m_viewConstantsBuffer = D3D11.Buffer.Create(
              m_device,
              D3D11.BindFlags.ConstantBuffer,
              ref m_viewConstants);

            m_deviceContext.VertexShader.SetConstantBuffer(0, m_viewConstantsBuffer);

            CreateCube(Vector3.Zero);
            CreateCube(Vector3.Right * 2);
            CreateCube(new Vector3(0, 2, 4));
        }

        void RenderScene()
        {


            if (m_C)
                CreateCube(new Vector3(new Random().Next(-10, 10), new Random().Next(-10, 10), new Random().Next(-10, 10)));

            RecreateViewConstants();


            Windows.UI.Color col = m_view.m_main.m_Layout.m_ViewProperties.m_Color.SelectedColor;
            m_deviceContext.ClearRenderTargetView(
                m_backBufferView,
                new RawColor4((float)(col.R / 255f), (float)(col.G / 255f), (float)(col.B / 255f), 1)); //new RawColor4(0.4f, 0.74f, 0.86f, 1));

            m_deviceContext.ClearDepthStencilView(m_depthStencilView, D3D11.DepthStencilClearFlags.Depth, 1f, 0);

            m_deviceContext.OutputMerger.SetRenderTargets(m_depthStencilView, m_backBufferView);


            m_deviceContext.UpdateSubresource(
              ref m_viewConstants,
              m_viewConstantsBuffer);

            lock (m_bufferMap)
            {
                foreach (var entry in m_bufferMap)
                {
                    m_deviceContext.VertexShader.SetConstantBuffer(1, entry.Value.constantsBuffer);

                    m_deviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(entry.Value.vertexBuffer, (int)entry.Value.vertexStride, 0));
                    m_deviceContext.InputAssembler.SetIndexBuffer(entry.Value.indexBuffer, (DXGI.Format)entry.Value.indexFormat, 0);

                    m_deviceContext.DrawIndexed(entry.Value.indexCount, 0, 0);

                }
            }
            m_swapChain.Present(0, DXGI.PresentFlags.None);
        }


        void CreateCube(Vector3 _v)
        {
            Engine_Obj obj = new Engine_Obj();
            float hs = 0.5f;
            obj.Vertices = new List<Vertex>{
                // Front Face
                new Vertex(-hs, -hs, -hs, 0, 1, 0, 0, 1),	//Bottom	Left
			    new Vertex(-hs, +hs, -hs, 0, 0, 0, 0, 1),	//Top		Left
			    new Vertex(+hs, +hs, -hs, 1, 0, 0, 0, 1),	//Top		Right
			    new Vertex(+hs, -hs, -hs, 1, 1, 0, 0, 1),	//Bottom	Right

			    // Left Face
			    new Vertex(-hs, -hs, +hs, 0, 1, 1, 0, 0),	//Bottom	Left
			    new Vertex(-hs, +hs, +hs, 0, 0, 1, 0, 0),	//Top		Left
			    new Vertex(-hs, +hs, -hs, 1, 0, 1, 0, 0),	//Top		Right
			    new Vertex(-hs, -hs, -hs, 1, 1, 1, 0, 0),	//Bottom	Right

			    // Back Face
			    new Vertex(+hs, -hs, +hs, 0, 1, 0, 0, -1),	//Bottom	Left
			    new Vertex(+hs, +hs, +hs, 0, 0, 0, 0, -1),	//Top		Left
			    new Vertex(-hs, +hs, +hs, 1, 0, 0, 0, -1),	//Top		Right
			    new Vertex(-hs, -hs, +hs, 1, 1, 0, 0, -1),	//Bottom	Right

			    // Right Face
			    new Vertex(+hs, -hs, -hs, 0, 1, -1, 0, 0),	//Bottom	Left
			    new Vertex(+hs, +hs, -hs, 0, 0, -1, 0, 0),	//Top		Left
			    new Vertex(+hs, +hs, +hs, 1, 0, -1, 0, 0),	//Top		Right
			    new Vertex(+hs, -hs, +hs, 1, 1, -1, 0, 0),	//Bottom	Right

			    // Top Face
			    new Vertex(-hs, +hs, -hs, 0, 0, 0, -1, 0),	//Top		Right
			    new Vertex(-hs, +hs, +hs, 1, 0, 0, -1, 0),	//Bottom	Right
			    new Vertex(+hs, +hs, +hs, 1, 1, 0, -1, 0),	//Bottom	Left
			    new Vertex(+hs, +hs, -hs, 0, 1, 0, -1, 0),	//Top		Left

			    // Base Face					 	
			    new Vertex(-hs, -hs, +hs, 1, 1, 0, 1, 0),	//Bottom	Left
			    new Vertex(-hs, -hs, -hs, 0, 1, 0, 1, 0),	//Top		Left
			    new Vertex(+hs, -hs, -hs, 0, 0, 0, 1, 0),	//Top		Right
			    new Vertex(+hs, -hs, +hs, 1, 0, 0, 1, 0)};	//Bottom	Right

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

            obj.VertexStride = (uint)Utilities.SizeOf<Vertex>();
            obj.IndexStride = sizeof(int);



            var bufferInfo = new Engine_MeshBufferInfo();

            unsafe
            {
                fixed (Vertex* p = obj.Vertices.ToArray())
                {
                    IntPtr ptr = (IntPtr)p;
                    int byteWidth = (int)(obj.Vertices.Count * obj.VertexStride);
                    bufferInfo.vertexBuffer = new D3D11.Buffer(
                      m_device,
                      ptr,
                      new D3D11.BufferDescription(
                          byteWidth,
                          D3D11.BindFlags.VertexBuffer,
                          D3D11.ResourceUsage.Default));
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
                      new D3D11.BufferDescription(
                          byteWidth,
                          D3D11.BindFlags.IndexBuffer,
                          D3D11.ResourceUsage.Default));
                }
            }
            bufferInfo.indexFormat = Windows.Graphics.DirectX.DirectXPixelFormat.R16UInt;
            bufferInfo.indexCount = obj.Indices.Count;


            Matrix scale = Matrix.Scaling(new Vector3(1, 1, 1));
            Matrix rotation = Matrix.RotationQuaternion(new Quaternion(0, 0, 0, 1));
            Matrix translation = Matrix.Translation(_v);

            var worldMatrix = Matrix.Transpose(scale * rotation * translation);
            PerModelConstantBuffer cb = new PerModelConstantBuffer() { World = worldMatrix };

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
            if (!m_IsDXInitialized)
                return;


            var newSize = new Size2((int)e.NewSize.Width, (int)e.NewSize.Height);

            Utilities.Dispose(ref m_backBufferView);
            Utilities.Dispose(ref m_backBufferTexture);
            Utilities.Dispose(ref m_depthStencilView);
            Utilities.Dispose(ref m_depthStencilTexture);

            m_swapChain.ResizeBuffers(
              m_swapChain.Description.BufferCount,
              (int)e.NewSize.Width,
              (int)e.NewSize.Height,
              m_swapChain.Description1.Format,
              m_swapChain.Description1.Flags);

            m_backBufferTexture = D3D11.Resource.FromSwapChain<D3D11.Texture2D>(m_swapChain, 0);
            m_backBufferView = new D3D11.RenderTargetView(m_device, m_backBufferTexture);

            m_depthStencilTextureDescription.Width = (int)e.NewSize.Width;
            m_depthStencilTextureDescription.Height = (int)e.NewSize.Height;
            using (m_depthStencilTexture = new D3D11.Texture2D(m_device, m_depthStencilTextureDescription))
                m_depthStencilView = new D3D11.DepthStencilView(m_device, m_depthStencilTexture);


            m_swapChain.SourceSize = newSize;

            m_deviceContext.Rasterizer.SetViewport(0, 0, (int)e.NewSize.Width, (int)e.NewSize.Height);
        }
    }
}

