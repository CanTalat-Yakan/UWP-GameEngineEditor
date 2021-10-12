using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11 = SharpDX.Direct3D11;
using WIC = SharpDX.WIC;
using DXGI = SharpDX.DXGI;

namespace Editor.Assets.Engine.Helper
{
    class Engine_ImgLoader
    {
        public static WIC.BitmapSource LoadBitmap(WIC.ImagingFactory2 factory, string filename)
        {
            var bitmapDecoder = new WIC.BitmapDecoder(
                factory,
                filename,
                WIC.DecodeOptions.CacheOnDemand
                );

            var formatConverter = new WIC.FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                WIC.PixelFormat.Format32bppPRGBA,
                WIC.BitmapDitherType.None,
                null,
                0.0,
                WIC.BitmapPaletteType.Custom);

            return formatConverter;
        }

        public static D3D11.Texture2D CreateTexture2DFromBitmap(D3D11.Device device, WIC.BitmapSource bitmapSource)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(stride, buffer);
                return new D3D11.Texture2D(device, new D3D11.Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = D3D11.BindFlags.ShaderResource,
                    Usage = D3D11.ResourceUsage.Immutable,
                    CpuAccessFlags = D3D11.CpuAccessFlags.None,
                    Format = DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = D3D11.ResourceOptionFlags.None,
                    SampleDescription = new DXGI.SampleDescription(1, 0),
                }, new SharpDX.DataRectangle(buffer.DataPointer, stride));
            }
        }
    }
}
