using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.dialogs
{
    public partial class msgDlg : Window
    {
        public msgDlg()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();

            Deactivated += MsgDlg_Deactivated;
#endif
        }

        private void MsgDlg_Deactivated(object? sender, System.EventArgs e)
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
