// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ProgressStepCollectionEditor.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
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
