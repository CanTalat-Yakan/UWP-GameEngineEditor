using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewNode = Microsoft.UI.Xaml.Controls.TreeViewNode;

namespace Editor.Assets.Control
{
    public class PathDisplay
    {
        public string DisplayText { get; set; }
        public string Path { get; set; }
        public PathDisplay() { }
        public PathDisplay(string d, string p)
        {
            DisplayText = d;
            Path = p;
        }
        public override string ToString()
        {
            return DisplayText;
        }
    }
    class Control_TreeView
    {
        internal string[] GetRelativePaths(string[] _list, string _relativeTo)
        {
            string[] list = new string[_list.Length];

            for (int i = 0; i < _list.Length; i++)
                list[i] = Path.GetRelativePath(_relativeTo, _list[i]);

            return list;
        }

        internal void PopulateTreeView(TreeView treeView, string[] paths, char pathSeparator)
        {
            TreeViewNode lastNode = null;
            string subPathAgg;
            long count = 0;

            foreach (string path in paths)
            {
                subPathAgg = string.Empty;
                foreach (string subPath in path.Split(pathSeparator))
                {
                    subPathAgg += subPath + pathSeparator;
                    var displayModel = new PathDisplay(subPath, subPathAgg);
                    TreeViewNode[] nodes = GetSameNodes(treeView.RootNodes, subPathAgg).ToArray();
                    if (nodes.Length == 0)
                    {
                        if (lastNode == null)
                        {
                            lastNode = new TreeViewNode() { Content = displayModel, IsExpanded = true };
                            treeView.RootNodes.Add(lastNode);
                        }
                        else
                        {
                            var node = new TreeViewNode() { Content = displayModel };
                            lastNode.Children.Add(node);
                            lastNode = node;
                        }
                        count++;
                    }
                    else
                    {
                        lastNode = nodes[0];
                    }
                }
                lastNode = null;
            }
        }

        private IEnumerable<TreeViewNode> GetSameNodes(IList<TreeViewNode> _nodes, string _path)
        {
            foreach (var node in _nodes)
            {
                var content = node.Content as PathDisplay;

                if (content?.Path == _path)
                    yield return node;
                else
                {
                    if (node.Children.Count > 0)
                    {
                        foreach (var item in GetSameNodes(node.Children, _path))
                            yield return item;
                    }
                }
            }
        }

    }
}
