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
            IsVisible = false;
        }

        #region public
        public async void Show(string text)
        {

            await Task.Run(async () =>
            {

                await Task.Run(() =>
                {
                    while (IsVisible) ;
                });

                IsVisible = true;
                Text = text;
                Opacity = 1.0;

                await Task.Delay(1000).ContinueWith((t) =>
                {
                    Opacity = 0;
                });

                await Task.Delay(200).ContinueWith((t) =>
                {
                    IsVisible = false;
                });

            });
        }
        #endregion
    }
}
