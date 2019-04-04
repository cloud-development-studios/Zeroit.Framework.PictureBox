// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="FlagCheckedListBoxItem.cs" company="Zeroit Dev Technologies">
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
