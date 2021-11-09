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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Editor
{
    public sealed partial class View_Hierarchy : UserControl
    {
        View_Main m_main;
        Control_Hierarchy hierarchy;
        Microsoft.UI.Xaml.Controls.TreeView m_treeView;

        public View_Hierarchy(View_Main _main)
        {
            this.InitializeComponent();
            m_main = _main;
            m_treeView = x_TreeView_Hierarchy;

            m_main.Loaded += Initialize0;
        }


        void Initialize0(object sender, RoutedEventArgs e)
        {
            m_main.m_Layout.m_ViewPort.Loaded += Initialize;
        }

        void Initialize(object sender, RoutedEventArgs e)
        {
            var engineObjectList = m_main.m_Layout.m_ViewPort.m_renderer.m_scene.m_objectManager.m_list;
            engineObjectList.OnAdd += list_OnAdd;

            CScene scene = new CScene();
            foreach (var item in engineObjectList)
            {
                var newEntry = new TreeEntry() { Object = item, Name = item.m_name, ID = item.ID };
                if (item.m_parent != null)
                    newEntry.IDparent = item.m_parent.ID;

                scene.m_Hierarchy.Add(newEntry);
            }

            hierarchy = new Control_Hierarchy(x_TreeView_Hierarchy, scene);
        }

        void list_OnAdd(object sender, EventArgs e)
        {
            var engineObjectList = m_main.m_Layout.m_ViewPort.m_renderer.m_scene.m_objectManager.m_list;

            CScene scene = new CScene();

            foreach (var item in engineObjectList)
            {
                var newEntry = new TreeEntry() { Object = item, Name = item.m_name, ID = item.ID };
                if (item.m_parent != null)
                    newEntry.IDparent = item.m_parent.ID;

                scene.m_Hierarchy.Add(newEntry);
            }

            hierarchy = new Control_Hierarchy(x_TreeView_Hierarchy, scene);
        }
    }
}
