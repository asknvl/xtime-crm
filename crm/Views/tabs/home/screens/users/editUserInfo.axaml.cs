using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.tabs.home.screens.users
{
    public partial class editUserInfo : UserControl
    {
        public editUserInfo()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
