using Editor.Assets.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TreeViewNode = Microsoft.UI.Xaml.Controls.TreeViewNode;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Editor
{
    public sealed partial class View_Hierarchy : UserControl
    {
        View_Main m_main;

        public View_Hierarchy(View_Main _main)
        {
            this.InitializeComponent();
            m_main = _main;

            m_main.Loaded += 
                (object _sender, RoutedEventArgs _e) => 
                    m_main.m_Layout.m_ViewPort.Loaded += 
                        (object sender, RoutedEventArgs e) => 
                            new Control_Hierarchy(_main, x_TreeView_Hierarchy); 
        }
    }
}
