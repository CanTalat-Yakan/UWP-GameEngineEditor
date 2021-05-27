using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Assets.Control
{
    struct SGameObject
    {
        string name;
    }
    struct SScene
    {
        Dictionary<SGameObject, List<SGameObject>> hierarchy;
        public string[] ToStringArray()
        {
            string[] s = new string[3];


            return s;
        }
    }
    class Control_Hierarchy
    {
        Control_TreeView m_control;
        internal TreeView m_tree;
        internal SScene m_scene;

        public Control_Hierarchy(TreeView _tree, SScene _scene)
        {
            m_tree = _tree;
            m_scene = _scene;
        }

        void Initialize()
        {
            m_control.PopulateTreeView(m_tree, m_scene.ToStringArray(), '-');
        }
    }
}
