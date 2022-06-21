using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public class CreativeItem : BaseCreative
    {
        #region properties        
        bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref isChecked, value);
                CheckedEvent?.Invoke(this, value);
            }
        }
        #endregion

        public CreativeItem()
        {

        }

        #region public
        public override Task UnicalizeAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region events
        public event Action<CreativeItem, bool> CheckedEvent;
        #endregion
    }
}
