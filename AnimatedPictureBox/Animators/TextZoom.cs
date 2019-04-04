// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="TextZoom.cs" company="Zeroit Dev Technologies">
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
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{
    #region ExtendedPictureBoxTextZoomAnimator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.TextZoom" /> of a
    /// <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Animations.AnimatorBase" />
	public class ZeroitEXPicBoxTextZoomAnimator : Helpers.Animations.AnimatorBase
    {
        #region Fields

        /// <summary>
        /// The default zoom
        /// </summary>
        private const float DEFAULT_ZOOM = 100f;

        /// <summary>
        /// The extended picture box
        /// </summary>
        private ZeroitEXPicBox _extendedPictureBox;
        /// <summary>
        /// The start zoom
        /// </summary>
        private float _startZoom;
        /// <summary>
        /// The end zoom
        /// </summary>
        private float _endZoom;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxTextZoomAnimator(IContainer container) : base(container)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxTextZoomAnimator()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            _startZoom = DEFAULT_ZOOM;
            _endZoom = DEFAULT_ZOOM;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the starting zoom for the animation.
        /// </summary>
        /// <value>The start zoom.</value>
        [Category("Appearance"), DefaultValue(DEFAULT_ZOOM)]
        [Browsable(true)]
        [Description("Gets or sets the starting zoom for the animation.")]
        public float StartZoom
        {
            get { return _startZoom; }
            set
            {
                if (_startZoom == value)
                    return;

                _startZoom = value;

                OnStartValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the ending zoom for the animation.
        /// </summary>
        /// <value>The end zoom.</value>
        [Category("Appearance"), DefaultValue(DEFAULT_ZOOM)]
        [Browsable(true)]
        [Description("Gets or sets the ending zoom for the animation.")]
        public float EndZoom
        {
            get { return _endZoom; }
            set
            {
                if (_endZoom == value)
                    return;

                _endZoom = value;

                OnEndValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox" /> which
        /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.TextZoom" /> should be animated.
        /// </summary>
        /// <value>The zeroit ex pic box.</value>
        [Browsable(true), DefaultValue(null), Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public ZeroitEXPicBox ZeroitEXPicBox
        {
            get { return _extendedPictureBox; }
            set
            {
                if (_extendedPictureBox == value)
                    return;

                if (_extendedPictureBox != null)
                    _extendedPictureBox.TextZoomChanged -= new EventHandler(OnCurrentValueChanged);

                _extendedPictureBox = value;

                if (_extendedPictureBox != null)
                    _extendedPictureBox.TextZoomChanged += new EventHandler(OnCurrentValueChanged);

                base.ResetValues();
            }
        }

        #endregion

        #region Overridden from AnimatorBase

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
        {
            get { return _extendedPictureBox == null ? (float)0 : _extendedPictureBox.TextZoom; }
            set
            {
                if (_extendedPictureBox != null)
                    _extendedPictureBox.TextZoom = (float)value;
            }
        }

        /// <summary>
        /// Gets or sets the starting value for the animation.
        /// </summary>
        /// <value>The start value.</value>
        public override object StartValue
        {
            get { return StartZoom; }
            set { StartZoom = (float)value; }
        }

        /// <summary>
        /// Gets or sets the ending value for the animation.
        /// </summary>
        /// <value>The end value.</value>
        public override object EndValue
        {
            get { return EndZoom; }
            set { EndZoom = (float)value; }
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
            float result = (float)InterpolateDoubleValues(_startZoom, _endZoom, step);
            return (float)InterpolateDoubleValues(_startZoom, _endZoom, step);
        }

        #endregion
    }
    #endregion


}
