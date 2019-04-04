// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="FlagCheckedListBoxItem.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region FlagCheckedListBoxItem
    /// <summary>
    /// Represents one item within a <see cref="FlagCheckedListBox" />.
    /// </summary>
	public class ZeroitFlagCheckedListBoxItem
    {
        #region Fields

        /// <summary>
        /// The value
        /// </summary>
        private int _value;
        /// <summary>
        /// The caption
        /// </summary>
        private string _caption;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value"><see cref="Value" /> of the new instance.</param>
        /// <param name="caption"><see cref="Caption" /> of the new instance</param>
        public ZeroitFlagCheckedListBoxItem(int value, string caption)
        {
            _value = value;
            _caption = caption;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Return the enumeration value this item represents.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Returns the caption to be shown for this item.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return _caption; }
        }

        /// <summary>
        /// Gets whether the value corresponds to a single bit being set.
        /// </summary>
        /// <value><c>true</c> if this instance is flag; otherwise, <c>false</c>.</value>
        public bool IsFlag
        {
            get { return ((_value & (_value - 1)) == 0); }
        }

        /// <summary>
        /// Gets whether true if this value is a member of the composite bit value.
        /// </summary>
        /// <param name="composite">The composite.</param>
        /// <returns><c>true</c> if [is member flag] [the specified composite]; otherwise, <c>false</c>.</returns>
        public bool IsMemberFlag(ZeroitFlagCheckedListBoxItem composite)
        {
            return (IsFlag && ((_value & composite.Value) == _value));
        }

        #endregion

        #region Overridden from Object

        /// <summary>
        /// Return the <see cref="Caption" />.
        /// </summary>
        /// <returns>The <see cref="Caption" />.</returns>
        public override string ToString()
        {
            return _caption;
        }

        #endregion
    }
    #endregion


}
