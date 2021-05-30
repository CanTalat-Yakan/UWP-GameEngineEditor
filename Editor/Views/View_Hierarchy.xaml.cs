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

        public View_Hierarchy(View_Main _main)
        {
            this.InitializeComponent();
            CScene scene = new CScene();
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Core" });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Content" });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Manager" });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Camera", ID_parent = scene.m_Hierarchy[0].ID });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Directional Light", ID_parent = scene.m_Hierarchy[0].ID });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Player", ID_parent = scene.m_Hierarchy[1].ID });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "VCamera", ID_parent = scene.m_Hierarchy[5].ID });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "Controller", ID_parent = scene.m_Hierarchy[5].ID });
            scene.m_Hierarchy.Add(new CGameObject() { Name = "GameManager", ID_parent = scene.m_Hierarchy[2].ID });
            hierarchy = new Control_Hierarchy(x_TreeView_Hierarchy, scene);
        }
    }
}
