using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Primitives;
using Editor.Assets.Engine;
using System.ComponentModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Editor
{
    public sealed partial class View_Port : Page
    {
        internal Engine_Main m_renderer;
        internal View_Main m_main;

        internal TextBlock m_debugProfiling;
        internal Grid m_borderBrush;

        public View_Port(View_Main _main)
        {
            this.InitializeComponent();
            m_main = _main;
            m_debugProfiling = x_TextBlock_Debug_FPS;
            m_borderBrush = x_Grid_ViewPort_BorderBrush;

            Loaded += Initialize;
        }

        void Initialize(object sender, RoutedEventArgs e)
        {
            m_renderer = new Engine_Main(x_SwapChainPanel_ViewPort, m_debugProfiling);

            PointerPressed += m_renderer.m_input.PointerPressed;
            PointerWheelChanged += m_renderer.m_input.PointerWheelChanged;
            Window.Current.CoreWindow.PointerReleased += m_renderer.m_input.PointerReleased;
            Window.Current.CoreWindow.PointerMoved += m_renderer.m_input.PointerMoved;
            Window.Current.CoreWindow.KeyDown += m_renderer.m_input.KeyDown;
            Window.Current.CoreWindow.KeyUp += m_renderer.m_input.KeyUp;
            //Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
        }

        void Slider_FOV_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) { Assets.Engine.Components.Engine_Camera.FOV = e.NewValue; }
        void NumberBox_Speed_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args) { Assets.Engine.Editor.Engine_CameraController.m_transformSpeed = (float)args.NewValue; }
    }
}
