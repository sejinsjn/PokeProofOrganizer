/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : CommandParameter                                        *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     :                                                         *
/// *   FILENAME    : CommandParameter.cs                                     *
/// ***************************************************************************
/// </summary>
using System;

namespace MVVMCore.Commands
{
    /// <summary>Base class for command parameters</summary>
    /// <typeparam name="TCustomParameter">The type of the parameter</typeparam>
	public class CommandParameter<TCustomParameter>
	{
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of <code>CommandParameter</code></summary>
        /// <param name="customParameter">The custom parameter of the generic type <code>TCustomParameter</code></param>
        protected CommandParameter(TCustomParameter customParameter)
        {
            this.CustomParameter = customParameter;
        }
        #endregion
        //
        #region ------------Properties Get/Set-------------
        /// <summary>Gets or (private) sets the custom parameter of the generic type <code>TCustomParameter</code></summary>
        public TCustomParameter CustomParameter { get; private set; }
        #endregion

        #region --------------public methods---------------
        /// <summary>Casts a parameter provided as <code>Object</code> into a <code>CommandParameter</code></summary>
        /// <param name="parameter">The parameter as <code>Object</code></param>
        /// <returns>The <code>CommandParameter</code> as cast result</returns>
        /// <exception cref="InvalidCastException">Occurs, if the parameter is not of the correct type</exception>
        public static CommandParameter<TCustomParameter> Cast(object parameter)
        {
            var parameterToCast = parameter as CommandParameter<object>;
            if (parameterToCast == null)
            {
                throw new InvalidCastException(string.Format("Failed to case {0} to {1}",
                    parameter.GetType(), typeof(CommandParameter<object>)));
            }
            CommandParameter<TCustomParameter> castedParameter = null;
            if (parameterToCast.CustomParameter is TCustomParameter)
            {
                castedParameter = new CommandParameter<TCustomParameter>(
                    (TCustomParameter)parameterToCast.CustomParameter);
            }

            return castedParameter;
        }
        #endregion
	}
}
