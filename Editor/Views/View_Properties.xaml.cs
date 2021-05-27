using Editor.Assets.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Editor
{
    public sealed partial class View_Properties : UserControl
    {
        Control_Properties m_control;

        public View_Properties()
        {
            this.InitializeComponent();

            m_control = new Control_Properties();
        }

        void AppBarButton_Click_SelectImagePath(object sender, RoutedEventArgs e)
        {
            m_control.SelectImage(Img_SelectTexture, TxtBlck_TexturePath);
        }

    }
}
