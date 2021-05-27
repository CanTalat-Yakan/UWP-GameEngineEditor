using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Editor.Assets.Control
{
    struct SGameObject
    {
        int m_id;
        public int id { get => m_id; set { m_id = new Random().Next(0, int.MaxValue); } }

        public int? id_parent;
        public string name;
        public string isActive;
        public string isStatic;
        public string tag;
        public string layer;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
    class Scene
    {
        public List<SGameObject> m_Hierarchy = new List<SGameObject>();
        public string[] ToStringArray()
        {
            string[] s = new string[m_Hierarchy.Count];

            for (int i = 0; i < m_Hierarchy.Count; i++)
                s[i] = GetParents(m_Hierarchy[i], m_Hierarchy[i].name, '/');

            return s;
        }

        string GetParents(SGameObject _current, string _path, char _pathSeperator)
        {
            if (_current.id_parent != null)
                foreach (SGameObject gameObject in m_Hierarchy)
                    if (gameObject.id == _current.id_parent)
                        _path = GetParents(
                            gameObject,
                            gameObject.name + _pathSeperator + _path,
                            _pathSeperator);

            return _path;
        }
    }
    class Control_Hierarchy
    {
        Control_TreeView m_control = new Control_TreeView();

        internal TreeView m_tree;
        internal Scene m_scene;

        public Control_Hierarchy(TreeView _tree, Scene _scene)
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
