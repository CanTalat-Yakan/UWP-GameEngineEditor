using Editor.Assets.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class View_Properties : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal Control_Properties m_Control;
        internal Microsoft.Toolkit.Uwp.UI.Controls.ColorPickerButton m_Color;

        SharpDX.Vector3 m_pos;
        internal SharpDX.Vector3 Pos
        {
            get { return m_pos = new SharpDX.Vector3((float)x_NumberBox_Position_X.Value, (float)x_NumberBox_Position_Y.Value, (float)x_NumberBox_Position_Z.Value); }
            set { x_NumberBox_Position_X.Value = value.X; x_NumberBox_Position_Y.Value = value.Y; x_NumberBox_Position_Z.Value = value.Z; }
        }

        SharpDX.Vector3 m_rot;
        internal SharpDX.Vector3 Rot
        {
            get { return m_rot = new SharpDX.Vector3((float)x_NumberBox_Rotation_X.Value, (float)x_NumberBox_Rotation_Y.Value, (float)x_NumberBox_Rotation_Z.Value); }
            set { x_NumberBox_Rotation_X.Value = value.X; x_NumberBox_Rotation_Y.Value = value.Y; x_NumberBox_Rotation_Z.Value = value.Z; }
        }

        SharpDX.Vector3 m_scale;
        internal SharpDX.Vector3 Sca
        {
            get { return m_scale = new SharpDX.Vector3((float)x_NumberBox_Scale_X.Value, (float)x_NumberBox_Scale_Y.Value, (float)x_NumberBox_Scale_Z.Value); }
            set { x_NumberBox_Scale_X.Value = value.X; x_NumberBox_Scale_Y.Value = value.Y; x_NumberBox_Scale_Z.Value = value.Z; }
        }


        public View_Properties(View_Main _main)
        {
            this.InitializeComponent();

            m_Control = new Control_Properties();
            m_Color = x_ColorPickerButton;
        }


        void AppBarButton_Click_SelectImagePath(object sender, RoutedEventArgs e) { m_Control.SelectImage(Img_SelectTexture, x_TextBlock_TexturePath); }
        void AppBarButton_Click_SelectFilePath(object sender, RoutedEventArgs e) { m_Control.SelectFile(x_TextBlock_FilePath); }
        void FirePropertyChanged([CallerMemberName] string memberName = null) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName)); }
    }
}
