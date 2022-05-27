using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace crm.ViewModels
{
    public class ViewModelBase : ReactiveObject, INotifyDataErrorInfo
    {

        #region error validattion
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // get errors by property
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }

        public bool HasErrors => _errors.Count > 0;

        // object is valid
        public bool IsValid => !HasErrors;

        public void AddError(string propertyName, string error)
        {
            // Add error to list
            _errors[propertyName] = new List<string>() { error };
            NotifyErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName)
        {
            // remove error
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);
            NotifyErrorsChanged(propertyName);
        }

        public void NotifyErrorsChanged(string propertyName)
        {
            // Notify
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion

        public event Action onCloseRequest;
        public void OnCloseRequest()
        {
            onCloseRequest?.Invoke();
        }
    }
}
