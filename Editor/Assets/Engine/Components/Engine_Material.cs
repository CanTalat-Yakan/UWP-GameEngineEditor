using Editor.Assets.Engine.Data;
using Editor.Assets.Engine.Utilities;
using SharpDX.D3DCompiler;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using Editor.Assets.Engine.Helper;

namespace Editor.Assets.Engine.Components
{
    class Engine_Material
    {
        Engine_Renderer m_d3d;

        D3D11.VertexShader m_vertexShader;
        D3D11.PixelShader m_pixelShader;
        D3D11.GeometryShader m_geometryShader;

        D3D11.InputLayout m_inputLayout;

        D3D11.ShaderResourceView m_resourceView;
        D3D11.SamplerState m_sampler;
        D3D11.Buffer m_model;

        internal Engine_Material(string _shaderFileName, string _imageFileName, bool _includeGeometryShader = false)
        {
            #region //Get Instances
            m_d3d = Engine_Renderer.Instance;
            #endregion

            #region //Create InputLayout
            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32B32_Float, 0, 0),
                new D3D11.InputElement("TEXCOORD", 0, DXGI.Format.R32G32_Float, D3D11.InputElement.AppendAligned, 0),
                new D3D11.InputElement("NORMAL", 0, DXGI.Format.R32G32B32_Float, D3D11.InputElement.AppendAligned, 0)};
            #endregion

            #region //Create VertexShader
            CompilationResult vsResult;
            using (vsResult = ShaderBytecode.CompileFromFile(_shaderFileName, "VS", "vs_4_0", ShaderFlags.None))
            {
                m_vertexShader = new D3D11.VertexShader(m_d3d.m_device, vsResult.Bytecode.Data);
                m_inputLayout = new D3D11.InputLayout(m_d3d.m_device, vsResult.Bytecode, inputElements);
            }
            #endregion

            #region //Create PixelShader 
            using (var psResult = ShaderBytecode.CompileFromFile(_shaderFileName, "PS", "ps_4_0", ShaderFlags.None))
                m_pixelShader = new D3D11.PixelShader(m_d3d.m_device, psResult.Bytecode.Data);
            #endregion

            #region //Create GeometryShader
            if (_includeGeometryShader)
                using (var psResult = ShaderBytecode.CompileFromFile(_shaderFileName, "GS", "gs_4_0", ShaderFlags.None))
                    m_geometryShader = new D3D11.GeometryShader(m_d3d.m_device, psResult.Bytecode.Data);
            #endregion

            #region //Create ConstantBuffers for Model
            SPerModelConstantBuffer cbModel = new SPerModelConstantBuffer();

            D3D11.BufferDescription bufferDescription = new D3D11.BufferDescription
            {
                BindFlags = D3D11.BindFlags.ConstantBuffer,
                CpuAccessFlags = D3D11.CpuAccessFlags.Write,
                Usage = D3D11.ResourceUsage.Dynamic,
            };

            m_model = D3D11.Buffer.Create(m_d3d.m_device, D3D11.BindFlags.ConstantBuffer, ref cbModel);
            #endregion

            #region //Create Texture and Sampler
            var texture = Engine_ImgLoader.CreateTexture2DFromBitmap(m_d3d.m_device, Engine_ImgLoader.LoadBitmap(new SharpDX.WIC.ImagingFactory2(), _imageFileName));
            m_resourceView = new D3D11.ShaderResourceView(m_d3d.m_device, texture);

            D3D11.SamplerStateDescription samplerStateDescription = new D3D11.SamplerStateDescription
            {
                Filter = D3D11.Filter.Anisotropic,
                AddressU = D3D11.TextureAddressMode.Clamp,
                AddressV = D3D11.TextureAddressMode.Clamp,
                AddressW = D3D11.TextureAddressMode.Clamp,
                ComparisonFunction = D3D11.Comparison.Always,
                MaximumAnisotropy = 16,
                MinimumLod = 0,
                MaximumLod = float.MaxValue,
            };

            m_sampler = new D3D11.SamplerState(m_d3d.m_device, samplerStateDescription);
            #endregion
        }

        internal void Render(SPerModelConstantBuffer _data)
        {
            m_d3d.m_deviceContext.InputAssembler.InputLayout = m_inputLayout;
            m_d3d.m_deviceContext.VertexShader.Set(m_vertexShader);
            m_d3d.m_deviceContext.PixelShader.Set(m_pixelShader);
            m_d3d.m_deviceContext.GeometryShader.Set(m_geometryShader);

            m_d3d.m_deviceContext.UpdateSubresource(ref _data, m_model);
            m_d3d.m_deviceContext.VertexShader.SetConstantBuffer(1, m_model);

            m_d3d.m_deviceContext.PixelShader.SetShaderResource(0, m_resourceView);
            m_d3d.m_deviceContext.PixelShader.SetSampler(0, m_sampler);
        }
    }
}
