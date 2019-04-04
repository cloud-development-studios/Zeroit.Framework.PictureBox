// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="TextColor.cs" company="Zeroit Dev Technologies">
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
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{
    #region ExtendedPictureBoxTextHaloColorAnimator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ExtendedPictureBox.TextHaloColor" /> of a
    /// <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Animations.ControlBackColorAnimator" />
	public class ZeroitEXPicBoxTextAnimator : Helpers.Animations.ControlBackColorAnimator
    {
        #region Fields

        /// <summary>
        /// The extended picture box
        /// </summary>
        private ZeroitEXPicBox _extendedPictureBox;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxTextAnimator(IContainer container) : base(container) { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxTextAnimator() : base() { }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox" /> which
        /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.TextHaloColor" /> should be animated.
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
                    _extendedPictureBox.TextHaloColorChanged -= new EventHandler(OnCurrentValueChanged);

                _extendedPictureBox = value;

                if (_extendedPictureBox != null)
                    _extendedPictureBox.TextHaloColorChanged += new EventHandler(OnCurrentValueChanged);

                _extendedPictureBox = value;

                base.ResetValues();
            }
        }

        #endregion

        #region Overridden from ControlBackColorAnimator

        /// <summary>
        /// Gets or sets the <see cref="Control" /> which
        /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.TextHaloColor" /> should be animated.
        /// </summary>
        /// <value>The control.</value>
        [Browsable(false), Category("Behavior")]
        [DefaultValue(null), RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public override Control Control
        {
            get { return _extendedPictureBox; }
            set { ZeroitEXPicBox = (ZeroitEXPicBox)value; }
        }

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
        {
            get { return _extendedPictureBox == null ? Color.Empty : _extendedPictureBox.TextHaloColor; }
            set
            {
                if (_extendedPictureBox != null)
                    _extendedPictureBox.TextHaloColor = (Color)value;
            }
        }

        #endregion
    }
    #endregion


}
