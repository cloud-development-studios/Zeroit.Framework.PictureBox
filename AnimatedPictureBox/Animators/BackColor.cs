// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="BackColor.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System;
using System.ComponentModel;
using System.Drawing;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{
    #region ExtendedPictureBoxBackColor2Animator
    /// <summary>
    /// Class inheriting <see cref="Animations.AnimatorBase" /> to animate the
    /// <see cref="ExtendedPictureBoxLib.ExtendedPictureBox.BackColor2" /> of a
    /// <see cref="ExtendedPictureBox" />.
    /// </summary>
    /// <seealso cref="Animations.ControlBackColorAnimator" />
	public class ZeroitEXPicBoxColorAnimator : Helpers.Animations.ControlBackColorAnimator
    {
        #region Fields

        /// <summary>
        /// The extended picture box
        /// </summary>
        private ZeroitEXPicBox _extendedPictureBox;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="container">Container the new instance should be added to.</param>
        public ZeroitEXPicBoxColorAnimator(IContainer container) : base(container) { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxColorAnimator() : base() { }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the <see cref="ZeroitEXPicBox" /> which
        /// <see cref="ZeroitEXPicBox.BackColor2" /> should be animated.
        /// </summary>
        /// <value>The zeroit ex pic box.</value>
        [Browsable(true), DefaultValue(null), Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public ZeroitEXPicBox ZeroitEXPicBox
        {
            get { return _extendedPictureBox; }
            set
            {
                if (_extendedPictureBox == value)
                    return;

                if (_extendedPictureBox != null)
                    _extendedPictureBox.BackColor2Changed -= new EventHandler(OnCurrentValueChanged);

                _extendedPictureBox = value;

                if (_extendedPictureBox != null)
                    _extendedPictureBox.BackColor2Changed += new EventHandler(OnCurrentValueChanged);

                _extendedPictureBox = value;

                base.ResetValues();
            }
        }

        #endregion

        #region Overridden from ControlBackColorAnimator

        /// <summary>
        /// Gets or sets the <see cref="Control" /> which
        /// <see cref="ZeroitEXPicBox.BackColor2" /> should be animated.
        /// </summary>
        /// <value>The control.</value>
        [Browsable(false), Category("Behavior")]
        [DefaultValue(null), RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets which ExtendedPictureBox should be animated.")]
        public override Control Control
        {
            get { return _extendedPictureBox; }
            set { ZeroitEXPicBox = (ZeroitEXPicBox)value; }
        }

        /// <summary>
        /// Gets or sets the currently shown value.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
        {
            get { return _extendedPictureBox == null ? Color.Empty : _extendedPictureBox.BackColor2; }
            set
            {
                if (_extendedPictureBox != null)
                    _extendedPictureBox.BackColor2 = (Color)value;
            }
        }

        #endregion
    }
    #endregion


}
