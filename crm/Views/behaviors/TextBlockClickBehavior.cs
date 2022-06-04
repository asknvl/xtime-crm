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
    public class TextBlockClickBehavior : Behavior<TextBlock>
    {
        public TextBlockClickBehavior()
        {
            
        }

        //public static readonly DirectProperty<TextBlockClickBehavior, ICommand?> CommandProperty =
        //    AvaloniaProperty.RegisterDirect<TextBlockClickBehavior, ICommand?>(nameof(Command),
        //        button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);

        public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<TextBlockClickBehavior, ICommand?>(nameof(Command));
        
        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly StyledProperty<string?> CommandParameterProperty = AvaloniaProperty.Register<TextBlockClickBehavior, string?>(nameof(CommandParameter));

        public string? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        
        protected override void OnAttached()
        {
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
            base.OnAttached();
        }

        private void AssociatedObject_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.MouseButton == Avalonia.Input.MouseButton.Left)
            {
                Command?.Execute(CommandParameter);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            base.OnDetaching();
        }
    }
}
