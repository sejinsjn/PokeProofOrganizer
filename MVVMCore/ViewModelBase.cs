/// ***************************************************************************
/// *   MODULNAME   : ViewModelBase                                           *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  : INotifyPropertyChanged, INotifyDataErrorInfo            *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : ViewModelBase.cs                                        *
/// ***************************************************************************
/// </summary>
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMCore
{
    [Serializable]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnAfterActivated()
        {
        }
        
        public abstract void Initialize(object data);

        public abstract void OnBeforeClose();
      
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
