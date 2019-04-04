// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ShadowMode.cs" company="Zeroit Dev Technologies">
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

//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region Eumeration ShadowMode

    /// <summary>
    /// Enumeration holding the modes how the shadow position of
    /// the main image is calculated in an <see cref="ExtendedPictureBox"/>.
    /// </summary>
    public enum ShadowMode
    {
        /// <summary>
        /// No shadow.
        /// </summary>
        Off,
        /// <summary>
        /// Shadow offset is calculated from the actual image position
        /// which includes the <see cref="ExtendedPictureBox.ImageOffset"/>.
        /// The offset is given in pixels.
        /// </summary>
        OffsetFromImage,
        /// <summary>
        /// Shadow offset is calculated from the actual image position
        /// which includes the <see cref="ExtendedPictureBox.ImageOffset"/>.
        /// The offset is given in percent of the image size.
        /// </summary>
        OffsetFromImagePercent,
        /// <summary>
        /// Shadow offset is calculated from the actual image position
        /// which doesn't include the <see cref="ExtendedPictureBox.ImageOffset"/>.
        /// The offset is given in pixels.
        /// </summary>
        OffsetFromCenter,
        /// <summary>
        /// Shadow offset is calculated from the actual image position
        /// which doesn't include the <see cref="ExtendedPictureBox.ImageOffset"/>.
        /// The offset is given in percent of the image size.
        /// </summary>
        OffsetFromCenterPercent
    }

    #endregion


}
