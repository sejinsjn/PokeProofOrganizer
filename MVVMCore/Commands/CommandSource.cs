/// ***************************************************************************
/// *   MODULNAME   : CommandSource                                           *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : CommandSource.cs                                        *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;

namespace MVVMCore.Commands
{
    /// <summary>Defines a <code>static</code> command source module</summary>
	public static class CommandSource
	{
		#region ------------Attached Properties------------
        //
		#region ------------------Trigger------------------
        /// <summary>Gets the <code>ICommandTrigger</code> as value for the command trigger</summary>
        /// <param name="source">The source of the trigger as <code>FrameworkElement</code></param>
        /// <returns>The interface to the <code>CommandTrigger</code> module</returns>
		public static ICommandTrigger GetTrigger(FrameworkElement source)
		{
			return (ICommandTrigger)source.GetValue(TriggerProperty);
		}
        /// <summary>Sets the <code>ICommandTrigger</code> as value for the command trigger</summary>
        /// <param name="source">The source of the trigger as <code>FrameworkElement</code></param>
        /// <param name="value">The <code>CommandTrigger</code> as <code>ICommandTrigger</code></param>
		public static void SetTrigger(FrameworkElement source, ICommandTrigger value)
		{
			source.SetValue(TriggerProperty, value);
		}
        /// <summary>
        /// Using a <code>DependencyProperty</code> as the backing store for Trigger.  
        /// This enables animation, styling, binding, etc...
        /// </summary>
		public static readonly DependencyProperty TriggerProperty =
			DependencyProperty.RegisterAttached(
				"Trigger",
				typeof(ICommandTrigger),
				typeof(CommandSource),
				new UIPropertyMetadata(null, TriggerPropertyChanged));
        /// <summary>Handles the <code>TriggerPropertyChanged</code> from the <code>DependencyProperty</code></summary>
        /// <param name="d">The <code>DependencyObject</code> as <code>FrameworkElement</code></param>
        /// <param name="e"><code>DependencyPropertyChangedEventArgs</code> arguments for handling the event</param>
		private static void TriggerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement element = d as FrameworkElement;

			ICommandTrigger commandTrigger = e.NewValue as ICommandTrigger;
			if (commandTrigger != null)
			{
				commandTrigger.Initialize(element);
			}
		}
		#endregion
        //
		#endregion
	}
}
