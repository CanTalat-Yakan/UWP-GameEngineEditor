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
        Control_Hierarchy hierarchy;

        public View_Hierarchy()
        {
            this.InitializeComponent();
            Scene scene = new Scene();
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Core" });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Content" });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Manager" });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Camera", id_parent = scene.m_Hierarchy[0].id });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Directional Light", id_parent = scene.m_Hierarchy[0].id });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Player", id_parent = scene.m_Hierarchy[1].id });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "VCamera", id_parent = scene.m_Hierarchy[5].id });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "Controller", id_parent = scene.m_Hierarchy[5].id });
            scene.m_Hierarchy.Add(new SGameObject() { id = 1, name = "GameManager", id_parent = scene.m_Hierarchy[2].id });
            hierarchy = new Control_Hierarchy(x_TreeView_Hierarchy, scene);
        }
    }
}
