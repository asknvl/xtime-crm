using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace crm.Views.dialogs
{
    public partial class rolesDlg : Window
    {
        public rolesDlg()
        {
            InitializeComponent();

            //Deactivated += RolesDlg_Deactivated;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void RolesDlg_Deactivated(object? sender, System.EventArgs e)
        {
            Close();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
