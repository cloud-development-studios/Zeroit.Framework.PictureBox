// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="AnimatedPictureBox.cs" company="Zeroit Dev Technologies">
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
//using System.Windows.Forms.VisualStyles;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region AnimatedPictureBox
    /// <summary>
    /// Control further extending the <see cref="ExtendedPictureBox"/> by
    /// adding animation capabilities.
    /// </summary>
    public class ZeroitEXPicBoxAnimated : ZeroitEXPicBox
    {
        #region Events

        /// <summary>
        /// Event which gets fired when <see cref="AnimationIntervall"/> has changed.
        /// </summary>
        public event EventHandler AnimationIntervallChanged;
        /// <summary>
        /// Event which gets fired when <see cref="AnimationStepSize"/> has changed.
        /// </summary>
        /// 
        public event EventHandler AnimationStepSizeChanged;

        /// <summary>
        /// Event which gets fired when animation has been started with <see cref="Animate"/>.
        /// </summary>
        public event EventHandler AnimationStarted;

        /// <summary>
        /// Event which gets fired when animation has finished.
        /// </summary>
        public event EventHandler AnimationFinished;

        /// <summary>
        /// Event which gets fired when animation has been stopped with <see cref="StopAnimation()"/>.
        /// </summary>
        public event EventHandler AnimationStopped;

        #endregion

        #region Fields

        private const int DEFAULT_ANIMATION_INTERVALL = 20;
        private const double DEFAULT_ANIMATION_STEP_SIZE = 10;
        private const PictureBoxStateProperties DEFAULT_ANIMATED_PROPERTIES = PictureBoxStateProperties.All;

        private ZeroitEXPicBoxStateAnimator _stateAnimator;
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicBoxAnimated() : base()
        {
            InitializeComponent();

            _stateAnimator.AnimatedProperties = DEFAULT_ANIMATED_PROPERTIES;

            _stateAnimator.Intervall = DEFAULT_ANIMATION_INTERVALL;
            _stateAnimator.StepSize = DEFAULT_ANIMATION_STEP_SIZE;
        }

        /// <summary>
        /// Frees used resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Designer generated code
        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._stateAnimator = new ZeroitEXPicBoxStateAnimator(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._stateAnimator)).BeginInit();
            // 
            // _stateAnimator
            // 
            this._stateAnimator.EndState = new PictureBoxState(((System.Byte)(255)), 0F, 100F, 0F, 0F, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, 0, 0F, 100F, Point.Empty, Point.Empty, Point.Empty);
            this._stateAnimator.ZeroitEXPicBox = this;
            this._stateAnimator.StartState = new PictureBoxState(((System.Byte)(255)), 0F, 100F, 0F, 0F, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, 0, 0F, 100F, Point.Empty, Point.Empty, Point.Empty);
            this._stateAnimator.IntervallChanged += new System.EventHandler(this.OnAnimationIntervallChanged);
            this._stateAnimator.AnimationStopped += new System.EventHandler(this.OnAnimationStopped);
            this._stateAnimator.StepSizeChanged += new System.EventHandler(this.OnAnimationStepSizeChanged);
            this._stateAnimator.AnimationStarted += new System.EventHandler(this.OnAnimationStarted);
            this._stateAnimator.AnimationFinished += new System.EventHandler(this.OnAnimationFinished);
            // 
            // AnimatedPictureBox
            // 
            this.Name = "AnimatedPictureBox";
            this.Size = new System.Drawing.Size(72, 72);
            ((System.ComponentModel.ISupportInitialize)(this._stateAnimator)).EndInit();

        }
        #endregion

        #region Internal interface

        /// <summary>
        /// Gets the internally used <see cref="ExtendedPictureBoxStateAnimator"/>.
        /// </summary>
        internal ZeroitEXPicBoxStateAnimator StateAnimator
        {
            get { return _stateAnimator; }
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets the intervall between updates of the animation (in milliseconds).
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_ANIMATION_INTERVALL)]
        [Description("Gets or sets the intervall between updates of the animation (in milliseconds).")]
        public int AnimationIntervall
        {
            get { return _stateAnimator.Intervall; }
            set { _stateAnimator.Intervall = value; }
        }

        /// <summary>
        /// Gets or sets the step size between updates of the animation
        /// (in % - 100 will result in one step -> no actual animation).
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_ANIMATION_STEP_SIZE)]
        [Description("Gets or sets the step size between updates of the animation.")]
        public double AnimationStepSize
        {
            get { return _stateAnimator.StepSize; }
            set { _stateAnimator.StepSize = value; }
        }

        /// <summary>
        /// Gets whether an animation is currently running.
        /// </summary>
        [Browsable(false)]
        public bool IsAnimationRunning
        {
            get { return _stateAnimator.IsRunning; }
        }

        /// <summary>
        /// Animates from the last end state to the given new state.
        /// </summary>
        /// <param name="state">Destination state of the animation.</param>
        public void Animate(PictureBoxState state)
        {
            UpdateEndValues(state);
            _stateAnimator.Start(true);
        }

        /// <summary>
        /// Stops the animation immediately.
        /// </summary>
        public void StopAnimation()
        {
            _stateAnimator.Stop();
        }

        /// <summary>
        /// Sets or gets which properties of given <see cref="PictureBoxState"/>s
        /// should be animated.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_ANIMATED_PROPERTIES)]
        [Description("Sets or gets which properties of given PictureBoxStates should be animated.")]
        public PictureBoxStateProperties AnimatedStateProperties
        {
            get { return _stateAnimator.AnimatedProperties; }
            set { _stateAnimator.AnimatedProperties = value; }
        }

        #endregion

        #region Privates

        private void OnAnimationStarted(object sender, System.EventArgs e)
        {
            OnAnimationStarted(e);
        }

        private void OnAnimationFinished(object sender, System.EventArgs e)
        {
            OnAnimationFinished(e);
        }

        private void OnAnimationStopped(object sender, System.EventArgs e)
        {
            OnAnimationStopped(e);
        }

        private void OnAnimationIntervallChanged(object sender, System.EventArgs e)
        {
            OnAnimationIntervallChanged(e);
        }

        private void OnAnimationStepSizeChanged(object sender, System.EventArgs e)
        {
            OnAnimationStepSizeChanged(e);
        }

        #endregion

        #region Protected interface

        /// <summary>
        /// Updates the starting state of the contained animator.
        /// </summary>
        /// <param name="state">State to set.</param>
        protected void UpdateStartValues(PictureBoxState state)
        {
            _stateAnimator.StartState = state;
        }

        /// <summary>
        /// Updates the ending state of the contained animator.
        /// </summary>
        /// <param name="state">State to set.</param>
        protected void UpdateEndValues(PictureBoxState state)
        {
            _stateAnimator.EndState = state;
        }

        #region Eventraiser

        /// <summary>
        /// Raises the <see cref="AnimationIntervallChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimationIntervallChanged(System.EventArgs eventArgs)
        {
            if (AnimationIntervallChanged != null)
                AnimationIntervallChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AnimationStepSizeChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimationStepSizeChanged(System.EventArgs eventArgs)
        {
            if (AnimationStepSizeChanged != null)
                AnimationStepSizeChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AnimationStarted"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimationStarted(System.EventArgs eventArgs)
        {
            if (AnimationStarted != null)
                AnimationStarted(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AnimationStopped"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimationStopped(System.EventArgs eventArgs)
        {
            if (AnimationStopped != null)
                AnimationStopped(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AnimationFinished"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimationFinished(System.EventArgs eventArgs)
        {
            if (AnimationFinished != null)
                AnimationFinished(this, eventArgs);
        }

        #endregion

        #endregion
    }
    #endregion


}
