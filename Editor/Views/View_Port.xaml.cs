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
using Editor.Assets.Engine;
using System.ComponentModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Editor
{
    public sealed partial class View_Port : Page
    {
        internal Engine_Main m_swapChainRenderer;
        internal View_Main m_main;
        internal TextBlock m_debugFrames;
        internal Grid m_borderBrush;

        public View_Port(View_Main _main)
        {
            this.InitializeComponent();

            m_main = _main;
            m_debugFrames = x_TextBlock_Debug_FPS;
            m_borderBrush = x_Grid_ViewPort_BorderBrush;

            Loaded += SwapChain_Init;

            PointerMoved += ViewPort_PointerMoved;
            //PointerPressed += ViewPort_PointerPressed;
            Window.Current.CoreWindow.PointerReleased += ViewPort_PointerReleased;
            Window.Current.CoreWindow.KeyDown += Grid_KeyDown;
            Window.Current.CoreWindow.KeyUp += Grid_KeyUp;
        }

        void SwapChain_Init(object sender, RoutedEventArgs e)
        {
            m_swapChainRenderer = new Engine_Main(x_SwapChainPanel_ViewPort);
            m_swapChainRenderer.Initialise(this);
        }

        void ViewPort_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(null);
                m_swapChainRenderer.m_IsRightButtonPressed = ptrPt.Properties.IsRightButtonPressed;
            }

            e.Handled = true;
        }
        void ViewPort_PointerReleased(CoreWindow sender, PointerEventArgs e) { m_swapChainRenderer.m_IsRightButtonPressed = false; }
        void Grid_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.W) m_swapChainRenderer.m_W = true;
            if (e.VirtualKey == Windows.System.VirtualKey.S) m_swapChainRenderer.m_S = true;
            if (e.VirtualKey == Windows.System.VirtualKey.A) m_swapChainRenderer.m_A = true;
            if (e.VirtualKey == Windows.System.VirtualKey.D) m_swapChainRenderer.m_D = true;
            if (e.VirtualKey == Windows.System.VirtualKey.E) m_swapChainRenderer.m_E = true;
            if (e.VirtualKey == Windows.System.VirtualKey.Q) m_swapChainRenderer.m_Q = true;
            if (e.VirtualKey == Windows.System.VirtualKey.C) m_swapChainRenderer.m_C = true;
            if (e.VirtualKey == Windows.System.VirtualKey.Shift) m_swapChainRenderer.m_Shift = true;
            if (e.VirtualKey == Windows.System.VirtualKey.Control) m_swapChainRenderer.m_Ctrl = true;

            e.Handled = true;
        }
        void Grid_KeyUp(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.W) m_swapChainRenderer.m_W = false;
            if (e.VirtualKey == Windows.System.VirtualKey.S) m_swapChainRenderer.m_S = false;
            if (e.VirtualKey == Windows.System.VirtualKey.A) m_swapChainRenderer.m_A = false;
            if (e.VirtualKey == Windows.System.VirtualKey.D) m_swapChainRenderer.m_D = false;
            if (e.VirtualKey == Windows.System.VirtualKey.E) m_swapChainRenderer.m_E = false;
            if (e.VirtualKey == Windows.System.VirtualKey.Q) m_swapChainRenderer.m_Q = false;
            if (e.VirtualKey == Windows.System.VirtualKey.C) m_swapChainRenderer.m_C = false;
            if (e.VirtualKey == Windows.System.VirtualKey.Shift) m_swapChainRenderer.m_Shift = false;
            if (e.VirtualKey == Windows.System.VirtualKey.Control) m_swapChainRenderer.m_Ctrl = false;

            e.Handled = true;
        }
    }
}
