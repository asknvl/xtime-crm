using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using Avalonia.Data;
using Avalonia.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Input;

namespace crm.Views.behaviors
{
    public class RightButtonClickBehavior : Behavior<ToggleButton>
    {

        public RightButtonClickBehavior()
        {
            
        }

        public static readonly DirectProperty<RightButtonClickBehavior, ICommand?> CommandProperty =
            AvaloniaProperty.RegisterDirect<RightButtonClickBehavior, ICommand?>(nameof(Command),
                button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);

        ICommand _command;
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
