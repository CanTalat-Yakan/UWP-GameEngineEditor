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

        #region Properties
        SharpDX.Vector3 m_pos;
        internal SharpDX.Vector3 Pos
        {
            get { return m_pos = new SharpDX.Vector3((float)x_NumberBox_Position_X.Value, (float)x_NumberBox_Position_Y.Value, (float)x_NumberBox_Position_Z.Value); }
            set { x_NumberBox_Position_X.Value = Math.Round(value.X, 5); x_NumberBox_Position_Y.Value = Math.Round(value.Y, 5); x_NumberBox_Position_Z.Value = Math.Round(value.Z, 5); }
        }

        SharpDX.Vector3 m_rot;
        internal SharpDX.Vector3 Rot
        {
            get { return m_rot = new SharpDX.Vector3((float)x_NumberBox_Rotation_X.Value, (float)x_NumberBox_Rotation_Y.Value, (float)x_NumberBox_Rotation_Z.Value); }
            set { x_NumberBox_Rotation_X.Value = Math.Round(value.X, 5); x_NumberBox_Rotation_Y.Value = Math.Round(value.Y, 5); x_NumberBox_Rotation_Z.Value = Math.Round(value.Z, 5); }
        }

        SharpDX.Vector3 m_scale;
        internal SharpDX.Vector3 Sca
        {
            get { return m_scale = new SharpDX.Vector3((float)x_NumberBox_Scale_X.Value, (float)x_NumberBox_Scale_Y.Value, (float)x_NumberBox_Scale_Z.Value); }
            set { x_NumberBox_Scale_X.Value = Math.Round(value.X, 4); x_NumberBox_Scale_Y.Value = Math.Round(value.Y, 4); x_NumberBox_Scale_Z.Value = Math.Round(value.Z, 4); }
        }
        #endregion


        public View_Properties(View_Main _main)
        {
            this.InitializeComponent();

            m_Control = new Control_Properties();


            List<Grid> collection = new List<Grid>();
            collection.Add(m_Control.CreateColorButton());
            collection.Add(m_Control.CreateNumberInput());
            collection.Add(m_Control.CreateTextInput());
            collection.Add(m_Control.CreateVec2Input());
            collection.Add(m_Control.CreateVec3Input());
            collection.Add(m_Control.CreateSlider());
            collection.Add(m_Control.CreateBool());
            collection.Add(m_Control.CreateTextureSlot());
            collection.Add(m_Control.CreateReferenceSlot());
            collection.Add(m_Control.CreateHeader());
            collection.Add(m_Control.WrapExpander(m_Control.CreateEvent()));
            x_StackPanel_Properties.Children.Add(m_Control.CreateScript("Example", collection.ToArray()));
            x_StackPanel_Properties.Children.Add(m_Control.CreateScript("Another", m_Control.CreateNumberInput()));
        }


        void AppBarButton_Click_SelectImagePath(object sender, RoutedEventArgs e) { }//m_Control.SelectImage(Img_SelectTexture, x_TextBlock_TexturePath); }
        void AppBarButton_Click_SelectFilePath(object sender, RoutedEventArgs e) { }//m_Control.SelectFile(x_TextBlock_FilePath); }
        void FirePropertyChanged([CallerMemberName] string memberName = null) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName)); }
    }
}
