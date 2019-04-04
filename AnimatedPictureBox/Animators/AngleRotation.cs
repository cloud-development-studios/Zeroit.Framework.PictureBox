// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="AngleRotation.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
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
    #region ExtendedPictureBoxRotationAngleAnimator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.RotationAngle" /> of a
    /// <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Animations.AnimatorBase" />
	public class ZeroitEXPicBoxAngleAnimator : Helpers.Animations.AnimatorBase
    {
        #region Fields

        /// <summary>
        /// The default rotation angle
        /// </summary>
        private const float DEFAULT_ROTATION_ANGLE = 0f;

        /// <summary>
        /// The extended picture box
        /// </summary>
        private ZeroitEXPicBox _extendedPictureBox;
        /// <summary>
        /// The start rotation angle
        /// </summary>
        private float _startRotationAngle;
        /// <summary>
        /// The end rotation angle
        /// </summary>
        private float _endRotationAngle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxAngleAnimator(IContainer container) : base(container)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxAngleAnimator()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            _startRotationAngle = DEFAULT_ROTATION_ANGLE;
            _endRotationAngle = DEFAULT_ROTATION_ANGLE;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the starting rotation angle for the animation.
        /// </summary>
        /// <value>The start rotation angle.</value>
        [Category("Appearance"), DefaultValue(DEFAULT_ROTATION_ANGLE)]
        [Browsable(true)]
        [Description("Gets or sets the starting rotation angle for the animation.")]
        public float StartRotationAngle
        {
            get { return _startRotationAngle; }
            set
            {
                if (_startRotationAngle == value)
                    return;

                _startRotationAngle = value;

                OnStartValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the ending rotation angle for the animation.
        /// </summary>
        /// <value>The end rotation angle.</value>
        [Category("Appearance"), DefaultValue(DEFAULT_ROTATION_ANGLE)]
        [Browsable(true)]
        [Description("Gets or sets the ending rotation angle for the animation.")]
        public float EndRotationAngle
        {
            get { return _endRotationAngle; }
            set
            {
                if (_endRotationAngle == value)
                    return;

                _endRotationAngle = value;

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

                if (ZeroitEXPicBoxInternal != null)
                    ZeroitEXPicBoxInternal.RotationAngleChanged -= new EventHandler(OnCurrentValueChanged);

                _extendedPictureBox = value;

                if (ZeroitEXPicBoxInternal != null)
                    ZeroitEXPicBoxInternal.RotationAngleChanged += new EventHandler(OnCurrentValueChanged);

                base.ResetValues();
            }
        }

        #endregion

        #region Protected interface

        /// <summary>
        /// Internal accessor for the animated <see cref="ZeroitEXPicBox" />.
        /// </summary>
        /// <value>The zeroit ex pic box internal.</value>
        protected ZeroitEXPicBox ZeroitEXPicBoxInternal
        {
            get { return _extendedPictureBox; }
            set { _extendedPictureBox = value; }
        }

        #endregion

        #region Overridden from AnimatorBase

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
        {
            get { return _extendedPictureBox == null ? (float)0 : _extendedPictureBox.RotationAngle; }
            set
            {
                if (_extendedPictureBox != null)
                    _extendedPictureBox.RotationAngle = (float)value;
            }
        }

        /// <summary>
        /// Gets or sets the starting value for the animation.
        /// </summary>
        /// <value>The start value.</value>
        public override object StartValue
        {
            get { return StartRotationAngle; }
            set { StartRotationAngle = (float)value; }
        }

        /// <summary>
        /// Gets or sets the ending value for the animation.
        /// </summary>
        /// <value>The end value.</value>
        public override object EndValue
        {
            get { return EndRotationAngle; }
            set { EndRotationAngle = (float)value; }
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
            float result = (float)InterpolateDoubleValues(_startRotationAngle, _endRotationAngle, step);
            return (float)InterpolateDoubleValues(_startRotationAngle, _endRotationAngle, step);
        }

        #endregion
    }
    #endregion


}
