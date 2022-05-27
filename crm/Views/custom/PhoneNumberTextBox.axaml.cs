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
    public partial class PhoneNumberTextBox : TextBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(TextBox);

        public PhoneNumberTextBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            Regex regex = new Regex(@"[0-9]|[.]|[(]|[)]|[+]|[ ]|[-]");
            e.Handled = !regex.IsMatch(e.Text);
            base.OnTextInput(e);
        }
    }
}
