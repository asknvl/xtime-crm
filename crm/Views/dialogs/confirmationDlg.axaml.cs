using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.dialogs
{
    public partial class confirmationDlg : Window
    {
        public confirmationDlg()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Deactivated += ConfirmationDlg_Deactivated;
        }

        private void ConfirmationDlg_Deactivated(object? sender, System.EventArgs e)
        {
            ((Window)sender).Owner?.Activate();
            ((Window)sender).Owner?.Focus();
            this.Close();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
