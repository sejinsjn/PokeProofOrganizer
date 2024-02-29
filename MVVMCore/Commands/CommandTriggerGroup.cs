/// ***************************************************************************
/// *   MODULNAME   : CommandTriggerGroup                                     *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  : ICommandTrigger                                         *
/// *   EXTENDS     : FreezableCollection                                     *
/// *   FILENAME    : CommandTriggerGroup.cs                                  *
/// ***************************************************************************
/// </summary>
using System;
using System.Collections.Generic;
using System.Windows;

namespace MVVMCore.Commands
{
    /// <summary>
    /// Defines a <code>sealed</code> collection of <code>Freezable</code> modules of type <code>CommandTrigger</code> and self implements
    /// the <code>ICommandTrigger</code> interface to act as a single trigger in a <code>CommandSource</code>
    /// </summary>
	public sealed class CommandTriggerGroup : FreezableCollection<CommandTrigger>, ICommandTrigger
	{
        #region ---------------Definitions-----------------
        /// <summary>The <code>HashSet</code> containing the group of commands</summary>
        private readonly HashSet<ICommandTrigger> initList = new HashSet<ICommandTrigger>();
        #endregion
        //
        #region --------------private methods--------------
        /// <summary>Initializes a single <code>CommandTrigger</code> module</summary>
        /// <param name="source">The source of the trigger to be initialized as <code>FrameworkElement</code></param>
        /// <param name="trigger">The <code>ICommandTrigger</code> module</param>
        private void InitializeCommandSource(FrameworkElement source, ICommandTrigger trigger)
        {
            trigger.Initialize(source);
            initList.Add(trigger);
        }
        #endregion
        //
        #region -------------internal methods--------------
        /// <summary>Clears the complete list of <code>ICommandTrigger</code> modules</summary>
        internal void ClearInitList()
        {
            this.initList.Clear();
        }
        #endregion
        //
        #region ----------ICommandTrigger member-----------
        /// <summary>Initializes the trigger for command execution</summary>
        /// <param name="source">The source of the trigger as <code>FrameworkElement</code></param>
        void ICommandTrigger.Initialize(FrameworkElement source)
        {
            foreach (ICommandTrigger child in this)
            {
                if (!initList.Contains(child))
                {
                    InitializeCommandSource(source, child);
                }
            }
        }
        #endregion
    }
}
