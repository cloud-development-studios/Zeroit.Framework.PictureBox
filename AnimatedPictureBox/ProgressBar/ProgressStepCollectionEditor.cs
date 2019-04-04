// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ProgressStepCollectionEditor.cs" company="Zeroit Dev Technologies">
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
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region ProgressStepCollectionEditor
    /// <summary>
	/// Controls the design time collection editor for a <see cref="ProgressStepCollection"/>.
	/// </summary>
	public class ProgressStepCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        #region Fields

        private CollectionForm _collectionForm;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance for the specified type.
        /// </summary>
        /// <param name="type">Type of the items to be edited.</param>
        public ProgressStepCollectionEditor(Type type) : base(type) { }

        #endregion

        #region Overridden from CollectionEditor

        /// <summary>
        /// Edits a value regarding a given service provider under a specified context.
        /// </summary>
        /// <param name="context">Context informations.</param>
        /// <param name="provider">Service provider.</param>
        /// <param name="value">Value to be edited.</param>
        /// <returns>The edited value.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (this._collectionForm != null && this._collectionForm.Visible)
            {
                ProgressStepCollectionEditor editor = new ProgressStepCollectionEditor(this.CollectionType);
                return editor.EditValue(context, provider, value);
            }
            else return base.EditValue(context, provider, value);
        }

        /// <summary>
        /// Creates a form to modifiy a collection.
        /// </summary>
        /// <returns></returns>
        protected override CollectionForm CreateCollectionForm()
        {
            this._collectionForm = base.CreateCollectionForm();
            return this._collectionForm;
        }

        /// <summary>
        /// Creates a new instance for the collection.
        /// </summary>
        /// <param name="itemType">Type of the instance to be created.</param>
        /// <returns>A new instance with default values.</returns>
        protected override object CreateInstance(Type itemType)
        {
            ZeroitProgressStep result = (ZeroitProgressStep)base.CreateInstance(itemType);

            return result;
        }

        #endregion
    }
    #endregion


}
