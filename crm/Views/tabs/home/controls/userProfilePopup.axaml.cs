using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.tabs.home.controls
{
    public partial class userProfilePopup : UserControl
    {
        public userProfilePopup()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
