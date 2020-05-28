using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ProgExtinction.Helpers
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> m_action;
        readonly Predicate<object> m_canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            m_action = execute;
            m_canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return m_canExecute == null ? true : m_canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) { m_action(parameter); }
    }
}
