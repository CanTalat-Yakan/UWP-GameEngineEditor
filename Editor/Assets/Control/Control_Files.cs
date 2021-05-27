using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Editor.Assets.Control
{
    class Control_Files
    {
        Control_TreeView m_control = new Control_TreeView();

        internal TreeView m_tree;

        public Control_Files(TreeView _tree)
        {
            m_tree = _tree;

            m_control.PopulateTreeView(
                    m_tree,
                    m_control.GetRelativePaths(
                        Directory.GetFiles(
                            Directory.GetCurrentDirectory()
                            + "\\Assets"),
                        Directory.GetCurrentDirectory())
                    , '\\');
        }
    }
}
