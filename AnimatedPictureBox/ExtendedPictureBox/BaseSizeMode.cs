// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="BaseSizeMode.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Zeroit.Framework.PictureBox
{

    #region Eumeration BaseSizeMode

    /// <summary>
    /// Enumeration holding the modes how the 100% zoom size
    /// is calculated in an <see cref="ExtendedPictureBox"/>.
    /// </summary>
    public enum BaseSizeMode
    {
        /// <summary>
        /// Size is calculated so that the image fits into the view given normal orientation.
        /// </summary>
        Normal,
        /// <summary>
        /// Size is calculated so that the image always fits into the view no matter of the rotation.
        /// </summary>
        Enhanced,
        /// <summary>
        /// A <see cref="ExtendedPictureBox.Zoom"/> of 100 will show the image in its original size in this mode.
        /// </summary>
        Original
    }

    #endregion


}
