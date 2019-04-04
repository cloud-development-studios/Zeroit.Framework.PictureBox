// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="StepAnimators.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

using System.Drawing;
//using System.Windows.Forms.VisualStyles;
using Zeroit.Framework.PictureBox.Helpers.Animations;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region StepAnimators
    /// <summary>
	/// Helper class for the <see cref="ZeroitEXPicProgressAnimated"/> managing the
	/// animations while a step is in progress.
	/// This is done by removing some of the animation capabilities of the underlying
	/// <see cref="ZeroitEXPicBoxAnimated"/> and instead adding some repeating animations.
	/// </summary>
	internal class ZeroitStepAnimators
    {
        #region Fields

        private ZeroitEXPicBoxAnimated _pictureBox;

        private ZeroitEXPicBoxImageAnimator _extraImageRotationAngleAnimator;
        private ZeroitEXPicBoxColorAnimator _backColor2Animator;
        private ControlBackColorAnimator _backColorAnimator;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="animateExtraImageRotationAngle">Indicates whether the the extra image should be rotated.</param>
        /// <param name="animateColor">Indicates whether the colors should be animated.</param>
        /// <param name="color1">First color of the animation.</param>
        /// <param name="color2">Second color of the animation.</param>
        /// <param name="pictureBox">AnimatedPictureBox.</param>
        internal ZeroitStepAnimators(bool animateExtraImageRotationAngle, bool animateColor, Color color1, Color color2, ZeroitEXPicBoxAnimated pictureBox)
        {
            _pictureBox = pictureBox;

            _extraImageRotationAngleAnimator = new ZeroitEXPicBoxImageAnimator();
            _extraImageRotationAngleAnimator.ZeroitEXPicBox = pictureBox;
            _extraImageRotationAngleAnimator.StartRotationAngle = 0f;
            _extraImageRotationAngleAnimator.EndRotationAngle = 360f;
            _extraImageRotationAngleAnimator.LoopMode = LoopMode.Repeat;

            _backColorAnimator = new ControlBackColorAnimator();
            _backColorAnimator.Control = pictureBox;
            _backColorAnimator.StartColor = color1;
            _backColorAnimator.EndColor = color2;
            _backColorAnimator.LoopMode = LoopMode.Bidirectional;

            _backColor2Animator = new ZeroitEXPicBoxColorAnimator();
            _backColor2Animator.ZeroitEXPicBox = pictureBox;
            _backColor2Animator.StartColor = color2;
            _backColor2Animator.EndColor = color1;
            _backColor2Animator.ParentAnimator = _backColorAnimator;
        }

        #endregion

        #region Internal interface

        /// <summary>
        /// Gets whether this instance is currently animating.
        /// </summary>
        internal bool IsAnimationRunning
        {
            get { return _extraImageRotationAngleAnimator.IsRunning || _backColorAnimator.IsRunning; }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        /// <param name="animateExtraImageRotationAngle">Indicates whether the the extra image should be rotated.</param>
        /// <param name="animateColor">Indicates whether the colors should be animated.</param>
        internal void Start(bool animateExtraImageRotationAngle, bool animateColor)
        {
            _pictureBox.StateAnimator[PictureBoxStateProperties.ExtraImageRotationAngle] = !animateExtraImageRotationAngle;
            _pictureBox.StateAnimator[PictureBoxStateProperties.BackColor] = !animateColor;
            _pictureBox.StateAnimator[PictureBoxStateProperties.BackColor2] = !animateColor;

            if (animateExtraImageRotationAngle)
            {
                _extraImageRotationAngleAnimator.SetStartValuesToCurrentValue();
                _extraImageRotationAngleAnimator.Start();
            }

            if (animateColor)
            {
                _backColorAnimator.SetStartValuesToCurrentValue();
                _backColorAnimator.Start();
            }
        }

        /// <summary>
        /// Stops the animation.
        /// </summary>
        internal void Stop()
        {
            _extraImageRotationAngleAnimator.Stop();
            _backColorAnimator.Stop();

            _pictureBox.StateAnimator[PictureBoxStateProperties.ExtraImageRotationAngle] = true;
            _pictureBox.StateAnimator[PictureBoxStateProperties.BackColor] = true;
            _pictureBox.StateAnimator[PictureBoxStateProperties.BackColor2] = true;
        }

        #endregion
    }
    #endregion


}
