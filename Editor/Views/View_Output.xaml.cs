using Editor.Assets;
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
        internal Control_Output m_control;

        View_Main m_main;

        public View_Output(View_Main _main, TextBlock _status)
        {
            this.InitializeComponent();

            m_main = _main;

            m_control = new Control_Output(_main, _status, x_Stackpanel_Output, x_ScrollViewer_Output, x_AppBarToggleButton_Output_Collapse, x_AppBarToggleButton_Filter_Messages, x_AppBarToggleButton_Filter_Warnings, x_AppBarToggleButton_Filter_Errors, x_AppBarToggleButton_Debug_ErrorPause, x_AppBarToggleButton_Debug_ClearPlay);

            DispatcherTimer dispatcher = new DispatcherTimer();
            dispatcher.Interval = TimeSpan.FromMilliseconds(42);
            dispatcher.Tick += Tick;
            //dispatcher.Start();
            DispatcherTimer dispatcherSec = new DispatcherTimer();
            dispatcherSec.Interval = TimeSpan.FromSeconds(1);
            dispatcherSec.Tick += TickSec;
            dispatcherSec.Start();
        }

        void Tick(object sender, object e)
        {
            if (m_main.m_GameMode.m_playMode == EGameMode.PLAYING)
                Update();
        }
        void TickSec(object sender, object e)
        {
            if (m_main.m_GameMode.m_playMode == EGameMode.PLAYING)
                UpdateSec();
        }

        void Update()
        {
            m_control.Log("Updated Frame..");
        }
        void UpdateSec()
        {
            ExampleSkriptDebugTest();
        }

        void ExampleSkriptDebugTest()
        {
            Random rnd = new Random();
            int i = rnd.Next(0, 24);

            m_control.Log(i.ToString());
            if (i < 5)
                m_control.Log("Error Example!", EMessageType.ERROR);
            if (i < 10 && i > 5)
                m_control.Log("A Warning.", EMessageType.WARNING);
            if (i < 15)
                m_control.Log("This is a Message");
            if (i > 15)
                Test();
        }

        void Test()
        {
            m_control.Log("Test");
        }

        private void AppBarButton_Output_Clear(object sender, RoutedEventArgs e) { m_control.ClearOutput(); }
        private void AppBarToggleButton_Output_Collapse_Click(object sender, RoutedEventArgs e) { m_control.IterateOutputMessages(); }
        private void AppBarToggleButton_Filter_Click(object sender, RoutedEventArgs e) { m_control.IterateOutputMessages(); }
        private void AppBarToggleButton_Debug_ErrorPause_Click(object sender, RoutedEventArgs e) { }
        private void AppBarToggleButton_Debug_ClearPlay_Click(object sender, RoutedEventArgs e) { }
    }
}
