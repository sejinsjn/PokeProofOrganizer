/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : CommandTrigger                                          *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  : ICommandTrigger                                         *
/// *   EXTENDS     : Freezable                                               *
/// *   FILENAME    : CommandTrigger.cs                                       *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;
using System.Windows.Input;

namespace MVVMCore.Commands
{
    /// <summary>Defines a base <code>abstract</code> class for triggering commands from any <code>UIElement</code></summary>
	public abstract class CommandTrigger : Freezable, ICommandTrigger
	{
        #region ---------------Declarations----------------
        /// <summary>Implements a queue for commands, in case of binding delay</summary>
        /// <remarks>The queue is for one command only. It is not a real queue for multiple commands!</remarks>
        private CommandParameter<object> commandParameterQueue;
        #endregion
        //
        #region ------------Properties Get/Set-------------
        /// <summary>Gets or sets an indicator, whether this module is properly initialized</summary>
        public bool IsInitialized { get; private set; }
        #endregion
        //
		#region -----------Dependency Properties----------
        //
		#region -------------Command Property--------------
        /// <summary>Identifies the Command dependency property</summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandTrigger),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandPropertyChanged)));
        /// <summary>Gets or sets a command to be executed</summary>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		#endregion
        //
		#region ---------CustomParameter Property----------
        /// <summary>Identifies the CustomParameterProperty dependency property</summary>
		public static readonly DependencyProperty CustomParameterProperty =
			DependencyProperty.Register("CustomParameter", typeof(object), typeof(CommandTrigger),
			new FrameworkPropertyMetadata(null));
        /// <summary>Gets or sets parameters for the command</summary>
		public object CustomParameter
		{
			get { return (object)GetValue(CustomParameterProperty); }
			set { SetValue(CustomParameterProperty, value); }
		}		

		#endregion
        //
		#region ----------CommandTarget Property----------
        /// <summary>Identifies the CommandTarget dependency property</summary>
		public static readonly DependencyProperty CommandTargetProperty =
			DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(CommandTrigger),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        /// <summary>description for CommandTarget property</summary>
		public IInputElement CommandTarget
		{
			get { return (IInputElement)GetValue(CommandTargetProperty); }
			set { SetValue(CommandTargetProperty, value); }
		}
		#endregion
		//
		#endregion
        //
        #region -------------protected methods-------------
        /// <summary><code>abstract</code> initialization function </summary>
        /// <param name="source">The source of the trigger as <code>FrameworkElement</code></param>
        protected abstract void InitializeCore(FrameworkElement source);
        /// <summary>Execution method of the command</summary>
        /// <param name="parameter">A special <code>CommandParameter</code></param>
        protected void ExecuteCommand(CommandParameter<object> parameter)
        {
            if (Command == null)
            {
                this.commandParameterQueue = parameter;
                return;
            }
            this.commandParameterQueue = null;

            RoutedCommand routedCommand = Command as RoutedCommand;
            if (routedCommand != null)
            {
                routedCommand.Execute(parameter, CommandTarget);
            }
            else
            {
                Command.Execute(parameter);
            }
        }
        #endregion
        //
        #region ------------Callback functions-------------
        /// <summary>Callback function for implementing a reaction on the change of the command property</summary>
        /// <param name="d">The <code>CommandProperty</code></param>
        /// <param name="e"><code>DependencyPropertyChangedEventArgs</code> containing all data for a reaction on the property change</param>
        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandTrigger trigger = d as CommandTrigger;
            if (trigger.commandParameterQueue != null)
                trigger.ExecuteCommand(trigger.commandParameterQueue);
        }
        #endregion
        //
        #region ----------ICommandTrigger Member-----------
        /// <summary>Initializes the trigger for command execution</summary>
        /// <param name="source">The source of the trigger as <code>FrameworkElement</code></param>
        void ICommandTrigger.Initialize(FrameworkElement source)
		{
			if (IsInitialized)
				return;
			
			InitializeCore(source);
			IsInitialized = true;
		}
        #endregion
	}
}
