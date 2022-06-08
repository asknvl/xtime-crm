using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using crm.ViewModels.dialogs;

namespace crm.Views.dialogs
{
    public partial class commentDlg : Window
    {
        public commentDlg()
        {
            InitializeComponent();
            this.Deactivated += TagsDlg_Deactivated;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void TagsDlg_Deactivated(object? sender, System.EventArgs e)
        {
            ((Window)sender).Owner?.Activate();
            ((Window)sender).Owner?.Focus();
            ((commentDlgVM)DataContext).OnClosing();            
            this.Close();
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
