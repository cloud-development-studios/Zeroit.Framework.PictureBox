// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="FlagEnumUIEditor.cs" company="Zeroit Dev Technologies">
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
using System.Drawing.Design;
using System.Windows.Forms.Design;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region FlagEnumUIEditor
    /// <summary>
    /// Controls the design time editor for a flags enumerations.
    /// </summary>
    /// <seealso cref="System.Drawing.Design.UITypeEditor" />
	public class ZeroitFlagEnumUIEditor : UITypeEditor
    {
        #region Fields

        /// <summary>
        /// The list box
        /// </summary>
        private ZeroitFlagCheckedListBox _listBox;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitFlagEnumUIEditor()
        {
            _listBox = new ZeroitFlagCheckedListBox();
            _listBox.BorderStyle = BorderStyle.None;
        }

        #endregion

        #region Overridden from UITypeEditor

        /// <summary>
        /// Edits a value regarding a given service provider under a specified context.
        /// </summary>
        /// <param name="context">Context informations.</param>
        /// <param name="provider">Service provider.</param>
        /// <param name="value">Value to be edited.</param>
        /// <returns>The edited value.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    Enum e = (Enum)Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
                    _listBox.EnumValue = e;
                    edSvc.DropDownControl(_listBox);
                    return _listBox.EnumValue;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the editor style.
        /// </summary>
        /// <param name="context">Context informations.</param>
        /// <returns>A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle" /> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)" /> method. If the <see cref="T:System.Drawing.Design.UITypeEditor" /> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle" /> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None" />.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion
    }
    #endregion


}
