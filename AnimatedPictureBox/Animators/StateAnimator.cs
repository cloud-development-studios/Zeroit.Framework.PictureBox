// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="StateAnimator.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System.ComponentModel;
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{
    #region ExtendedPictureBoxStateAnimator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ZeroitEXPicBox.State" /> of a
    /// <see cref="ZeroitEXPicBox" />.
    /// It can be altered by setting which parts of the state should be animated.
    /// </summary>
    /// <seealso cref="Animations.DummyAnimator" />
	public class ZeroitEXPicBoxStateAnimator : Helpers.Animations.DummyAnimator
    {
        #region Fields

        /// <summary>
        /// The components
        /// </summary>
        private System.ComponentModel.IContainer _components = null;

        /// <summary>
        /// The alpha animator
        /// </summary>
        private ZeroitEXPicBoxAlphaAnimator _alphaAnimator;
        /// <summary>
        /// The back color animator
        /// </summary>
        private Helpers.Animations.ControlBackColorAnimator _backColorAnimator;
        /// <summary>
        /// The back color2 animator
        /// </summary>
        private ZeroitEXPicBoxColorAnimator _backColor2Animator;
        /// <summary>
        /// The back color gradient rotation angle animator
        /// </summary>
        private ZeroitEXPicBoxGradientAnimator _backColorGradientRotationAngleAnimator;
        /// <summary>
        /// The rotation angle animator
        /// </summary>
        private ZeroitEXPicBoxAngleAnimator _rotationAngleAnimator;
        /// <summary>
        /// The extra image rotation angle animator
        /// </summary>
        private ZeroitEXPicBoxImageAnimator _extraImageRotationAngleAnimator;
        /// <summary>
        /// The zoom animator
        /// </summary>
        private ZeroitEXPicBoxZoomAnimator _zoomAnimator;
        /// <summary>
        /// The fore color animator
        /// </summary>
        private Helpers.Animations.ControlForeColorAnimator _foreColorAnimator;
        /// <summary>
        /// The text halo color animator
        /// </summary>
        private ZeroitEXPicBoxTextAnimator _textHaloColorAnimator;
        /// <summary>
        /// The text rotation angle animator
        /// </summary>
        private ZeroitEXPicBoxTextAngleAnimator _textRotationAngleAnimator;
        /// <summary>
        /// The text halo width animator
        /// </summary>
        private ZeroitEXPicBoxTextWidthAnimator _textHaloWidthAnimator;
        /// <summary>
        /// The text zoom animator
        /// </summary>
        private ZeroitEXPicBoxTextZoomAnimator _textZoomAnimator;
        /// <summary>
        /// The shadow offset animator
        /// </summary>
        private ZeroitEXPicBoxShadowAnimator _shadowOffsetAnimator;
        /// <summary>
        /// The image offset animator
        /// </summary>
        private ZeroitEXPicBoxImageOffsetAnimator _imageOffsetAnimator;
        /// <summary>
        /// The text offset animator
        /// </summary>
        private ZeroitEXPicBoxTextOffsetAnimator _textOffsetAnimator;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxStateAnimator(IContainer container) : base(container)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxStateAnimator()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            _components = new Container();
            _alphaAnimator = new ZeroitEXPicBoxAlphaAnimator(_components);
            _backColorAnimator = new Helpers.Animations.ControlBackColorAnimator(_components);
            _backColor2Animator = new ZeroitEXPicBoxColorAnimator(_components);
            _backColorGradientRotationAngleAnimator = new ZeroitEXPicBoxGradientAnimator(_components);
            _rotationAngleAnimator = new ZeroitEXPicBoxAngleAnimator(_components);
            _extraImageRotationAngleAnimator = new ZeroitEXPicBoxImageAnimator(_components);
            _zoomAnimator = new ZeroitEXPicBoxZoomAnimator(_components);
            _foreColorAnimator = new Helpers.Animations.ControlForeColorAnimator(_components);
            _textHaloColorAnimator = new ZeroitEXPicBoxTextAnimator(_components);
            _textRotationAngleAnimator = new ZeroitEXPicBoxTextAngleAnimator(_components);
            _textHaloWidthAnimator = new ZeroitEXPicBoxTextWidthAnimator(_components);
            _textZoomAnimator = new ZeroitEXPicBoxTextZoomAnimator(_components);
            _shadowOffsetAnimator = new ZeroitEXPicBoxShadowAnimator(_components);
            _imageOffsetAnimator = new ZeroitEXPicBoxImageOffsetAnimator(_components);
            _textOffsetAnimator = new ZeroitEXPicBoxTextOffsetAnimator(_components);
        }

        /// <summary>
        /// Frees used resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _components != null)
                _components.Dispose();

            base.Dispose(disposing);
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the starting state for the animation.
        /// </summary>
        /// <value>The start state.</value>
        [Category("Appearance")]
        [Browsable(true)]
        [Description("Gets or sets the starting state for the animation.")]
        public PictureBoxState StartState
        {
            get
            {
                return new PictureBoxState(_alphaAnimator.StartAlpha,
                    _rotationAngleAnimator.StartRotationAngle, _zoomAnimator.StartZoom,
                    _extraImageRotationAngleAnimator.StartRotationAngle,
                    _backColorGradientRotationAngleAnimator.StartRotationAngle,
                    _backColorAnimator.StartColor, _backColor2Animator.StartColor,
                    _foreColorAnimator.StartColor, _textHaloColorAnimator.StartColor,
                    _textHaloWidthAnimator.StartWidth, _textRotationAngleAnimator.StartRotationAngle,
                    _textZoomAnimator.StartZoom, _shadowOffsetAnimator.StartOffset,
                    _imageOffsetAnimator.StartOffset, _textOffsetAnimator.StartOffset);
            }
            set
            {
                _alphaAnimator.StartAlpha = value.Alpha;
                _rotationAngleAnimator.StartRotationAngle = value.RotationAngle;
                _zoomAnimator.StartZoom = value.Zoom;
                _extraImageRotationAngleAnimator.StartRotationAngle = value.ExtraImageRotationAngle;
                _backColorGradientRotationAngleAnimator.StartRotationAngle = value.BackColorGradientRotationAngle;
                _backColorAnimator.StartColor = value.BackColor;
                _backColor2Animator.StartColor = value.BackColor2;
                _foreColorAnimator.StartColor = value.ForeColor;
                _textHaloColorAnimator.StartColor = value.TextHaloColor;
                _textRotationAngleAnimator.StartRotationAngle = value.RotationAngle;
                _textHaloWidthAnimator.StartWidth = value.TextHaloWidth;
                _textZoomAnimator.StartZoom = value.TextZoom;
                _shadowOffsetAnimator.StartOffset = value.ShadowOffset;
                _imageOffsetAnimator.StartOffset = value.ImageOffset;
                _textOffsetAnimator.StartOffset = value.TextOffset;
            }
        }

        /// <summary>
        /// Gets or sets the ending state for the animation.
        /// </summary>
        /// <value>The end state.</value>
        [Category("Appearance")]
        [Browsable(true)]
        [Description("Gets or sets the ending state for the animation.")]
        public PictureBoxState EndState
        {
            get
            {
                return new PictureBoxState(_alphaAnimator.EndAlpha,
                    _rotationAngleAnimator.EndRotationAngle, _zoomAnimator.EndZoom,
                    _extraImageRotationAngleAnimator.EndRotationAngle,
                    _backColorGradientRotationAngleAnimator.EndRotationAngle,
                    _backColorAnimator.EndColor, _backColor2Animator.EndColor,
                    _foreColorAnimator.EndColor, _textHaloColorAnimator.EndColor,
                    _textHaloWidthAnimator.EndWidth, _textRotationAngleAnimator.EndRotationAngle,
                    _textZoomAnimator.EndZoom, _shadowOffsetAnimator.EndOffset,
                    _imageOffsetAnimator.EndOffset, _textOffsetAnimator.EndOffset);
            }
            set
            {
                _alphaAnimator.EndAlpha = value.Alpha;
                _rotationAngleAnimator.EndRotationAngle = value.RotationAngle;
                _zoomAnimator.EndZoom = value.Zoom;
                _extraImageRotationAngleAnimator.EndRotationAngle = value.ExtraImageRotationAngle;
                _backColorGradientRotationAngleAnimator.EndRotationAngle = value.BackColorGradientRotationAngle;
                _backColorAnimator.EndColor = value.BackColor;
                _backColor2Animator.EndColor = value.BackColor2;
                _foreColorAnimator.EndColor = value.ForeColor;
                _textHaloColorAnimator.EndColor = value.TextHaloColor;
                _textRotationAngleAnimator.EndRotationAngle = value.TextRotationAngle;
                _textHaloWidthAnimator.EndWidth = value.TextHaloWidth;
                _textZoomAnimator.EndZoom = value.TextZoom;
                _shadowOffsetAnimator.EndOffset = value.ShadowOffset;
                _imageOffsetAnimator.EndOffset = value.ImageOffset;
                _textOffsetAnimator.EndOffset = value.TextOffset;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox" /> which
        /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.State" /> should be animated.
        /// </summary>
        /// <value>The zeroit ex pic box.</value>
        [Browsable(true), DefaultValue(null), Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public ZeroitEXPicBox ZeroitEXPicBox
        {
            get { return _alphaAnimator.ZeroitEXPicBox; }
            set
            {
                _alphaAnimator.ZeroitEXPicBox = value;
                _backColorAnimator.Control = value;
                _backColor2Animator.ZeroitEXPicBox = value;
                _backColorGradientRotationAngleAnimator.ZeroitEXPicBox = value;
                _rotationAngleAnimator.ZeroitEXPicBox = value;
                _extraImageRotationAngleAnimator.ZeroitEXPicBox = value;
                _zoomAnimator.ExtendedPictureBox = value;
                _foreColorAnimator.Control = value;
                _textHaloColorAnimator.ZeroitEXPicBox = value;
                _textRotationAngleAnimator.ZeroitEXPicBox = value;
                _textHaloWidthAnimator.ZeroitEXPicBox = value;
                _textZoomAnimator.ZeroitEXPicBox = value;
                _shadowOffsetAnimator.ZeroitEXPicBox = value;
                _imageOffsetAnimator.ZeroitEXPicBox = value;
                _textOffsetAnimator.ZeroitEXPicBox = value;
            }
        }

        /// <summary>
        /// Sets or gets which properties of a given <see cref="ExtendedPictureBox" />
        /// should be animated.
        /// </summary>
        /// <value>The animated properties.</value>
        public PictureBoxStateProperties AnimatedProperties
        {
            get
            {
                PictureBoxStateProperties result = PictureBoxStateProperties.None;
                if (_alphaAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.Alpha;
                if (_backColorAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.BackColor;
                if (_backColor2Animator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.BackColor2;
                if (_backColorGradientRotationAngleAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.BackColorGradientRotationAngle;
                if (_rotationAngleAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.RotationAngle;
                if (_extraImageRotationAngleAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.ExtraImageRotationAngle;
                if (_zoomAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.Zoom;
                if (_foreColorAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.ForeColor;
                if (_textHaloColorAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.TextHaloColor;
                if (_textRotationAngleAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.TextRotationAngle;
                if (_textHaloWidthAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.TextHaloWidth;
                if (_textZoomAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.TextZoom;
                if (_shadowOffsetAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.ShadowOffset;
                if (_imageOffsetAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.ImageOffset;
                if (_textOffsetAnimator.ParentAnimator == this)
                    result |= PictureBoxStateProperties.TextOffset;
                return result;
            }
            set
            {
                _alphaAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.Alpha) ? this : null;
                _backColorAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.BackColor) ? this : null;
                _backColor2Animator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.BackColor2) ? this : null;
                _backColorGradientRotationAngleAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.BackColorGradientRotationAngle) ? this : null;
                _rotationAngleAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.RotationAngle) ? this : null;
                _extraImageRotationAngleAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.ExtraImageRotationAngle) ? this : null;
                _zoomAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.Zoom) ? this : null;
                _foreColorAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.ForeColor) ? this : null;
                _textHaloColorAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.TextHaloColor) ? this : null;
                _textRotationAngleAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.TextRotationAngle) ? this : null;
                _textHaloWidthAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.TextHaloWidth) ? this : null;
                _textZoomAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.TextZoom) ? this : null;
                _shadowOffsetAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.ShadowOffset) ? this : null;
                _imageOffsetAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.ImageOffset) ? this : null;
                _textOffsetAnimator.ParentAnimator = PictureBoxState.IsPropertySet(value, PictureBoxStateProperties.TextOffset) ? this : null;
            }
        }

        /// <summary>
        /// Gets or sets whether a given <see cref="PictureBoxStateProperties" /> is set
        /// in <see cref="AnimatedProperties" />.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool this[PictureBoxStateProperties property]
        {
            get { return PictureBoxState.IsPropertySet(this.AnimatedProperties, property); }
            set
            {
                if (value)
                    this.AnimatedProperties = this.AnimatedProperties | property;
                else
                    this.AnimatedProperties = this.AnimatedProperties & ~property;
            }
        }

        #endregion
    }
    #endregion


}
