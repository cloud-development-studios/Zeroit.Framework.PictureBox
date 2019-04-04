// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="AnimatedPictureButton.cs" company="Zeroit Dev Technologies">
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
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region AnimatedPictureButton

    /// <summary>
    /// Control further extending the <see cref="AnimatedPictureBox" /> by
    /// defining a <see cref="StartState" /> and an <see cref="EndState" />.
    /// It animtes itself between those two states when the mouse moves over
    /// or leaves the control.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitEXPicBoxAnimated" />
    public class ZeroitEXPicButtonAnimated : ZeroitEXPicBoxAnimated
    {
        #region Events

        /// <summary>
        /// Event which gets fired when <see cref="StartState" /> has changed.
        /// </summary>
        public event EventHandler StartStateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="EndState" /> has changed.
        /// </summary>
        public event EventHandler EndStateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="PushedState" /> has changed.
        /// </summary>
        public event EventHandler PushedStateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="PushedState" /> has changed.
        /// </summary>
        public event EventHandler PushedPropertiesChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ButtonState" /> has changed.
        /// </summary>
        public event EventHandler ButtonStateChanged;

        #endregion

        #region Fields

        /// <summary>
        /// The default shadow mode
        /// </summary>
        private const ShadowMode DEFAULT_SHADOW_MODE = ShadowMode.OffsetFromCenter;
        /// <summary>
        /// The default push properties
        /// </summary>
        private const PictureBoxStateProperties DEFAULT_PUSH_PROPERTIES = PictureBoxStateProperties.ImageProperties;

        /// <summary>
        /// The components
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// The start state
        /// </summary>
        private PictureBoxState _startState;
        /// <summary>
        /// The end state
        /// </summary>
        private PictureBoxState _endState;
        /// <summary>
        /// The pushed state
        /// </summary>
        private PictureBoxState _pushedState;
        /// <summary>
        /// The push properties
        /// </summary>
        private PictureBoxStateProperties _pushProperties = DEFAULT_PUSH_PROPERTIES;

        /// <summary>
        /// The button state
        /// </summary>
        private AnimatedButtonState _buttonState = AnimatedButtonState.Start;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ZeroitEXPicButtonAnimated() : base()
        {
            InitializeComponent();

            _startState = DefaultStartState;
            _endState = DefaultEndState;
            _pushedState = DefaultPushedState;
            base.ShadowMode = DEFAULT_SHADOW_MODE;
            base.State = _startState;

            UpdateSettings();
        }

        /// <summary>
        /// Frees used resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Public interface

        /// <summary>
        /// Gets the state of the button.
        /// </summary>
        /// <value>The state of the button.</value>
        [Browsable(false)]
        public AnimatedButtonState ButtonState
        {
            get { return _buttonState; }
        }

        /// <summary>
        /// Gets or sets the state of the button when the mouse is not over it.
        /// </summary>
        /// <value>The start state.</value>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the state of the button when the mouse does not over it.")]
        public PictureBoxState StartState
        {
            get { return _startState; }
            set
            {
                if (_startState == value)
                    return;

                _startState = value;

                OnStartStateChanged(EventArgs.Empty);

                UpdateSettings();
            }
        }

        /// <summary>
        /// Gets or sets the state of the button when the mouse is over it.
        /// </summary>
        /// <value>The end state.</value>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the state of the button when the mouse is over it.")]
        public PictureBoxState EndState
        {
            get { return _endState; }
            set
            {
                if (_endState == value)
                    return;

                _endState = value;

                OnEndStateChanged(EventArgs.Empty);

                UpdateSettings();
            }
        }

        /// <summary>
        /// Gets or sets the state of the button when the mouse is over it.
        /// </summary>
        /// <value>The state of the pushed.</value>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the state of the button when the mouse is over it.")]
        public PictureBoxState PushedState
        {
            get { return _pushedState; }
            set
            {
                if (_pushedState == value)
                    return;

                _pushedState = value;

                OnPushedStateChanged(EventArgs.Empty);

                UpdateSettings();
            }
        }

        /// <summary>
        /// Gets or sets which properties of the <see cref="PushedState" />
        /// should be applied when the button is clicked.
        /// </summary>
        /// <value>The pushed properties.</value>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets which properties of the PushedState should be applied when the button is clicked..")]
        public PictureBoxStateProperties PushedProperties
        {
            get { return _pushProperties; }
            set
            {
                if (_pushProperties == value)
                    return;

                _pushProperties = value;

                OnPushedPropertiesChanged(EventArgs.Empty);

                UpdateSettings();
            }
        }

        /// <summary>
        /// Animates the control to its <see cref="StartState" />.
        /// </summary>
        public void AnimateToStart()
        {
            if (_buttonState == AnimatedButtonState.Pushed)
                Release();
            base.Animate(_startState);
            SetState(AnimatedButtonState.Start);
        }

        /// <summary>
        /// Animates the control to its <see cref="EndState" />.
        /// </summary>
        public void AnimateToEnd()
        {
            base.Animate(_endState);
            SetState(AnimatedButtonState.End);
        }

        /// <summary>
        /// Sets the buttons state to <see cref="PushedState" /> (no animation).
        /// </summary>
        public void Push()
        {
            base.StopAnimation();
            ApplyPushedState();
            SetState(AnimatedButtonState.Pushed);
        }

        /// <summary>
        /// Sets the buttons state to <see cref="EndState" /> (no animation).
        /// </summary>
        public void Release()
        {
            base.StopAnimation();
            base.State = _endState;
            SetState(AnimatedButtonState.End);
        }

        #endregion

        #region Protected interface

        #region Defaults

        /// <summary>
        /// Gets the default value for <see cref="StartState" />.
        /// </summary>
        /// <value>The default state of the start.</value>
        protected virtual PictureBoxState DefaultStartState
        {
            get
            {
                return new PictureBoxState(100, 180f, 50f, -180f, 90f,
                    Color.LightGreen, Color.LightBlue, Color.Black,
                    Color.White, 0, 0f, 100f, new Point(2, 2), Point.Empty,
                    Point.Empty);
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="EndState" />.
        /// </summary>
        /// <value>The default state of the end.</value>
        protected virtual PictureBoxState DefaultEndState
        {
            get
            {
                return new PictureBoxState(255, 0f, 100f, 0f, 0f,
                    Color.LightGreen, Color.LightBlue, Color.Black,
                    Color.White, 0, 0f, 100f, new Point(2, 2), Point.Empty,
                    Point.Empty);
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="PushedState" />.
        /// </summary>
        /// <value>The default state of the pushed.</value>
        protected virtual PictureBoxState DefaultPushedState
        {
            get
            {
                PictureBoxState result = DefaultEndState;
                result.ImageOffset = new Point(2, 2);
                return result;
            }
        }

        #endregion

        #region ShouldSerialize

        /// <summary>
        /// Indicates the designer whether <see cref="StartState" /> needs
        /// to be serialized.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ShouldSerializeStartState()
        {
            return _startState != DefaultStartState;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="EndState" /> needs
        /// to be serialized.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ShouldSerializeEndState()
        {
            return _endState != DefaultEndState;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="PushedState" /> needs
        /// to be serialized.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ShouldSerializePushedState()
        {
            return _pushedState != DefaultPushedState;
        }

        #endregion

        #region Eventraiser

        /// <summary>
        /// Raises the <see cref="StartStateChanged" /> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnStartStateChanged(System.EventArgs eventArgs)
        {
            if (StartStateChanged != null)
                StartStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="EndStateChanged" /> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnEndStateChanged(System.EventArgs eventArgs)
        {
            if (EndStateChanged != null)
                EndStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="PushedStateChanged" /> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnPushedStateChanged(System.EventArgs eventArgs)
        {
            if (PushedStateChanged != null)
                PushedStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="PushedPropertiesChanged" /> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnPushedPropertiesChanged(System.EventArgs eventArgs)
        {
            if (PushedPropertiesChanged != null)
                PushedPropertiesChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ButtonStateChanged" /> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnButtonStateChanged(System.EventArgs eventArgs)
        {
            if (ButtonStateChanged != null)
                ButtonStateChanged(this, eventArgs);
        }

        #endregion

        #endregion

        #region Privates

        /// <summary>
        /// Sets the state.
        /// </summary>
        /// <param name="newState">The new state.</param>
        private void SetState(AnimatedButtonState newState)
        {
            if (_buttonState == newState)
                return;

            _buttonState = newState;

            OnButtonStateChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Applies the state of the pushed.
        /// </summary>
        private void ApplyPushedState()
        {
            PictureBoxState state = _endState;
            state.Apply(_pushedState, _pushProperties);
            base.State = state;
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        private void UpdateSettings()
        {
            base.StopAnimation();
            switch (_buttonState)
            {
                case AnimatedButtonState.Pushed:
                    ApplyPushedState();
                    base.State = _pushedState;

                    base.UpdateStartValues(_startState);
                    base.UpdateEndValues(_endState);
                    break;
                case AnimatedButtonState.Start:
                    base.State = _startState;

                    base.UpdateStartValues(_endState);
                    base.UpdateEndValues(_startState);
                    break;
                case AnimatedButtonState.End:
                    base.State = _endState;

                    base.UpdateStartValues(_startState);
                    base.UpdateEndValues(_endState);
                    break;
            }
        }

        #endregion

        #region Overridden from AnimatedPictureBox

        #region Unwanted base properties

        /// <summary>
        /// Gets or sets the alpha value which should be applied to the image.
        /// The alpha value is calcualted on a per pixel basis and pixels already
        /// having an alpha value less then 255 will be reduced further. The effect is
        /// that transparent parts of an image will remain transparent.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The alpha.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override byte Alpha
        {
            get { return base.Alpha; }
            set { base.Alpha = value; }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the main image in degrees.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The rotation angle.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float RotationAngle
        {
            get { return base.RotationAngle; }
            set { base.RotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the zoom factor with which the main image should be drawn.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The zoom.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float Zoom
        {
            get { return base.Zoom; }
            set { base.Zoom = value; }
        }

        /// <summary>
        /// Angle of the <see cref="ExtendedPictureBox.ExtraImage" /> in degrees.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The extra image rotation angle.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float ExtraImageRotationAngle
        {
            get { return base.ExtraImageRotationAngle; }
            set { base.ExtraImageRotationAngle = value; }
        }

        /// <summary>
        /// Angle of the background gradient in degrees.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The back color gradient rotation angle.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float BackColorGradientRotationAngle
        {
            get { return base.BackColorGradientRotationAngle; }
            set { base.BackColorGradientRotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the first background color.
        /// Readjusts also <see cref="BackColor2" /> if it has the same value currently.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The color of the back.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the second color to draw the background gradient.
        /// Overridden to disable designer support.
        /// </summary>
        /// <value>The back color2.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor2
        {
            get { return base.BackColor2; }
            set { base.BackColor2 = value; }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the text in degrees.
        /// </summary>
        /// <value>The text rotation angle.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float TextRotationAngle
        {
            get { return base.TextRotationAngle; }
            set { base.TextRotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the width of the halo of the text.
        /// 0 or smaller if now halo should be shown.
        /// </summary>
        /// <value>The width of the text halo.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float TextHaloWidth
        {
            get { return base.TextHaloWidth; }
            set { base.TextHaloWidth = value; }
        }

        /// <summary>
        /// Gets or sets the width of the color of the halo of the text.
        /// </summary>
        /// <value>The color of the text halo.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color TextHaloColor
        {
            get { return base.TextHaloColor; }
            set { base.TextHaloColor = value; }
        }

        /// <summary>
        /// Gets or sets the zoom factor with which the text should be drawn.
        /// </summary>
        /// <value>The text zoom.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override float TextZoom
        {
            get { return base.TextZoom; }
            set { base.TextZoom = value; }
        }

        /// <summary>
        /// Gets or sets the offset of the main image shadow.
        /// </summary>
        /// <value>The shadow offset.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Point ShadowOffset
        {
            get { return base.ShadowOffset; }
            set { base.ShadowOffset = value; }
        }

        /// <summary>
        /// Gets or sets the offset of the main image.
        /// </summary>
        /// <value>The image offset.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Point ImageOffset
        {
            get { return base.ImageOffset; }
            set { base.ImageOffset = value; }
        }

        /// <summary>
        /// Gets or sets the offset of the text.
        /// </summary>
        /// <value>The text offset.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Point TextOffset
        {
            get { return base.TextOffset; }
            set { base.TextOffset = value; }
        }

        /// <summary>
        /// Gets or sets whether a shadow of the main image should be drawn
        /// and how the offset is calculated.
        /// </summary>
        /// <value>The shadow mode.</value>
        [DefaultValue(DEFAULT_SHADOW_MODE)]
        public override ShadowMode ShadowMode
        {
            get { return base.ShadowMode; }
            set { base.ShadowMode = value; }
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="Control.MouseEnter" /> event and
        /// starts animation to <see cref="EndState" />.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (base.Enabled && base.ParentForm.ContainsFocus)
                AnimateToEnd();
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave" /> event and
        /// starts animation to <see cref="StartState" />.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (base.Enabled)
                AnimateToStart();
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown" /> event and
        /// sets the state to <see cref="PushedState" />.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Push();
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp" /> event and
        /// sets the state to <see cref="PushedState" />.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Release();
        }

        /// <summary>
        /// Raises the <see cref="Control.EnabledChanged" /> event and
        /// starts animation to <see cref="StartState" />.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AnimateToStart();
        }

        #endregion
    }
    #endregion


}
