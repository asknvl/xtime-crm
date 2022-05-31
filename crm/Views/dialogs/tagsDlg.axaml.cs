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

        MouseDevice mouse;

        public tagsDlg()
        {
            InitializeComponent();

            Deactivated += TagsDlg_Deactivated;
            
            
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

        
        
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //        Close();
        //}

    }
}
