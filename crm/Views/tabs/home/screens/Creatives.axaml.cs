using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using vm = crm.ViewModels.tabs.home.screens;

namespace crm.Views.tabs.home.screens
{
    public partial class Creatives : UserControl
    {
        Grid dragAndDropGrid;

        public Creatives()
        {
            InitializeComponent();

            dragAndDropGrid = this.FindControl<Grid>("DragAndDropGrid");
            dragAndDropGrid.AddHandler(DragDrop.DropEvent, (sender, args) => {
                var s = args.Data.GetDataFormats();
                dynamic fn = args.Data.Get("FileNames");

                List<string> names = new List<string>();
                foreach (var item in fn)
                {
                    names.Add(item);
                }

                if (names.Count == 1)
                {

                    TODO

                } else if (names.Count > 1) 
                    ((vm.Creatives)DataContext)?.OnDragDrop(names);
                
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
