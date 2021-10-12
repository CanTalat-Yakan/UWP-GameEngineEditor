using System;
using System.Collections.Generic;
using System.Threading;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Input
    {
        enum Input_State { DOWN, UP, PRESSED }
        Dictionary<VirtualKey, bool> m_virtualKeyDic = new Dictionary<VirtualKey, bool>();
        Windows.UI.Input.PointerPoint m_pointer;

        public bool GetKey(VirtualKey _key) { return m_virtualKeyDic[_key]; }

        internal void KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (!m_virtualKeyDic.ContainsKey(e.VirtualKey))
                m_virtualKeyDic.Add(e.VirtualKey, true);
            else
                m_virtualKeyDic[e.VirtualKey] = true;

            e.Handled = true;
        }
        internal void KeyUp(CoreWindow sender, KeyEventArgs e)
        {
            if (!m_virtualKeyDic.ContainsKey(e.VirtualKey))
                m_virtualKeyDic.Add(e.VirtualKey, false);
            else
                m_virtualKeyDic[e.VirtualKey] = false;

            e.Handled = true;
        }

        internal void PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                m_pointer = e.GetCurrentPoint(null);
                _ = m_pointer.Properties.IsRightButtonPressed;
            }

            e.Handled = true;
        }
        internal void PointerReleased(CoreWindow sender, PointerEventArgs e)
        {
            m_pointer = e.CurrentPoint;
            _ = m_pointer.Properties.IsRightButtonPressed;

            e.Handled = true;
        }
    }
}
