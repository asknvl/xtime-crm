using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.tabs.home.screens.users
{
    public partial class UserEdit : UserControl
    {
        public UserEdit()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
