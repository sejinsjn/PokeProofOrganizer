/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : PropertyCommandParameter                                *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     : CommandParameter                                        *
/// *   FILENAME    : PropertyCommandParameter.cs                             *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;

namespace MVVMCore.Commands
{
    /// <summary><code>CommandParameter</code> used in the environment of <code>PropertyCommandTrigger</code> modules</summary>
    /// <typeparam name="TCustomParameter">The type of the custom parameter</typeparam>
    /// <typeparam name="TValue">The type of the value</typeparam>
    public class PropertyCommandParameter<TCustomParameter, TValue> : CommandParameter<TCustomParameter>
    {
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of <code>PropertyCommandParameter</code></summary>
        /// <param name="customParameter">The custom parameter of type <code>TCustomParameter</code></param>
        /// <param name="property">The <code>DependencyProperty</code> specifying the property of the parameter</param>
        /// <param name="value">The value</param>
        public PropertyCommandParameter(TCustomParameter customParameter,
                                        DependencyProperty property,
                                        TValue value)
            : base(customParameter)
        {
            this.Property = property;
            this.Value = value;
        }
        #endregion
        //
        #region ------------Properties Get/Set-------------
        /// <summary>Gets or (private)sets the <code>DependencyProperty</code> </summary>
        public DependencyProperty Property { get; private set; }
        /// <summary>Gets or (private)sets the value of the parameter of type <code>TValue</code></summary>
        public TValue Value { get; private set; }
        #endregion
        //
        #region --------------public methods---------------
        /// <summary>Casts a parameter provided as <code>Object</code> into a <code>PropertyCommandParameter</code></summary>
        /// <param name="parameter">The parameter as <code>Object</code></param>
        /// <returns>The <code>PropertyCommandParameter</code> as cast result</returns>
        /// <exception cref="InvalidCastException">Occurs, if the parameter is not of the correct type</exception>
        public new static PropertyCommandParameter<TCustomParameter, TValue> Cast(object parameter)
        {
            var parameterToCast = parameter as PropertyCommandParameter<object, object>;
            if (parameterToCast == null)
            {
                throw new InvalidCastException(string.Format("Failed to case {0} to {1}",
                    parameter.GetType(), typeof(PropertyCommandParameter<object, object>)));
            }

            var castedParameter = new PropertyCommandParameter<TCustomParameter, TValue>(
                (TCustomParameter)parameterToCast.CustomParameter,
                parameterToCast.Property,
                (TValue)parameterToCast.Value);

            return castedParameter;
        }
        #endregion
    }
}
