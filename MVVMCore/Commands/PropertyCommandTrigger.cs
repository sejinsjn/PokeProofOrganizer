/// <summary>
/// ***************************************************************************
/// *   MODULNAME   : PropertyCommandTrigger                                  *
/// *   MODULTYPE   : class                                                   *
/// *   IMPLEMENTS  :                                                         *
/// *   EXTENDS     : CommandTrigger                                          *
/// *   FILENAME    : PropertyCommandTrigger.cs                               *
/// ***************************************************************************
/// </summary>
using System;
using System.Windows;
using System.ComponentModel;

namespace MVVMCore.Commands
{
    /// <summary><code>CommandTrigger</code> triggered by property value change</summary>
	public sealed class PropertyCommandTrigger : CommandTrigger
	{
        #region ---------------Construction----------------
        /// <summary>Initializes a new instance of the <code>PropertyCommandTrigger</code></summary>
        /// <returns>The <code>PropertyCommandTrigger</code> as <code>Freezable</code></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new PropertyCommandTrigger();
        }
        #endregion
        //
        #region -----------Dependency Properties-----------
        //
        #region -----------------Property------------------
        /// <summary>Identifies the Property dependency property</summary>
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(PropertyCommandTrigger),
            new FrameworkPropertyMetadata(null));
        /// <summary>Gets or sets the description for the Property property</summary>
        public DependencyProperty Property
        {
            get { return (DependencyProperty)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }
        #endregion
        //
        #region -------------------Value-------------------
        /// <summary>Identifies the Value dependency property</summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(PropertyCommandTrigger),
            new FrameworkPropertyMetadata(null));
        /// <summary>Gets or sets the description for the Value property</summary>
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion
        //
        #endregion
        //
        #region ----------------T Property-----------------
        /// <summary>Identifies the T dependency property</summary>
		public static readonly DependencyProperty TProperty =
			DependencyProperty.Register("T", typeof(object), typeof(PropertyCommandTrigger),
			new FrameworkPropertyMetadata(null, OnTChanged));
        /// <summary>Gets or sets the description for the T property</summary>
		public object T
		{
			get { return (object)GetValue(TProperty); }
			set { SetValue(TProperty, value); }
		}
		/// <summary>Invoked on T change.</summary>
		/// <param name="d">The object that was changed</param>
		/// <param name="e">Dependency property changed event arguments</param>
		static void OnTChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		#endregion
        //
        #region -------------protected methods-------------
        /// <summary>Initializes the core of the <code>CommandTrigger</code> based on properties</summary>
        /// <param name="source">The source as <code>FrameworkElement</code></param>
        protected override void InitializeCore(FrameworkElement source)
        {
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(Property, source.GetType());
            descriptor.AddValueChanged(source, (s, e) =>
            {
                CommandParameter<object> parameter = new PropertyCommandParameter<object, object>(
                    CustomParameter, Property, source.GetValue(Property));

                object value = Value;
                if (descriptor.Converter.CanConvertFrom(typeof(string)))
                {
                    value = descriptor.Converter.ConvertFromString(Value);
                }

                if (object.Equals(source.GetValue(Property), value))
                    ExecuteCommand(parameter);
            });
        }
        #endregion
	}
}
