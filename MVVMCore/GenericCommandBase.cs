/// ***************************************************************************
/// *   MODULNAME   : GenericCommandBase of T                                 *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  : ICommand, IActiveAware                                  *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : GenericCommandBase.cs                                   *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows.Input;

namespace MVVMCore
{
    /// <summary>Implementation of a command handler for WPF user interfaces</summary>
    /// <typeparam name="T">The type of the command</typeparam>
    public class GenericCommandBase<T>: ICommand, IActiveAware
    {
        #region ---------------Declarations----------------
        /// <summary>The "execute" action to be executed if the command raises</summary>
        private readonly Action<T> execute;
        /// <summary>The predicate controlling the execution of the command</summary>
        private readonly Predicate<T> canExecute;
        /// <summary> Occurs the the active state of the implementing module changed </summary>
        private event EventHandler isActiveChangedEvent = null;
        /// <summary>Indicator, if the command is currently activated. The default value is true</summary>
        private bool isActive = true;
        #endregion
        //
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of <code>CommandBase</code> delivering the execution only</summary>
        /// <param name="execute">The <code>Action</code> to be executed, if the commend raises</param>
        public GenericCommandBase(Action<T> execute)
            : this(execute, null)
        {
        }
        /// <summary>Initializes a new instance of <code>CommandBase</code> delivering action and predicate</summary>
        /// <param name="execute">The <code>Action</code> to be executed, if the commend raises</param>
        /// <param name="canExecute">The <code>Predicate</code> for controlling the execution</param>
        public GenericCommandBase(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            this.execute = execute;
            this.canExecute = canExecute;
        }
        #endregion // Constructors 
        //
        #region -------------protected methods-------------
        /// <summary>Raises the <code>IsActiveChanged</code> event, if the state of activation has been changed</summary>
        protected void OnIsActiveChanged()
        {
            if (this.isActiveChangedEvent != null)
                this.isActiveChangedEvent("Command", EventArgs.Empty);
        }
        #endregion
        //
        #region --------------ICommand Member--------------
        /// <summary>Specifies, if the command is to be executed or not</summary>
        /// <param name="parameter">An parameter as object</param>
        /// <returns>true, if the command is to be executed</returns>
        public bool CanExecute(object parameter)
        {
            if (!this.isActive)
                return false;
            if (this.canExecute != null)
            {
                if (parameter is T)
                    return this.canExecute((T)parameter);
                else if (parameter == null)
                {
                    try
                    {
                        return this.canExecute((T)(new object()));
                    }
                    catch 
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>Occurs, if the possibility of execution of the command has been changed</summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        } 
        /// <summary>Executes the command</summary>
        /// <param name="parameter">parameter for execution</param>
        public void Execute(object parameter)
        {
            if (((parameter == null) || (parameter is T)) && (this.isActive))
                this.execute((T)parameter);
        }
        #endregion
        //
        #region ------------IActiveAware Member------------
        /// <summary>Gets or sets the active state of the command</summary>
        public bool IsActive
        {
            get { return this.isActive; }
            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;
                    this.OnIsActiveChanged();
                }
            }
        }
        /// <summary>Occurs, if the active state of the command has changed</summary>
        public event EventHandler IsActiveChanged
        {
            add { this.isActiveChangedEvent += value; }
            remove { this.isActiveChangedEvent -= value; }
        }
        #endregion
    }
}
