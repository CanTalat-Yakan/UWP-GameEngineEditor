using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;

namespace Editor.Assets.Control
{
    class TreeEntry
    {
        public Guid ID;
        public Guid? IDparent;
        public string Name;
        public Engine.Utilities.Engine_Object Object;
    }
    class CScene
    {
        public List<TreeEntry> m_Hierarchy = new List<TreeEntry>();
        public string[] ToStringArray()
        {
            string[] s = new string[m_Hierarchy.Count];

            for (int i = 0; i < m_Hierarchy.Count; i++)
                s[i] = GetParents(m_Hierarchy[i], m_Hierarchy[i].Name, '/');

            return s;
        }

        string GetParents(TreeEntry _current, string _path, char _pathSeperator)
        {
            if (_current.IDparent != null)
                foreach (var item in m_Hierarchy)
                    if (item.ID == _current.IDparent)
                        _path = GetParents(
                            item,
                            item.Name + _pathSeperator + _path,
                            _pathSeperator);

            return _path;
        }
    }
    class Control_Hierarchy
    {
        Control_TreeView m_control = new Control_TreeView();

        internal TreeView m_tree;
        internal CScene m_scene;

        public Control_Hierarchy(TreeView _tree, CScene _scene)
        {
            m_tree = _tree;
            m_scene = _scene;

            Initialize();
        }

        void Initialize()
        {
            m_control.PopulateTreeView(m_tree, m_scene.ToStringArray(), '/');
        }
    }
}
