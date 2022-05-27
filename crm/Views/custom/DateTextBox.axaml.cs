using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;
using System.Text.RegularExpressions;

namespace crm.Views.custom
{
    public partial class DateTextBox : TextBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(TextBox);

        public DateTextBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            Regex regex = new Regex(@"[0-9]|[.]");
            e.Handled = !regex.IsMatch(e.Text);
            base.OnTextInput(e);
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{

        //    if ((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod || e.Key == Key.Oem2)
        //    {                

        //    } else
        //        e.Handled = true;
        //    base.OnKeyDown(e);
        //}
    }
}
