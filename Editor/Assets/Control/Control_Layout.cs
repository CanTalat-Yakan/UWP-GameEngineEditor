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
        public double MinHeight = 40;
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

            m_GridContent = CreateLayoutNew(
                WrapGrid(m_ViewPort = _port),
                WrapInTabView(new TabViewItemDataTemplate() { Header = "Output", Content = m_ViewOutput = _output, Symbol = Symbol.Message },
                    new TabViewItemDataTemplate() { Header = "Files", Content = m_ViewFiles = _files, Symbol = Symbol.Document }),
                WrapGrid(m_ViewHierarchy = _hierarchy),
                WrapGrid(m_ViewProperties = _properties));

            //m_GridContent = CreateLayout1(
            //    WrapInTabView(new TabViewItemDataTemplate() { Header = "Viewport", Content = m_ViewPort = _port, Symbol = Symbol.View },
            //         new TabViewItemDataTemplate() { Header = "Settings", Content = m_ViewSettings = _settings, Symbol = Symbol.Setting }),
            //    WrapInTabView(new TabViewItemDataTemplate() { Header = "Output", Content = m_ViewOutput = _output, Symbol = Symbol.Message }),
            //    WrapInTabView(new TabViewItemDataTemplate() { Header = "Scene", Content = m_ViewHierarchy = _hierarchy, Symbol = Symbol.List }),
            //    WrapInTabView(new TabViewItemDataTemplate() { Header = "Files", Content = m_ViewFiles = _files, Symbol = Symbol.Document }),
            //    WrapInTabView(new TabViewItemDataTemplate() { Header = "Properties", Content = m_ViewProperties = _properties, Symbol = Symbol.Edit }));

            //m_GridContent = CreateLayout2(
            //    Wrap(new TabViewItemDataTemplate() { Header = "Scene", Content = m_ViewHierarchy = _hierarchy, Symbol = Symbol.List }),
            //    Wrap(new TabViewItemDataTemplate() { Header = "Viewport", Content = m_ViewPort = _port, Symbol = Symbol.View },
            //         new TabViewItemDataTemplate() { Header = "Settings", Content = m_ViewSettings = _settings, Symbol = Symbol.Setting }),
            //    Wrap(new TabViewItemDataTemplate() { Header = "Output", Content = m_ViewOutput = _output, Symbol = Symbol.Message },
            //         new TabViewItemDataTemplate() { Header = "Files", Content = m_ViewFiles = _files, Symbol = Symbol.Document }),
            //    Wrap(new TabViewItemDataTemplate() { Header = "Properties", Content = m_ViewProperties = _properties, Symbol = Symbol.Edit }));


            _content.Children.Add(m_GridContent);
        }

        Grid CreateLayout1(params Grid[] _panel)
        {
            var a = PairVertical(
                new GridDataTemeplate() { Content = _panel[0], MinHeight = 44 },
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
        Grid CreateLayoutNew(params Grid[] _panel)
        {
            var a = PairVertical(
                new GridDataTemeplate() { Content = _panel[0], MinHeight = 0, Length = new GridLength(5, GridUnitType.Star) },
                new GridDataTemeplate() { Content = _panel[1], MinHeight = 0 });

            var b = PairVertical(
                new GridDataTemeplate() { Content = _panel[2], MinHeight = 0 },
                new GridDataTemeplate() { Content = _panel[3], MinHeight = 0, Length = new GridLength(5, GridUnitType.Star) });

            return WrapSplitView(a, b);
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
                CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover,
                ResizeBehavior = GridSplitter.GridResizeBehavior.BasedOnAlignment
            };

            grid.Children.Add(_left.Content);
            grid.Children.Add(_right.Content);
            Grid.SetColumn((FrameworkElement)_right.Content, 1);
            grid.Children.Add(splitH);


            return grid;
        }
        Grid PairHorizontal(GridDataTemeplate _left, GridDataTemeplate _center, GridDataTemeplate _right)
        {
            Grid grid = new Grid() { ColumnSpacing = 16 };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = _left.Length, MinWidth = _left.MinWidth });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = _center.Length, MinWidth = _center.MinWidth });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = _right.Length, MinWidth = _right.MinWidth });

            GridSplitter splitH = new GridSplitter()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, -16, 0),
                Opacity = 0.5f,
                CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover,
                ResizeBehavior = GridSplitter.GridResizeBehavior.CurrentAndNext,
            };
            GridSplitter splitH2 = new GridSplitter()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, -16, 0),
                Opacity = 0.5f,
                CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover,
                ResizeBehavior = GridSplitter.GridResizeBehavior.PreviousAndNext,
            };
            Grid.SetColumn(splitH2, 1);

            grid.Children.Add(_left.Content);
            grid.Children.Add(_center.Content);
            Grid.SetColumn((FrameworkElement)_center.Content, 1);
            grid.Children.Add(_right.Content);
            Grid.SetColumn((FrameworkElement)_right.Content, 2);
            grid.Children.Add(splitH);
            grid.Children.Add(splitH2);


            return grid;
        }
        Grid PairVertical(GridDataTemeplate _top, GridDataTemeplate _bottom)
        {
            Grid grid = new Grid() { };
            grid.RowDefinitions.Add(new RowDefinition() { Height = _top.Length, MinHeight = _top.MinHeight });
            grid.RowDefinitions.Add(new RowDefinition() { Height = _bottom.Length, MinHeight = _bottom.MinHeight });

            GridSplitter splitV = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Opacity = 0.5f, CursorBehavior = GridSplitter.SplitterCursorBehavior.ChangeOnGripperHover };

            ((Grid)_top.Content).Margin = new Thickness(0, 0, 0, 16);

            grid.Children.Add(_top.Content);
            grid.Children.Add(_bottom.Content);
            Grid.SetRow((FrameworkElement)_bottom.Content, 1);
            grid.Children.Add(splitV);


            return grid;
        }

        Grid WrapInTabView(params TabViewItemDataTemplate[] _i)
        {
            Grid grid = new Grid();
            Control_TabViewPage tabViewPage = new Control_TabViewPage(m_main, _i);
            grid.Children.Add(tabViewPage.m_TabView);

            return grid;
        }
        Grid WrapSplitView(Grid _content, Grid _pane)
        {
            _content.Background = new SolidColorBrush((Windows.UI.Color)Application.Current.Resources["SystemChromeAltHighColor"]);
            Grid grid = new Grid();
            SplitView split = new SplitView() { IsPaneOpen = true, DisplayMode = SplitViewDisplayMode.CompactInline, PanePlacement = SplitViewPanePlacement.Right, Pane = _pane, Content = _content };
            grid.Children.Add(split);

            return grid;
        }
        Grid WrapGrid(UIElement _content)
        {
            Grid grid = new Grid();
            grid.Children.Add(_content);

            return grid;
        }
    }
}