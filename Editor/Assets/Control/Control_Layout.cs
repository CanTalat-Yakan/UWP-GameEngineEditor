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
    class GridDataTemeplate
    {
        public GridLength Length = new GridLength(1, GridUnitType.Star);
        public double MinWidth = 1;
        public double MinHeight = 1;
        public UIElement Content;
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

            m_GridContent = CreateLayout1(
                Wrap(new TabViewItemDataTemplate() { Header = "Viewport", Content = m_ViewPort = _port, Symbol = Symbol.View },
                     new TabViewItemDataTemplate() { Header = "Settings", Content = m_ViewSettings = _settings, Symbol = Symbol.Setting }),
                Wrap(new TabViewItemDataTemplate() { Header = "Output", Content = m_ViewOutput = _output, Symbol = Symbol.Message }),
                Wrap(new TabViewItemDataTemplate() { Header = "Scene", Content = m_ViewHierarchy = _hierarchy, Symbol = Symbol.List }),
                Wrap(new TabViewItemDataTemplate() { Header = "Files", Content = m_ViewFiles = _files, Symbol = Symbol.Document }),
                Wrap(new TabViewItemDataTemplate() { Header = "Properties", Content = m_ViewProperties = _properties, Symbol = Symbol.Edit }));

            //m_GridContent = CreateLayout2(
            //    Wrap(new TabViewItemDataTemplate() { Header = "Scene", Content = m_ViewHierarchy = _hierarchy, Symbol = Symbol.List }),
            //    Wrap(new TabViewItemDataTemplate() { Header = "Viewport", Content = m_ViewPort = _port, Symbol = Symbol.View },
            //         new TabViewItemDataTemplate() { Header = "Settings", Content = m_ViewSettings = _settings, Symbol = Symbol.Setting }),
            //    Wrap(new TabViewItemDataTemplate() { Header = "Output", Content = m_ViewOutput = _output, Symbol = Symbol.Message },
            //         new TabViewItemDataTemplate() { Header = "Files", Content = m_ViewFiles = _files, Symbol = Symbol.Document }),
            //    Wrap(new TabViewItemDataTemplate() { Header = "Properties", Content = m_ViewProperties = _properties, Symbol = Symbol.Edit }));


            _content.Children.Add(m_GridContent);
        }

        Grid CreateLayout2(params Grid[] _panel)
        {
            var a = PairHorizontal(
                new GridDataTemeplate() { Content = _panel[0], Length = new GridLength(190, GridUnitType.Pixel) },
                new GridDataTemeplate() { Content = _panel[1] });

            var b = PairVertical(
                new GridDataTemeplate() { Content = a },
                new GridDataTemeplate() { Content = _panel[2], Length = new GridLength(200, GridUnitType.Pixel) });

            var c = PairHorizontal(
                new GridDataTemeplate() { Content = b },
                new GridDataTemeplate() { Content = _panel[3], Length = new GridLength(310, GridUnitType.Pixel) });

            return c;
        }
        Grid CreateLayout1(params Grid[] _panel)
        {
            var a = PairVertical(
                new GridDataTemeplate() { Content = _panel[0] },
                new GridDataTemeplate() { Content = _panel[1], Length = new GridLength(200, GridUnitType.Pixel) });

            var b = PairVertical(
                new GridDataTemeplate() { Content = _panel[2] },
                new GridDataTemeplate() { Content = _panel[3] });

            var c = PairHorizontal(
                new GridDataTemeplate() { Content = a },
                new GridDataTemeplate() { Content = b, Length = new GridLength(190, GridUnitType.Pixel) },
                new GridDataTemeplate() { Content = _panel[4], Length = new GridLength(310, GridUnitType.Pixel) });

            return c;
        }


        Grid PairHorizontal(GridDataTemeplate _left, GridDataTemeplate _right)
        {
            Grid grid = new Grid() { ColumnSpacing = 16 };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = _left.Length, MinWidth = _left.MinWidth });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = _right.Length, MinWidth = _right.MinWidth });

            GridSplitter splitH = new GridSplitter()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, -16, 0),
                Opacity = 0.5f,
                CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover
            };

            grid.Children.Add(_left.Content);
            grid.Children.Add(_right.Content);
            Grid.SetColumn((FrameworkElement)_right.Content, 1);
            grid.Children.Add(splitH);


            return grid;
        }
        Grid PairHorizontal(GridDataTemeplate _left, GridDataTemeplate _center, GridDataTemeplate _right)
        {
            var a = PairHorizontal(_left, _center);
            var b = PairHorizontal(new GridDataTemeplate() { Content = a }, _right);

            return b;
        }
        Grid PairVertical(GridDataTemeplate _top, GridDataTemeplate _bottom)
        {
            Grid grid = new Grid() { RowSpacing = 16 };
            grid.RowDefinitions.Add(new RowDefinition() { Height = _top.Length, MinHeight = _top.MinHeight });
            grid.RowDefinitions.Add(new RowDefinition() { Height = _bottom.Length, MinHeight = _bottom.MinHeight });

            GridSplitter splitV = new GridSplitter()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, -16),
                Opacity = 0.5f,
                CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover
            };

            grid.Children.Add(_top.Content);
            grid.Children.Add(_bottom.Content);
            Grid.SetRow((FrameworkElement)_bottom.Content, 1);
            grid.Children.Add(splitV);


            return grid;
        }

        Grid Wrap(params TabViewItemDataTemplate[] _i)
        {
            Grid grid = new Grid();
            Control_TabViewPage tabViewPage = new Control_TabViewPage(m_main, _i);
            grid.Children.Add(tabViewPage.m_TabView);

            return grid;
        }
    }
}