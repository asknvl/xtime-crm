using crm.ViewModels;
using crm.Views;
using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crm.ViewModels.dialogs;
using crm.Views.dialogs;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading;

namespace crm.WS
{
    public class WindowService : IWindowService
    {

        static WindowService instance;
        Window main;
        List<Window> windowList;
        PixelPoint startupPosition;


        private WindowService()
        {
            windowList = new List<Window>();
        }

        public static WindowService getInstance()
        {
            if (instance == null)
            {
                instance = new WindowService();
            }
            return instance;
        }

        public void ShowWindow(ViewModelBase vm)
        {

            Window wnd = null;

            if (vm is mainVM)
            {
                wnd = new MainWindow();
                main = wnd;
                startupPosition = main.Position;
                main.PositionChanged += Main_PositionChanged;
            }

            wnd.DataContext = vm;
            windowList.Add(wnd);
            vm.onCloseRequest += () => { wnd.Close(); windowList.Remove(wnd); };
            wnd.Show();

        }

        private void Main_PositionChanged(object? sender, PixelPointEventArgs e)
        {
            int x = main.Position.X;
            int y = main.Position.Y;
            int dx = x - startupPosition.X;
            int dy = y - startupPosition.Y;
            startupPosition = new PixelPoint(x, y);

            foreach (var item in windowList)
            {
                if (item is MainWindow)
                    continue;

                item.Position = new PixelPoint(item.Position.X + dx, item.Position.Y + dy);

            }
        }

        public void ShowDialog(ViewModelBase vm)
        {
            Window wnd = null;

            if (vm is errMsgVM)
            {
                wnd = new errMsgDlg();
            }

            if (vm is msgDlgVM)
            {
                wnd = new msgDlg();
            }

            if (vm is tagsDlgVM)
            {
                wnd = new tagsDlg();
            }

            if (vm is rolesDlgVM)
            {
                wnd = new rolesDlg();
            }

            if (vm is commentDlgVM)
            {
                wnd = new commentDlg();
            }

            if (vm is confirmationDlgVM)
            {
                wnd = new confirmationDlg();
            }

            wnd.DataContext = vm;
            windowList.Add(wnd);

            wnd.Closed += (s, e) =>
            {
                //((MainWindow)main).overlayGrid.IsVisible = false;                
                //((MainWindow)main).overlayGrid.IsVisible = false;                
                //main.IsEnabled = true;                
                //main.Activate();
                ((MainWindow)main).Grid_WorkArea.IsEnabled = true;                
                windowList.Remove(wnd);                
            };

            vm.onCloseRequest += () =>
            {
                wnd.Close();                
            };

            //main.IsEnabled = false;
            //((MainWindow)main).overlayGrid.IsVisible = true;
            ((MainWindow)main).Grid_WorkArea.IsEnabled =false;                

            wnd.Position = wnd.Position = new PixelPoint( //Нельзя передавать Owner в Show(main) потому что потом не работает закрытие диалога по клику?
                main.Position.X + (int)((main.Width - wnd.Width) / 2),
                main.Position.Y + (int)((main.Height - wnd.Height) / 2));

            wnd.Show();
        }
        public void ShowDialog(ViewModelBase vm, ViewModelBase parent)
        {
            Window wnd = null;

            if (vm is errMsgVM)
            {
                wnd = new errMsgDlg();
            }

            wnd.DataContext = vm;
            
            windowList.Add(wnd);
            wnd.Closed += (s, e) =>
            {             
                var p = windowList.FirstOrDefault(w => w.DataContext == parent);
                //p.IsEnabled = true;
                if (p is MainWindow)
                {
                    ((MainWindow)p).overlayGrid.IsVisible = false;
                }
            };
            vm.onCloseRequest += () =>
            {
                wnd.Close();
                windowList.Remove(wnd);
            };            
            var p = windowList.FirstOrDefault(w => w.DataContext == parent);
            //p.IsEnabled = false;
            if (p is MainWindow)
            {
                ((MainWindow)p).overlayGrid.IsVisible = true;
            }

            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;           
            wnd.Show(main);            
        }

        public async Task<string> ShowFileDialog(string title, ViewModelBase parent)
        {
            var dialog = new OpenFolderDialog() { Title = title };
            var p = windowList.FirstOrDefault(w => w.DataContext == parent);
            return await dialog.ShowAsync(p);
        }

        //public async Task ShowModalWindow(ViewModelBase vm)
        //{
        //    var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
        //        .Windows[0];

        //    Window window = new tagsDlg();
        //    window.DataContext = vm;

        //    await window.ShowDialog(mainWindow);
            
        //}

        private void Wnd_Closed(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
