using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Editor.Assets.Control
{
    internal class Control_Layout
    {
        View_Main m_main;

        internal Grid m_Content;
        internal Grid m_SwapChain;
        internal Grid m_Output;
        internal Grid m_Hierarchy;
        internal Grid m_Files;
        internal Grid m_Properties;

        public Control_Layout(View_Main main, Grid content, View_Output output, View_Hierarchy hierarchy)
        {
            m_main = main;

            m_Content = CreateLayout();

            m_Output.Children.Add(output);
            m_Hierarchy.Children.Add(hierarchy);

            content.Children.Add(m_Content);
        }

        Grid CreateLayout()
        {
            GridSplitter splitH = new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0) };
            Grid.SetColumn(splitH, 0);
            GridSplitter splitH2 = new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0) };
            Grid.SetColumn(splitH2, 1);
            GridSplitter splitV = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, -16) };
            Grid.SetColumn(splitV, 0);
            GridSplitter splitV2 = new GridSplitter() { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, -16) };
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
            collumn2.Children.Add(new GridSplitter() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, -16, 0) });


            collumn0.Children.Add(m_SwapChain = new Grid());
            collumn0.Children.Add(m_Output = new Grid());
            Grid.SetRow(m_Output, 1);
            collumn0.Children.Add(splitV);
            grid.Children.Add(collumn0);

            collumn1.Children.Add(m_Hierarchy = new Grid());
            collumn1.Children.Add(m_Files = new Grid());
            Grid.SetRow(m_Files, 1);
            collumn1.Children.Add(splitV2);
            grid.Children.Add(collumn1);

            collumn2.Children.Add(m_Properties = new Grid());
            grid.Children.Add(collumn2);

            grid.Children.Add(splitH);
            grid.Children.Add(splitH2);


            return grid;
        }
    }
}
