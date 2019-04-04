// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ShadowMode.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
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
