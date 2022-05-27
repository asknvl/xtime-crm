using Avalonia;

using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using Avalonia.Data;
using Avalonia.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Controls;

namespace crm.Views.behaviors
{
    public class OutsideClickBehavior : Behavior<Grid>
    {
        public OutsideClickBehavior()
        {
        }

        public static readonly DirectProperty<OutsideClickBehavior, ICommand?> CommandProperty =
           AvaloniaProperty.RegisterDirect<OutsideClickBehavior, ICommand?>(nameof(Command),
               grid => grid.Command, (grid, command) => grid.Command = command, enableDataValidation: false);

        ICommand _command;
        public ICommand? Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
            AssociatedObject.PointerEnter += AssociatedObject_PointerEnter;
            base.OnAttached();
        }

        private void AssociatedObject_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            
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
