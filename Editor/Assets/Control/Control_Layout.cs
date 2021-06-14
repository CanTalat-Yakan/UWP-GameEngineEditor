using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Editor.Assets.Control
{
    internal struct STabItem
    {
        public string Header;
        public object Content;
        public Symbol Symbol;
    }
    internal class Control_Layout
    {
        View_Main m_main;

        internal Grid m_GridContent;

        internal View_Port m_ViewPort;
        internal View_Settings m_ViewSettings;
        internal View_Output m_ViewOutput;
        internal View_Hierarchy m_ViewHierarchy;
        internal View_Files m_ViewFiles;
        internal View_Properties m_ViewProperties;

        public Control_Layout(View_Main _main, Grid _content, View_Output _output, View_Hierarchy _hierarchy, View_Files _files, View_Properties _properties, View_Settings _settings, View_Port _port)
        {
            m_main = _main;

            m_GridContent = CreateLayout(
                Wrap(new STabItem() { Header = "Viewport", Content = m_ViewPort = _port, Symbol = Symbol.View },
                     new STabItem() { Header = "Settings", Content = m_ViewSettings = _settings, Symbol = Symbol.Setting }),
                Wrap(new STabItem() { Header = "Output", Content = m_ViewOutput = _output, Symbol = Symbol.Message }),
                Wrap(new STabItem() { Header = "Scene", Content = m_ViewHierarchy = _hierarchy, Symbol = Symbol.List }),
                Wrap(new STabItem() { Header = "Files", Content = m_ViewFiles = _files, Symbol = Symbol.Document }),
                Wrap(new STabItem() { Header = "Properties", Content = m_ViewProperties = _properties, Symbol = Symbol.Edit }));

            _content.Children.Add(m_GridContent);
        }

        Grid CreateLayout(params Grid[] _panel)
        {
            GridSplitter splitH = new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0), Opacity = 0.5f,CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover };
            Grid.SetColumn(splitH, 0);
            GridSplitter splitH2 = new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0), Opacity = 0.5f, CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover };
            Grid.SetColumn(splitH2, 1);
            GridSplitter splitV = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, -16), Opacity = 0.5f, CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover };
            Grid.SetColumn(splitV, 0);
            GridSplitter splitV2 = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, -16), Opacity = 0.5f,  CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover };
            Grid.SetColumn(splitV2, 1);

            Grid grid = new Grid() { ColumnSpacing = 16 };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(190, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(310, GridUnitType.Pixel) });


            Grid collumn0 = new Grid() { RowSpacing = 16 };
            collumn0.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star), MinHeight = 40 });
            collumn0.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(200, GridUnitType.Pixel), MinHeight = 40 });

            Grid collumn1 = new Grid() { RowSpacing = 16 };
            Grid.SetColumn(collumn1, 1);
            collumn1.RowDefinitions.Add(new RowDefinition() { MinHeight = 40 });
            collumn1.RowDefinitions.Add(new RowDefinition() { MinHeight = 40 });


            Grid collumn2 = new Grid() { RowSpacing = 16 };
            Grid.SetColumn(collumn2, 2);


            //SwapChain
            collumn0.Children.Add(_panel[0]);
            //Output
            collumn0.Children.Add(_panel[1]);
            Grid.SetRow(_panel[1], 1);
            collumn0.Children.Add(splitV);
            grid.Children.Add(collumn0);

            //Hierarchy
            collumn1.Children.Add(_panel[2]);
            //Files
            collumn1.Children.Add(_panel[3]);
            Grid.SetRow(_panel[3], 1);
            collumn1.Children.Add(splitV2);
            grid.Children.Add(collumn1);

            //Properties
            collumn2.Children.Add(_panel[4]);
            grid.Children.Add(collumn2);

            grid.Children.Add(splitH);
            grid.Children.Add(splitH2);


            return grid;
        }

        Grid Wrap(params STabItem[] _i)
        {
            Grid grid = new Grid();
            TabView tabView = new TabView() { CloseButtonOverlayMode = TabViewCloseButtonOverlayMode.Auto, TabWidthMode = TabViewWidthMode.Compact };
            foreach (STabItem p in _i)
            {
                TabViewItem item = new TabViewItem() { Header = p.Header, Content = p.Content, IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = p.Symbol } };
                tabView.TabItems.Add(item);
            }
            grid.Children.Add(tabView);

            return grid;
        }
    }
}