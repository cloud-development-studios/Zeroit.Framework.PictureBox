// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ProgressStepCollection.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System.Collections;
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region ProgressStepCollection
    /// <summary>
    /// A typesafe collection class for <see cref="ProgressStep"/> instances.
    /// </summary>
    public class ZeroitProgressStepCollection : CollectionBase
    {
        #region Constructors

        /// <summary>
        /// Creats a new empty instance.
        /// </summary>
        public ZeroitProgressStepCollection() { }

        #endregion

        #region Public interface

        /// <summary>
        /// Adds a <see cref="ProgressStep"/> to the end of the collection.
        /// </summary>
        /// <param name="progressStep">Step to be added.</param>
        public void Add(ZeroitProgressStep progressStep)
        {
            base.InnerList.Add(progressStep);
        }

        /// <summary>
        /// Removes a <see cref="ProgressStep"/> from the collection.
        /// </summary>
        /// <param name="progressStep">Step to be removed.</param>
        public void Remove(ZeroitProgressStep progressStep)
        {
            base.InnerList.Remove(progressStep);
        }

        /// <summary>
        /// Gets a <see cref="ProgressStep"/> from a specified position.
        /// </summary>
        public ZeroitProgressStep this[int index]
        {
            get { return (ZeroitProgressStep)base.InnerList[index]; }
        }

        #endregion
    }
    #endregion


}
