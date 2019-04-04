// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="TextOffset.cs" company="Zeroit Dev Technologies">
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
    #region ExtendedPictureBoxTextOffsetAnimator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.TextOffset" /> of a
    /// <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitEXPicBoxOffsetAnimatorBase" />
    public class ZeroitEXPicBoxTextOffsetAnimator : ZeroitEXPicBoxOffsetAnimatorBase
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxTextOffsetAnimator(IContainer container) : base(container) { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxTextOffsetAnimator() { }

        #endregion

        #region Overridden from AnimatorBase

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox" /> which
        /// <see cref="ExtendedPictureBox" /> should be animated.
        /// </summary>
        /// <value>The zeroit ex pic box.</value>
        public override ZeroitEXPicBox ZeroitEXPicBox
        {
            get { return base.ZeroitEXPicBox; }
            set
            {
                if (base.ZeroitEXPicBox != null)
                    base.ZeroitEXPicBox.TextOffsetChanged -= new EventHandler(OnCurrentValueChanged);

                base.ZeroitEXPicBox = value;

                if (base.ZeroitEXPicBox != null)
                    base.ZeroitEXPicBox.TextOffsetChanged += new EventHandler(OnCurrentValueChanged);
            }
        }

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current offset.</value>
        protected override Point CurrentOffset
        {
            get { return base.ZeroitEXPicBox.TextOffset; }
            set { base.ZeroitEXPicBox.TextOffset = value; }
        }

        #endregion
    }
    #endregion


}
