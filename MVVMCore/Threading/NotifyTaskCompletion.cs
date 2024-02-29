/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : NotifyTaskCompletion                                    *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  : INotifyPropertyChanged                                  *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : NotifyTaskCompletion.cs                                 *
/// ***************************************************************************
/// </summary>
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MVVMCore.Threading
{
    /// <summary>Implements an observer class for awaitable <code>Task</code> classes for HMI binding</summary>
    /// <typeparam name="TResult">A result parameter <code>TResult</code></typeparam>
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        #region ---------------Declarations----------------
        /// <summary>Event indicating the task is finished</summary>
        public event EventHandler TaskCompleted;
        #endregion
        //
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of <code>NotifyTaskCompletion</code></summary>
        /// <param name="task">The <code>Task</code> to be observed</param>
        public NotifyTaskCompletion(Task<TResult> task)
        {
            this.Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }
        #endregion
        //
        #region ------------Properties Get/Set-------------
        /// <summary>Gets or (private) sets the <code>Task</code> currently observed</summary>
        public Task<TResult> Task
        { get; private set; }
        /// <summary>Gets the result of the current <code>Task</code></summary>
        public TResult Result
        {
            get
            {
                return (this.Task.Status == TaskStatus.RanToCompletion) ? this.Task.Result : default(TResult);
            }
        }
        /// <summary>Gets the status of the <code>Task</code> currently observed</summary>
        public TaskStatus Status
        {
            get { return this.Task.Status; }
        }
        /// <summary>Gets an indicator, if the <code>Task</code> currently observed is already completed</summary>
        public bool IsCompleted
        {
            get { return this.Task.IsCompleted; }
        }
        /// <summary>Gets an indicator, if the <code>Task</code> currently observed is already completed in success</summary>
        public bool IsSuccessfullyCompleted
        {
            get { return (this.Task.Status == TaskStatus.RanToCompletion); }
        }
        /// <summary>Gets an indicator, if the <code>Task</code> currently observed is canceled</summary>
        public bool IsCanceled
        {
            get { return this.Task.IsCanceled; }
        }
        /// <summary>Gets an indicator, if the <code>Task</code> currently observed is faulted</summary>
        public bool IsFaulted
        {
            get { return this.Task.IsFaulted; }
        }
        /// <summary>Gets the <code>AggregatException</code> thrown by the <code>Task</code> currently observed</summary>
        public AggregateException Exception
        { get { return this.Task.Exception; } }
        /// <summary>Gets the inner Exception of the <code>AggregatException</code> thrown by the <code>Task</code> currently observed</summary>
        public Exception InnerException
        { 
            get 
            {
                return (this.Exception != null) ? this.Exception.InnerException : null;
            }
        }
        /// <summary>Gets the ErrorMessage of the inner Exception of the <code>AggregatException</code> thrown by the <code>Task</code> currently observed</summary>
        public String ErrorMessage
        {
            get
            {
                return (this.InnerException != null) ? this.InnerException.Message : null;
            }
        }
        #endregion
        //
        #region --------------private methods--------------
        /// <summary>Raises the <code>PropertyChanged</code> event for signalling a value change of a property to the UI</summary>
        /// <param name="propertyName">The name of the property</param>
        private void OnPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>Raises the <code>TaskCompleted</code> event for signalling the completition of the task</summary>
        private void OnTaskCompleted()
        {
            if (this.TaskCompleted != null)
                this.TaskCompleted(this, EventArgs.Empty);
        }
        /// <summary>Awaites the completion of the <code>Task</code> currently observed</summary>
        /// <param name="task">The awaitable <code>Task</code></param>
        /// <returns>The <code>Task</code></returns>
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch { }
            this.OnPropertyChanged("Status");
            this.OnPropertyChanged("IsCompleted");
            if (task.IsCanceled)
                this.OnPropertyChanged("IsCanceled");
            else if (task.IsFaulted)
            {
                this.OnPropertyChanged("IsFaulted");
                this.OnPropertyChanged("Exception");
                this.OnPropertyChanged("InnerException");
                this.OnPropertyChanged("ErrorMessage");
            }
            else
            {
                this.OnPropertyChanged("IsSuccessfullyCompleted");
                this.OnPropertyChanged("Result");
            }
            this.OnTaskCompleted();
        }
        #endregion
        //
        #region -------INotifyPropertyChanged Member-------
        /// <summary>
        /// Occurs, if a property, identified by it's name has changed it's value</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
