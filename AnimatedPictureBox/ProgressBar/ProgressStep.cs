// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ProgressStep.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System.ComponentModel;
using System.Drawing;
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region ProgressStep
    /// <summary>
	/// Class describing on step in a <see cref="AnimatedPicturesProgressBar"/>.
	/// </summary>
	[TypeConverter(typeof(ProgressStepConverter))]
    public class ZeroitProgressStep
    {
        #region Fields

        private Image _image;
        private string _name;
        private string _text;
        private string _description;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitProgressStep()
        {
            _image = null;
            _name = "StepName";
            _text = "StepText";
            _description = "Processing step '{0}' ({1}/{2})...";
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="image">Image applied to the step.</param>
        /// <param name="name">Name applied to the step.</param>
        /// <param name="text">Text applied to the step.</param>
        /// <param name="description">Description applied to the step.</param>
        public ZeroitProgressStep(Image image, string name, string text, string description)
        {
            _image = image;
            _name = name;
            _text = text;
            _description = description;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the image applied to the step.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Gets or sets the name applied to the step.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the text applied to the step.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the description applied to the step.
        /// The <see cref="AnimatedPicturesProgressBar"/> will replace some
        /// keywords before showing the description anyhwere.
        /// The replacements are the following:
        /// {0} -> Name of the step.
        /// {1} -> Index of the step.
        /// {2} -> Total number of steps.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion
    }
    #endregion


}
