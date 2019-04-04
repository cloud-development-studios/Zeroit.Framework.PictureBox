// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ProgressStepConverter.cs" company="Zeroit Dev Technologies">
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
using System.Globalization;
//using System.Windows.Forms.VisualStyles;
using System.ComponentModel.Design.Serialization;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region ProgressStepConverter
    /// <summary>
	/// Designer converter class for <see cref="ProgressStep"/>s.
	/// </summary>
	public class ProgressStepConverter : ExpandableObjectConverter
    {
        #region Overridden from ExpandableObjectConverter

        /// <summary>
        /// Determines whether this converter can convert a <see cref="ProgressStep"/>
        /// to a given type in the specified context.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <param name="destType">The type the conversion should result into.</param>
        /// <returns>True if the converter can handle the conversion, otherwise false.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            if (destType == typeof(InstanceDescriptor))
                return true;

            return base.CanConvertTo(context, destType);
        }

        /// <summary>
        /// Converts a specified value (which must be a <see cref="ProgressStep"/>) into a given
        /// type un the specified context.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <param name="info">The culture under which the conversion should be performed.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="destType">The type the conversion should result into.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo info, object value, Type destType)
        {
            if (destType == typeof(InstanceDescriptor))
            {
                ZeroitProgressStep step = (ZeroitProgressStep)value;
                Type[] ctorTypes = new Type[] { typeof(Image), typeof(string), typeof(string), typeof(string) };
                object[] ctorParams = new object[] { step.Image, step.Name, step.Text, step.Description };
                return new InstanceDescriptor(typeof(ZeroitProgressStep).GetConstructor(ctorTypes), ctorParams, true);
            }

            return base.ConvertTo(context, info, value, destType);
        }

        #endregion
    }
    #endregion


}
