using SharpDX;
using SharpDX.Mathematics.Interop;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Renderer
    {
        public static Engine_Renderer Instance { get; private set; }

        internal string m_profile;

        internal D3D11.Device2 m_device;
        internal D3D11.DeviceContext m_deviceContext;
        internal DXGI.SwapChain2 m_swapChain;
        internal SwapChainPanel m_swapChainPanel;

        D3D11.Texture2D m_backBufferTexture;
        D3D11.RenderTargetView m_backBufferView;
        D3D11.Texture2D m_depthStencilTexture;
        D3D11.Texture2DDescription m_depthStencilTextureDescription;
        D3D11.DepthStencilView m_depthStencilView;
        D3D11.BlendState m_blendState;


        internal Engine_Renderer(SwapChainPanel _swapChainPanel)
        {
            #region //Create Instance
            Instance = this;
            #endregion

            #region //Set SwapChainPanel and SizeChanger
            m_swapChainPanel = _swapChainPanel;
            m_swapChainPanel.SizeChanged += OnSwapChainPanelSizeChanged;
            #endregion

            #region //Create Buffer Description for swapChain description
            var swapChainDescription = new DXGI.SwapChainDescription1()
            {
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
            #endregion

            #region //Create device, device context & swap chain
            using (var defaultDevice = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.Debug))
            {
                m_device = defaultDevice.QueryInterface<D3D11.Device2>();
                m_deviceContext = m_device.ImmediateContext2;
            }

            using (var dxgiDevice3 = m_device.QueryInterface<DXGI.Device3>())
            using (var dxgiFactory3 = dxgiDevice3.Adapter.GetParent<DXGI.Factory3>())
            {
                var swapChain1 = new DXGI.SwapChain1(dxgiFactory3, m_device, ref swapChainDescription);
                m_swapChain = swapChain1.QueryInterface<DXGI.SwapChain2>();
            }
            using (var nativeObject = ComObject.As<DXGI.ISwapChainPanelNative>(m_swapChainPanel))
                nativeObject.SwapChain = m_swapChain;
            #endregion

            #region //Create render target view, get back buffer texture before
            m_backBufferTexture = m_swapChain.GetBackBuffer<D3D11.Texture2D>(0);
            m_backBufferView = new D3D11.RenderTargetView(m_device, m_backBufferTexture);
            #endregion

            #region //Create depth stencil view
            m_depthStencilTextureDescription = new D3D11.Texture2DDescription
            {
                Format = DXGI.Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 0,
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

            #endregion

            #region //Create rasterizer state
            var rasterizerDesc = new D3D11.RasterizerStateDescription()
            {
                FillMode = D3D11.FillMode.Solid,
                CullMode = D3D11.CullMode.Back,
                IsAntialiasedLineEnabled = true,
                IsMultisampleEnabled = true,
            };

            m_deviceContext.Rasterizer.State = new D3D11.RasterizerState(m_device, rasterizerDesc);
            #endregion

            #region //Create Blend State
            D3D11.BlendStateDescription m_blendStateDesc = new D3D11.BlendStateDescription();
            var blendDesc = new D3D11.RenderTargetBlendDescription()
            {
                IsBlendEnabled = true,
                SourceBlend = D3D11.BlendOption.SourceAlpha,
                DestinationBlend = D3D11.BlendOption.InverseSourceAlpha,
                BlendOperation = D3D11.BlendOperation.Add,
                SourceAlphaBlend = D3D11.BlendOption.One,
                DestinationAlphaBlend = D3D11.BlendOption.Zero,
                AlphaBlendOperation = D3D11.BlendOperation.Add,
                RenderTargetWriteMask = D3D11.ColorWriteMaskFlags.All
            };

            m_blendStateDesc.RenderTarget[0] = blendDesc;
            m_blendState = m_deviceContext.OutputMerger.BlendState = new D3D11.BlendState(m_device, m_blendStateDesc);
            #endregion

            #region //Set ViewPort
            m_deviceContext.Rasterizer.SetViewport(0, 0, (int)m_swapChainPanel.ActualWidth, (int)m_swapChainPanel.ActualHeight);
            #endregion
        }

        internal void Clear()
        {
            //Clear back buffer with solid color
            var col = new RawColor4(0.15f, 0.15f, 0.15f, 1);
            m_deviceContext.ClearRenderTargetView(m_backBufferView, col);
            m_deviceContext.ClearDepthStencilView(m_depthStencilView, D3D11.DepthStencilClearFlags.Depth, 1f, 0);
            m_deviceContext.OutputMerger.SetRenderTargets(m_depthStencilView, m_backBufferView);
        }

        internal void Present()
        {
            m_swapChain.Present(0, DXGI.PresentFlags.None);
        }

        internal void RenderMesh(D3D11.Buffer _vertexBuffer, int _vertexStride, D3D11.Buffer _indexBuffer, int _indexCount)
        {
            m_deviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(_vertexBuffer, _vertexStride, 0));
            m_deviceContext.InputAssembler.SetIndexBuffer(_indexBuffer, DXGI.Format.R16_UInt, 0);
            m_deviceContext.OutputMerger.SetBlendState(m_blendState);
            m_deviceContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
            m_deviceContext.DrawIndexed(_indexCount, 0, 0);
        }

        internal void OnSwapChainPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newSize = new Size2((int)e.NewSize.Width, (int)e.NewSize.Height);

            SharpDX.Utilities.Dispose(ref m_backBufferView);
            SharpDX.Utilities.Dispose(ref m_backBufferTexture);
            SharpDX.Utilities.Dispose(ref m_depthStencilView);
            SharpDX.Utilities.Dispose(ref m_depthStencilTexture);

            m_profile = e.NewSize.Width.ToString() + "x" + e.NewSize.Height.ToString();

            m_swapChain.ResizeBuffers(
              m_swapChain.Description.BufferCount,
              Math.Max(1, (int)e.NewSize.Width),
              Math.Max(1, (int)e.NewSize.Height),
              m_swapChain.Description1.Format,
              m_swapChain.Description1.Flags);

            m_backBufferTexture = D3D11.Resource.FromSwapChain<D3D11.Texture2D>(m_swapChain, 0);
            m_backBufferView = new D3D11.RenderTargetView(m_device, m_backBufferTexture);

            m_depthStencilTextureDescription.Width = Math.Max(1, (int)e.NewSize.Width);
            m_depthStencilTextureDescription.Height = Math.Max(1, (int)e.NewSize.Height);
            using (m_depthStencilTexture = new D3D11.Texture2D(m_device, m_depthStencilTextureDescription))
                m_depthStencilView = new D3D11.DepthStencilView(m_device, m_depthStencilTexture);


            m_swapChain.SourceSize = newSize;

            m_deviceContext.Rasterizer.SetViewport(0, 0, (int)e.NewSize.Width, (int)e.NewSize.Height);
        }
    }
}
