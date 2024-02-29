/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : EventCommandTrigger                                     *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     : CommandTrigger                                          *
/// *   FILENAME    : EventCommandTrigger.cs                                  *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;

namespace MVVMCore.Commands
{
    /// <summary><code>CommandTrigger</code> triggered by framework events such as <code>RoutedEvents</code></summary>
    public sealed class EventCommandTrigger : CommandTrigger
	{
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of the <code>EventCommandTrigger</code></summary>
        /// <returns>The <code>EventCommandTrigger</code> as <code>Freezable</code></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new EventCommandTrigger();
        }
        #endregion
        //
        #region -----------Dependency Properties-----------
        //
        #region -----------RoutedEvent Property------------
        /// <summary>Identifies the RoutedEvent dependency property</summary>
		public static readonly DependencyProperty RoutedEventProperty =
			DependencyProperty.Register("RoutedEvent", typeof(RoutedEvent), typeof(EventCommandTrigger),
			new FrameworkPropertyMetadata(null));
        /// <summary>Identifies the HandledAfterExecuption dependency property</summary>
        public static readonly DependencyProperty HandledAfterExecutionProperty =
            DependencyProperty.Register("HandledAfterExecution", typeof(Boolean), typeof(EventCommandTrigger),
            new FrameworkPropertyMetadata(false));
        /// <summary>gets or sets the description for the RoutedEvent property</summary>
		public RoutedEvent RoutedEvent
		{
			get { return (RoutedEvent)GetValue(RoutedEventProperty); }
			set { SetValue(RoutedEventProperty, value); }
		}
        /// <summary>Gets or sets an indicator whether to set handled after execution of the first execution</summary>
        public bool HandledAfterExecution
        {
            get { return (bool)GetValue(HandledAfterExecutionProperty); }
            set { SetValue(HandledAfterExecutionProperty, value); }
        }
		#endregion
		
		#endregion
        //
        #region --------------private methods--------------
        /// <summary>Executes the command based on events</summary>
        /// <param name="sender"></param>
        /// <param name="args"><code>RoutedEventArgs</code> containing all data for event handling</param>
        private void ExecuteCommand(object sender, RoutedEventArgs args)
        {
            CommandParameter<object> parameter = new EventCommandParameter<object, RoutedEventArgs>(
                CustomParameter, RoutedEvent, args);

            base.ExecuteCommand(parameter);
            //Stop execution, if requested!
            args.Handled = this.HandledAfterExecution;
        }
        #endregion
        //
        #region -------------protected methods-------------
        /// <summary>Initializes the core of the <code>CommandTrigger</code> based on events</summary>
        /// <param name="source">The source as <code>FrameworkElement</code></param>
        protected override void InitializeCore(FrameworkElement source)
        {
            source.AddHandler(RoutedEvent, (RoutedEventHandler)ExecuteCommand);
        }
        #endregion
	}
}
