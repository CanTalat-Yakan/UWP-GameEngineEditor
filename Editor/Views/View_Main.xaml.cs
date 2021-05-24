﻿using System;
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
using Editor.Assets.Control;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Editor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View_Main : Page
    {
        Control_GameMode m_gameMode;
        Control_Layout m_layout;

        public View_Main()
        {
            this.InitializeComponent();

            m_gameMode = new Control_GameMode(
                this,
                x_AppBarToggleButton_Status_Play,
                x_AppBarToggleButton_Status_Pause,
                x_AppBarButton_Status_Forward,
                x_AppBarButton_Status_Repeat,
                x_AppBarButton_Status_Kill,
                x_TextBlock_Status_Content);

            m_layout = new Control_Layout(
                this, 
                x_Grid_Main, 
                new View_Output(
                    this, 
                    x_TextBlock_Status_Content), 
                new View_Hierarchy(), 
                new View_Files(), 
                new View_Properties(), 
                new View_Settings());
            //output.Children.Add(m_output = new View_Output(this, x_TextBlock_Status_Content));
            //hierarchy.Children.Add(m_hierarchy = new View_Hierarchy());
            //x_Grid_Properties.Children.Add(m_output = new View_Output(this, x_TextBlock_Status_Content));
        }

        private void AppBarToggleButton_Status_Play_Click(object sender, RoutedEventArgs e) { m_gameMode.Play(); }
        private void AppBarToggleButton_Status_Pause_Click(object sender, RoutedEventArgs e) { m_gameMode.Pause(); }
        private void AppBarButton_Status_Forward_Click(object sender, RoutedEventArgs e) { m_gameMode.Forward(); }
        private void AppBarButton_Status_Repeat_Click(object sender, RoutedEventArgs e) { m_gameMode.Repeat(); }
        private void AppBarButton_Status_Kill_Click(object sender, RoutedEventArgs e) { m_gameMode.Kill(); }
        private void AppBarToggleButton_Status_Light(object sender, RoutedEventArgs e) { this.RequestedTheme = this.RequestedTheme == ElementTheme.Light ? ElementTheme.Dark : this.RequestedTheme = ElementTheme.Light; }

    }
}