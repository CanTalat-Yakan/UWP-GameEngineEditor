using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Assets.Control
{
    class Control_Settings
    {
        View_Main m_main;

        MarkdownTextBlock m_content;

        public Control_Settings(View_Main _main, MarkdownTextBlock _content)
        {
            m_main = _main;
            m_content = _content;

            string path = Directory.GetCurrentDirectory() + "\\Assets\\Settings.txt";
            m_content.Text = File.ReadAllText(path);
        }
    }
}
