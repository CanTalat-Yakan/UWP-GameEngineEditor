using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Editor.Assets.Control
{
    class CGameObject
    {
        public CGameObject()
        {
            ID = new Random().Next(0, int.MaxValue);
        }

        public int ID;
        public int? ID_parent;
        public string Name;
        public string IsActive;
        public string IsStatic;
        public string Tag;
        public string Layer;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }
    class CScene
    {
        public List<CGameObject> m_Hierarchy = new List<CGameObject>();
        public string[] ToStringArray()
        {
            string[] s = new string[m_Hierarchy.Count];

            for (int i = 0; i < m_Hierarchy.Count; i++)
                s[i] = GetParents(m_Hierarchy[i], m_Hierarchy[i].Name, '/');

            return s;
        }

        string GetParents(CGameObject _current, string _path, char _pathSeperator)
        {
            if (_current.ID_parent != null)
                foreach (CGameObject gameObject in m_Hierarchy)
                    if (gameObject.ID == _current.ID_parent)
                        _path = GetParents(
                            gameObject,
                            gameObject.Name + _pathSeperator + _path,
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
