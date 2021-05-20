using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Editor.Assets
{
    internal enum EMessageType
    {
        MESSAGE,
        WARNING,
        ERROR
    }
    internal struct SMessageInfo
    {
        public EMessageType Type;
        public string Script;
        public string Method;
        public int Line;
        public string Message;

        public string GetInfo() { return Script.Split("\\").Last() + $":{Line} ({Method})"; }
    }

    internal class Control_Output
    {
        Dictionary<SMessageInfo, List<DateTime>> m_messageCollection = new Dictionary<SMessageInfo, List<DateTime>>();

        TextBlock m_status;
        StackPanel m_stack;
        ScrollViewer m_scroll;
        AppBarToggleButton m_collapse;

        internal Control_Output(TextBlock status, StackPanel stack, ScrollViewer scroll, AppBarToggleButton collapse)
        {
            m_status = status;
            m_stack = stack;
            m_scroll = scroll; 
            m_collapse = collapse;
        }


        void SetStatus(SMessageInfo _m)
        {
            m_status.Text = _m.Message;
        }

        UIElement CreateMessage(DateTime _d, SMessageInfo _m, int _i = 0)
        {
            //Content of the message
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal, Spacing = 10, Margin = new Thickness(10, 0, 0, 0) };
            stack.Children.Add(new SymbolIcon() { Symbol = _m.Type == EMessageType.MESSAGE ? Symbol.Message : Symbol.ReportHacked });
            stack.Children.Add(new TextBlock() { Text = "[" + _d.TimeOfDay.ToString("hh\\:mm\\:ss").ToString() + "]" });
            stack.Children.Add(new TextBlock() { Text = _m.Message });

            //The flyout when clicked on the message
            StackPanel stackFlyout = new StackPanel() { Orientation = Orientation.Vertical };
            stackFlyout.Children.Add(new TextBlock() { Text = _m.GetInfo() + "\n" });
            stackFlyout.Children.Add(new MarkdownTextBlock() { Text = _m.Message, Padding = new Thickness(2) });
            stackFlyout.Children.Add(new HyperlinkButton() { Content = Path.GetRelativePath(Directory.GetCurrentDirectory(), _m.Script) + ":" + _m.Line, Foreground = new SolidColorBrush(Windows.UI.Colors.CadetBlue) });
            Flyout flyout = new Flyout() { OverlayInputPassThroughElement = stack, Content = stackFlyout };

            //Create main grid that gets returned
            Grid grid = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch };
            grid.Children.Add(stack);
            //If there is a count the number gets shown on the right
            if (_i != 0)
                grid.Children.Add(new TextBlock() { Margin = new Thickness(0, 0, 10, 0), MinWidth = 25, HorizontalAlignment = HorizontalAlignment.Right, Text = _i.ToString() });
            //Set flyout to button that stretches along the grid
            grid.Children.Add(new Button()
            {
                Flyout = flyout,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(
                    _m.Type == EMessageType.MESSAGE ?
                        m_stack.Children.Count() % 2 == 0 ?
                            Windows.UI.Colors.Transparent :
                            Windows.UI.Color.FromArgb(50, 50, 50, 50) :
                        _m.Type == EMessageType.ERROR ?
                            Windows.UI.Color.FromArgb(88, 255, 0, 0) :
                            Windows.UI.Color.FromArgb(88, 255, 255, 0))
            });

            return grid;
        }

        internal void Log(string _m, EMessageType _t = EMessageType.MESSAGE, [CallerLineNumber] int _l = 0, [CallerMemberName] string _c = null, [CallerFilePath] string _s = null)
        {
            SMessageInfo message = new SMessageInfo() { Script = _s, Method = _c, Line = _l, Message = _m, Type = _t };

            if (!m_messageCollection.ContainsKey(message))
                m_messageCollection.Add(message, new List<DateTime>() { DateTime.Now });
            else
                m_messageCollection[message].Add(DateTime.Now);

            SetStatus(message);

            IterateOutputMessages();
        }

        internal void IterateOutputMessages()
        {
            Dictionary<DateTime, SMessageInfo> dic = new Dictionary<DateTime, SMessageInfo>();
            m_stack.Children.Clear();

            if (m_collapse.IsChecked.Value)
            {
                foreach (var k in m_messageCollection.Keys)
                    dic.Add(m_messageCollection[k].Last(), k);

                var l = dic.OrderBy(key => key.Key);
                dic = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                foreach (var kv in dic)
                    m_stack.Children.Add(CreateMessage(kv.Key, kv.Value, m_messageCollection[kv.Value].Count()));
            }
            else
            {
                foreach (var k in m_messageCollection.Keys)
                    foreach (var v in m_messageCollection[k])
                        dic.Add(v, k);

                var l = dic.OrderBy(key => key.Key);
                dic = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                foreach (var kv in dic)
                    m_stack.Children.Add(CreateMessage(kv.Key, kv.Value));
            }

            m_stack.UpdateLayout();
            m_scroll.ChangeView(0, double.MaxValue, 1);
        }

        internal void ClearOutput()
        {
            m_messageCollection.Clear();
            m_stack.Children.Clear();
        }
    }
}
