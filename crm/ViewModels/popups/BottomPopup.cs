using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace crm.ViewModels.popups
{
    public class BottomPopup : ViewModelBase, IBottomPopupService
    {
        #region properties
        string text;
        public string Text
        {
            get => text;
            set => this.RaiseAndSetIfChanged(ref text, value);
        }

        bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        double opacity;
        public double Opacity
        {
            get => opacity;
            set => this.RaiseAndSetIfChanged(ref opacity, value);
        }

        double height;
        public double Height
        {
            get => height;
            set => this.RaiseAndSetIfChanged(ref height, value);
        }
        #endregion

        public BottomPopup()
        {
            Opacity = 0.0;
        }

        #region public
        public async void Show(string text)
        {

            await Task.Run(() =>
            {

                Text = text;                                
                Opacity = 1.0;
                IsVisible = true;
                Task.Delay(2000).ContinueWith((t) => {
                    Opacity = 0.0;
                    Task.Delay(1000).ContinueWith((t) => {
                        IsVisible = false;
                    });
                });
               

                //Height = 0;

                //Thread.Sleep(1000);

                //Opacity = 0.0;

                //await Task.Run(() => {
                //    Task.Delay(1000);
                //});

                //Thread.Sleep(1000);

                //IsVisible = false;
            });
        }
        #endregion
    }
}
