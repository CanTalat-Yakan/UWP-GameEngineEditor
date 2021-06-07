using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
                Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint((Grid)sender);

                if (ptrPt.Properties.IsRightButtonPressed)
                    m_swapChainRenderer.m_isRightButtonPressed = true;
            }

            e.Handled = true;
        }
    }
}
