// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="AnimatedProgressBar.cs" company="Zeroit Dev Technologies">
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

    #region AnimatedPicturesProgressBar

    /// <summary>
	/// Control taking a <see cref="ProgressStepCollection"/>, creating one
	/// <see cref="AnimatedPictureBox"/> foreach step and adding control
	/// to one after another animation of them.
	/// Thus is can be used to indicate the progress in a long process of
	/// separated steps.
	/// </summary>
	public class ZeroitEXPicProgressAnimated : System.Windows.Forms.UserControl, ISupportInitialize
    {
        #region Events

        /// <summary>
        /// Event which gets fired when <see cref="AnimationIntervall"/> has changed.
        /// </summary>
        public event EventHandler AnimationIntervallChanged;

        /// <summary>
        /// Event which gets fired when <see cref="AnimationStepSize"/> has changed.
        /// </summary>
        public event EventHandler AnimationStepSizeChanged;

        /// <summary>
        /// Event which gets fired when <see cref="StepNotificationControl"/> has changed.
        /// </summary>
        public event EventHandler StepNotificationControlChanged;

        /// <summary>
        /// Event which gets fired when <see cref="InitialState"/> has changed.
        /// </summary>
        public event EventHandler InitialStateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="InProgressState"/> has changed.
        /// </summary>
        public event EventHandler InProgressStateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="FinishedState"/> has changed.
        /// </summary>
        public event EventHandler FinishedStateChanged;

        /// <summary>
        /// Event which gets fired when <see cref="InitialExtraImage"/> has changed.
        /// </summary>
        public event EventHandler InitialExtraImageChanged;

        /// <summary>
        /// Event which gets fired when <see cref="InProgressExtraImage"/> has changed.
        /// </summary>
        public event EventHandler InProgressExtraImageChanged;

        /// <summary>
        /// Event which gets fired when <see cref="FinishedExtraImage"/> has changed.
        /// </summary>
        public event EventHandler FinishedExtraImageChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ExtraImageAlignment"/> has changed.
        /// </summary>
        public event EventHandler ExtraImageAlignmentChanged;

        /// <summary>
        /// Event which gets fired when <see cref="TextAlignment"/> has changed.
        /// </summary>
        public event EventHandler TextAlignmentChanged;

        /// <summary>
        /// Event which gets fired when <see cref="AnimateExtraImage"/> has changed.
        /// </summary>
        public event EventHandler AnimateExtraImageChanged;

        /// <summary>
        /// Event which gets fired when <see cref="AnimateBackColor"/> has changed.
        /// </summary>
        public event EventHandler AnimateBackColorChanged;

        /// <summary>
        /// Event which gets fired when <see cref="ShowExtraImages"/> has changed.
        /// </summary>
        public event EventHandler ShowExtraImagesChanged;

        /// <summary>
        /// Event which gets fired when <see cref="BlockNextStepCall"/> has changed.
        /// </summary>
        public event EventHandler BlockNextStepCallChanged;

        /// <summary>
        /// Event which gets fired when <see cref="FinishedNotificationText"/> has changed.
        /// </summary>
        public event EventHandler FinishedNotificationTextChanged;

        #endregion

        #region Fields

        private const ContentAlignment DEFAULT_EXTRA_IMAGE_ALIGNMENT = ContentAlignment.BottomRight;
        private const ContentAlignment DEFAULT_TEXT_ALIGNMENT = ContentAlignment.MiddleCenter;
        private const bool DEFAULT_ANIMATE_EXTRA_IMAGE = true;
        private const bool DEFAULT_ANIMATE_BACK_COLOR = false;
        private const bool DEFAULT_SHOW_EXTRA_IMAGES = true;
        private const bool DEFAULT_BLOCK_NEXT_STEP_CALL = false;
        private const string DEFAULT_FINISHED_NOTIFICATION_TEXT = "Finished initialization ({0} steps total)!";
        private const int DEFAULT_ANIMATION_INTERVALL = 20;
        private const double DEFAULT_ANIMATION_STEP_SIZE = 5;

        private static Image _defaultInitialExtraImage;
        private static Image _defaultInProgressExtraImage;
        private static Image _defaultFinishedExtraImage;

        private System.ComponentModel.Container components = null;
        private ZeroitProgressStepCollection _steps;
        private int _currentStep = -1;

        private Control _stepNotificationControl;

        private PictureBoxState _initialState;
        private PictureBoxState _inProgressState;
        private PictureBoxState _finishedState;

        private Image _initialExtraImage;
        private Image _inProgressExtraImage;
        private Image _finishedExtraImage;
        private ContentAlignment _extraImageAlignment = DEFAULT_EXTRA_IMAGE_ALIGNMENT;
        private ContentAlignment _textAlignment = DEFAULT_TEXT_ALIGNMENT;

        private int _animationIntervall = DEFAULT_ANIMATION_INTERVALL;
        private double _animationStepSize = DEFAULT_ANIMATION_STEP_SIZE;
        private bool _animateExtraImage = DEFAULT_ANIMATE_EXTRA_IMAGE;
        private bool _animateBackColor = DEFAULT_ANIMATE_BACK_COLOR;
        private bool _showExtraImages = DEFAULT_SHOW_EXTRA_IMAGES;

        private bool _blockNextStepCall = DEFAULT_BLOCK_NEXT_STEP_CALL;
        private string _finishedNotificationText = DEFAULT_FINISHED_NOTIFICATION_TEXT;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Creates a new empty instance.
        /// </summary>
        public ZeroitEXPicProgressAnimated()
        {
            InitializeComponent();

            _steps = new ZeroitProgressStepCollection();

            _initialState = DefaultInitialState;
            _inProgressState = DefaultInProgressState;
            _finishedState = DefaultFinishedState;

            _initialExtraImage = DefaultInitialExtraImage;
            _inProgressExtraImage = DefaultInProgressExtraImage;
            _finishedExtraImage = DefaultFinishedExtraImage;
        }

        /// <summary>
        /// Freeus used resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
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
            // 
            // AnimatedPicturesProgressBar
            // 
            this.Name = "AnimatedPicturesProgressBar";
            this.Size = new System.Drawing.Size(424, 40);

        }
        #endregion

        #region Public interface

        /// <summary>
        /// Gets or sets whether the <see cref="ExtendedPictureBox.ExtraImage"/>s
        /// should be shown or not.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_SHOW_EXTRA_IMAGES)]
        [Description("Gets or sets whether extra images should be shown or not.")]
        public bool ShowExtraImages
        {
            get { return _showExtraImages; }
            set
            {
                if (_showExtraImages == value)
                    return;

                _showExtraImages = value;

                OnShowExtraImagesChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="ExtendedPictureBox.ExtraImageRotationAngle"/>
        /// should be animated or not.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_ANIMATE_EXTRA_IMAGE)]
        [Description("Gets or sets whether the rotation angle of the extra images should be animated or not.")]
        public bool AnimateExtraImage
        {
            get { return _animateExtraImage; }
            set
            {
                if (_animateExtraImage == value)
                    return;

                _animateExtraImage = value;

                OnAnimateExtraImageChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="ExtendedPictureBox.BackColor"/>,
        /// <see cref="ExtendedPictureBox.BackColor2"/> and 
        /// <see cref="ExtendedPictureBox.BackColorGradientRotationAngle"/>
        /// should be animated or not.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_ANIMATE_BACK_COLOR)]
        [Description("Gets or sets whether the two back colors and the corresponding rotation angle should be animated or not.")]
        public bool AnimateBackColor
        {
            get { return _animateBackColor; }
            set
            {
                if (_animateBackColor == value)
                    return;

                _animateBackColor = value;

                OnAnimateBackColorChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the state of the steps when they just have been initialized.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the state of the steps when they just have been initialized.")]
        public PictureBoxState InitialState
        {
            get { return _initialState; }
            set
            {
                if (_initialState == value)
                    return;

                _initialState = value;

                OnInitialStateChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the state of the steps while they are being processed.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the state of the steps while they are being processed.")]
        public PictureBoxState InProgressState
        {
            get { return _inProgressState; }
            set
            {
                if (_inProgressState == value)
                    return;

                _inProgressState = value;

                OnInProgressStateChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the state of the steps when they have been finished.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the state of the steps when they have been finished.")]
        public PictureBoxState FinishedState
        {
            get { return _finishedState; }
            set
            {
                if (_finishedState == value)
                    return;

                _finishedState = value;

                OnFinishedStateChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the intervall between updates of the animation (in milliseconds).
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_ANIMATION_INTERVALL)]
        [Description("Gets or sets the intervall between updates of the animation (in milliseconds).")]
        public int AnimationIntervall
        {
            get { return _animationIntervall; }
            set
            {
                if (_animationIntervall == value)
                    return;

                _animationIntervall = value;

                OnAnimationIntervallChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the step size between updates of the animation
        /// (in % - 100 will result in one step -> no actual animation).
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_ANIMATION_STEP_SIZE)]
        [Description("Gets or sets the step size between updates of the animation.")]
        public double AnimationStepSize
        {
            get { return _animationStepSize; }
            set
            {
                _animationStepSize = value;
                if (_animationStepSize == value)
                    return;

                _animationStepSize = value;

                OnAnimationStepSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection where <see cref="ProgressStep"/>s can be added/edited or removed
        /// in order to configure the steps which should be shown by this instance.
        /// Note that changes to this collection do not have any effect until
        /// <see cref="EndInit"/> is called.
        /// </summary>
        [Browsable(true), Category("Data")]
        [Description("Gets a collection where steps can be added/edited or removed.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(ProgressStepCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ZeroitProgressStepCollection Steps
        {
            get { return _steps; }
        }

        /// <summary>
        /// Gets or sets whether a call to <see cref="NextStep"/> should be blocked
        /// until the animation has finished.
        /// This might be useful when the background processing is not located in
        /// a separate thread and would block the GUI thread.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_BLOCK_NEXT_STEP_CALL)]
        [Description("Gets or sets whether a call to NextStep() should be blocked until the animation has finished.")]
        public bool BlockNextStepCall
        {
            get { return _blockNextStepCall; }
            set
            {
                if (_blockNextStepCall == value)
                    return;

                _blockNextStepCall = value;

                OnBlockNextStepCallChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the index of the current <see cref="ProgressStep"/> instance
        /// within the <see cref="Steps"/> collection.
        /// </summary>
        [Browsable(false)]
        public int CurrentStepIndex
        {
            get { return _currentStep; }
        }

        /// <summary>
        /// Gets the current <see cref="ProgressStep"/> instance.
        /// </summary>
        [Browsable(false)]
        public ZeroitProgressStep CurrentStep
        {
            get { return _steps[_currentStep]; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> which should be notified
        /// when the step progress has changed by setting its
        /// <see cref="Control.Text"/> property.
        /// This can be used for example for setting notification texts in
        /// a separate <see cref="Label"/>.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        [Description("Gets or sets the control which should be notified when the step progress has changed.")]
        public Control StepNotificationControl
        {
            get { return _stepNotificationControl; }
            set
            {
                if (_stepNotificationControl == value)
                    return;

                _stepNotificationControl = value;

                OnStepNotificationControlChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the text which should be broadcasted when all steps have been
        /// set to <see cref="FinishedState"/> and thus the processing should be over.
        /// To insert the total count of steps dynamically into this text just insert
        /// '{0}' a the position where the count should be displayed.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DEFAULT_FINISHED_NOTIFICATION_TEXT)]
        [Description("Gets or sets the text which should be broadcasted when all steps have been finished.")]
        public string FinishedNotificationText
        {
            get { return _finishedNotificationText; }
            set
            {
                if (_finishedNotificationText == value)
                    return;

                _finishedNotificationText = value;

                OnFinishedNotificationTextChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox.ExtraImage"/> which
        /// should be shown when a step is in initial state.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the extra image which should be shown when a step is in initial state.")]
        public Image InitialExtraImage
        {
            get { return _initialExtraImage; }
            set
            {
                if (_initialExtraImage == value)
                    return;

                _initialExtraImage = value;

                OnInitialExtraImageChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox.ExtraImage"/> which
        /// should be shown while a step is in progress.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the extra image which should be shown when a step is in progress.")]
        public Image InProgressExtraImage
        {
            get { return _inProgressExtraImage; }
            set
            {
                if (_inProgressExtraImage == value)
                    return;

                _inProgressExtraImage = value;

                OnInProgressExtraImageChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExtendedPictureBox.ExtraImage"/> which
        /// should be shown when a step has been finished.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Gets or sets the extra image which should be shown when a step has been finished.")]
        public Image FinishedExtraImage
        {
            get { return _finishedExtraImage; }
            set
            {
                if (_finishedExtraImage == value)
                    return;

                _finishedExtraImage = value;

                OnFinishedExtraImageChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the extra images shown in the steps.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_EXTRA_IMAGE_ALIGNMENT)]
        [Description("Gets or sets the alignment of the extra images shown in the steps.")]
        public ContentAlignment ExtraImageAlignment
        {
            get { return _extraImageAlignment; }
            set
            {
                if (_extraImageAlignment == value)
                    return;

                _extraImageAlignment = value;

                OnExtraImageAlignmentChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text shown in the steps.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(DEFAULT_TEXT_ALIGNMENT)]
        [Description("Gets or sets the alignment of the text shown in the steps.")]
        public ContentAlignment TextAlignment
        {
            get { return _textAlignment; }
            set
            {
                if (_textAlignment == value)
                    return;

                _textAlignment = value;

                OnTextAlignmentChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Transforms the <see cref="CurrentStep"/> to <see cref="FinishedState"/>,
        /// sets <see cref="CurrentStep"/> to the next following step and transforms it
        /// into <see cref="InProgressState"/> also change the extra images to
        /// the appropriate values.
        /// If <see cref="BlockNextStepCall"/> is set to true than this call will not
        /// return until the animations are still runnning.
        /// </summary>
        public void NextStep()
        {
            ZeroitEXPicBoxAnimated finishingAnimatedPictureBox = null;
            ZeroitEXPicBoxAnimated inProgressAnimatedPictureBox = null;

            if (_currentStep >= 0 && _currentStep < _steps.Count)
            {
                finishingAnimatedPictureBox = GetAnimatedPictureBox(_currentStep);
                finishingAnimatedPictureBox.BorderStyle = ButtonBorderStyle.None;
                ZeroitStepAnimators stepAnimators = (ZeroitStepAnimators)finishingAnimatedPictureBox.Tag;
                stepAnimators.Stop();
                if (_showExtraImages)
                    finishingAnimatedPictureBox.ExtraImage = _finishedExtraImage;
                finishingAnimatedPictureBox.Animate(_finishedState);
            }

            _currentStep++;

            if (_currentStep >= 0 && _currentStep < _steps.Count)
            {
                inProgressAnimatedPictureBox = GetAnimatedPictureBox(_currentStep);
                inProgressAnimatedPictureBox.BorderStyle = ButtonBorderStyle.Dashed;
                inProgressAnimatedPictureBox.Animate(_inProgressState);
                if (_showExtraImages)
                    inProgressAnimatedPictureBox.ExtraImage = _inProgressExtraImage;
                if (_stepNotificationControl != null)
                    _stepNotificationControl.Text = string.Format(CurrentStep.Description, CurrentStep.Name, _currentStep + 1, _steps.Count);
                ZeroitStepAnimators stepAnimators = (ZeroitStepAnimators)inProgressAnimatedPictureBox.Tag;
                stepAnimators.Start(_animateExtraImage, _animateBackColor);
            }
            if (inProgressAnimatedPictureBox == null && _stepNotificationControl != null)
                _stepNotificationControl.Text = string.Format(_finishedNotificationText, _steps.Count);

            if (_blockNextStepCall)
            {
                while ((inProgressAnimatedPictureBox != null && inProgressAnimatedPictureBox.IsAnimationRunning) || (finishingAnimatedPictureBox != null && finishingAnimatedPictureBox.IsAnimationRunning))
                {
                    System.Threading.Thread.Sleep(10);
                    Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// Resets all steps so that all are set to initial state.
        /// </summary>
        public void Reset()
        {
            foreach (ZeroitEXPicBoxAnimated animatedPictureBox in this.Controls)
            {
                animatedPictureBox.BorderStyle = ButtonBorderStyle.None;
                animatedPictureBox.StopAnimation();
                animatedPictureBox.State = _initialState;
                animatedPictureBox.Animate(_initialState);
                animatedPictureBox.State = _initialState;
                if (_showExtraImages)
                    animatedPictureBox.ExtraImage = _initialExtraImage;
                animatedPictureBox.StopAnimation();
            }

            _currentStep = -1;
        }

        /// <summary>
        /// Stops all currently ongoing animations.
        /// </summary>
        public void StopAnimations()
        {
            foreach (ZeroitEXPicBoxAnimated animatedPictureBox in this.Controls)
                ((ZeroitStepAnimators)animatedPictureBox.Tag).Stop();
        }

        #endregion

        #region ISupportInitialize Member

        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit()
        {
            StopAnimations();
        }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        public void EndInit()
        {
            InitializeSteps();
        }

        #endregion

        #region Privates

        private Image GetImageResource(string name)
        {
            System.Reflection.Assembly assembly = this.GetType().Assembly;

            name = "ExtendedPictureBoxLib.Resources." + name + ".png";
            System.IO.Stream stream = assembly.GetManifestResourceStream(name);
            return Image.FromStream(stream);
        }

        private ZeroitEXPicBoxAnimated GetAnimatedPictureBox(int index)
        {
            return this.Controls[index] as ZeroitEXPicBoxAnimated;
        }

        private void InitializeSteps()
        {
            StopAnimations();

            this.Controls.Clear();

            foreach (ZeroitProgressStep step in _steps)
            {
                ZeroitEXPicBoxAnimated animatedPictureBox = new ZeroitEXPicBoxAnimated();
                animatedPictureBox.Image = step.Image;
                animatedPictureBox.ExtraImageAlignment = _extraImageAlignment;
                animatedPictureBox.TextAlign = _textAlignment;
                animatedPictureBox.AnimationIntervall = _animationIntervall;
                animatedPictureBox.AnimationStepSize = _animationStepSize;
                animatedPictureBox.AllowDisabledPainting = false;
                animatedPictureBox.Text = step.Text;

                ZeroitStepAnimators stepAnimators = new ZeroitStepAnimators(_animateExtraImage, _animateBackColor, _inProgressState.BackColor, _inProgressState.BackColor2, animatedPictureBox);
                animatedPictureBox.Tag = stepAnimators;

                this.Controls.Add(animatedPictureBox);
            }

            Reset();
            RepositionIcons();
        }

        private void RepositionIcons()
        {
            int i = 0;
            foreach (ZeroitEXPicBoxAnimated animatedPictureBox in this.Controls)
            {
                Point newPosition = new Point(this.Height * i, 0);
                Size newSize = new Size(this.Height, this.Height);
                animatedPictureBox.Bounds = new Rectangle(newPosition, newSize);
                i++;
            }
        }

        #endregion

        #region Overriden from UserControl

        /// <summary>
        /// Raises the <see cref="Control.Resize"/> event and repositions the
        /// contained steps.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RepositionIcons();
        }

        #endregion

        #region Protected interface

        #region Defaults

        /// <summary>
        /// Gets the default value for <see cref="InitialState"/>.
        /// </summary>
        protected virtual PictureBoxState DefaultInitialState
        {
            get
            {
                return new PictureBoxState(120, 180f, 40, 0f, 90f,
                    Color.PaleVioletRed, Color.IndianRed, Color.Black,
                    Color.White, 0, 0f, 100f, Point.Empty, Point.Empty,
                    Point.Empty);
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="InProgressState"/>.
        /// </summary>
        protected virtual PictureBoxState DefaultInProgressState
        {
            get
            {
                return new PictureBoxState(255, 0, 100, 0f, 90f,
                    Color.Yellow, Color.Orange, Color.Black,
                    Color.White, 0, 0f, 100f, Point.Empty, Point.Empty,
                    Point.Empty);
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="FinishedState"/>.
        /// </summary>
        protected virtual PictureBoxState DefaultFinishedState
        {
            get
            {
                return new PictureBoxState(190, 0, 80, 0f, 90f,
                    Color.LightGreen, Color.YellowGreen, Color.Black,
                    Color.White, 0, 0f, 100f, Point.Empty, Point.Empty,
                    Point.Empty);
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="InitialExtraImage"/>.
        /// </summary>
        protected virtual Image DefaultInitialExtraImage
        {
            get
            {
                if (_defaultInitialExtraImage == null)
                    _defaultInitialExtraImage = GetImageResource("Initial");
                return _defaultInitialExtraImage;
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="InProgressExtraImage"/>.
        /// </summary>
        protected virtual Image DefaultInProgressExtraImage
        {
            get
            {
                if (_defaultInProgressExtraImage == null)
                    _defaultInProgressExtraImage = GetImageResource("InProgress");
                return _defaultInProgressExtraImage;
            }
        }

        /// <summary>
        /// Gets the default value for <see cref="FinishedExtraImage"/>.
        /// </summary>
        protected virtual Image DefaultFinishedExtraImage
        {
            get
            {
                if (_defaultFinishedExtraImage == null)
                    _defaultFinishedExtraImage = GetImageResource("Finished");
                return _defaultFinishedExtraImage;
            }
        }

        #endregion

        #region ShouldSerialize

        /// <summary>
        /// Indicates the designer whether <see cref="InitialState"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeInitialState()
        {
            return _initialState != DefaultInitialState;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="InProgressState"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeInProgressState()
        {
            return _inProgressState != DefaultInProgressState;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="FinishedState"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeFinishedState()
        {
            return _finishedState != DefaultFinishedState;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="InitialExtraImage"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeInitialExtraImage()
        {
            return _initialExtraImage != DefaultInitialExtraImage;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="InProgressExtraImage"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeInProgressExtraImage()
        {
            return _inProgressExtraImage != DefaultInProgressExtraImage;
        }

        /// <summary>
        /// Indicates the designer whether <see cref="FinishedExtraImage"/> needs
        /// to be serialized.
        /// </summary>
        protected virtual bool ShouldSerializeFinishedExtraImage()
        {
            return _finishedExtraImage != DefaultFinishedExtraImage;
        }

        #endregion

        #region Eventraiser

        /// <summary>
        /// Raises the <see cref="StepNotificationControlChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnStepNotificationControlChanged(EventArgs eventArgs)
        {
            if (StepNotificationControlChanged != null)
                StepNotificationControlChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="InitialStateChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnInitialStateChanged(EventArgs eventArgs)
        {
            if (InitialStateChanged != null)
                InitialStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="InProgressStateChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnInProgressStateChanged(EventArgs eventArgs)
        {
            if (InProgressStateChanged != null)
                InProgressStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="FinishedStateChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnFinishedStateChanged(EventArgs eventArgs)
        {
            if (FinishedStateChanged != null)
                FinishedStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="InitialExtraImageChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnInitialExtraImageChanged(EventArgs eventArgs)
        {
            if (InitialExtraImageChanged != null)
                InitialExtraImageChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="InProgressExtraImageChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnInProgressExtraImageChanged(EventArgs eventArgs)
        {
            if (InProgressExtraImageChanged != null)
                InProgressExtraImageChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="FinishedExtraImageChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnFinishedExtraImageChanged(EventArgs eventArgs)
        {
            if (FinishedExtraImageChanged != null)
                FinishedExtraImageChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ExtraImageAlignmentChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnExtraImageAlignmentChanged(EventArgs eventArgs)
        {
            if (ExtraImageAlignmentChanged != null)
                ExtraImageAlignmentChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="TextAlignmentChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnTextAlignmentChanged(EventArgs eventArgs)
        {
            if (TextAlignmentChanged != null)
                TextAlignmentChanged(this, eventArgs);
        }

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
        /// Raises the <see cref="AnimateExtraImageChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimateExtraImageChanged(EventArgs eventArgs)
        {
            if (AnimateExtraImageChanged != null)
                AnimateExtraImageChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="AnimateBackColorChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnAnimateBackColorChanged(EventArgs eventArgs)
        {
            if (AnimateBackColorChanged != null)
                AnimateBackColorChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="ShowExtraImagesChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnShowExtraImagesChanged(EventArgs eventArgs)
        {
            if (ShowExtraImagesChanged != null)
                ShowExtraImagesChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="BlockNextStepCallChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnBlockNextStepCallChanged(EventArgs eventArgs)
        {
            if (BlockNextStepCallChanged != null)
                BlockNextStepCallChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="FinishedNotificationTextChanged"/> event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnFinishedNotificationTextChanged(EventArgs eventArgs)
        {
            if (FinishedNotificationTextChanged != null)
                FinishedNotificationTextChanged(this, eventArgs);
        }

        #endregion

        #endregion
    }

    #endregion


}
