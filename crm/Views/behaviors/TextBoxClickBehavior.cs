using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace crm.Views.behaviors
{
    public class TextBoxClickBehavior : Behavior<TextBox>
    {
        public TextBoxClickBehavior()
        {

        }

        public static readonly DirectProperty<TextBoxClickBehavior, ICommand?> CommandProperty =
            AvaloniaProperty.RegisterDirect<TextBoxClickBehavior, ICommand?>(nameof(Command),
                button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);

        ICommand? _command;
        public ICommand? Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
            base.OnAttached();
        }

        private void AssociatedObject_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.MouseButton == Avalonia.Input.MouseButton.Right)
            {
                Command.Execute(null);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            base.OnDetaching();
        }
    }
}
