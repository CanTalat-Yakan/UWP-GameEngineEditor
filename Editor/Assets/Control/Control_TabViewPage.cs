using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace Editor.Assets.Control
{
    public class TabViewItemDataTemplate
    {
        public string Header;
        public object Content;
        public Symbol Symbol;
    }

    public sealed partial class Control_TabViewPage : Page
    {
        internal TabView m_TabView;
        View_Main m_main;

        internal Control_TabViewPage(View_Main _main, params TabViewItemDataTemplate[] _icollection)
        {
            m_main = _main;

            m_TabView = new TabView() { TabWidthMode = TabViewWidthMode.Compact, CloseButtonOverlayMode = TabViewCloseButtonOverlayMode.Auto, IsAddTabButtonVisible = false };
            m_TabView.AddTabButtonClick += TabView_AddButtonClick;
            //m_TabView.TabCloseRequested += TabView_TabCloseRequested;


            foreach (var item in _icollection)
                m_TabView.TabItems.Add(CreateNewTab(item));
        }


        void TabView_AddButtonClick(TabView sender, object args)
        {
            var item = new TabViewItemDataTemplate() { Header = "Viewport", Content = new View_Port(m_main), Symbol = Symbol.View };
            m_TabView.TabItems.Add(CreateNewTab(item));
        }
        void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        TabViewItem CreateNewTab(TabViewItemDataTemplate _i)
        {
            TabViewItem newItem = new TabViewItem
            {
                Header = _i.Header,
                Content = _i.Content,
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = _i.Symbol }
            };

            return newItem;
        }

        internal async void TabViewWindowingButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Window.Current.Content = new View_Output(m_main);
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }
    }
}
