using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace crm.Views.dialogs
{
    public partial class tagsDlg : Window
    {

        public tagsDlg()
        {
            InitializeComponent();

            this.Deactivated += TagsDlg_Deactivated;

#if DEBUG
            this.AttachDevTools();
#endif            
        }
        private void TagsDlg_Deactivated(object? sender, System.EventArgs e)
        {
            Close();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);            
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

    }
}
