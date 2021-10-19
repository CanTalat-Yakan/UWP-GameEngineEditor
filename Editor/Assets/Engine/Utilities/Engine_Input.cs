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

        public enum Input_State { DOWN, PRESSED, UP }
        public enum Mouse_Input { IsLeftButtonPressed, IsRightButtonPressed, IsMiddleButtonPressed }

        Dictionary<VirtualKey, bool[]> m_virtualKeyDic = new Dictionary<VirtualKey, bool[]>();
        List<VirtualKey> m_bufferKeys = new List<VirtualKey>();


        Dictionary<Mouse_Input, bool[]> m_pointerPointDic = new Dictionary<Mouse_Input, bool[]>();
        List<Mouse_Input> m_bufferPoints = new List<Mouse_Input>();

        PointerPoint m_pointer;
        int m_mouseWheelDelta;
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
            if (m_pointer != null)
            {
                m_pointerPosition = m_pointer.Position;

                m_mouseAxis.X = (float)(m_tmpPoint.X - m_pointerPosition.X);
                m_mouseAxis.Y = (float)(m_tmpPoint.Y - m_pointerPosition.Y);

                m_tmpPoint = m_pointerPosition;

                if (m_pointer.Position.X == 0)
                {
                    CoreWindow.GetForCurrentThread().PointerPosition = new Point(Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Width, m_pointer.Position.Y);
                    m_pointerPosition = CoreWindow.GetForCurrentThread().PointerPosition;
                    m_tmpPoint = m_pointerPosition;
                }
                if (m_pointer.Position.X >= Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Width)
                {
                    CoreWindow.GetForCurrentThread().PointerPosition = new Point(0, m_pointer.Position.Y);
                    m_pointerPosition = CoreWindow.GetForCurrentThread().PointerPosition;
                    m_tmpPoint = m_pointerPosition;
                }
                if (m_pointer.Position.Y == 0)
                {
                    CoreWindow.GetForCurrentThread().PointerPosition = new Point(m_pointer.Position.X, Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Height);
                    m_pointerPosition = CoreWindow.GetForCurrentThread().PointerPosition;
                    m_tmpPoint = m_pointerPosition;
                }
                if (m_pointer.Position.Y >= Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Height)
                {
                    CoreWindow.GetForCurrentThread().PointerPosition = new Point(m_pointer.Position.Y, 0);
                    m_pointerPosition = CoreWindow.GetForCurrentThread().PointerPosition;
                    m_tmpPoint = m_pointerPosition;
                }
            }
        }
        public void LateUpdate()
        {
            foreach (var item in m_bufferKeys)
            {
                m_virtualKeyDic[item][(int)Input_State.DOWN] = false;
                m_virtualKeyDic[item][(int)Input_State.UP] = false;
            }
            foreach (var item in m_bufferPoints)
            {
                m_pointerPointDic[item][(int)Input_State.DOWN] = false;
                m_pointerPointDic[item][(int)Input_State.UP] = false;
            }

            m_bufferKeys.Clear();
            m_bufferPoints.Clear();

            m_mouseWheelDelta = 0;
        }


        public bool GetKey(VirtualKey _key, Input_State _state = Input_State.PRESSED)
        {
            if (m_virtualKeyDic.ContainsKey(_key))
                return m_virtualKeyDic[_key][(int)_state];

            return false;
        }
        public bool GetButton(Mouse_Input _input, Input_State _state = Input_State.PRESSED)
        {
            if (m_pointerPointDic.ContainsKey(_input))
                return m_pointerPointDic[_input][(int)_state];

            return false;
        }
        public Vector2 GetMouseAxis() { return m_mouseAxis; }
        public int GetMouseWheel() { return m_mouseWheelDelta; }


        void SetKeyDic(VirtualKey _input, bool[] _newBool)
        {
            if (!m_virtualKeyDic.ContainsKey(_input))
                m_virtualKeyDic.Add(_input, _newBool);
            else
                m_virtualKeyDic[_input] = _newBool;

            m_bufferKeys.Add(_input);
        }
        internal void KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            var newBool = new bool[3];
            newBool[(int)Input_State.DOWN] = true;
            newBool[(int)Input_State.PRESSED] = true;
            newBool[(int)Input_State.UP] = false;

            SetKeyDic(e.VirtualKey, newBool);

            e.Handled = true;
        }
        internal void KeyUp(CoreWindow sender, KeyEventArgs e)
        {
            var newBool = new bool[3];
            newBool[(int)Input_State.DOWN] = false;
            newBool[(int)Input_State.PRESSED] = false;
            newBool[(int)Input_State.UP] = true;

            SetKeyDic(e.VirtualKey, newBool);

            e.Handled = true;
        }


        void SetPointerDic(Mouse_Input _input, bool[] _newBool)
        {
            if (!m_pointerPointDic.ContainsKey(_input))
                m_pointerPointDic.Add(_input, _newBool);
            else
                m_pointerPointDic[_input] = _newBool;

            m_bufferPoints.Add(_input);
        }
        internal void PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                m_pointer = e.GetCurrentPoint(null);

                var newBool = new bool[3];
                newBool[(int)Input_State.DOWN] = true;
                newBool[(int)Input_State.PRESSED] = true;
                newBool[(int)Input_State.UP] = false;

                if (m_pointer.Properties.IsLeftButtonPressed)
                    SetPointerDic(Mouse_Input.IsLeftButtonPressed, newBool);

                if (m_pointer.Properties.IsMiddleButtonPressed)
                    SetPointerDic(Mouse_Input.IsMiddleButtonPressed, newBool);

                if (m_pointer.Properties.IsRightButtonPressed)
                    SetPointerDic(Mouse_Input.IsRightButtonPressed, newBool);
            }

            e.Handled = true;
        }
        internal void PointerReleased(CoreWindow sender, PointerEventArgs e)
        {
            m_pointer = e.CurrentPoint;

            var newBool = new bool[3];
            newBool[(int)Input_State.DOWN] = false;
            newBool[(int)Input_State.PRESSED] = false;
            newBool[(int)Input_State.UP] = true;

            if (!m_pointer.Properties.IsLeftButtonPressed)
                SetPointerDic(Mouse_Input.IsLeftButtonPressed, newBool);

            if (!m_pointer.Properties.IsMiddleButtonPressed)
                SetPointerDic(Mouse_Input.IsMiddleButtonPressed, newBool);

            if (!m_pointer.Properties.IsRightButtonPressed)
                SetPointerDic(Mouse_Input.IsRightButtonPressed, newBool);

            e.Handled = true;
        }
        internal void PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                m_pointer = e.GetCurrentPoint(null);

                m_mouseWheelDelta = Math.Clamp(m_pointer.Properties.MouseWheelDelta, -1, 1);
            }

            e.Handled = true;
        }
        internal void PointerMoved(CoreWindow sender, PointerEventArgs e)
        {
            m_pointer = e.CurrentPoint;

            e.Handled = true;
        }
    }
}
