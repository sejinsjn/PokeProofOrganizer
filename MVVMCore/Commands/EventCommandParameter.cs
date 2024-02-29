/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : EventCommandParameter                                   *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     : CommandParameter                                        *
/// *   FILENAME    : EventCommandParameter.cs                                *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;

namespace MVVMCore.Commands
{
    /// <summary><code>CommandParameter</code> to be used in the environment of <code>EventCommandTrigger</code></summary>
    /// <typeparam name="TCustomParameter">The type of the parameter</typeparam>
    /// <typeparam name="TEventArgs">the type of the event arguments based on <code>RoutedEventArgs</code></typeparam>
    public class EventCommandParameter<TCustomParameter, TEventArgs> : CommandParameter<TCustomParameter>
        where TEventArgs : RoutedEventArgs
    {
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of <code>EventCommandParameter</code></summary>
        /// <param name="customParameter">The custom parameter of type <code>TCustomParameter</code></param>
        /// <param name="routedEvent">The <code>RoutedEvent</code> specifying the event of the parameter</param>
        /// <param name="eventArgs">event arguments for the event as <code>TEventArgs</code></param>
        public EventCommandParameter(TCustomParameter customParameter,
                                     RoutedEvent routedEvent,
                                     TEventArgs eventArgs)
            : base(customParameter)
        {
            this.RoutedEvent = routedEvent;
            this.EventArgs = eventArgs;
        }
        #endregion
        //
        #region ------------Properties Get/Set-------------
        /// <summary>Gets or (private)sets the <code>RoutedEvent</code> for the parameter</summary>
        public RoutedEvent RoutedEvent { get; private set; }
        /// <summary>Event arguments of type <code>TEventArgs</code></summary>
        public TEventArgs EventArgs { get; private set; }
        #endregion
        //
        #region --------------public methods---------------
        /// <summary>Casts a parameter provided as <code>Object</code> into a <code>EventCommandParameter</code></summary>
        /// <param name="parameter">The parameter as <code>Object</code></param>
        /// <returns>The <code>EventCommandParameter</code> as cast result</returns>
        /// <exception cref="InvalidCastException">Occurs, if the parameter is not of the correct type</exception>
        public new static EventCommandParameter<TCustomParameter, TEventArgs> Cast(object parameter)
        {
            var parameterToCast = parameter as EventCommandParameter<object, RoutedEventArgs>;
            if (parameterToCast == null)
            {
                throw new InvalidCastException(string.Format("Failed to case {0} to {1}",
                    parameter.GetType(), typeof(EventCommandParameter<object, RoutedEventArgs>)));
            }

            var castedParameter = new EventCommandParameter<TCustomParameter, TEventArgs>(
                (TCustomParameter)parameterToCast.CustomParameter,
                parameterToCast.RoutedEvent,
                (TEventArgs)parameterToCast.EventArgs);

            return castedParameter;
        }
        #endregion
    }
}
