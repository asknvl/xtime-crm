using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace crm.Views
{
    public partial class MainWindow : Window
    {

        public Grid overlayGrid;
        public Grid topGrid;

        public MainWindow()
        {            
            InitializeComponent();
            overlayGrid = this.FindControl<Grid>("OverlayGrid");
            overlayGrid.IsVisible = false;

            topGrid = this.FindControl<Grid>("TOP");
            topGrid.PointerPressed += TopGrid_PointerPressed;
        }

        private void TopGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            BeginMoveDrag(e);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            
            base.OnPointerPressed(e);            
        }

        

    }
}
