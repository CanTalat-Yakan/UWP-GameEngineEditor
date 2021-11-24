using Editor.Assets.Engine.Utilities;
using System;
using System.Collections.Generic;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewNode = Microsoft.UI.Xaml.Controls.TreeViewNode;

namespace Editor.Assets.Control
{
    class TreeEntry
    {
        public Guid ID;
        public Guid? IDparent;
        public string Name;
        public Engine.Utilities.Engine_Object Object;
        public Microsoft.UI.Xaml.Controls.TreeViewNode Node;
    }
    class TreeEntryCollection
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
        internal TreeEntryCollection m_collection;
        internal MyList<Engine_Object> m_engineObjectsList;

        public Control_Hierarchy(View_Main _main, TreeView _tree)
        {
            m_tree = _tree;
            m_collection = new TreeEntryCollection();

            m_engineObjectsList = _main.m_Layout.m_ViewPort.m_renderer.m_scene.m_objectManager.m_list;
            m_engineObjectsList.OnAdd += (object sender, EventArgs e) => { Initialize(); };

            Initialize();
        }

        void Initialize()
        {
            foreach (var item in m_engineObjectsList)
                m_collection.m_Hierarchy.Add(
                    new TreeEntry()
                    {
                        Object = item,
                        Name = item.m_name,
                        ID = item.ID,
                        Node = new TreeViewNode()
                        {
                            Content = item.m_name,
                            IsExpanded = true
                        }
                    });

            foreach (var entry in m_collection.m_Hierarchy)
                if (entry.Object.m_parent != null)
                    entry.IDparent = entry.Object.m_parent.ID;

            List<TreeViewNode> hierarchy = new List<TreeViewNode>();
            foreach (var entry in m_collection.m_Hierarchy)
                if (entry.IDparent is null)
                    hierarchy.Add(RecursiveGetChildren(m_collection, entry).Node);

            foreach (var entry in hierarchy)
                m_tree.RootNodes.Add(entry);
        }

        void list_OnAdd(object sender, EventArgs e)
        {
            Initialize();
        }

        TreeEntry RecursiveGetChildren(TreeEntryCollection _collection, TreeEntry _input)
        {
            foreach (var item in _collection.m_Hierarchy)
                if (item.IDparent == _input.ID)
                    _input.Node.Children.Add(RecursiveGetChildren(_collection, item).Node);

            return _input;
        }
    }
}
