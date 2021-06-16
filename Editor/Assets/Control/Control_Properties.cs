using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using NavigationViewItemSeparator = Windows.UI.Xaml.Controls.NavigationViewItemSeparator;

namespace Editor.Assets.Control
{
    class Control_Properties
    {
        internal async void SelectImage(Image _image, TextBlock _path)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage() { DecodePixelHeight = 48, DecodePixelWidth = 48 };
                    await bitmapImage.SetSourceAsync(fileStream);
                    _image.Source = bitmapImage;
                }
                _path.Text = file.Name;
            }
        }

        internal async void SelectFile(TextBlock _path)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                _path.Text = file.Name;
            }
        }

        internal Grid CreateNumberInput(string _header = "Float", float _number = 0)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            NumberBox numInput = new NumberBox() { Value = _number, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact };

            stack.Children.Add(header);
            stack.Children.Add(numInput);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateTextInput(string _header = "String", string _text = "Example")
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            TextBox textInput = new TextBox() { Text = _text, MaxWidth = 212 };

            stack.Children.Add(header);
            stack.Children.Add(textInput);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateVec2Input(string _header = "Vector2", float _number = 0, float _number2 = 0)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            NumberBox numInput = new NumberBox() { Value = _number, Margin = new Thickness(0, 0, 4, 0) };
            NumberBox num2Input = new NumberBox() { Value = _number2, Margin = new Thickness(0, 0, 4, 0) };

            stack.Children.Add(header);
            stack.Children.Add(numInput);
            stack.Children.Add(num2Input);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateVec3Input(string _header = "Vector3", float _number = 0, float _number2 = 0, float _number3 = 0)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            NumberBox numInput = new NumberBox() { Value = _number, Margin = new Thickness(0, 0, 4, 0) };
            NumberBox num2Input = new NumberBox() { Value = _number2, Margin = new Thickness(0, 0, 4, 0) };
            NumberBox num3Input = new NumberBox() { Value = _number3, Margin = new Thickness(0, 0, 4, 0) };

            stack.Children.Add(header);
            stack.Children.Add(numInput);
            stack.Children.Add(num2Input);
            stack.Children.Add(num3Input);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateSlider(string _header = "Slider", float _value = 0)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            Slider numInput = new Slider() { Value = _value, Width = 200, Margin = new Thickness(0, 0, 0, -5.5) };
            TextBlock numPreview = new TextBlock() { Padding = new Thickness(4, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };

            stack.Children.Add(header);
            stack.Children.Add(numInput);
            stack.Children.Add(numPreview);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateBool(string _header = "Bool", bool _value = false)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            CheckBox check = new CheckBox() { IsChecked = _value, Margin = new Thickness(0, 0, 0, -5.5) };

            stack.Children.Add(header);
            stack.Children.Add(check);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateTextureSlot(string _header = "Texture")
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            Grid container = new Grid() { Width = 48, Height = 48, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(2), BorderBrush = (Brush)Application.Current.Resources["SystemControlForegroundBaseMediumBrush"] };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            Image img = new Image() { Stretch = Stretch.UniformToFill };
            AppBarButton button = new AppBarButton() { };
            TextBlock path = new TextBlock() { Text = "None", Margin = new Thickness(4, 0, 0, 0), VerticalAlignment = VerticalAlignment.Bottom };

            container.Children.Add(img);
            container.Children.Add(button);
            stack.Children.Add(header);
            stack.Children.Add(container);
            stack.Children.Add(path);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateReferenceSlot(string _header = "Reference")
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            Button button = new Button() { Content = "..." };
            TextBlock reference = new TextBlock() { Text = "None(type)", Margin = new Thickness(4, 0, 0, 0), VerticalAlignment = VerticalAlignment.Bottom };

            stack.Children.Add(header);
            stack.Children.Add(button);
            stack.Children.Add(reference);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateEvent(string _header = "Button", string _button = "Event")
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            Button button = new Button() { Content = _button };

            stack.Children.Add(header);
            stack.Children.Add(button);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateColorButton(string _header = "Color", byte r = 0, byte g = 0, byte b = 0, byte a = 0)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock header = new TextBlock() { Text = _header + ":", Width = 80, VerticalAlignment = VerticalAlignment.Bottom };
            Windows.UI.Color col = new Windows.UI.Color();
            col.R = r; col.G = g; col.B = b; col.A = a;
            ColorPickerButton colbutton = new ColorPickerButton() { SelectedColor = col };

            stack.Children.Add(header);
            stack.Children.Add(colbutton);
            grid.Children.Add(stack);

            return grid;
        }

        internal Grid CreateHeader(string _header = "Header")
        {
            Grid grid = new Grid();
            TextBlock header = new TextBlock() { Text = _header, FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 20, 0, 0) };

            grid.Children.Add(header);

            return grid;
        }

        internal Grid WrapExpander(Grid _content, string _header = "Expander")
        {
            Grid grid = new Grid();
            Expander expander = new Expander() { Header = _header, Margin = new Thickness(14, 0, 0, 0), HorizontalContentAlignment = HorizontalAlignment.Stretch };
            _content.Margin = new Thickness(0, 4, 0, 10);

            expander.Content = _content;
            grid.Children.Add(expander);

            return grid;
        }

        internal Grid CreateScript(string _header = "ExampleScript", params Grid[] _properties)
        {
            Grid grid = new Grid();
            StackPanel stack = new StackPanel() { Orientation = Orientation.Vertical, Spacing = 10, Margin = new Thickness(10, 4, 0, 20) };
            Expander expander = new Expander() { Header = _header, ExpandDirection = ExpandDirection.Down, HorizontalContentAlignment = HorizontalAlignment.Stretch };
            CheckBox checkBox = new CheckBox() { IsChecked = true, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 4, -80, 0) };
            NavigationViewItemSeparator seperator = new NavigationViewItemSeparator() { VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(-10) };

            foreach (var item in _properties)
                stack.Children.Add(item);

            expander.Content = stack;
            grid.Children.Add(expander);
            grid.Children.Add(checkBox);
            grid.Children.Add(seperator);

            return grid;
        }


    }
}
