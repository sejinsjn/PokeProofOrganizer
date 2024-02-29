/// ***************************************************************************
/// *   MODULNAME   : ICommandTrigger                                         *
/// *   MODULTYPE   : interface                                               *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : ICommandTrigger.cs                                      *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;

namespace MVVMCore.Commands
{
    /// <summary>Defines an interface to special <code>CommandTrigger</code> implementations</summary>
	public interface ICommandTrigger
	{
        /// <summary>Initializes the trigger for command execution</summary>
        /// <param name="source">The source of the trigger as <code>FrameworkElement</code></param>
		void Initialize(FrameworkElement source);
	}
}
