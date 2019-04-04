// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="TransparentButton.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region TransparentButton

    #region Control
    /// <summary>
    /// A help button control used as a transparent close button in PictureTracker control
    /// </summary>
    internal partial class TransparentButton : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TransparentButton()
        {
            InitializeComponent();
        }
    }
    #endregion

    #region Designer Generated Code

    /// <summary>
    /// A help button control used as a close button in PictureTracker control
    /// </summary>
    internal partial class TransparentButton
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransparentButton));
            this.SuspendLayout();
            // 
            // TransparentButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = Properties.Resources.BackgroundImage; /*((System.Drawing.Image)(resources.GetObject("BackgroundImage")));*/
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DoubleBuffered = true;
            this.Name = "TransparentButton";
            this.Size = new System.Drawing.Size(16, 16);
            this.ResumeLayout(false);

        }

        #endregion
    }

    #endregion

    #endregion

}
