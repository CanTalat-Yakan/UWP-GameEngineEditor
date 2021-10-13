using System;
using System.Collections.Generic;
using System.Threading;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Vector2 = SharpDX.Vector2;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Input
    {
        public static Engine_Input Instance { get; private set; }

        enum Input_State { DOWN, UP, PRESSED }
        Dictionary<VirtualKey, bool> m_virtualKeyDic = new Dictionary<VirtualKey, bool>();
        Windows.UI.Input.PointerPoint m_pointer;
        Vector2 m_mouseAxis = Vector2.Zero;
        Windows.Foundation.Point m_tmpPoint = new Windows.Foundation.Point();
        Windows.Foundation.Point m_pointerPosition = new Windows.Foundation.Point();

        public Engine_Input()
        {
            #region //Set Instance
            Instance = this;
            #endregion
        }

        public void Update()
        {
            #region //Get PointerDelta
            m_pointerPosition = CoreWindow.GetForCurrentThread().PointerPosition;

            m_mouseAxis.X = (float)(m_pointerPosition.X - m_tmpPoint.X);
            m_mouseAxis.Y = (float)(m_pointerPosition.Y - m_tmpPoint.Y);

            m_tmpPoint = m_pointerPosition;
            #endregion
        }

        public bool GetKey(VirtualKey _key) { return false; }
        public Vector2 GetMouseAxis() { return m_mouseAxis; }

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

        internal void PointerPressed(object sender, PointerRoutedEventArgs e)
        {
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
