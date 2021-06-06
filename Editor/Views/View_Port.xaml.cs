using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SharpDX;
using SharpDX.Direct3D11;
//using SharpDX.Toolkit.Graphics;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

using System.ComponentModel;
using System.Numerics;
using Windows.Perception.Spatial;
using Windows.Perception.Spatial.Surfaces;
using Windows.UI.Popups;
using Editor.Assets.Engine;
using Vector3 = System.Numerics.Vector3;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Editor
{
    public sealed partial class View_Port : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        SwapChainPanelRenderer m_swapChainRenderer;

        public View_Port(View_Main _main)
        {
            InitializeComponent();
            Loaded += Init;
        }

        void Init(object sender, RoutedEventArgs e)
        {
            m_swapChainRenderer = new SwapChainPanelRenderer(this.x_SwapChainPanel_ViewPort);
            m_swapChainRenderer.Initialise();
        }

        void FirePropertyChanged([CallerMemberName] string memberName = null) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName)); }
    }
}
