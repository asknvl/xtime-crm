using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.tabs
{
    public partial class TextTabHeader : UserControl
    {
        public TextTabHeader()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
