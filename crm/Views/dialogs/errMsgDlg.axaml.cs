using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace crm.Views.dialogs
{
    public partial class errMsgDlg : Window
    {
        public errMsgDlg()
        {
            InitializeComponent();
            this.Deactivated += ErrMsgDlg_Deactivated; ;
#if DEBUG
            this.AttachDevTools();
#endif
            
        }

        private void ErrMsgDlg_Deactivated(object? sender, System.EventArgs e)
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
