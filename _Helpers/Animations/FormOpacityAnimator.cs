// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-25-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="FormOpacityAnimator.cs" company="Zeroit Dev Technologies">
//    This program is for creating Image controls.
//    Copyright ©  2017  Zeroit Dev Technologies
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
//    You can contact me at zeroitdevnet@gmail.com or zeroitdev@outlook.com
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Zeroit.Framework.PictureBox.Helpers.Animations
{
    /// <summary>
    /// Class FormOpacityAnimator.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.PictureBox.Helpers.Animations.AnimatorBase" />
    public class FormOpacityAnimator : AnimatorBase
  {
        /// <summary>
        /// The start opacity
        /// </summary>
        private double _startOpacity = 1.0;
        /// <summary>
        /// The end opacity
        /// </summary>
        private double _endOpacity = 1.0;
        /// <summary>
        /// The default opacity
        /// </summary>
        private const double DEFAULT_OPACITY = 1.0;
        /// <summary>
        /// The form
        /// </summary>
        private Form _form;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormOpacityAnimator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public FormOpacityAnimator(IContainer container)
      : base(container)
    {
    }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormOpacityAnimator"/> class.
        /// </summary>
        public FormOpacityAnimator()
    {
    }

        /// <summary>
        /// Gets or sets the start opacity.
        /// </summary>
        /// <value>The start opacity.</value>
        [DefaultValue(1.0)]
    [Category("Appearance")]
    [TypeConverter(typeof (OpacityConverter))]
    [Description("Gets or sets the starting opacity for the animation.")]
    [Browsable(true)]
    public double StartOpacity
    {
      get
      {
        return this._startOpacity;
      }
      set
      {
        if (this._startOpacity == value)
          return;
        this._startOpacity = value;
        this.OnStartValueChanged(EventArgs.Empty);
      }
    }

        /// <summary>
        /// Gets or sets the end opacity.
        /// </summary>
        /// <value>The end opacity.</value>
        [DefaultValue(1.0)]
    [Browsable(true)]
    [TypeConverter(typeof (OpacityConverter))]
    [Description("Gets or sets the ending opacity for the animation.")]
    [Category("Appearance")]
    public double EndOpacity
    {
      get
      {
        return this._endOpacity;
      }
      set
      {
        if (this._endOpacity == value)
          return;
        this._endOpacity = value;
        this.OnEndValueChanged(EventArgs.Empty);
      }
    }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>The form.</value>
        [Browsable(true)]
    [Category("Behavior")]
    [Description("Gets or sets which Form should be animated.")]
    [DefaultValue(null)]
    [RefreshProperties(RefreshProperties.Repaint)]
    public Form Form
    {
      get
      {
        return this._form;
      }
      set
      {
        if (this._form == value)
          return;
        this._form = value;
        this.ResetValues();
      }
    }

        /// <summary>
        /// Gets or sets the current value internal.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
    {
      get
      {
        return (object) (this._form == null ? 0.0 : this._form.Opacity);
      }
      set
      {
        if (this._form == null)
          return;
        this._form.Opacity = (double) value;
      }
    }

        /// <summary>
        /// Gets or sets the start value.
        /// </summary>
        /// <value>The start value.</value>
        public override object StartValue
    {
      get
      {
        return (object) this.StartOpacity;
      }
      set
      {
        this.StartOpacity = (double) value;
      }
    }

        /// <summary>
        /// Gets or sets the end value.
        /// </summary>
        /// <value>The end value.</value>
        public override object EndValue
    {
      get
      {
        return (object) this.EndOpacity;
      }
      set
      {
        this.EndOpacity = (double) value;
      }
    }

        /// <summary>
        /// Gets the value for step.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>System.Object.</returns>
        protected override object GetValueForStep(double step)
    {
      return (object) AnimatorBase.InterpolateDoubleValues(this._startOpacity, this._endOpacity, step);
    }
  }
}
