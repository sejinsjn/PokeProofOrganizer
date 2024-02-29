/// ***************************************************************************
/// *   MODULNAME   : IActiveAware                                            *
/// *   MODULTYPE   : interface                                               *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : IActiveAware.cs                                         *
/// ***************************************************************************
/// </summary>
using System;

namespace MVVMCore
{
    /// <summary>Implementation of the an interface for awareness of the activation</summary>
    public interface IActiveAware
    {
        /// <summary>Gets or sets an indicator, if the component is active</summary>
        bool IsActive { get; set; }
        /// <summary>Occurs, if the active state changed</summary>
        event EventHandler IsActiveChanged;
    }
}
