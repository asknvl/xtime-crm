using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.tabs.home.screens
{
    public partial class UserList : UserControl
    {
        public UserList()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
