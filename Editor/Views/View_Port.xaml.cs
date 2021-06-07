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
        internal SwapChainPanelRenderer m_swapChainRenderer;

        public View_Port(View_Main _main)
        {
            this.InitializeComponent();

            Loaded += SwapChain_Init;

            PointerMoved += ViewPort_PointerMoved;
            Window.Current.CoreWindow.KeyDown += Grid_KeyDown;
            Window.Current.CoreWindow.KeyUp += Grid_KeyUp;
        }

        void SwapChain_Init(object sender, RoutedEventArgs e)
        {
            m_swapChainRenderer = new SwapChainPanelRenderer(x_SwapChainPanel_ViewPort);
            m_swapChainRenderer.Initialise(this);
        }

        void ViewPort_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Pointer ptr = e.Pointer;

            if (ptr.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint((UIElement)sender);

                m_swapChainRenderer.m_IsRightButtonPressed = ptrPt.Properties.IsRightButtonPressed;
            }

            e.Handled = true;
        }
        void Grid_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.W) m_swapChainRenderer.m_W = true;
            if (e.VirtualKey == Windows.System.VirtualKey.S) m_swapChainRenderer.m_S = true;
            if (e.VirtualKey == Windows.System.VirtualKey.A) m_swapChainRenderer.m_A = true;
            if (e.VirtualKey == Windows.System.VirtualKey.D) m_swapChainRenderer.m_D = true;
            if (e.VirtualKey == Windows.System.VirtualKey.E) m_swapChainRenderer.m_E = true;
            if (e.VirtualKey == Windows.System.VirtualKey.Q) m_swapChainRenderer.m_Q = true;
        }
        void Grid_KeyUp(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.W) m_swapChainRenderer.m_W = false;
            if (e.VirtualKey == Windows.System.VirtualKey.S) m_swapChainRenderer.m_S = false;
            if (e.VirtualKey == Windows.System.VirtualKey.A) m_swapChainRenderer.m_A = false;
            if (e.VirtualKey == Windows.System.VirtualKey.D) m_swapChainRenderer.m_D = false;
            if (e.VirtualKey == Windows.System.VirtualKey.E) m_swapChainRenderer.m_E = false;
            if (e.VirtualKey == Windows.System.VirtualKey.Q) m_swapChainRenderer.m_Q = false;
        }

        void Slider_MouseSensitivity_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (m_swapChainRenderer != null)
                m_swapChainRenderer.m_MovementSpeed = (float)x_Slider_MouseSensitivity.Value * 0.01f;
        }
    }
}
