using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace crm.Views.tabs.home.screens
{
    public partial class userListItem : UserControl
    {
        Grid itemGrid;
        Button showTagsButton;
        ToggleButton itemChecked;

        public userListItem()
        {
            InitializeComponent();

            itemGrid = this.FindControl<Grid>("PART_ItemGrid");
            itemGrid.PointerEnter += ItemGrid_PointerEnter;
            itemGrid.PointerLeave += ItemGrid_PointerLeave;

            itemChecked = this.FindControl<ToggleButton>("PART_ItemChecked");
            itemChecked.Checked += ItemChecked_Checked;
            itemChecked.Unchecked += ItemChecked_Unchecked;

            showTagsButton = this.FindControl<Button>("PART_ShowTags");
            showTagsButton.PointerEnter += ShowTagsButton_PointerEnter;
            showTagsButton.PointerLeave += ShowTagsButton_PointerLeave;

        }

        private void ItemChecked_Unchecked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            itemHighLight(false);
        }

        private void ItemChecked_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            itemHighLight(true);
        }

        private void ShowTagsButton_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            
            if (itemGrid.IsPointerOver)
                itemHighLight(true);
        }

        private void ShowTagsButton_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            itemHighLight(false);
        }

        private void ItemGrid_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            if (itemChecked.IsChecked != true)
                    itemHighLight(false);
        }

        private void ItemGrid_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            itemHighLight(true);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void itemHighLight(bool state)
        {
            if (state)
                itemGrid.Background = new SolidColorBrush(0xFFF5F5F5);
            else
                itemGrid.Background = new SolidColorBrush(0x00000000);
        }       
    }
}
