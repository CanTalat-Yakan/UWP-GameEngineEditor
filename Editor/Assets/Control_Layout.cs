using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Editor.Assets.Control
{
    internal class Control_Layout
    {
        View_Main m_main;

        internal Grid m_Content;
        internal Grid m_SwapChain = new Grid();
        internal Grid m_Output = new Grid();
        internal Grid m_Hierarchy = new Grid();
        internal Grid m_Files = new Grid();
        internal Grid m_Properties = new Grid();

        public Control_Layout(View_Main _main, Grid _content, View_Output _output, View_Hierarchy _hierarchy, View_Files _files, View_Properties _properties, View_Settings _settings)
        {
            m_main = _main;

            m_Content = CreateLayout();

            m_SwapChain.Children.Add(Wrap("View Port", new Grid(), _settings));
            m_Output.Children.Add(Wrap("Output", _output));
            m_Hierarchy.Children.Add(Wrap("Hierarchy", _hierarchy));
            m_Files.Children.Add(Wrap("Files", _files));
            m_Properties.Children.Add(Wrap("Properties", _properties));

            _content.Children.Add(m_Content);
        }

        Grid CreateLayout()
        {
            GridSplitter splitH = new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0), Opacity = 0.5f };
            Grid.SetColumn(splitH, 0);
            GridSplitter splitH2 = new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0), Opacity = 0.5f };
            Grid.SetColumn(splitH2, 1);
            GridSplitter splitV = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, -16), Opacity = 0.5f };
            Grid.SetColumn(splitV, 0);
            GridSplitter splitV2 = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, -16), Opacity = 0.5f };
            Grid.SetColumn(splitV2, 1);

            Grid grid = new Grid() { ColumnSpacing = 16 };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());


            Grid collumn0 = new Grid() { RowSpacing = 16 };
            collumn0.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            collumn0.RowDefinitions.Add(new RowDefinition());

            Grid collumn1 = new Grid() { RowSpacing = 16 };
            Grid.SetColumn(collumn1, 1);
            collumn1.RowDefinitions.Add(new RowDefinition());
            collumn1.RowDefinitions.Add(new RowDefinition());


            Grid collumn2 = new Grid() { RowSpacing = 16 };
            Grid.SetColumn(collumn2, 2);


            //SwapChain
            collumn0.Children.Add(m_SwapChain);
            //Output
            collumn0.Children.Add(m_Output);
            Grid.SetRow(m_Output, 1);
            collumn0.Children.Add(splitV);
            grid.Children.Add(collumn0);

            //Hierarchy
            collumn1.Children.Add(m_Hierarchy);
            //Files
            collumn1.Children.Add(m_Files);
            Grid.SetRow(m_Files, 1);
            collumn1.Children.Add(splitV2);
            grid.Children.Add(collumn1);

            //Properties
            collumn2.Children.Add(m_Properties);
            grid.Children.Add(collumn2);

            grid.Children.Add(splitH);
            grid.Children.Add(splitH2);


            return grid;
        }

        Grid Wrap(string _header, params object[] _panel)
        {
            Grid grid = new Grid();
            TabView tabView = new TabView() { CloseButtonOverlayMode = TabViewCloseButtonOverlayMode.OnPointerOver };
            foreach (object p in _panel)
            {
                TabViewItem item = new TabViewItem() { Header = _header, Content = p };
                tabView.TabItems.Add(item);
            }
            grid.Children.Add(tabView);

            return grid;
        }
    }
}


/*                  <my:TabView Grid.Row="1" Margin="0,16,0,0" CloseButtonOverlayMode="OnPointerOver">
                        <my:TabViewItem Header="Output">*/
