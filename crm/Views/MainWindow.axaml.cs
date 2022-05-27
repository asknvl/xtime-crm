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

        public MainWindow()
        {            
            InitializeComponent();

            overlayGrid = this.FindControl<Grid>("OverlayGrid");
            overlayGrid.IsVisible = false;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            BeginMoveDrag(e);
        }
        
    }
}
