// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ImageRotation.cs" company="Zeroit Dev Technologies">
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
    #region ExtendedPictureBoxExtraImageRotationAngleAnimator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.ExtraImageRotationAngle" /> of a
    /// <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitEXPicBoxAngleAnimator" />
	public class ZeroitEXPicBoxImageAnimator : ZeroitEXPicBoxAngleAnimator
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxImageAnimator(IContainer container) : base(container) { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxImageAnimator() : base() { }

        #endregion

        #region Overridden from ExtendedPictureBoxRotationAngleAnimator		

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox" /> which
        /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.ExtraImageRotationAngle" /> should be animated.
        /// </summary>
        /// <value>The zeroit ex pic box.</value>
        [Browsable(true), DefaultValue(null), Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public override ZeroitEXPicBox ZeroitEXPicBox
        {
            get { return ZeroitEXPicBoxInternal; }
            set
            {
                if (ZeroitEXPicBoxInternal == value)
                    return;

                if (ZeroitEXPicBoxInternal != null)
                    ZeroitEXPicBoxInternal.ExtraImageRotationAngleChanged -= new EventHandler(OnCurrentValueChanged);

                ZeroitEXPicBoxInternal = value;

                if (ZeroitEXPicBoxInternal != null)
                    ZeroitEXPicBoxInternal.ExtraImageRotationAngleChanged += new EventHandler(OnCurrentValueChanged);

                base.ResetValues();
            }
        }

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
        {
            get { return ZeroitEXPicBox == null ? (float)0 : ZeroitEXPicBox.ExtraImageRotationAngle; }
            set
            {
                if (ZeroitEXPicBox != null)
                    ZeroitEXPicBox.ExtraImageRotationAngle = (float)value;
            }
        }

        #endregion
    }
    #endregion


}
