using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CostCounter.ViewModel
{
    public class Command : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public Command(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
