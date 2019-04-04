// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ExtendedPictureBox.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region ExtendedPictureBox
    
    /// <summary>
    /// Control which displays an image with many extra properties.
    /// The background can be a gradient in any angle between two colors.
    /// The image can be adjusted regarding it's transparency, rotation and sizing.
    /// A small extra image can be added at a specified position.
    /// A border of different styles and color can be added.
    /// </summary>
    public class ZeroitEXPicBox : UserControl
    {
        #region Events

        /// <summary>
        /// Event which gets fired when <see cref="Image"/> has changed.
        /// </summary>
        public event EventHandler ImageChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ExtraImage"/> has changed.
        /// </summary>
        public event EventHandler ExtraImageChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ExtraImageAlignment"/> has changed.
        /// </summary>
        public event EventHandler ExtraImageAlignmentChanged;

        /// <summary>
        /// Event which gets fired when <see cref="BackColor2"/> has changed.
        /// </summary>
        public event EventHandler BackColor2Changed;

        /// <summary>
        /// Event which gets fired when <see cref="BackColorGradientRotationAngle"/> has changed.
        /// </summary>
        public event EventHandler BackColorGradientRotationAngleChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ExtraImageRotationAngle"/> has changed.
        /// </summary>
        public event EventHandler ExtraImageRotationAngleChanged;

        /// <summary>
        /// Event which gets fired when <see cref="Zoom"/> has changed.
        /// </summary>
        public event EventHandler ZoomChanged;

        /// <summary>
        /// Event which gets fired when <see cref="RotationAngle"/> has changed.
        /// </summary>
        public event EventHandler RotationAngleChanged;

        /// <summary>
        /// Event which gets fired when <see cref="Alpha"/> has changed.
        /// </summary>
        public event EventHandler AlphaChanged;

        /// <summary>
        /// Event which gets fired when <see cref="State"/> has changed.
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="BaseSizeMode"/> has changed.
        /// </summary>
        public event EventHandler BaseSizeModeChanged;

        /// <summary>
        /// Event which gets fired when <see cref="BorderStyle"/> has changed.
        /// </summary>
        public event EventHandler BorderStyleChanged;

        /// <summary>
        /// Event which gets fired when <see cref="BorderColor"/> has changed.
        /// </summary>
        public event EventHandler BorderColorChanged;

        /// <summary>
        /// Event which gets fired when <see cref="AllowDisabledPainting"/> has changed.
        /// </summary>
        public event EventHandler AllowDisabledPaintingChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextAlign"/> has changed.
        /// </summary>
        public event EventHandler TextAlignChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextRotationAngle"/> has changed.
        /// </summary>
        public event EventHandler TextRotationAngleChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextHaloWidth"/> has changed.
        /// </summary>
        public event EventHandler TextHaloWidthChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextHaloColor"/> has changed.
        /// </summary>
        public event EventHandler TextHaloColorChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextZoom"/> has changed.
        /// </summary>
        public event EventHandler TextZoomChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ShadowOffset"/> has changed.
        /// </summary>
        public event EventHandler ShadowOffsetChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ShadowMode"/> has changed.
        /// </summary>
        public event EventHandler ShadowModeChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextOffset"/> has changed.
        /// </summary>
        public event EventHandler TextOffsetChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ImageOffset"/> has changed.
        /// </summary>
        public event EventHandler ImageOffsetChanged;

        #endregion

        #region Fields

        private const ContentAlignment DEFAULT_EXTRA_IMAGE_ALIGNMENT = ContentAlignment.BottomRight;
        private const float DEFAULT_ZOOM = 100f;
        private const byte DEFAULT_ALPHA = 255;
        private const float DEFAULT_ROTATION_ANGLE = 0f;
        private const BaseSizeMode DEFAULT_BASE_SIZE_MODE = BaseSizeMode.Normal;
        private const ButtonBorderStyle DEFAULT_BORDER_STYLE = ButtonBorderStyle.Solid;
        private const float DEFAULT_BACKCOLOR_GRADIENT_ROTATION_ANGLE = 0f;
        private const float DEFAULT_EXTRA_IMAGE_ROTATION_ANGLE = 0f;
        private const bool DEFAULT_ALLOW_DISABLED_PAINTING = true;
        private const ContentAlignment DEFAULT_TEXT_ALIGNMENT = ContentAlignment.MiddleCenter;
        private const float DEFAULT_TEXT_ROTATION_ANGLE = 0f;
        private const float DEFAULT_TEXT_HALO_WIDTH = 0f;
        private const float DEFAULT_TEXT_ZOOM = 100f;
        private const ShadowMode DEFAULT_SHADOW_MODE = ShadowMode.Off;

        private Image _image = null;
        private Image _extraImage = null;
        private ContentAlignment _extraImageAlignment = DEFAULT_EXTRA_IMAGE_ALIGNMENT;
        private float _zoom = DEFAULT_ZOOM;
        private byte _alpha = DEFAULT_ALPHA;
        private float _rotationAngle = DEFAULT_ROTATION_ANGLE;
        private BaseSizeMode _baseSizeMode = DEFAULT_BASE_SIZE_MODE;
        private ButtonBorderStyle _borderStyle = DEFAULT_BORDER_STYLE;
        private Color _borderColor;
        private bool _allowDisabledPainting = DEFAULT_ALLOW_DISABLED_PAINTING;

        private Color _backColor2;
        private float _backColorGradientRotationAngle = DEFAULT_BACKCOLOR_GRADIENT_ROTATION_ANGLE;
        private float _extraImageRotationAngle = DEFAULT_EXTRA_IMAGE_ROTATION_ANGLE;

        private ContentAlignment _textAlign = DEFAULT_TEXT_ALIGNMENT;
        private float _textRotationAngle = DEFAULT_TEXT_ROTATION_ANGLE;
        private float _textZoom = DEFAULT_TEXT_ZOOM;
        private float _textHaloWidth = DEFAULT_TEXT_HALO_WIDTH;
        private Color _textHaloColor;
        private string _text;
        private StringFormat _stringFormat;

        private ShadowMode _shadowMode = DEFAULT_SHADOW_MODE;
        private Point _shadowOffset;
        private Point _textOffset;
        private Point _imageOffset;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBox()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);

            _backColor2 = this.BackColor;
            _textHaloColor = this.ForeColor;

            _borderColor = DefaultBorderColor;
            _shadowOffset = DefaultShadowOffset;
            _textOffset = DefaultTextOffset;
            _imageOffset = DefaultImageOffset;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets a complete description of the current visual state of the
        /// control only missing the two images, the <see cref="ExtraImageAlignment"/>,
        /// <see cref="BaseSizeMode"/> and <see cref="BorderStyle"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PictureBoxState State
        {
            get
            {
                return new PictureBoxState(_alpha, _rotationAngle, _zoom, _extraImageRotationAngle,
                    _backColorGradientRotationAngle, this.BackColor, _backColor2, this.ForeColor,
                    _textHaloColor, _textHaloWidth, _textRotationAngle, _textZoom, _shadowOffset,
                    _imageOffset, _textOffset);
            }
            set
            {
                if (State == value)
                    return;

                _alpha = value.Alpha;
                _rotationAngle = value.RotationAngle;
                _zoom = value.Zoom;
                _extraImageRotationAngle = value.ExtraImageRotationAngle;
                _backColorGradientRotationAngle = value.BackColorGradientRotationAngle;
                BackColor = value.BackColor;
                _backColor2 = value.BackColor2;
                this.ForeColor = value.ForeColor;
                _textHaloColor = value.TextHaloColor;
                _textHaloWidth = value.TextHaloWidth;
                _textRotationAngle = value.TextRotationAngle;
                _textZoom = value.TextZoom;
                _shadowOffset = value.ShadowOffset;
                _imageOffset = value.ImageOffset;
                _textOffset = value.TextOffset;

                OnStateChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        #region Dynamic properties

        /// <summary>
        /// Gets or sets the second color to draw the background gradient.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the second color to draw the background gradient.")]
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Color BackColor2
        {
            get { return _backColor2; }
            set
            {
                if (_backColor2 == value)
                    return;

                _backColor2 = value;

                OnBackColor2Changed(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Angle of the background gradient in degrees.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_BACKCOLOR_GRADIENT_ROTATION_ANGLE)]
        [Description("Angle of the background gradient in degrees.")]
        public virtual float BackColorGradientRotationAngle
        {
            get { return _backColorGradientRotationAngle; }
            set
            {
                if (_backColorGradientRotationAngle == value)
                    return;

                _backColorGradientRotationAngle = value;

                OnBackColorGradientRotationAngleChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Angle of the <see cref="ExtraImage"/> in degrees.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_EXTRA_IMAGE_ROTATION_ANGLE)]
        [Description("Angle of the extra image in degrees.")]
        public virtual float ExtraImageRotationAngle
        {
            get { return _extraImageRotationAngle; }
            set
            {
                if (_extraImageRotationAngle == value)
                    return;

                _extraImageRotationAngle = value;

                OnExtraImageRotationAngleChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the zoom factor with which the main image should be drawn.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_ZOOM)]
        [Description("Gets or sets the zoom factor with which the main image should be drawn.")]
        public virtual float Zoom
        {
            get { return _zoom; }
            set
            {
                if (_zoom == value)
                    return;

                _zoom = value;

                OnZoomChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the main image in degrees.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_ROTATION_ANGLE)]
        [Description("Gets or sets the rotation angle of the main image in degrees.")]
        public virtual float RotationAngle
        {
            get { return _rotationAngle; }
            set
            {
                if (_rotationAngle == value)
                    return;

                _rotationAngle = value;

                OnRotationAngleChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the alpha value which should be applied to the <see cref="Image"/>.
        /// The alpha value is calcualted on a per pixel basis and pixels already
        /// having an alpha value less then 255 will be reduced further. The effect is
        /// that transparent parts of an image will remain transparent.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_ALPHA)]
        [Description("Gets or sets the alpha value which should be applied to the image.")]
        public virtual byte Alpha
        {
            get { return _alpha; }
            set
            {
                if (_alpha == value)
                    return;

                _alpha = value;

                OnAlphaChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the text in degrees.
        /// </summary>
        [Description("Gets or sets the rotation angle of the text in degrees.")]
        [Browsable(true), DefaultValue(DEFAULT_TEXT_ROTATION_ANGLE), Category("Appearance")]
        public virtual float TextRotationAngle
        {
            get { return _textRotationAngle; }
            set
            {
                if (_textRotationAngle == value)
                    return;

                _textRotationAngle = value;

                OnTextRotationAngleChanged(EventArgs.Empty);
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the width of the halo of the text.
        /// 0 or smaller if now halo should be shown.
        /// </summary>
        [Description("Gets or sets the width of the halo of the text.")]
        [Browsable(true), DefaultValue(DEFAULT_TEXT_HALO_WIDTH), Category("Appearance")]
        public virtual float TextHaloWidth
        {
            get { return _textHaloWidth; }
            set
            {
                if (_textHaloWidth == value)
                    return;

                _textHaloWidth = value;

                OnTextHaloWidthChanged(EventArgs.Empty);
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the width of the color of the halo of the text.
        /// </summary>
        [Description("Gets or sets the width of the color of the halo of the text.")]
        [Browsable(true), Category("Appearance")]
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Color TextHaloColor
        {
            get { return _textHaloColor; }
            set
            {
                if (_textHaloColor == value)
                    return;

                _textHaloColor = value;

                OnTextHaloColorChanged(EventArgs.Empty);
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the zoom factor with which the text should be drawn.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_TEXT_ZOOM)]
        [Description("Gets or sets the zoom factor with which the text should be drawn.")]
        public virtual float TextZoom
        {
            get { return _textZoom; }
            set
            {
                if (_textZoom == value)
                    return;

                _textZoom = value;

                OnTextZoomChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the offset of the main image shadow.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the offset of the main image shadow.")]
        public virtual Point ShadowOffset
        {
            get { return _shadowOffset; }
            set
            {
                if (_shadowOffset == value)
                    return;

                _shadowOffset = value;

                OnShadowOffsetChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the offset of the main image.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the offset of the main image.")]
        public virtual Point ImageOffset
        {
            get { return _imageOffset; }
            set
            {
                if (_imageOffset == value)
                    return;

                _imageOffset = value;

                OnImageOffsetChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the offset of the text.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the offset of the text.")]
        public virtual Point TextOffset
        {
            get { return _textOffset; }
            set
            {
                if (_textOffset == value)
                    return;

                _textOffset = value;

                OnTextOffsetChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        #endregion

        #region Constant properties

        /// <summary>
        /// Gets or sets the color of the surrounding border.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the color of the surrounding border.")]
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor == value)
                    return;

                _borderColor = value;

                OnBorderColorChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the main image to be shown.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Gets or sets the main image to be shown.")]
        public Image Image
        {
            get { return _image; }
            set
            {
                if (_image == value)
                    return;

                _image = value;

                OnImageChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a little extra image to be shown.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Gets or sets a little extra image to be shown.")]
        public Image ExtraImage
        {
            get { return _extraImage; }
            set
            {
                if (_extraImage == value)
                    return;

                _extraImage = value;

                OnExtraImageChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the <see cref="ExtraImage"/>.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_EXTRA_IMAGE_ALIGNMENT)]
        [Description("Gets or sets the alignment of the extra image.")]
        public ContentAlignment ExtraImageAlignment
        {
            get { return _extraImageAlignment; }
            set
            {
                if (_extraImageAlignment == value)
                    return;

                _extraImageAlignment = value;

                OnExtraImageAlignmentChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the mode how the 100% zoom size is calculated.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_BASE_SIZE_MODE)]
        [Description("Gets or sets the mode how the 100% zoom size is calculated.")]
        public BaseSizeMode BaseSizeMode
        {
            get { return _baseSizeMode; }
            set
            {
                if (_baseSizeMode == value)
                    return;

                _baseSizeMode = value;

                OnBaseSizeModeChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the style of the surrounding border.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_BORDER_STYLE)]
        [Description("Gets or sets the style of the surrounding border.")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (_borderStyle == value)
                    return;

                _borderStyle = value;

                OnBorderStyleChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the images will be painted specially when
        /// <see cref="Control.Enabled"/> is set to false.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_ALLOW_DISABLED_PAINTING)]
        [Description("Gets or sets whether the images will be painted specially when disabled.")]
        public bool AllowDisabledPainting
        {
            get { return _allowDisabledPainting; }
            set
            {
                if (_allowDisabledPainting == value)
                    return;

                _allowDisabledPainting = value;

                OnAllowDisabledPaintingChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text.
        /// </summary>
        [Description("Gets or sets the alignment of the text.")]
        [Browsable(true), DefaultValue(DEFAULT_TEXT_ALIGNMENT), Category("Appearance")]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                if (_textAlign == value)
                    return;

                _textAlign = value;
                ClearStringFormat();
                base.Invalidate();
                OnTextAlignChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether a shadow of the main image should be drawn
        /// and how the offset is calculated.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_SHADOW_MODE)]
        [Description("Gets or sets whether a shadow of the main image should be drawn and how the offset is calculated.")]
        public virtual ShadowMode ShadowMode
        {
            get { return _shadowMode; }
            set
            {
                if (_shadowMode == value)
                    return;

                _shadowMode = value;

                OnShadowModeChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        #endregion

        #endregion

        #region Protected interface

        #region ShouldSerialize

        /// <summary>
        /// Indicates the designer whether <see cref="BackColor2"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeBackColor2()
        {
            return _backColor2 != base.BackColor;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="TextHaloColor"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeTextHaloColor()
        {
            return _textHaloColor != base.ForeColor;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="BorderColor"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeBorderColor()
        {
            return _borderColor != DefaultBorderColor;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="ShadowOffset"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeShadowOffset()
        {
            return _shadowOffset != DefaultShadowOffset;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="ImageOffset"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeImageOffset()
        {
            return _imageOffset != DefaultImageOffset;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="TextOffset"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeTextOffset()
        {
            return _textOffset != DefaultTextOffset;
        }

        #endregion

        #region Defaults

        /// <summary>
        /// Gets the default value for <see cref="ShadowOffset"/>.
        /// </summary>
        protected virtual Point DefaultShadowOffset
        {
            get { return Point.Empty; }
        }

        /// <summary>
        /// Gets the default value for <see cref="TextOffset"/>.
        /// </summary>
        protected virtual Point DefaultTextOffset
        {
            get { return Point.Empty; }
        }

        /// <summary>
        /// Gets the default value for <see cref="ImageOffset"/>.
        /// </summary>
        protected virtual Point DefaultImageOffset
        {
            get { return Point.Empty; }
        }

        /// <summary>
        /// Gets the default value for <see cref="BorderColor"/>.
        /// </summary>
        protected virtual Color DefaultBorderColor
        {
            get { return Color.Black; }
        }

        #endregion

        #region Eventraiser

        /// <summary>
        /// Raises the <see cref="ImageChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnImageChanged(EventArgs eventArgs)
        {
            if (ImageChanged != null)
                ImageChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ExtraImageChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnExtraImageChanged(EventArgs eventArgs)
        {
            if (ExtraImageChanged != null)
                ExtraImageChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ExtraImageAlignmentChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnExtraImageAlignmentChanged(EventArgs eventArgs)
        {
            if (ExtraImageAlignmentChanged != null)
                ExtraImageAlignmentChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="BackColor2Changed"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBackColor2Changed(EventArgs eventArgs)
        {
            if (BackColor2Changed != null)
                BackColor2Changed(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="BackColorGradientRotationAngleChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBackColorGradientRotationAngleChanged(EventArgs eventArgs)
        {
            if (BackColorGradientRotationAngleChanged != null)
                BackColorGradientRotationAngleChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ExtraImageRotationAngleChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnExtraImageRotationAngleChanged(EventArgs eventArgs)
        {
            if (ExtraImageRotationAngleChanged != null)
                ExtraImageRotationAngleChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ZoomChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnZoomChanged(EventArgs eventArgs)
        {
            if (ZoomChanged != null)
                ZoomChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="RotationAngleChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnRotationAngleChanged(EventArgs eventArgs)
        {
            if (RotationAngleChanged != null)
                RotationAngleChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AlphaChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAlphaChanged(EventArgs eventArgs)
        {
            if (AlphaChanged != null)
                AlphaChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnStateChanged(EventArgs eventArgs)
        {
            if (StateChanged != null)
                StateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="BaseSizeModeChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBaseSizeModeChanged(EventArgs eventArgs)
        {
            if (BaseSizeModeChanged != null)
                BaseSizeModeChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="BorderStyleChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBorderStyleChanged(EventArgs eventArgs)
        {
            if (BorderStyleChanged != null)
                BorderStyleChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="BorderColorChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBorderColorChanged(EventArgs eventArgs)
        {
            if (BorderColorChanged != null)
                BorderColorChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AllowDisabledPaintingChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAllowDisabledPaintingChanged(EventArgs eventArgs)
        {
            if (AllowDisabledPaintingChanged != null)
                AllowDisabledPaintingChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextAlignChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected void OnTextAlignChanged(EventArgs eventArgs)
        {
            if (TextAlignChanged != null)
                TextAlignChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextRotationAngleChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected void OnTextRotationAngleChanged(EventArgs eventArgs)
        {
            if (TextRotationAngleChanged != null)
                TextRotationAngleChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextHaloWidthChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected void OnTextHaloWidthChanged(EventArgs eventArgs)
        {
            if (TextHaloWidthChanged != null)
                TextHaloWidthChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextHaloColorChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected void OnTextHaloColorChanged(EventArgs eventArgs)
        {
            if (TextHaloColorChanged != null)
                TextHaloColorChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextZoomChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnTextZoomChanged(EventArgs eventArgs)
        {
            if (TextZoomChanged != null)
                TextZoomChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ShadowModeChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnShadowModeChanged(EventArgs eventArgs)
        {
            if (ShadowModeChanged != null)
                ShadowModeChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ShadowOffsetChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnShadowOffsetChanged(EventArgs eventArgs)
        {
            if (ShadowOffsetChanged != null)
                ShadowOffsetChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ImageOffsetChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnImageOffsetChanged(EventArgs eventArgs)
        {
            if (ImageOffsetChanged != null)
                ImageOffsetChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextOffsetChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnTextOffsetChanged(EventArgs eventArgs)
        {
            if (TextOffsetChanged != null)
                TextOffsetChanged(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        #endregion

        #endregion

        #region Privates

        #region Image calculations

        private Size BaseSize
        {
            get
            {
                if (_image == null)
                    return Size.Empty;

                switch (_baseSizeMode)
                {
                    case BaseSizeMode.Normal:
                        int drawnImageWidth = this.Width;
                        int drawnImageHeight = drawnImageWidth * _image.Height / _image.Width;
                        if (drawnImageHeight > this.Height)
                        {
                            drawnImageHeight = this.Height;
                            drawnImageWidth = drawnImageHeight * _image.Width / _image.Height;
                        }

                        return new Size(drawnImageWidth, drawnImageHeight);
                    case BaseSizeMode.Enhanced:
                        double diag = Math.Sqrt(_image.Height * _image.Height + _image.Width * _image.Width);

                        double f;
                        if (this.Height < this.Width)
                            f = this.Height / diag;
                        else
                            f = this.Width / diag;

                        return new Size(Convert.ToInt32(_image.Width * f), Convert.ToInt32(_image.Height * f));
                    case BaseSizeMode.Original:
                        return _image.Size;
                    default:
                        return Size.Empty;
                }
            }
        }

        private Size ZoomSize
        {
            get
            {
                if (_image == null || _zoom <= 0)
                    return Size.Empty;

                Size baseSize = BaseSize;
                return new Size((int)(baseSize.Width * _zoom / 100), (int)(baseSize.Height * _zoom / 100));
            }
        }

        private Rectangle ImageRectangle
        {
            get
            {
                Size zoomSize = ZoomSize;
                if (zoomSize.IsEmpty)
                    return Rectangle.Empty;

                return new Rectangle(this.Width / 2 - zoomSize.Width / 2, this.Height / 2 - zoomSize.Height / 2, zoomSize.Width, zoomSize.Height);
            }
        }

        private Bitmap GetScaledImage(Image image)
        {
            Size zoomSize = ZoomSize;
            if (zoomSize.IsEmpty)
                return null;

            Bitmap result = new Bitmap(ZoomSize.Width, ZoomSize.Height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(image, new Rectangle(Point.Empty, ZoomSize));
            }
            return result;
        }

        private unsafe void ApplyAlphaToImage(Bitmap image)
        {
            if (image == null || _alpha == 255)
                return;

            int imageWidth = image.Width;
            int imageHeight = image.Height;

            BitmapData data = null;
            try
            {
                data = image.LockBits(new Rectangle(0, 0, imageWidth, imageHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                int scan0 = data.Scan0.ToInt32();
                int stride = data.Stride;

                byte* colPixel;

                byte invertedAlpha = (byte)(255 - _alpha);

                byte* rowPixel = (byte*)scan0;
                for (int y = 0; y < imageHeight; y++)
                {
                    colPixel = rowPixel + 3;
                    for (int x = 0; x < imageWidth; x++)
                    {
                        if (*(colPixel) < invertedAlpha)
                            *(colPixel) = 0;
                        else
                            *(colPixel) = (byte)(*(colPixel) - invertedAlpha);
                        //*(colPixel) = (byte)Math.Max(*(colPixel) - invertedAlpha, 0);
                        colPixel += 4;
                    }
                    rowPixel += stride;
                }

            }
            finally
            {
                if (data != null)
                    image.UnlockBits(data);
            }
        }

        private Bitmap CurrentImage
        {
            get
            {
                Bitmap result = GetScaledImage(_image);
                ApplyAlphaToImage(result);
                return result;
            }
        }

        private Point CalcualteExtraImageLocation()
        {
            int x = 0;
            int y = 0;
            switch (_extraImageAlignment)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                    y = 2;
                    break;
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    y = this.Height / 2 - _extraImage.Height / 2;
                    break;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    y = this.Height - _extraImage.Height - 2;
                    break;
            }

            switch (_extraImageAlignment)
            {
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    x = 2;
                    break;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    x = this.Width / 2 - _extraImage.Width / 2;
                    break;
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    x = this.Width - _extraImage.Width - 2;
                    break;
            }

            return new Point(x, y);
        }

        #endregion

        #region StringFormat

        private void ClearStringFormat()
        {
            if (_stringFormat != null)
            {
                _stringFormat.Dispose();
                _stringFormat = null;
            }
        }

        private StringFormat GetStringFormat()
        {
            if (_stringFormat == null)
                _stringFormat = CreateStringFormat();
            return _stringFormat;
        }

        private StringFormat CreateStringFormat()
        {
            StringFormat stringFormat = new StringFormat();

            if ((this.TextAlign & (ContentAlignment.BottomRight |
                ContentAlignment.MiddleRight | ContentAlignment.TopRight))
                != (ContentAlignment)0)
                stringFormat.Alignment = StringAlignment.Far;
            else if ((this.TextAlign & (ContentAlignment.BottomCenter |
                ContentAlignment.MiddleCenter | ContentAlignment.TopCenter))
                != (ContentAlignment)0)
                stringFormat.Alignment = StringAlignment.Center;
            else
                stringFormat.Alignment = StringAlignment.Near;

            if ((this.TextAlign & (ContentAlignment.BottomRight |
                ContentAlignment.BottomCenter | ContentAlignment.BottomLeft))
                != (ContentAlignment)0)
                stringFormat.LineAlignment = StringAlignment.Far;
            else if ((this.TextAlign & (ContentAlignment.MiddleCenter |
                ContentAlignment.MiddleLeft | ContentAlignment.MiddleRight))
                != (ContentAlignment)0)
                stringFormat.LineAlignment = StringAlignment.Center;
            else
                stringFormat.LineAlignment = StringAlignment.Near;

            if (this.RightToLeft == RightToLeft.Yes)
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            return stringFormat;
        }

        #endregion

        #region Painting

        private void Paintbackground(Graphics graphics)
        {
            Rectangle totalRect = new Rectangle(Point.Empty, this.Size);
            if (!this.Enabled && _allowDisabledPainting)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, Color.Gray)))
                {
                    graphics.FillRectangle(brush, totalRect);
                }
            }
            else if (this.BackColor.Equals(_backColor2) && _backColor2.A == byte.MaxValue)
            {
                graphics.Clear(this.BackColor);
            }
            else
            {
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(totalRect, this.BackColor, _backColor2, _backColorGradientRotationAngle, false))
                {
                    graphics.FillRectangle(gradientBrush, totalRect);
                }
            }
        }

        //		private void PaintImage(Graphics graphics)
        //		{
        //			if (_image != null && _zoom > 0f)
        //			{
        //				Image image = CurrentImage;
        //					
        //				if (image != null)
        //				{
        //					Matrix m = new Matrix();
        //					m.RotateAt(_rotationAngle, new PointF(this.Width / 2, this.Height / 2));
        //					graphics.Transform = m;
        //
        //					int imageX = ImageRectangle.Left + _imageOffset.X;
        //					int imageY = ImageRectangle.Top + _imageOffset.Y;
        //					
        //					if (base.Enabled || !_allowDisabledPainting)
        //					{
        //						if (_shadowMode != ShadowMode.Off)
        //						{
        //							int shadowImageX = imageX;
        //							int shadowImageY = imageY;
        //							switch (_shadowMode)
        //							{
        //								case ShadowMode.OffsetFromCenter:
        //									shadowImageX = _shadowOffset.X + ImageRectangle.Left;
        //									shadowImageY = _shadowOffset.Y + ImageRectangle.Top;
        //									break;
        //								case ShadowMode.OffsetFromCenterPercent:
        //									shadowImageX = _shadowOffset.X * image.Width / 100 + ImageRectangle.Left;
        //									shadowImageY = _shadowOffset.Y * image.Height / 100 + ImageRectangle.Top;
        //									break;
        //								case ShadowMode.OffsetFromImage:
        //									shadowImageX = _shadowOffset.X + imageX;
        //									shadowImageY = _shadowOffset.Y + imageY;
        //									break;
        //								case ShadowMode.OffsetFromImagePercent:
        //									shadowImageX = _shadowOffset.X * image.Width / 100 + imageX;
        //									shadowImageY = _shadowOffset.Y * image.Height / 100 + imageY;
        //									break;
        //							}
        //
        //							ControlPaint.DrawImageDisabled(graphics, image, shadowImageX, shadowImageY, base.BackColor);
        //						}
        //						graphics.DrawImageUnscaled(image, imageX, imageY);
        //					}
        //					else
        //					{
        //						ControlPaint.DrawImageDisabled(graphics, image, imageX, imageY, base.BackColor);
        //					}
        //					
        //					graphics.Transform = new Matrix();
        //				}
        //			}
        //		}

        private void PaintImage(Graphics graphics)
        {
            if (_image != null && _zoom > 0f)
            {
                Image image = CurrentImage;

                if (image != null)
                {
                    int imageX = ImageRectangle.Left + _imageOffset.X;
                    int imageY = ImageRectangle.Top + _imageOffset.Y;

                    Matrix m;
                    if (base.Enabled || !_allowDisabledPainting)
                    {
                        if (_shadowMode != ShadowMode.Off)
                        {
                            int shadowImageX = _imageOffset.X;
                            int shadowImageY = _imageOffset.Y;
                            switch (_shadowMode)
                            {
                                case ShadowMode.OffsetFromCenter:
                                    shadowImageX = _shadowOffset.X;
                                    shadowImageY = _shadowOffset.Y;
                                    break;
                                case ShadowMode.OffsetFromCenterPercent:
                                    shadowImageX = _shadowOffset.X * image.Width / 100;
                                    shadowImageY = _shadowOffset.Y * image.Height / 100;
                                    break;
                                case ShadowMode.OffsetFromImage:
                                    shadowImageX = _shadowOffset.X + _imageOffset.X;
                                    shadowImageY = _shadowOffset.Y + _imageOffset.Y;
                                    break;
                                case ShadowMode.OffsetFromImagePercent:
                                    shadowImageX = _shadowOffset.X * image.Width / 100 + _imageOffset.X;
                                    shadowImageY = _shadowOffset.Y * image.Height / 100 + _imageOffset.Y;
                                    break;
                            }

                            m = new Matrix();
                            m.Translate(shadowImageX, shadowImageY);
                            m.RotateAt(_rotationAngle, new PointF(this.Width / 2, this.Height / 2));
                            graphics.Transform = m;

                            ControlPaint.DrawImageDisabled(graphics, image, ImageRectangle.Left, ImageRectangle.Top, base.BackColor);
                        }

                        m = new Matrix();
                        m.Translate(_imageOffset.X, _imageOffset.Y);
                        m.RotateAt(_rotationAngle, new PointF(this.Width / 2, this.Height / 2));
                        graphics.Transform = m;

                        graphics.DrawImageUnscaled(image, ImageRectangle.Left, ImageRectangle.Top);
                    }
                    else
                    {
                        m = new Matrix();
                        m.Translate(_imageOffset.X, _imageOffset.Y);
                        m.RotateAt(_rotationAngle, new PointF(this.Width / 2, this.Height / 2));
                        graphics.Transform = m;

                        ControlPaint.DrawImageDisabled(graphics, image, imageX, imageY, base.BackColor);
                    }

                    graphics.Transform = new Matrix();
                }
            }
        }

        //		private void PaintImage(Graphics graphics)
        //		{
        //			if (_image != null && _zoom > 0f)
        //			{
        //				Image image = CurrentImage;
        //					
        //				if (image != null)
        //				{
        //					Matrix m = new Matrix();
        //					m.Translate(_imageOffset.X, _imageOffset.Y);
        //					m.RotateAt(_rotationAngle, new PointF(this.Width / 2, this.Height / 2));
        //					graphics.Transform = m;
        //
        ////					int imageX = ImageRectangle.Left + _imageOffset.X;
        ////					int imageY = ImageRectangle.Top + _imageOffset.Y;
        //					
        //					if (base.Enabled || !_allowDisabledPainting)
        //					{
        //						graphics.DrawImageUnscaled(image, ImageRectangle.Left, ImageRectangle.Top);
        //					}
        ////					else
        ////					{
        ////						ControlPaint.DrawImageDisabled(graphics, image, imageX, imageY, base.BackColor);
        ////					}
        //					
        //					graphics.Transform = new Matrix();
        //				}
        //			}
        //		}

        private void PaintExtraImage(Graphics graphics)
        {
            if (_extraImage != null)
            {
                Point extraImageLocation = CalcualteExtraImageLocation();

                Matrix m = new Matrix();
                m.RotateAt(_extraImageRotationAngle, new PointF(extraImageLocation.X + _extraImage.Width / 2f, extraImageLocation.Y + _extraImage.Height / 2f));
                graphics.Transform = m;

                if (base.Enabled || !_allowDisabledPainting)
                    graphics.DrawImageUnscaled(_extraImage, extraImageLocation);
                else
                    ControlPaint.DrawImageDisabled(graphics, _extraImage, extraImageLocation.X, extraImageLocation.Y, base.BackColor);

                graphics.Transform = new Matrix();
            }
        }

        private void PaintText(Graphics graphics)
        {
            if (this.Text != null && this.Text.Length > 0 && _textZoom > 0)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                GraphicsPath path = new GraphicsPath();
                RectangleF layoutRect = new RectangleF(0, 0, this.Width, this.Height);
                path.AddString(this.Text, base.Font.FontFamily, (int)base.Font.Style, base.Font.Size * _textZoom / 100, layoutRect, GetStringFormat());

                float rotateX = 0;
                float rotateY = 0;
                switch (_textAlign)
                {
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.BottomCenter:
                        rotateX = this.Width / 2f;
                        break;
                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        rotateX = this.Width - 1;
                        break;
                }
                switch (_textAlign)
                {
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        rotateY = this.Height - 1;
                        break;
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        rotateY = this.Height / 2f;
                        break;
                }

                Matrix m = new Matrix();
                m.Translate(_textOffset.X, _textOffset.Y);
                m.RotateAt(_textRotationAngle, new PointF(rotateX, rotateY));
                graphics.Transform = m;

                if (_textHaloWidth > 0)
                {
                    using (Pen pen = new Pen(this.Enabled || !_allowDisabledPainting ? _textHaloColor : ControlPaint.Dark(_textHaloColor), _textHaloWidth))
                    {
                        graphics.DrawPath(pen, path);
                    }
                }

                using (SolidBrush brush = new SolidBrush(base.ForeColor))
                {
                    graphics.FillPath(brush, path);
                    if (!this.Enabled && _allowDisabledPainting)
                    {
                        brush.Color = ControlPaint.Dark(brush.Color);
                        graphics.TranslateTransform(-1f, -1f);
                        graphics.FillPath(brush, path);
                    }
                }

                graphics.Transform = new Matrix();
            }
        }

        private void PaintBorder(Graphics graphics)
        {
            ControlPaint.DrawBorder(graphics, new Rectangle(Point.Empty, this.Size), this.Enabled || !_allowDisabledPainting ? _borderColor : ControlPaint.Dark(this._borderColor), _borderStyle);
        }

        #endregion

        #endregion

        #region Overridden from UserControl

        /// <summary>
        /// Gets or sets the first background color.
        /// </summary>
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        /// Redirects the given text to the contained <see cref="Label"/>.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                _text = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// Reinitializes the internal string format of the text.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            ClearStringFormat();
        }

        /// <summary>
        /// Paints the control.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                Paintbackground(e.Graphics);
                PaintImage(e.Graphics);
                PaintExtraImage(e.Graphics);
                PaintText(e.Graphics);
                PaintBorder(e.Graphics);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.GetType().FullName);
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
        }

        #endregion
    }

    #endregion


}
