using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;

namespace Editor.Assets.Control
{
    class Control_Files
    {
        Control_TreeView m_control = new Control_TreeView();

        internal TreeView m_tree;
        internal BreadcrumbBar m_breadcrumbar;

        public Control_Files(TreeView _tree, BreadcrumbBar _bar)
        {
            m_tree = _tree;
            m_breadcrumbar = _bar;

            m_control.PopulateTreeView(
                    m_tree,
                    m_control.GetRelativePaths(
                        Directory.GetFiles(
                            Directory.GetCurrentDirectory()
                            + "\\Assets"),
                        Directory.GetCurrentDirectory())
                    , '\\');

            m_breadcrumbar.ItemsSource = Directory.GetFiles(
                            Directory.GetCurrentDirectory()
                            + "\\Assets");
        }
    }
}
