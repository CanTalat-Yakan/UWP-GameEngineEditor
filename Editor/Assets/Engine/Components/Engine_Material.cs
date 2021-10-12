using Editor.Assets.Engine.Data;
using Editor.Assets.Engine.Utilities;
using SharpDX.D3DCompiler;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

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
        D3D11.Buffer m_view;

        internal Engine_Material(string _shaderFileName, bool _includeGeometryShader = false)
        {
            #region //Get Instances
            m_d3d = Engine_Renderer.Instance;
            #endregion

            #region //Create VertexShader
            CompilationResult vsResult;
            using (vsResult = ShaderBytecode.CompileFromFile(_shaderFileName, "VS", "vs_4_0", ShaderFlags.None))
            {
                m_vertexShader = new D3D11.VertexShader(m_d3d.m_device, vsResult.Bytecode.Data);
            }

            m_d3d.m_deviceContext.VertexShader.Set(m_vertexShader);
            #endregion

            #region //Create InputLayout
            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32B32_Float, 0, 0),
                new D3D11.InputElement("TEXCOORD", 0, DXGI.Format.R32G32_Float, D3D11.InputElement.AppendAligned, 0),
                new D3D11.InputElement("NORMAL", 0, DXGI.Format.R32G32B32_Float, D3D11.InputElement.AppendAligned, 0)};

            m_inputLayout = new D3D11.InputLayout(m_d3d.m_device, vsResult.Bytecode, inputElements);
            m_d3d.m_deviceContext.InputAssembler.InputLayout = m_inputLayout;
            #endregion

            #region //CreatePixelShader 
            using (var psResult = ShaderBytecode.CompileFromFile(
            _shaderFileName, "PS", "ps_4_0", ShaderFlags.None))
            {
                m_pixelShader = new D3D11.PixelShader(m_d3d.m_device, psResult.Bytecode.Data);
            }

            m_d3d.m_deviceContext.PixelShader.Set(m_pixelShader);
            #endregion

            #region //Create GeometryShader
            if (_includeGeometryShader)
            {
                using (var psResult = ShaderBytecode.CompileFromFile(
                _shaderFileName, "GS", "gs_4_0", ShaderFlags.None))
                {
                    m_geometryShader = new D3D11.GeometryShader(m_d3d.m_device, psResult.Bytecode.Data);
                }
                m_d3d.m_deviceContext.GeometryShader.Set(m_geometryShader);
            }
            #endregion

            #region //Create ConstantBuffers
            CreateBufferModel();
            CreateBufferView();
            #endregion
        }

        internal void Render(SPerModelConstantBuffer _data, SViewConstantsBuffer _data2)
        {
            m_d3d.m_deviceContext.UpdateSubresource(ref _data, m_model);
            m_d3d.m_deviceContext.UpdateSubresource(ref _data2, m_view);
        }

        void CreateBufferModel()
        {
            SPerModelConstantBuffer cbModel = new SPerModelConstantBuffer();

            D3D11.BufferDescription bufferDescription = new D3D11.BufferDescription
            {
                BindFlags = D3D11.BindFlags.ConstantBuffer,
                CpuAccessFlags = D3D11.CpuAccessFlags.Write,
                Usage = D3D11.ResourceUsage.Dynamic,
            };
            m_model = D3D11.Buffer.Create(m_d3d.m_device, ref cbModel, bufferDescription);
            m_d3d.m_deviceContext.VertexShader.SetConstantBuffer(0, m_model);
        }
        void CreateBufferView()
        {
            SViewConstantsBuffer cbView = new SViewConstantsBuffer();

            D3D11.BufferDescription bufferDescription = new D3D11.BufferDescription
            {
                BindFlags = D3D11.BindFlags.ConstantBuffer,
                CpuAccessFlags = D3D11.CpuAccessFlags.Write,
                Usage = D3D11.ResourceUsage.Dynamic,
            };
            m_view = D3D11.Buffer.Create(m_d3d.m_device, ref cbView, bufferDescription);
            m_d3d.m_deviceContext.VertexShader.SetConstantBuffer(1, m_view);
        }

        void CreateTextureAndSampler()
        {
            D3D11.SamplerStateDescription samplerStateDescription = new D3D11.SamplerStateDescription
            {
                Filter = D3D11.Filter.Anisotropic,
                AddressU = D3D11.TextureAddressMode.Wrap,
                AddressV = D3D11.TextureAddressMode.Wrap,
                AddressW = D3D11.TextureAddressMode.Wrap,
                ComparisonFunction = D3D11.Comparison.Always,
                MaximumAnisotropy = 16,
                MinimumLod = 0,
                MaximumLod = float.MaxValue,
            };
            m_sampler = new D3D11.SamplerState(m_d3d.m_device, samplerStateDescription);

            m_d3d.m_deviceContext.PixelShader.SetShaderResource(0, m_resourceView);
            m_d3d.m_deviceContext.PixelShader.SetSampler(0, m_sampler);
        }
    }
}
