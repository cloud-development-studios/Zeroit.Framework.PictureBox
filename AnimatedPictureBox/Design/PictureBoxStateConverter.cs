// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="PictureBoxStateConverter.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
//using System.Windows.Forms.VisualStyles;
using System.ComponentModel.Design.Serialization;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region PictureBoxStateConverter
    /// <summary>
    /// Designer converter class for <see cref="PictureBoxState" />s.
    /// </summary>
    /// <seealso cref="System.ComponentModel.ExpandableObjectConverter" />
	public class PictureBoxStateConverter : ExpandableObjectConverter
    {
        #region Overridden from ExpandableObjectConverter

        /// <summary>
        /// Determines whether this converter can convert a <see cref="PictureBoxState" />
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
        /// Converts a specified value (which must be a <see cref="PictureBoxState" />) into a given
        /// type un the specified context.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <param name="info">The culture under which the conversion should be performed.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="destType">The type the conversion should result into.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo info, object value, Type destType)
        {
            PictureBoxState state = (PictureBoxState)value;
            if (destType == typeof(InstanceDescriptor))
            {
                Type[] ctorTypes = new Type[] { typeof(byte), typeof(float), typeof(float), typeof(float),
                                                  typeof(float), typeof(Color), typeof(Color), typeof(Color),
                                                  typeof(Color), typeof(float), typeof(float), typeof(float),
                                                  typeof(Point), typeof(Point), typeof(Point) };
                object[] ctorParams = new object[] { state.Alpha, state.RotationAngle, state.Zoom,
                                                       state.ExtraImageRotationAngle, state.BackColorGradientRotationAngle,
                                                       state.BackColor, state.BackColor2, state.ForeColor, state.TextHaloColor,
                                                       state.TextHaloWidth, state.TextRotationAngle, state.TextZoom,
                                                       state.ShadowOffset, state.ImageOffset, state.TextOffset};
                return new InstanceDescriptor(typeof(PictureBoxState).GetConstructor(ctorTypes), ctorParams, true);
            }

            return base.ConvertTo(context, info, value, destType);
        }

        /// <summary>
        /// Creates a <see cref="PictureBoxState" /> instance from a collection of properties.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <param name="propertyValues">Collecion of properties.</param>
        /// <returns>A new <see cref="PictureBoxState" /> instance.</returns>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new PictureBoxState((byte)propertyValues["Alpha"], (float)propertyValues["RotationAngle"],
                (float)propertyValues["Zoom"], (float)propertyValues["ExtraImageRotationAngle"],
                (float)propertyValues["BackColorGradientRotationAngle"], (Color)propertyValues["BackColor"],
                (Color)propertyValues["BackColor2"], (Color)propertyValues["ForeColor"],
                (Color)propertyValues["TextHaloColor"], (float)propertyValues["TextHaloWidth"],
                (float)propertyValues["TextRotationAngle"], (float)propertyValues["TextZoom"],
                (Point)propertyValues["ShadowOffset"], (Point)propertyValues["ImageOffset"],
                (Point)propertyValues["TextOffset"]);
        }

        /// <summary>
        /// Gets whether <see cref="CreateInstance" /> is supported in the specified context.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <returns>True.</returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets the properties associated with a <see cref="PictureBoxState" />.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <param name="value">The value to obtain the properties from.</param>
        /// <param name="attributes">Array of attributes.</param>
        /// <returns>Collection of properties.</returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection result = TypeDescriptor.GetProperties(typeof(PictureBoxState), attributes);
            return result;
        }

        /// <summary>
        /// Gets whether <see cref="GetProperties" /> is supported in the specified context.
        /// </summary>
        /// <param name="context">The formatting context.</param>
        /// <returns>True.</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        #endregion
    }
    #endregion


}
