using System;
using System.Collections.Generic;
using System.Threading;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Vector2 = SharpDX.Vector2;
using Windows.Foundation;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Input
    {
        public static Engine_Input Instance { get; private set; }

        public enum Input_State { DOWN, UP, PRESSED }
        public enum Mouse_Input { IsLeftMouseButtonPressed, IsRightMouseButtonPressed, IsMiddleMouseButtonPressed }

        Dictionary<VirtualKey, bool[]> m_virtualKeyDic = new Dictionary<VirtualKey, bool[]>();
        List<VirtualKey> m_bufferKeys = new List<VirtualKey>();


        Dictionary<Mouse_Input, bool[]> m_pointerPointDic = new Dictionary<Mouse_Input, bool[]>();
        List<Mouse_Input> m_bufferPoints = new List<Mouse_Input>();

        PointerPoint m_pointer;

        Vector2 m_mouseAxis = Vector2.Zero;
        Point m_pointerPosition = new Point(), m_tmpPoint = new Point();


        public Engine_Input()
        {
            #region //Set Instance
            Instance = this;
            #endregion
        }

        public void Update()
        {
            m_pointerPosition = CoreWindow.GetForCurrentThread().PointerPosition;

            m_mouseAxis.X = (float)(m_pointerPosition.X - m_tmpPoint.X);
            m_mouseAxis.Y = (float)(m_pointerPosition.Y - m_tmpPoint.Y);

            m_tmpPoint = m_pointerPosition;
        }
        public void LateUpdate()
        {
            foreach (var item in m_bufferKeys)
            {
                m_virtualKeyDic[item][0] = false;
                m_virtualKeyDic[item][1] = false;
            }

            m_bufferKeys.Clear();
        }

        public bool GetKey(VirtualKey _key, Input_State _state = Input_State.PRESSED)
        {
            if (m_virtualKeyDic.ContainsKey(_key))
                return m_virtualKeyDic[_key][(int)_state];

            return false;
        }
        public bool GetButton(PointerPointProperties _point, Input_State _state = Input_State.PRESSED)
        {
            //if (m_pointerPointDic.ContainsKey(_point))
            //    return m_pointerPointDic[_point][(int)_state];

            return false;
        }
        public Vector2 GetMouseAxis() { return m_mouseAxis; }


        internal void KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (!m_virtualKeyDic.ContainsKey(e.VirtualKey))
                m_virtualKeyDic.Add(e.VirtualKey, new bool[3] { true, false, true });
            else
                m_virtualKeyDic[e.VirtualKey] = new bool[3] { true, false, true };

            m_bufferKeys.Add(e.VirtualKey);

            e.Handled = true;
        }
        internal void KeyUp(CoreWindow sender, KeyEventArgs e)
        {
            if (!m_virtualKeyDic.ContainsKey(e.VirtualKey))
                m_virtualKeyDic.Add(e.VirtualKey, new bool[3] { false, true, false });
            else
                m_virtualKeyDic[e.VirtualKey] = new bool[3] { false, true, false };

            m_bufferKeys.Add(e.VirtualKey);

            e.Handled = true;
        }


        internal void PointerPressed(object sender, PointerRoutedEventArgs e)
        {

            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                m_pointer = e.GetCurrentPoint(null);

                //if (!m_pointerPointDic.ContainsKey(m_pointer.Properties))
                //    m_pointerPointDic.Add(m_pointer.Properties, new bool[3] { true, false, true });
                //else
                //    m_pointerPointDic[m_pointer.Properties] = new bool[3] { true, false, true };
            }

            e.Handled = true;
        }

        internal void PointerReleased(CoreWindow sender, PointerEventArgs e)
        {
            m_pointer = e.CurrentPoint;

            //if (!m_pointerPointDic.ContainsKey(m_pointer.Properties))
            //    m_pointerPointDic.Add(m_pointer.Properties., new bool[3] { false, true, false });
            //else
            //    m_pointerPointDic[m_pointer.Properties] = new bool[3] { false, true, false };

            e.Handled = true;
        }

        internal void PointerWheelChanged(CoreWindow sender, PointerEventArgs e)
        {
            m_pointer = e.CurrentPoint;
            _ = m_pointer.Properties;

            e.Handled = true;
        }

    }
}
