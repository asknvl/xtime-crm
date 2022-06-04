using crm.Models.appcontext;
using crm.Models.user;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class commentDlgVM : ViewModelBase
    {
        #region properties
        bool isEditable;
        public bool IsEditable {
            get => isEditable; 
            set => this.RaiseAndSetIfChanged(ref isEditable, value);
        }

        string text;
        public string Text
        {
            get => text;
            set => this.RaiseAndSetIfChanged(ref text, value);
        }
        #endregion
                
        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        public ReactiveCommand<Unit, Unit> acceptCmd { get; }
        #endregion
        
        public commentDlgVM()
        {
            Text = @"Соображения высшего порядка, а также реализация
намеченного плана развития играет важную роль
в формировании позиций, занимаемых участниками в
отношении поставленных задач. С другой стороны
сложившаяся структура организации напрямую зависит от модели развития!";
        }

        public commentDlgVM(BaseUser user, bool iseditable)
        {
            IsEditable = iseditable;

            cancelCmd = ReactiveCommand.Create(() =>
            {
                OnCloseRequest();
            });

            acceptCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                OnCloseRequest();
            });
        }
    }
}
