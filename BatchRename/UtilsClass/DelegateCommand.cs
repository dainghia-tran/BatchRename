using System;
using System.Windows.Input;

namespace BatchRename.UtilsClass
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> executeMethod = null;
        private readonly Func<object, bool> canExecuteMethod = null;

        public event EventHandler CanExecuteChanged
        {
            add { return; }
            remove { return; }
        }

        public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod == null) return true;
            return this.canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            if (executeMethod == null) return;
            this.executeMethod(parameter);
        }
    }
}
