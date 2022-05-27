using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using Avalonia.Data;

namespace crm.Views
{
    public class LostFocusUpdateBindingBehavior : Behavior<TextBox>
    {
        static LostFocusUpdateBindingBehavior()
        {
            TextProperty.Changed.Subscribe(e =>
            {
                ((LostFocusUpdateBindingBehavior)e.Sender).OnBindingValueChanged();
            });
        }

        protected override void UpdateDataValidation<T>(AvaloniaProperty<T> property, BindingValue<T> value)
        {
            base.UpdateDataValidation(property, value);

            if (property == TextProperty && AssociatedObject != null)
            {
                if (value.Type == BindingValueType.DataValidationError && value.HasError)
                    DataValidationErrors.SetError(AssociatedObject, value.Error);
                else if (!value.HasError)
                    DataValidationErrors.ClearErrors(AssociatedObject);
            }
        }

        public static readonly DirectProperty<LostFocusUpdateBindingBehavior, string> TextProperty = AvaloniaProperty.RegisterDirect<LostFocusUpdateBindingBehavior, string>(
            nameof(Text), o => o.Text, (o, v) => o.Text = v, null, BindingMode.TwoWay, true);

        private string _text;
        public string Text
        {
            get { return _text; }
            set { this.SetAndRaise(TextProperty, ref _text, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.LostFocus += OnLostFocus;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LostFocus -= OnLostFocus;
            base.OnDetaching();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null)
                Text = AssociatedObject.Text;
        }

        private void OnBindingValueChanged()
        {
            if (AssociatedObject != null)
                AssociatedObject.Text = Text;
        }
    }
}