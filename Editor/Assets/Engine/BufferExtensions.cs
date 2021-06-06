namespace Editor.Assets.Engine
{
    using System;
    using Windows.Storage.Streams;


    public static class BufferExtensions
    {
        public unsafe static IntPtr AsIntPtr(this IBuffer buffer, out int size)
        {
            byte* bufferPointer = null;
            size = (int)buffer.Length;
            ((IBufferByteAccess)buffer).Buffer(out bufferPointer);
            return (new IntPtr(bufferPointer));
        }
    }

    [System.Runtime.InteropServices.GuidAttribute(
        "905a0fef-bc53-11df-8c49-001e4fc686da"),
     System.Runtime.InteropServices.InterfaceType(
        System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
    interface IBufferByteAccess
    {
        unsafe void Buffer(out byte* pByte);
    }
}
