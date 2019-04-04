// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="OffsetAnimatorBase.cs" company="Zeroit Dev Technologies">
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
#region Imports

using System;
using System.ComponentModel;
using System.Drawing;
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{
    #region ExtendedPictureBoxOffsetAnimatorBase
    /// <summary>
    /// Base class inheriting <see cref="Animations.AnimatorBase" /> helping to
    /// animate thevone of the offset properties of an <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Animations.AnimatorBase" />
	public class ZeroitEXPicBoxOffsetAnimatorBase : Helpers.Animations.AnimatorBase
    {
        #region Fields

        /// <summary>
        /// The extended picture box
        /// </summary>
        private ZeroitEXPicBox _extendedPictureBox;
        /// <summary>
        /// The start offset
        /// </summary>
        private Point _startOffset;
        /// <summary>
        /// The end offset
        /// </summary>
        private Point _endOffset;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxOffsetAnimatorBase(IContainer container) : base(container)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxOffsetAnimatorBase()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            _startOffset = DefaultOffset;
            _endOffset = DefaultOffset;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the starting offset for the animation.
        /// </summary>
        /// <value>The start offset.</value>
        [Category("Appearance"), Browsable(true)]
        [Description("Gets or sets the starting offset for the animation.")]
        public Point StartOffset
        {
            get { return _startOffset; }
            set
            {
                if (_startOffset == value)
                    return;

                _startOffset = value;

                OnStartValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the ending offset for the animation.
        /// </summary>
        /// <value>The end offset.</value>
        [Category("Appearance"), Browsable(true)]
        [Description("Gets or sets the ending offset for the animation.")]
        public Point EndOffset
        {
            get { return _endOffset; }
            set
            {
                if (_endOffset == value)
                    return;

                _endOffset = value;

                OnEndValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox" /> which
        /// <see cref="ExtendedPictureBox" /> should be animated.
        /// </summary>
        /// <value>The zeroit ex pic box.</value>
        [Browsable(true), DefaultValue(null), Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public virtual ZeroitEXPicBox ZeroitEXPicBox
        {
            get { return _extendedPictureBox; }
            set
            {
                if (_extendedPictureBox == value)
                    return;

                _extendedPictureBox = value;

                base.ResetValues();
            }
        }

        #endregion

        #region Protected interface

        /// <summary>
        /// Unsafe getter and setter for inheriting classes to match
        /// to the actual property.
        /// </summary>
        /// <value>The current offset.</value>
        protected virtual Point CurrentOffset
        {
            get { return DefaultOffset; }
            set { }
        }

        /// <summary>
        /// Gets the default value for <see cref="StartOffset" /> and
        /// <see cref="EndOffset" />.
        /// </summary>
        /// <value>The default offset.</value>
        protected virtual Point DefaultOffset
        {
            get { return Point.Empty; }
        }

        /// <summary>
        /// Indicates the designer whether <see cref="StartOffset" /> needs
        /// to be serialized.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ShouldSerializeStartOffset()
        {
            return _startOffset == DefaultOffset;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="EndOffset" /> needs
        /// to be serialized.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ShouldSerializeEndOffset()
        {
            return _endOffset == DefaultOffset;
        }

        #endregion

        #region Overridden from AnimatorBase

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
        {
            get { return _extendedPictureBox == null ? DefaultOffset : CurrentOffset; }
            set
            {
                if (_extendedPictureBox != null)
                    CurrentOffset = (Point)value;
            }
        }

        /// <summary>
        /// Gets or sets the starting value for the animation.
        /// </summary>
        /// <value>The start value.</value>
        public override object StartValue
        {
            get { return StartOffset; }
            set { StartOffset = (Point)value; }
        }

        /// <summary>
        /// Gets or sets the ending value for the animation.
        /// </summary>
        /// <value>The end value.</value>
        public override object EndValue
        {
            get { return EndOffset; }
            set { EndOffset = (Point)value; }
        }

        /// <summary>
        /// Calculates an interpolated value between <see cref="StartValue" /> and
        /// <see cref="EndValue" /> for a given step in %.
        /// Giving 0 will return the <see cref="StartValue" />.
        /// Giving 100 will return the <see cref="EndValue" />.
        /// </summary>
        /// <param name="step">Animation step in %</param>
        /// <returns>Interpolated value for the given step.</returns>
        protected override object GetValueForStep(double step)
        {
            return InterpolatePoints(_startOffset, _endOffset, step);
        }

        #endregion
    }
    #endregion


}
