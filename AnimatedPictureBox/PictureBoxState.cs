// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="PictureBoxState.cs" company="Zeroit Dev Technologies">
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
using System.Drawing.Design;
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region PictureBox State

    /// <summary>
	/// Defines the properties of <see cref="PictureBoxState"/>.
	/// </summary>
	[Flags, Editor("ExtendedPictureBoxLib.Design.FlagEnumUIEditor", typeof(UITypeEditor))]
    public enum PictureBoxStateProperties
    {
        /// <summary>
        /// No property.
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.Alpha"/> property.
        /// </summary>
        Alpha = 1,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.RotationAngle"/> property.
        /// </summary>
        RotationAngle = 2,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.Zoom"/> property.
        /// </summary>
        Zoom = 4,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.Zoom"/> property.
        /// </summary>
        ExtraImageRotationAngle = 8,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.BackColorGradientRotationAngle"/> property.
        /// </summary>
        BackColorGradientRotationAngle = 16,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.BackColor"/> property.
        /// </summary>
        BackColor = 32,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.BackColor2"/> property.
        /// </summary>
        BackColor2 = 64,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.ForeColor"/> property.
        /// </summary>
        ForeColor = 128,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.TextHaloColor"/> property.
        /// </summary>
        TextHaloColor = 256,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.TextHaloWidth"/> property.
        /// </summary>
        TextHaloWidth = 512,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.TextRotationAngle"/> property.
        /// </summary>
        TextRotationAngle = 1024,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.TextZoom"/> property.
        /// </summary>
        TextZoom = 2048,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.ShadowOffset"/> property.
        /// </summary>
        ShadowOffset = 4096,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.ImageOffset"/> property.
        /// </summary>
        ImageOffset = 8192,
        /// <summary>
        /// Represents the <see cref="PictureBoxState.TextOffset"/> property.
        /// </summary>
        TextOffset = 16384,
        /// <summary>
        /// Combines <see cref="Alpha"/>, <see cref="RotationAngle"/>, <see cref="Zoom"/>, 
        /// <see cref="ExtraImageRotationAngle"/>, <see cref="ShadowOffset"/> and <see cref="ImageOffset"/>.
        /// </summary>
        ImageProperties = Alpha | RotationAngle | Zoom | ExtraImageRotationAngle | ShadowOffset | ImageOffset,
        /// <summary>
        /// Combines <see cref="TextHaloWidth"/>, <see cref="TextRotationAngle"/>, <see cref="TextZoom"/> 
        /// and <see cref="TextOffset"/>.
        /// </summary>
        TextProperties = TextHaloWidth | TextRotationAngle | TextZoom | TextOffset,
        /// <summary>
        /// Combines <see cref="BackColorGradientRotationAngle"/>, <see cref="BackColor"/>, 
        /// <see cref="BackColor2"/>, <see cref="ForeColor"/> and <see cref="TextHaloColor"/>.
        /// </summary>
        ColorProperties = BackColorGradientRotationAngle | BackColor | BackColor2 | ForeColor | TextHaloColor,
        /// <summary>
        /// All properties.
        /// </summary>
        All = ImageProperties | TextProperties | ColorProperties
    }

    /// <summary>
    /// Structure holding all properties of an <see cref="ExtendedPictureBox"/>
    /// except the two images, the <see cref="ExtendedPictureBox.ExtraImageAlignment"/>,
    /// <see cref="ExtendedPictureBox.BaseSizeMode"/> and 
    /// <see cref="ExtendedPictureBox.BorderStyle"/>.
    /// The class encapsualtes all properties which can be animated.
    /// </summary>
    [TypeConverter(typeof(PictureBoxStateConverter))]
    public struct PictureBoxState
    {
        #region Fields

        private static Random _randomizer;

        private byte _alpha;
        private float _rotationAngle;
        private float _zoom;
        private float _extraImageRotationAngle;
        private float _backColorGradientRotationAngle;
        private Color _backColor;
        private Color _backColor2;
        private Color _foreColor;
        private Color _textHaloColor;
        private float _textHaloWidth;
        private float _textRotationAngle;
        private float _textZoom;
        private Point _shadowOffset;
        private Point _imageOffset;
        private Point _textOffset;

        #endregion

        #region Constructors

        static PictureBoxState()
        {
            _randomizer = new Random();
        }

        /// <summary>
        /// Creates a new instance and initializes it with
        /// the specified values.
        /// </summary>
        /// <param name="alpha">The initial <see cref="Alpha"/> value.</param>
        /// <param name="rotationAngle">The initial <see cref="RotationAngle"/> value.</param>
        /// <param name="zoom">The initial <see cref="Zoom"/> value.</param>
        /// <param name="extraImageRotationAngle">The initial <see cref="ExtraImageRotationAngle"/> value.</param>
        /// <param name="backColorGradientRotationAngle">The initial <see cref="BackColorGradientRotationAngle"/> value.</param>
        /// <param name="backColor">The initial <see cref="BackColor"/> value.</param>
        /// <param name="backColor2">The initial <see cref="BackColor2"/> value.</param>
        public PictureBoxState(byte alpha, float rotationAngle, float zoom, float extraImageRotationAngle,
            float backColorGradientRotationAngle, Color backColor, Color backColor2)
            : this(alpha, rotationAngle, zoom, extraImageRotationAngle, backColorGradientRotationAngle,
            backColor, backColor2, SystemColors.ControlText, SystemColors.ControlText, 0, 0f, 100f,
            Point.Empty, Point.Empty, Point.Empty)
        { }

        /// <summary>
        /// Creates a new instance and initializes it with
        /// the specified values.
        /// </summary>
        /// <param name="alpha">The initial <see cref="Alpha"/> value.</param>
        /// <param name="rotationAngle">The initial <see cref="RotationAngle"/> value.</param>
        /// <param name="zoom">The initial <see cref="Zoom"/> value.</param>
        /// <param name="extraImageRotationAngle">The initial <see cref="ExtraImageRotationAngle"/> value.</param>
        /// <param name="backColorGradientRotationAngle">The initial <see cref="BackColorGradientRotationAngle"/> value.</param>
        /// <param name="backColor">The initial <see cref="BackColor"/> value.</param>
        /// <param name="backColor2">The initial <see cref="BackColor2"/> value.</param>
        /// <param name="foreColor">The initial <see cref="ForeColor"/> value.</param>
        /// <param name="textHaloColor">The initial <see cref="TextHaloColor"/> value.</param>
        /// <param name="textHaloWidth">The initial <see cref="TextHaloWidth"/> value.</param>
        /// <param name="textRotationAngle">The initial <see cref="TextRotationAngle"/> value.</param>
        /// <param name="textZoom">The initial <see cref="TextZoom"/> value.</param>
        public PictureBoxState(byte alpha, float rotationAngle, float zoom, float extraImageRotationAngle,
            float backColorGradientRotationAngle, Color backColor, Color backColor2, Color foreColor,
            Color textHaloColor, float textHaloWidth, float textRotationAngle, float textZoom)
            : this(alpha, rotationAngle, zoom, extraImageRotationAngle, backColorGradientRotationAngle,
            backColor, backColor2, foreColor, textHaloColor, textHaloWidth, textRotationAngle,
            textZoom, Point.Empty, Point.Empty, Point.Empty)
        { }

        /// <summary>
        /// Creates a new instance and initializes it with
        /// the specified values.
        /// </summary>
        /// <param name="alpha">The initial <see cref="Alpha"/> value.</param>
        /// <param name="rotationAngle">The initial <see cref="RotationAngle"/> value.</param>
        /// <param name="zoom">The initial <see cref="Zoom"/> value.</param>
        /// <param name="extraImageRotationAngle">The initial <see cref="ExtraImageRotationAngle"/> value.</param>
        /// <param name="backColorGradientRotationAngle">The initial <see cref="BackColorGradientRotationAngle"/> value.</param>
        /// <param name="backColor">The initial <see cref="BackColor"/> value.</param>
        /// <param name="backColor2">The initial <see cref="BackColor2"/> value.</param>
        /// <param name="foreColor">The initial <see cref="ForeColor"/> value.</param>
        /// <param name="textHaloColor">The initial <see cref="TextHaloColor"/> value.</param>
        /// <param name="textHaloWidth">The initial <see cref="TextHaloWidth"/> value.</param>
        /// <param name="textRotationAngle">The initial <see cref="TextRotationAngle"/> value.</param>
        /// <param name="textZoom">The initial <see cref="TextZoom"/> value.</param>
        /// <param name="shadowOffset">The initial <see cref="ShadowOffset"/> value.</param>
        /// <param name="imageOffset">The initial <see cref="ImageOffset"/> value.</param>
        /// <param name="textOffset">The initial <see cref="TextOffset"/> value.</param>
        public PictureBoxState(byte alpha, float rotationAngle, float zoom, float extraImageRotationAngle,
            float backColorGradientRotationAngle, Color backColor, Color backColor2, Color foreColor,
            Color textHaloColor, float textHaloWidth, float textRotationAngle, float textZoom,
            Point shadowOffset, Point imageOffset, Point textOffset)
        {
            _alpha = alpha;
            _rotationAngle = rotationAngle;
            _zoom = zoom;
            _extraImageRotationAngle = extraImageRotationAngle;
            _backColorGradientRotationAngle = backColorGradientRotationAngle;
            _backColor = backColor;
            _backColor2 = backColor2;
            _foreColor = foreColor;
            _textHaloColor = textHaloColor;
            _textHaloWidth = textHaloWidth;
            _textRotationAngle = textRotationAngle;
            _textZoom = textZoom;
            _shadowOffset = shadowOffset;
            _imageOffset = imageOffset;
            _textOffset = textOffset;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Applies the a defined set of properties from a given state to
        /// the current instance.
        /// </summary>
        /// <param name="state">State from which to transfer the properties.</param>
        /// <param name="properties">Definition of which properties to transfer.</param>
        public void Apply(PictureBoxState state, PictureBoxStateProperties properties)
        {
            if (IsPropertySet(properties, PictureBoxStateProperties.Alpha))
                _alpha = state._alpha;
            if (IsPropertySet(properties, PictureBoxStateProperties.RotationAngle))
                _rotationAngle = state._rotationAngle;
            if (IsPropertySet(properties, PictureBoxStateProperties.Zoom))
                _zoom = state._zoom;
            if (IsPropertySet(properties, PictureBoxStateProperties.ExtraImageRotationAngle))
                _extraImageRotationAngle = state._extraImageRotationAngle;
            if (IsPropertySet(properties, PictureBoxStateProperties.BackColorGradientRotationAngle))
                _backColorGradientRotationAngle = state._backColorGradientRotationAngle;
            if (IsPropertySet(properties, PictureBoxStateProperties.BackColor))
                _backColor = state._backColor;
            if (IsPropertySet(properties, PictureBoxStateProperties.BackColor2))
                _backColor2 = state._backColor2;
            if (IsPropertySet(properties, PictureBoxStateProperties.ForeColor))
                _foreColor = state._foreColor;
            if (IsPropertySet(properties, PictureBoxStateProperties.TextHaloColor))
                _textHaloColor = state._textHaloColor;
            if (IsPropertySet(properties, PictureBoxStateProperties.TextHaloWidth))
                _textHaloWidth = state._textHaloWidth;
            if (IsPropertySet(properties, PictureBoxStateProperties.TextRotationAngle))
                _textRotationAngle = state._textRotationAngle;
            if (IsPropertySet(properties, PictureBoxStateProperties.TextZoom))
                _textZoom = state._textZoom;
            if (IsPropertySet(properties, PictureBoxStateProperties.ShadowOffset))
                _shadowOffset = state._shadowOffset;
            if (IsPropertySet(properties, PictureBoxStateProperties.ImageOffset))
                _imageOffset = state._imageOffset;
            if (IsPropertySet(properties, PictureBoxStateProperties.TextOffset))
                _textOffset = state._textOffset;
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.Alpha"/> property.
        /// </summary>
        public byte Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.RotationAngle"/> property.
        /// </summary>
        public float RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.Zoom"/> property.
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.ExtraImageRotationAngle"/> property.
        /// </summary>
        public float ExtraImageRotationAngle
        {
            get { return _extraImageRotationAngle; }
            set { _extraImageRotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.BackColorGradientRotationAngle"/> property.
        /// </summary>
        public float BackColorGradientRotationAngle
        {
            get { return _backColorGradientRotationAngle; }
            set { _backColorGradientRotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.BackColor"/> property.
        /// </summary>
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.BackColor2"/> property.
        /// </summary>
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public Color BackColor2
        {
            get { return _backColor2; }
            set { _backColor2 = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="System.Windows.Forms.Control.ForeColor"/> property.
        /// </summary>
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.TextHaloColor"/> property.
        /// </summary>
        [Editor(typeof(ColorEditorEx), typeof(System.Drawing.Design.UITypeEditor))]
        public Color TextHaloColor
        {
            get { return _textHaloColor; }
            set { _textHaloColor = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.TextHaloWidth"/> property.
        /// </summary>
        public float TextHaloWidth
        {
            get { return _textHaloWidth; }
            set { _textHaloWidth = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.TextRotationAngle"/> property.
        /// </summary>
        public float TextRotationAngle
        {
            get { return _textRotationAngle; }
            set { _textRotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.TextZoom"/> property.
        /// </summary>
        public float TextZoom
        {
            get { return _textZoom; }
            set { _textZoom = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.ShadowOffset"/> property.
        /// </summary>
        public Point ShadowOffset
        {
            get { return _shadowOffset; }
            set { _shadowOffset = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.ImageOffset"/> property.
        /// </summary>
        public Point ImageOffset
        {
            get { return _imageOffset; }
            set { _imageOffset = value; }
        }

        /// <summary>
        /// Gets or sets the value for the
        /// <see cref="ExtendedPictureBox.TextOffset"/> property.
        /// </summary>
        public Point TextOffset
        {
            get { return _textOffset; }
            set { _textOffset = value; }
        }

        #endregion

        #region Public static interface

        /// <summary>
        /// Checks whether one <see cref="PictureBoxStateProperties"/> is part of
        /// another one.
        /// </summary>
        /// <param name="allProperties"></param>
        /// <param name="testProperty"></param>
        /// <returns></returns>
        public static bool IsPropertySet(PictureBoxStateProperties allProperties, PictureBoxStateProperties testProperty)
        {
            return (allProperties & testProperty) == testProperty;
        }

        /// <summary>
        /// Creates a new <see cref="PictureBoxState"/> with random properties.
        /// </summary>
        /// <returns>A random <see cref="PictureBoxState"/>.</returns>
        public static PictureBoxState GetRandomState()
        {
            return new PictureBoxState((byte)_randomizer.Next(0, 255), GetRandomAngle(),
                _randomizer.Next(0, 120), GetRandomAngle(), GetRandomAngle(), GetRandomColor(),
                GetRandomColor(), GetRandomColor(), GetRandomColor(), _randomizer.Next(1, 10),
                GetRandomAngle(), _randomizer.Next(0, 120), GetRandomPoint(30), GetRandomPoint(30),
                GetRandomPoint(30));
        }

        /// <summary>
        /// Creates a random <see cref="Point"/>.
        /// </summary>
        /// <returns>A random <see cref="Point"/>.</returns>
        public static Point GetRandomPoint(int maxValue)
        {
            return new Point(_randomizer.Next(maxValue * 2) - maxValue, _randomizer.Next(maxValue * 2) - maxValue);
        }

        /// <summary>
        /// Creates a new random <see cref="Single"/> which can be used in
        /// any of the angle properties of an <see cref="ExtendedPictureBox"/>.
        /// </summary>
        /// <returns>A random angle between 0 and 359.</returns>
        public static float GetRandomAngle()
        {
            return (float)_randomizer.Next(0, 359);
        }

        /// <summary>
        /// Creates a random color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color GetRandomColor()
        {
            return Color.FromArgb((byte)_randomizer.Next(0, 255), (byte)_randomizer.Next(0, 255), (byte)_randomizer.Next(0, 255));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Defines the '==' operator for <see cref="PictureBoxState"/> instances.
        /// </summary>
        /// <param name="state1">The first state.</param>
        /// <param name="state2">The second state.</param>
        /// <returns>True if all properties have the same values, otherwise false.</returns>
        public static bool operator ==(PictureBoxState state1, PictureBoxState state2)
        {
            return state1._alpha == state2._alpha
                && state1._rotationAngle == state2._rotationAngle
                && state1._zoom == state2._zoom
                && state1._extraImageRotationAngle == state2._extraImageRotationAngle
                && state1._backColorGradientRotationAngle == state2._backColorGradientRotationAngle
                && state1._backColor == state2._backColor
                && state1._backColor2 == state2._backColor2
                && state1._foreColor == state2._foreColor
                && state1._textHaloColor == state2._textHaloColor
                && state1._textHaloWidth == state2._textHaloWidth
                && state1._textRotationAngle == state2._textRotationAngle
                && state1._textZoom == state2._textZoom
                && state1._textOffset == state2._textOffset
                && state1._imageOffset == state2._imageOffset
                && state1._shadowOffset == state2._shadowOffset;
        }

        /// <summary>
        /// Defines the '!=' operator for <see cref="PictureBoxState"/> instances.
        /// </summary>
        /// <param name="state1">The first state.</param>
        /// <param name="state2">The second state.</param>
        /// <returns>Returns true if at least one property is different, otherwise false.</returns>
        public static bool operator !=(PictureBoxState state1, PictureBoxState state2)
        {
            return !(state1 == state2);
        }

        #endregion

        #region Overridden from Object

        /// <summary>
        /// Gets a hash code for this instance.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return _alpha.GetHashCode() ^ _rotationAngle.GetHashCode() ^ _zoom.GetHashCode()
                ^ _extraImageRotationAngle.GetHashCode() ^ _backColorGradientRotationAngle.GetHashCode()
                ^ _backColor.GetHashCode() ^ _backColor2.GetHashCode() ^ _foreColor.GetHashCode()
                ^ _textHaloColor.GetHashCode() ^ _textHaloWidth.GetHashCode()
                ^ _textRotationAngle.GetHashCode() ^ _textZoom.GetHashCode()
                ^ _imageOffset.GetHashCode() ^ _shadowOffset.GetHashCode()
                ^ _shadowOffset.GetHashCode();
        }

        /// <summary>
        /// Determines whether this instance and another value
        /// are equal.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns>True if obj is of type <see cref="PictureBoxState"/> and all properties are equal.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PictureBoxState))
                return false;

            return this == (PictureBoxState)obj;
        }

        #endregion
    }

    #endregion


}
