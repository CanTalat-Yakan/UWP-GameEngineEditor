﻿using Editor.Assets;
using Editor.Assets.Control;
using Microsoft.Toolkit.Uwp.UI.Controls;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Editor
{
    public sealed partial class View_Output : UserControl
    {
        internal Control_Output m_output;

        public View_Output(View_Main _main, TextBlock _status)
        {
            this.InitializeComponent();

            m_output = new Control_Output(_status, x_Stackpanel_Output, x_ScrollViewer_Output, x_AppBarToggleButton_Output_Collapse, x_AppBarToggleButton_Filter_Messages, x_AppBarToggleButton_Filter_Warnings, x_AppBarToggleButton_Filter_Errors);

            m_output.Log("Hi");
            Test();
            Test();
            m_output.Log("Hi2", EMessageType.WARNING);
            m_output.Log("Hi3", EMessageType.ERROR);
        }

        void Test()
        {
            m_output.Log("Test");
        }

        private void AppBarButton_Output_Clear(object sender, RoutedEventArgs e) { m_output.ClearOutput(); }
        private void AppBarToggleButton_Output_Collapse_Click(object sender, RoutedEventArgs e) { m_output.IterateOutputMessages(); }
        private void AppBarToggleButton_Filter_Click(object sender, RoutedEventArgs e) { m_output.IterateOutputMessages(); }
        private void AppBarToggleButton_Debug_ErrorPause_Click(object sender, RoutedEventArgs e) { }
    }
}
