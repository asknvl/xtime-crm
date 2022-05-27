using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace crm.Views.dialogs
{
    public partial class tagsDlg : Window
    {

        public tagsDlg()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif            
        }     
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);            
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

    }
}
