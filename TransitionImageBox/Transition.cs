// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="Transition.cs" company="Zeroit Dev Technologies">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Runtime.CompilerServices;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{


    #region Transition ImageBox

    #region Control
    /// <summary>
    /// Class ZeroitImageTransitionBox.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="System.ComponentModel.ICustomTypeDescriptor" />
    public partial class ZeroitImageTransitionBox
        : UserControl, ICustomTypeDescriptor
    {

        #region Random Number Generator
        /// <summary>
        /// The random
        /// </summary>
        private static Random _random = null;
        /// <summary>
        /// Gets the random.
        /// </summary>
        /// <value>The random.</value>
        private static Random Random
        {
            get
            {
                if (_random == null)
                    _random = new Random();
                return _random;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroitImageTransitionBox"/> class.
        /// </summary>
        public ZeroitImageTransitionBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, false);
            UpdateStyles();

            base.TabStop = false;
            _defaultBitmap.PropertyChanged += _defaultBitmap_PropertyChanged;
            HandleCreated += ucImageShow_HandleCreated;
        }

        /// <summary>
        /// Handles the HandleCreated event of the ucImageShow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void ucImageShow_HandleCreated(object sender, EventArgs e)
        {

            if (!DesignMode)
            {
                if (ParentForm != null)
                    ParentForm.Shown += ParentForm_Shown;
                else if (AutoStart)
                {
                    _loadedTime = DateTime.Now;
                    Start();
                }
            }
        }

        /// <summary>
        /// Handles the Shown event of the ParentForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void ParentForm_Shown(object sender, EventArgs e)
        {
            if (AutoStart)
            {
                _loadedTime = DateTime.Now;
                Start();
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the _defaultBitmap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        void _defaultBitmap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_backgroundIndex == -1 && IsHandleCreated)
            {
                if (_defaultBitmap.Image != null)
                    _background = ResizeImageToFit(-1);
                else
                    _background = null;
                Invalidate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroitImageTransitionBox"/> class.
        /// </summary>
        /// <param name="Images">The images.</param>
        public ZeroitImageTransitionBox(IEnumerable<Bitmap> Images) : this()
        {
            _images.AddRange(Images.Select(i => new ImageEntry() { Image = i }));
        }

        // Get the time the control is loaded, to ensure a suitable transition period
        /// <summary>
        /// The loaded time
        /// </summary>
        DateTime _loadedTime;
        /// <summary>
        /// Handles the Load event of the ucImageShow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ucImageShow_Load(object sender, EventArgs e)
        {
            if (_backgroundIndex == -1 && IsHandleCreated)
            {
                Bitmap bg = ResizeImageToFit(-1);
                lock (_imageLock)
                {
                    _background = bg;
                }
            }
        }

        #endregion

        #region Bitmaps for drawing
        // These bitmaps are generated to be the same size as the
        // drawing area of the control.

        /// <summary>
        /// The background
        /// </summary>
        internal Bitmap _background = null;
        /// <summary>
        /// The foreground
        /// </summary>
        internal Bitmap _foreground = null;

        /// <summary>
        /// The background index
        /// </summary>
        internal int _backgroundIndex = -1;
        /// <summary>
        /// The foreground index
        /// </summary>
        internal int _foregroundIndex = -1;
        /// <summary>
        /// The image lock
        /// </summary>
        internal object _imageLock = new object();



        // Resizes one of the original images to properly fit and position in the client rectangle
        // It also writes any text in the appropriate position
        /// <summary>
        /// Resizes the image to fit.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Bitmap.</returns>
        Bitmap ResizeImageToFit(int index)
        {
            ImageEntry orig = index >= 0 && index < _images.Count ? _images[index] : _defaultBitmap;
            Rectangle clientRectangle = Rectangle.Empty;

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate {
                    clientRectangle = this.ClientRectangle;
                }));
            }
            else
            {
                clientRectangle = this.ClientRectangle;
            }

            Bitmap newBmp = new Bitmap(clientRectangle.Width, clientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(newBmp))
            {
                Point imgOrigin = Point.Empty;
                Size bmpSize = clientRectangle.Size;
                Rectangle targetRectangle;

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.PageUnit = GraphicsUnit.Pixel;
                if (orig.Image != null)
                {
                    if (orig.SizeMode == ImageDrawMode.Zoom)
                    {
                        double ratio = (double)bmpSize.Width / (double)orig.Image.Width;
                        double ratioh = (double)bmpSize.Height / (double)orig.Image.Height;
                        if (ratioh < ratio)
                            ratio = ratioh;
                        bmpSize = new Size((int)(ratio * orig.Image.Width), (int)(ratio * orig.Image.Height));
                        imgOrigin = LocateBitmap(clientRectangle.Size, bmpSize);
                        targetRectangle = new Rectangle(imgOrigin, bmpSize);
                        if (_borderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
                            targetRectangle.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
                        else if (_borderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
                            targetRectangle.Inflate(-1, -1);
                        g.DrawImage(orig.Image, targetRectangle, new Rectangle(0, 0, orig.Image.Width, orig.Image.Height), GraphicsUnit.Pixel);
                    }
                    else if (orig.SizeMode == ImageDrawMode.Copy)
                    {
                        imgOrigin = LocateBitmap(clientRectangle.Size, orig.Image.Size);
                        targetRectangle = new Rectangle(imgOrigin, orig.Image.Size);
                        targetRectangle.Intersect(clientRectangle);
                        if (_borderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
                            targetRectangle.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
                        else if (_borderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
                            targetRectangle.Inflate(-1, -1);
                        g.DrawImageUnscaledAndClipped(orig.Image, targetRectangle);
                    }
                    else
                    { // orig.SizeMode == ImageDrawMode.Stretch
                        targetRectangle = clientRectangle;
                        if (_borderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
                            targetRectangle.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
                        else if (_borderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
                            targetRectangle.Inflate(-1, -1);
                        g.DrawImage(orig.Image, targetRectangle);
                    }
                    switch (_borderStyle)
                    {
                        case System.Windows.Forms.BorderStyle.FixedSingle:
                            ControlPaint.DrawBorder(g, targetRectangle, Color.Black, ButtonBorderStyle.Solid);
                            break;
                        case System.Windows.Forms.BorderStyle.Fixed3D:
                            ControlPaint.DrawBorder3D(g, targetRectangle, _border3D);
                            break;
                        case System.Windows.Forms.BorderStyle.None:
                            break;
                    }
                }
            }
            return newBmp;
        }

        /// <summary>
        /// Locates the bitmap.
        /// </summary>
        /// <param name="clientSize">Size of the client.</param>
        /// <param name="bmpSize">Size of the BMP.</param>
        /// <returns>Point.</returns>
        Point LocateBitmap(Size clientSize, Size bmpSize)
        {
            Point imgOrigin = Point.Empty;

            switch (_alignment)
            {
                case ContentAlignment.TopCenter:
                    imgOrigin.X = (clientSize.Width - bmpSize.Width) / 2;
                    break;
                case ContentAlignment.TopRight:
                    imgOrigin.X = clientSize.Width - bmpSize.Width;
                    break;
                case ContentAlignment.MiddleLeft:
                    imgOrigin.Y = (clientSize.Height - bmpSize.Height) / 2;
                    break;
                case ContentAlignment.MiddleCenter:
                    imgOrigin.X = (clientSize.Width - bmpSize.Width) / 2;
                    imgOrigin.Y = (clientSize.Height - bmpSize.Height) / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    imgOrigin.X = clientSize.Width - bmpSize.Width;
                    imgOrigin.Y = (clientSize.Height - bmpSize.Height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                    imgOrigin.Y = clientSize.Height - bmpSize.Height;
                    break;
                case ContentAlignment.BottomCenter:
                    imgOrigin.X = (clientSize.Width - bmpSize.Width) / 2;
                    imgOrigin.Y = clientSize.Height - bmpSize.Height;
                    break;
                case ContentAlignment.BottomRight:
                    imgOrigin.X = clientSize.Width - bmpSize.Width;
                    imgOrigin.Y = clientSize.Height - bmpSize.Height;
                    break;
            }
            return imgOrigin;
        }
        #endregion

        #region Transitioning Effects Properties



        /// <summary>
        /// The images
        /// </summary>
        List<ImageEntry> _images = new List<ImageEntry>();
        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <value>The images.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The collection of images to cycle through")]
        [Category("Effects")]
        public List<ImageEntry> Images
        {
            get { return _images; }
        }
        /// <summary>
        /// Shoulds the serialize images.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ShouldSerializeImages()
        {
            return _images.Count > 0;
        }
        /// <summary>
        /// Resets the images.
        /// </summary>
        void ResetImages()
        {
            _images.Clear();
        }

        /// <summary>
        /// The default bitmap
        /// </summary>
        ImageEntry _defaultBitmap = new ImageEntry();
        /// <summary>
        /// Gets the default bitmap.
        /// </summary>
        /// <value>The default bitmap.</value>
        [Description("The image to display when the list is empty")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Effects")]
        public ImageEntry DefaultBitmap
        {
            get { return _defaultBitmap; }
        }
        /// <summary>
        /// Resets the default bitmap.
        /// </summary>
        void ResetDefaultBitmap()
        {
            _defaultBitmap.Image = null;
            _defaultBitmap.Path = null;
            _defaultBitmap.SizeMode = ImageDrawMode.Zoom;
            if (_backgroundIndex == -1)
                _background = null;
        }
        /// <summary>
        /// Shoulds the serialize default bitmap.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ShouldSerializeDefaultBitmap()
        {
            return _defaultBitmap != null && !(_defaultBitmap.Image == null && string.IsNullOrEmpty(_defaultBitmap.Path) && _defaultBitmap.SizeMode == ImageDrawMode.Zoom);
        }

        /// <summary>
        /// The alignment
        /// </summary>
        ContentAlignment _alignment = ContentAlignment.MiddleCenter;
        /// <summary>
        /// Gets or sets the image alignment.
        /// </summary>
        /// <value>The image alignment.</value>
        [DefaultValue(ContentAlignment.MiddleCenter)]
        [Category("Effects")]
        public ContentAlignment ImageAlignment
        {
            get { return _alignment; }
            set
            {
                if (_alignment != value)
                {
                    _alignment = value;
                    Bitmap bg = ResizeImageToFit(_backgroundIndex);
                    Bitmap fg = ResizeImageToFit(_foregroundIndex);
                    lock (_imageLock)
                    {
                        _background = bg;
                        _foreground = fg;
                    }
                    if (_effect != null)
                        _effect.Resize(fg, bg);
                    else
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// The transition effects
        /// </summary>
        TransitionEffects _transitionEffects = TransitionEffects.Random;
        /// <summary>
        /// The current effect
        /// </summary>
        TransitionEffects _currentEffect = TransitionEffects.Random;
        /// <summary>
        /// Gets or sets the transition effect.
        /// </summary>
        /// <value>The transition effect.</value>
        [Category("Effects")]
        [DefaultValue(TransitionEffects.Random)]
        public TransitionEffects TransitionEffect
        {
            get { return _transitionEffects; }
            set { _currentEffect = _transitionEffects = value; }
        }

        /// <summary>
        /// Gets the current effect.
        /// </summary>
        /// <value>The current effect.</value>
        [Browsable(false)]
        public TransitionEffects CurrentEffect
        {
            get
            {
                if (_currentEffect == TransitionEffects.Random)
                {
                    _currentEffect = (TransitionEffects)Random.Next((int)TransitionEffects.Random);
                }
                return _currentEffect;
            }
        }

        /// <summary>
        /// The random order
        /// </summary>
        bool _randomOrder = false;
        /// <summary>
        /// Gets or sets a value indicating whether [random order].
        /// </summary>
        /// <value><c>true</c> if [random order]; otherwise, <c>false</c>.</value>
        [Category("Effects")]
        [Description("Cycle throug the images in random order")]
        [DefaultValue(false)]
        public bool RandomOrder
        {
            get { return _randomOrder; }
            set { _randomOrder = value; }
        }

        /// <summary>
        /// The delay
        /// </summary>
        int _delay = 3000;
        /// <summary>
        /// Gets or sets the delay time.
        /// </summary>
        /// <value>The delay time.</value>
        [Category("Effects")]
        [DefaultValue(3000)]
        [Description("The time in milliseconds between each transition")]
        public int DelayTime
        {
            get { return _delay; }
            set { _delay = value; }
        }

        /// <summary>
        /// The transition time
        /// </summary>
        int _transitionTime = 1000;
        /// <summary>
        /// Gets or sets the transition time.
        /// </summary>
        /// <value>The transition time.</value>
        [Category("Effects")]
        [DefaultValue(1000)]
        [Description("The time in milliseconds taken by the transition")]
        public int TransitionTime
        {
            get { return _transitionTime; }
            set { _transitionTime = value; }
        }

        /// <summary>
        /// The transition frames
        /// </summary>
        int _transitionFrames = 25;
        /// <summary>
        /// Gets or sets the transition frames per second.
        /// </summary>
        /// <value>The transition frames per second.</value>
        [Category("Effects")]
        [Description("The number of frames per second to complete trhe transition")]
        [DefaultValue(25)]
        public int TransitionFramesPerSecond
        {
            get { return _transitionFrames; }
            set { _transitionFrames = value; }
        }

        /// <summary>
        /// The automatic start
        /// </summary>
        bool _autoStart = true;
        /// <summary>
        /// Gets or sets a value indicating whether [automatic start].
        /// </summary>
        /// <value><c>true</c> if [automatic start]; otherwise, <c>false</c>.</value>
        [Category("Effects")]
        [Description("Start the ZeroitSlideShow automatically")]
        [DefaultValue(true)]
        public bool AutoStart
        {
            get { return _autoStart; }
            set { _autoStart = value; }
        }

        /// <summary>
        /// The border3 d
        /// </summary>
        Border3DStyle _border3D = Border3DStyle.Flat;
        /// <summary>
        /// Gets or sets the image border3 d style.
        /// </summary>
        /// <value>The image border3 d style.</value>
        [Category("Effects")]
        [DefaultValue(Border3DStyle.Flat)]
        public Border3DStyle ImageBorder3DStyle
        {
            get { return _border3D; }
            set
            {
                if (_border3D != value)
                {
                    _border3D = value;
                    Bitmap bg = ResizeImageToFit(_backgroundIndex);
                    Bitmap fg = ResizeImageToFit(_foregroundIndex);
                    lock (_imageLock)
                    {
                        _background = bg;
                        _foreground = fg;
                    }
                    if (_effect != null)
                        _effect.Resize(fg, bg);
                    else
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// The border style
        /// </summary>
        BorderStyle _borderStyle = BorderStyle.None;
        /// <summary>
        /// Gets or sets the image border style.
        /// </summary>
        /// <value>The image border style.</value>
        [DefaultValue(BorderStyle.None)]
        [Category("Effects")]
        public BorderStyle ImageBorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (_borderStyle != value)
                {
                    _borderStyle = value;
                    Bitmap bg = ResizeImageToFit(_backgroundIndex);
                    Bitmap fg = ResizeImageToFit(_foregroundIndex);
                    lock (_imageLock)
                    {
                        _background = bg;
                        _foreground = fg;
                    }
                    if (_effect != null)
                        _effect.Resize(fg, bg);
                    else
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is transitioning.
        /// </summary>
        /// <value><c>true</c> if this instance is transitioning; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        bool IsTransitioning
        {
            get { return _transitioning; }
        }

        /// <summary>
        /// Occurs when [transitions started].
        /// </summary>
        public event EventHandler TransitionsStarted;
        /// <summary>
        /// Occurs when [transitions stopped].
        /// </summary>
        public event EventHandler TransitionsStopped;

        #endregion

        #region Standard properties to hide from designer because they are not applicable

        /// <summary>
        /// The exclude browsable properties
        /// </summary>
        private string[] _excludeBrowsableProperties = {
            "AutoScroll",
            "AutoScrollOffset",
            "AutoScrollMargin",
            "AutoScrollMinSize",
            "AutoSize",
            "AutoSizeMode",
            "AutoValidate",
            "CausesValidation",
            "Font",
            "ForeColor",
            "ImeMode",
            "RightToLeft",
            "TabIndex",
            "TabStop"
        };

        /// <summary>
        /// The exclude browsable events
        /// </summary>
        private string[] _excludeBrowsableEvents = {
            "AutoSizeChanged",
            "AutoValidateChanged",
            "BindingContextChanged",
            "CausesValidationChanged",
            "ChangeUICues",
            "FontChanged",
            "ForeColorChanged",
            "ImeModeChanged",
            "RightToLeftChanged",
            "Scroll",
            "TabIndexChanged",
            "TabStopChanged",
            "Validated",
            "Validating"
        };

        /// <summary>
        /// Filters the properties.
        /// </summary>
        /// <param name="originalCollection">The original collection.</param>
        /// <returns>PropertyDescriptorCollection.</returns>
        private PropertyDescriptorCollection FilterProperties(PropertyDescriptorCollection originalCollection)
        {
            // Create an enumerator containing only the properties that are not in the provided list of property names
            // and fill an array with those selected properties
            IEnumerable<PropertyDescriptor> selectedProperties = originalCollection.OfType<PropertyDescriptor>().Where(p => !_excludeBrowsableProperties.Contains(p.Name));
            PropertyDescriptor[] descriptors = selectedProperties.ToArray();

            // Return a PropertyDescriptorCollection containing only the filtered descriptors
            PropertyDescriptorCollection newCollection = new PropertyDescriptorCollection(descriptors);
            return newCollection;
        }

        /// <summary>
        /// Filters the events.
        /// </summary>
        /// <param name="origEvents">The original events.</param>
        /// <returns>EventDescriptorCollection.</returns>
        private EventDescriptorCollection FilterEvents(EventDescriptorCollection origEvents)
        {
            // Create an enumerator containing only the events that are not in the provided list of event names
            // and fill an array with those selected events
            IEnumerable<EventDescriptor> selectedEvents = origEvents.OfType<EventDescriptor>().Where(e => !_excludeBrowsableEvents.Contains(e.Name));
            EventDescriptor[] descriptors = selectedEvents.ToArray();

            // Return an EventDescriptorCollection containing only the filtered descriptors
            EventDescriptorCollection newCollection = new EventDescriptorCollection(descriptors);
            return newCollection;
        }

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection" /> containing the attributes for this object.</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>The class name of the object, or null if the class does not have a name.</returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>The name of the object, or null if the object does not have a name.</returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter" /> that is the converter for this object, or null if there is no <see cref="T:System.ComponentModel.TypeConverter" /> for this object.</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="T:System.ComponentModel.EventDescriptor" /> that represents the default event for this object, or null if this object does not have events.</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the default property for this object, or null if this object does not have properties.</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A <see cref="T:System.Type" /> that represents the editor for this object.</param>
        /// <returns>An <see cref="T:System.Object" /> of the specified type that is the editor for this object, or null if the editor cannot be found.</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the filtered events for this component instance.</returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            EventDescriptorCollection orig = TypeDescriptor.GetEvents(this, attributes, true);
            return FilterEvents(orig);
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the events for this component instance.</returns>
        public EventDescriptorCollection GetEvents()
        {
            EventDescriptorCollection orig = TypeDescriptor.GetEvents(this, true);
            return FilterEvents(orig);
        }

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the filtered properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection orig = TypeDescriptor.GetProperties(this, attributes, true);
            return FilterProperties(orig);
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties for this component instance.</returns>
        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection orig = TypeDescriptor.GetProperties(this, true);
            return FilterProperties(orig);
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="T:System.Object" /> that represents the owner of the specified property.</returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        #region The transitions implementation

        /// <summary>
        /// The effect
        /// </summary>
        Transition _effect = null;

        /// <summary>
        /// Class Transition.
        /// </summary>
        abstract class Transition
        {
            /// <summary>
            /// The back
            /// </summary>
            protected Bitmap _back;
            /// <summary>
            /// The front
            /// </summary>
            protected Bitmap _front;
            /// <summary>
            /// The transition time
            /// </summary>
            protected int _transitionTime;
            /// <summary>
            /// The step time
            /// </summary>
            protected int _stepTime;
            /// <summary>
            /// The current step
            /// </summary>
            protected int _currentStep;
            /// <summary>
            /// The transitioning
            /// </summary>
            protected bool _transitioning;
            /// <summary>
            /// The finished
            /// </summary>
            protected bool _finished;
            /// <summary>
            /// The synchronize
            /// </summary>
            protected object _sync;

            /// <summary>
            /// Creates the transition.
            /// </summary>
            /// <param name="effect">The effect.</param>
            /// <param name="front">The front.</param>
            /// <param name="back">The back.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <returns>Transition.</returns>
            public static Transition CreateTransition(TransitionEffects effect, Bitmap front, Bitmap back, int transitionTime, int stepTime)
            {
                switch (effect)
                {
                    case TransitionEffects.ZoomIn:
                        return new ZoomInTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.ZoomOut:
                        return new ZoomOutTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.Fade:
                        return new FadeTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.Dissolve:
                        return new DissolveTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.SlideDown:
                        return new SlideDownTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.SlideUp:
                        return new SlideUpTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.SlideRight:
                        return new SlideRightTransition(front, back, transitionTime, stepTime);
                    case TransitionEffects.SlideLeft:
                        return new SlideLeftTransition(front, back, transitionTime, stepTime);
                    default:
                        return new NoTransition(front);
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Transition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            public Transition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
            {
                _back = bg;
                _front = fg;
                _transitionTime = transitionTime;
                _stepTime = stepTime;
                _currentStep = 0;
                _transitioning = false;
                _finished = false;
                _sync = new object();
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public virtual void Start()
            {
                lock (_sync)
                {
                    _transitioning = true;
                }
                RaiseChanged();
            }
            /// <summary>
            /// Stops this instance.
            /// </summary>
            public virtual void Stop()
            {
                Finish();
            }
            /// <summary>
            /// Steps this instance.
            /// </summary>
            public virtual void Step()
            {
                lock (_sync)
                {
                    if (_currentStep > _transitionTime)
                        Finish();
                    else
                        RaiseChanged();
                }
            }
            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public virtual void Finish()
            {
                lock (_sync)
                {
                    _transitioning = false;
                    _finished = true;
                }
                RaiseFinished();
            }
            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public abstract void Draw(Graphics g);
            /// <summary>
            /// Gets a value indicating whether this instance is transitioning.
            /// </summary>
            /// <value><c>true</c> if this instance is transitioning; otherwise, <c>false</c>.</value>
            public virtual bool IsTransitioning
            {
                get
                {
                    bool value;
                    lock (_sync)
                    {
                        value = _transitioning;
                    }
                    return value;
                }
            }
            /// <summary>
            /// Gets a value indicating whether this instance is finished.
            /// </summary>
            /// <value><c>true</c> if this instance is finished; otherwise, <c>false</c>.</value>
            public virtual bool IsFinished
            {
                get
                {
                    bool value;
                    lock (_sync)
                    {
                        value = _finished;
                    }
                    return value;
                }
            }
            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public abstract void Resize(Bitmap newFront, Bitmap newBack);

            /// <summary>
            /// Raises the changed.
            /// </summary>
            protected virtual void RaiseChanged()
            {
                if (Changed != null)
                {
                    foreach (Delegate d in Changed.GetInvocationList())
                    {
                        ISynchronizeInvoke s = d.Target as ISynchronizeInvoke;
                        if (s != null && s.InvokeRequired)
                            s.BeginInvoke(d, new object[] { this, EventArgs.Empty });
                        else
                            d.DynamicInvoke(this, EventArgs.Empty);
                    }
                }
            }
            /// <summary>
            /// Raises the finished.
            /// </summary>
            protected virtual void RaiseFinished()
            {
                if (Finished != null)
                {
                    foreach (Delegate d in Finished.GetInvocationList())
                    {
                        ISynchronizeInvoke s = d.Target as ISynchronizeInvoke;
                        if (s != null && s.InvokeRequired)
                            s.BeginInvoke(d, new object[] { this, EventArgs.Empty });
                        else
                            d.DynamicInvoke(this, EventArgs.Empty);
                    }
                }
            }
            /// <summary>
            /// Occurs when [changed].
            /// </summary>
            public event EventHandler Changed;
            /// <summary>
            /// Occurs when [finished].
            /// </summary>
            public event EventHandler Finished;
        }

        /// <summary>
        /// Class NoTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class NoTransition : Transition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NoTransition"/> class.
            /// </summary>
            /// <param name="img">The img.</param>
            public NoTransition(Bitmap img) : base(img, null, 0, 0) { }


            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                base.Start();
                Finish();   // No transition period for this one
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                // No steps as this transition finishes immediately
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                base.Finish();
            }

            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                g.DrawImage(_front, 0, 0);
            }

            /// <summary>
            /// Resizes the specified new fg.
            /// </summary>
            /// <param name="newFg">The new fg.</param>
            /// <param name="newBg">The new bg.</param>
            public override void Resize(Bitmap newFg, Bitmap newBg)
            {
                _front = newFg;
                // No need to actually do anything here
                // As a resize cant happen during the transition
            }
        }
        /// <summary>
        /// Class FadeTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class FadeTransition : Transition
        {
            /// <summary>
            /// The cm fg
            /// </summary>
            ColorMatrix _cmFg;
            /// <summary>
            /// The cm bg
            /// </summary>
            ColorMatrix _cmBg;
            /// <summary>
            /// The fade
            /// </summary>
            float _fade;
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;

            /// <summary>
            /// Initializes a new instance of the <see cref="FadeTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public FadeTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime) : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                {
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                    _back = new Bitmap(_clientRectangle.Width, _clientRectangle.Height, PixelFormat.Format32bppArgb);
                }
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
                _fade = 0F;
                _cmFg = new ColorMatrix();
                _cmBg = new ColorMatrix();
            }

            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                            Step();
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _cmBg.Matrix33 = 1F;
                    _cmFg.Matrix33 = 0F;
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                }
                base.Start();
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    _fade = Math.Min(1F, (float)_currentStep / (float)_transitionTime);
                    _cmBg.Matrix33 = 1F - (_cmFg.Matrix33 = _fade);
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                }
                base.Finish();
            }

            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                    {
                        ImageAttributes attr = new ImageAttributes();
                        if (_back != null)
                        {
                            attr.SetColorMatrix(_cmBg);
                            g.DrawImage(_back, _clientRectangle, 0, 0, _clientRectangle.Width, _clientRectangle.Height, GraphicsUnit.Pixel, attr);
                        }
                        if (_front != null)
                        {
                            attr.SetColorMatrix(_cmFg);
                            g.DrawImage(_front, _clientRectangle, 0, 0, _clientRectangle.Width, _clientRectangle.Height, GraphicsUnit.Pixel, attr);
                        }
                    }
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class SlideLeftTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class SlideLeftTransition : Transition
        {
            /// <summary>
            /// The distance
            /// </summary>
            int _distance;
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;

            /// <summary>
            /// Initializes a new instance of the <see cref="SlideLeftTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public SlideLeftTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
                : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
            }
            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                        {
                            Step();
                        }
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                    _distance = 0;
                }
                base.Start();
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    _distance = Math.Min(_clientRectangle.Width * _currentStep / _transitionTime, _clientRectangle.Width);
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                }
                base.Finish();
            }


            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                    {
                        Rectangle src = _clientRectangle;
                        src.X = _distance;
                        src.Width -= _distance;
                        Rectangle area = src;
                        area.X = 0;
                        if (_back != null)
                            g.DrawImage(_back, area, src, GraphicsUnit.Pixel);
                        src = _clientRectangle;
                        src.Width = _distance;
                        area = src;
                        area.X = _clientRectangle.Width - _distance;
                        if (_front != null)
                            g.DrawImage(_front, area, src, GraphicsUnit.Pixel);
                    }
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                    _distance = Math.Min(_clientRectangle.Width * _currentStep / _transitionTime, _clientRectangle.Width);
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class SlideRightTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class SlideRightTransition : Transition
        {
            /// <summary>
            /// The distance
            /// </summary>
            int _distance;
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;

            /// <summary>
            /// Initializes a new instance of the <see cref="SlideRightTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public SlideRightTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
                : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
            }
            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                        {
                            Step();
                        }
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                    _distance = 0;
                }
                base.Start();
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    _distance = Math.Min(_clientRectangle.Width * _currentStep / _transitionTime, _clientRectangle.Width);
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                }
                base.Finish();
            }


            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                    {
                        Rectangle src = _clientRectangle;
                        src.Width = _clientRectangle.Width - _distance;
                        Rectangle area = src;
                        area.X = _distance;
                        if (_back != null)
                            g.DrawImage(_back, area, src, GraphicsUnit.Pixel);
                        src = _clientRectangle;
                        src.X = _clientRectangle.Width - _distance;
                        src.Width = _distance;
                        area = src;
                        area.X = 0;
                        if (_front != null)
                            g.DrawImage(_front, area, src, GraphicsUnit.Pixel);
                    }
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class SlideDownTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class SlideDownTransition : Transition
        {
            /// <summary>
            /// The distance
            /// </summary>
            int _distance;
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;

            /// <summary>
            /// Initializes a new instance of the <see cref="SlideDownTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public SlideDownTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
                : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
            }
            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                        {
                            Step();
                        }
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                    _distance = 0;
                }
                base.Start();
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    _distance = Math.Min(_clientRectangle.Height * _currentStep / _transitionTime, _clientRectangle.Height);
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                }
                base.Finish();
            }


            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                    {
                        Rectangle src = _clientRectangle;
                        src.Height = _clientRectangle.Height - _distance;
                        Rectangle area = src;
                        area.Y = _distance;
                        if (_back != null)
                            g.DrawImage(_back, area, src, GraphicsUnit.Pixel);
                        src = _clientRectangle;
                        src.Y = _clientRectangle.Height - _distance;
                        src.Height = _distance;
                        area = src;
                        area.Y = 0;
                        if (_front != null)
                            g.DrawImage(_front, area, src, GraphicsUnit.Pixel);
                    }
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class SlideUpTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class SlideUpTransition : Transition
        {
            /// <summary>
            /// The distance
            /// </summary>
            int _distance;
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;

            /// <summary>
            /// Initializes a new instance of the <see cref="SlideUpTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public SlideUpTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
                : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
            }
            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                        {
                            Step();
                        }
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                    _distance = 0;
                }
                base.Start();
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    _distance = Math.Min(_clientRectangle.Height * _currentStep / _transitionTime, _clientRectangle.Height);
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                }
                base.Finish();
            }

            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                    {
                        Rectangle src; ;
                        Rectangle area;
                        if (_back != null)
                        {
                            src = _clientRectangle;
                            src.Y = _distance;
                            src.Height = _clientRectangle.Height - _distance;
                            area = src;
                            area.Y = 0;
                            g.DrawImage(_back, area, src, GraphicsUnit.Pixel);
                        }
                        if (_front != null)
                        {
                            src = _clientRectangle;
                            src.Height = _distance;
                            area = src;
                            area.Y = _clientRectangle.Height - _distance;
                            g.DrawImage(_front, area, src, GraphicsUnit.Pixel);
                        }
                    }
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class DissolveTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class DissolveTransition : Transition
        {
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;
            /// <summary>
            /// The random pixels
            /// </summary>
            List<int> _randomPixels;
            /// <summary>
            /// The image size
            /// </summary>
            int _imageSize;
            /// <summary>
            /// The pixels dissolved
            /// </summary>
            int _pixelsDissolved;
            /// <summary>
            /// The transition
            /// </summary>
            Bitmap _transition;

            /// <summary>
            /// Initializes a new instance of the <see cref="DissolveTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public DissolveTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime) : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                {
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                    _back = new Bitmap(_clientRectangle.Width, _clientRectangle.Height, PixelFormat.Format32bppArgb);
                }
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
                _imageSize = _clientRectangle.Width * _clientRectangle.Height;
                _pixelsDissolved = 0;

                // Generate a ranom order to dissolve the pixels

                _randomPixels = new List<int>(_imageSize);
                for (int i = 0; i < _imageSize; ++i)
                    _randomPixels.Add(i * 4);
                for (int i = 0; i < _imageSize; ++i)
                {
                    int j = Random.Next(_imageSize);
                    if (i != j)
                    {
                        _randomPixels[i] ^= _randomPixels[j];
                        _randomPixels[j] ^= _randomPixels[i];
                        _randomPixels[i] ^= _randomPixels[j];
                    }
                }
                _transition = (Bitmap)bg.Clone();
            }

            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                            Step();
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                }
                base.Start();
            }

            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    int endPoint = Math.Min(_imageSize, (int)((long)_imageSize * _currentStep / _transitionTime));
                    BitmapData src = _front.LockBits(_clientRectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    BitmapData target = _transition.LockBits(_clientRectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    for (int i = _pixelsDissolved; i < endPoint; ++i)
                    {
                        Marshal.WriteInt32(target.Scan0, _randomPixels[i], Marshal.ReadInt32(src.Scan0, _randomPixels[i]));
                    }
                    _transition.UnlockBits(target);
                    _front.UnlockBits(src);
                    _pixelsDissolved = endPoint;
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                }
                base.Finish();
            }

            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                    {
                        g.DrawImage(_transition ?? _back, 0, 0);
                    }
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                    _transition.Dispose();
                    _transition = (Bitmap)_back.Clone();
                    int newSize = _clientRectangle.Width * _clientRectangle.Height;
                    if (newSize > _imageSize)
                    {
                        _randomPixels.Capacity = newSize;
                        for (int i = _imageSize; i < newSize; ++i)
                        {
                            _randomPixels.Add(i * 4);
                        }
                        for (int i = _imageSize; i < newSize; ++i)
                        {
                            int j = Random.Next(_imageSize);
                            if (i != j)
                            {
                                _randomPixels[i] ^= _randomPixels[j];
                                _randomPixels[j] ^= _randomPixels[i];
                                _randomPixels[i] ^= _randomPixels[j];
                            }
                        }
                    }
                    else if (newSize < _imageSize)
                    {
                        int maxPoint = (newSize - 1) * 4;
                        _randomPixels.RemoveAll(i => i > maxPoint);
                        _randomPixels.TrimExcess();
                    }
                    _imageSize = newSize;
                    int endPoint = Math.Min(_imageSize, (int)((long)_imageSize * _currentStep / _transitionTime));
                    BitmapData src = _front.LockBits(_clientRectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    BitmapData target = _transition.LockBits(_clientRectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    for (int i = 0; i < endPoint; ++i)
                    {
                        Marshal.WriteInt32(target.Scan0, _randomPixels[i], Marshal.ReadInt32(src.Scan0, _randomPixels[i]));
                    }
                    _transition.UnlockBits(target);
                    _front.UnlockBits(src);
                    _pixelsDissolved = endPoint;
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class ZoomInTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class ZoomInTransition : Transition
        {
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;
            /// <summary>
            /// The zoom rectangle
            /// </summary>
            Rectangle _zoomRectangle;
            /// <summary>
            /// The transition
            /// </summary>
            Bitmap _transition = null;
            /// <summary>
            /// Initializes a new instance of the <see cref="ZoomInTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public ZoomInTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
                : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
            }
            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                        {
                            Step();
                        }
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                    _zoomRectangle = Rectangle.Inflate(_clientRectangle, _clientRectangle.Width / 5, _clientRectangle.Height / 5);
                    CreateTransitionImage();
                }
                base.Start();
            }

            /// <summary>
            /// Creates the transition image.
            /// </summary>
            void CreateTransitionImage()
            {
                if (_transition != null)
                    _transition.Dispose();

                int distanceX = _zoomRectangle.Width / 2 * _currentStep / _transitionTime;
                int distanceY = _zoomRectangle.Height / 2 * _currentStep / _transitionTime;
                Rectangle _drawRect = _zoomRectangle;
                _drawRect.Inflate(-distanceX, -distanceY);
                if (_drawRect.Width > 0 && _drawRect.Height > 0)
                {
                    _transition = new Bitmap(_clientRectangle.Width, _clientRectangle.Height, PixelFormat.Format32bppArgb);
                    GraphicsPath p = new GraphicsPath();
                    p.AddEllipse(_drawRect);
                    using (Graphics g = Graphics.FromImage(_transition))
                    {
                        g.SetClip(p);
                        g.DrawImage(_back, 0, 0);
                        g.SetClip(_zoomRectangle, CombineMode.Xor);
                        g.DrawImage(_front, 0, 0);
                        g.ResetClip();
                    }
                }
                else
                    _transition = null;
            }
            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    CreateTransitionImage();
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                    if (_transition != null)
                    {
                        _transition.Dispose();
                        _transition = null;
                    }
                }
                base.Finish();
            }

            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                        g.DrawImage(_transition ?? _front, 0, 0);
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                    _zoomRectangle = Rectangle.Inflate(_clientRectangle, _clientRectangle.Width / 5, _clientRectangle.Height / 5);
                    CreateTransitionImage();
                }
                RaiseChanged();
            }
        }
        /// <summary>
        /// Class ZoomOutTransition.
        /// </summary>
        /// <seealso cref="Zeroit.Framework.PictureBox.ZeroitImageTransitionBox.Transition" />
        class ZoomOutTransition : Transition
        {
            /// <summary>
            /// The client rectangle
            /// </summary>
            Rectangle _clientRectangle;
            /// <summary>
            /// The timer
            /// </summary>
            System.Threading.Timer _timer;
            /// <summary>
            /// The zoom rectangle
            /// </summary>
            Rectangle _zoomRectangle;
            /// <summary>
            /// The transition
            /// </summary>
            Bitmap _transition = null;
            /// <summary>
            /// Initializes a new instance of the <see cref="ZoomOutTransition"/> class.
            /// </summary>
            /// <param name="fg">The fg.</param>
            /// <param name="bg">The bg.</param>
            /// <param name="transitionTime">The transition time.</param>
            /// <param name="stepTime">The step time.</param>
            /// <exception cref="ArgumentNullException">Either the foreground or background image must be non-null for fade transition - (Exception)null</exception>
            public ZoomOutTransition(Bitmap fg, Bitmap bg, int transitionTime, int stepTime)
                : base(fg, bg, transitionTime, stepTime)
            {
                if (bg == null && fg == null)
                    throw new ArgumentNullException("Either the foreground or background image must be non-null for fade transition", (Exception)null);
                if (bg == null)
                    _clientRectangle = new Rectangle(0, 0, fg.Width, fg.Height);
                else
                    _clientRectangle = new Rectangle(0, 0, bg.Width, bg.Height);
            }
            /// <summary>
            /// Timers the specified state.
            /// </summary>
            /// <param name="state">The state.</param>
            void Timer(object state)
            {
                try
                {
                    lock (_sync)
                    {
                        if (_transitioning && !_finished)
                        {
                            Step();
                        }
                    }
                }
                catch { }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public override void Start()
            {
                lock (_sync)
                {
                    _timer = new System.Threading.Timer(Timer, this, _stepTime, _stepTime);
                    _zoomRectangle = Rectangle.Inflate(_clientRectangle, _clientRectangle.Width / 5, _clientRectangle.Height / 5);
                    CreateTransitionImage();
                }
                base.Start();
            }

            /// <summary>
            /// Creates the transition image.
            /// </summary>
            void CreateTransitionImage()
            {
                if (_transition != null)
                    _transition.Dispose();

                int distanceX = Math.Max(_zoomRectangle.Width / 2 * _currentStep / _transitionTime, 0);
                int distanceY = Math.Max(_zoomRectangle.Height / 2 * _currentStep / _transitionTime, 0);

                Rectangle _drawRect = new Rectangle(_clientRectangle.Width / 2, _clientRectangle.Height / 2, 0, 0);
                _drawRect.Inflate(new Size(distanceX, distanceY));
                _transition = new Bitmap(_clientRectangle.Width, _clientRectangle.Height, PixelFormat.Format32bppArgb);
                GraphicsPath p = new GraphicsPath();
                p.AddEllipse(_drawRect);
                using (Graphics g = Graphics.FromImage(_transition))
                {
                    g.SetClip(p);
                    g.DrawImage(_front, 0, 0);
                    g.SetClip(_zoomRectangle, CombineMode.Xor);
                    g.DrawImage(_back, 0, 0);
                    g.ResetClip();
                }
            }
            /// <summary>
            /// Steps this instance.
            /// </summary>
            public override void Step()
            {
                lock (_sync)
                {
                    _currentStep += _stepTime;
                    CreateTransitionImage();
                }
                base.Step();
            }

            /// <summary>
            /// Finishes this instance.
            /// </summary>
            public override void Finish()
            {
                lock (_sync)
                {
                    if (_timer != null)
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();
                        _timer = null;
                    }
                    if (_transition != null)
                    {
                        _transition.Dispose();
                        _transition = null;
                    }
                }
                base.Finish();
            }

            /// <summary>
            /// Draws the specified g.
            /// </summary>
            /// <param name="g">The g.</param>
            public override void Draw(Graphics g)
            {
                lock (_sync)
                {
                    if (_transitioning)
                        g.DrawImage(_transition ?? _front, 0, 0);
                    else if (_finished)
                        g.DrawImage(_front, 0, 0);
                    else
                        g.DrawImage(_back, 0, 0);
                }
            }

            /// <summary>
            /// Resizes the specified new front.
            /// </summary>
            /// <param name="newFront">The new front.</param>
            /// <param name="newBack">The new back.</param>
            public override void Resize(Bitmap newFront, Bitmap newBack)
            {
                lock (_sync)
                {
                    _front = newFront;
                    _back = newBack;
                    if (_back == null)
                    {
                        _clientRectangle = new Rectangle(0, 0, _front.Width, _front.Height);
                    }
                    else
                        _clientRectangle = new Rectangle(0, 0, _back.Width, _back.Height);
                    _zoomRectangle = Rectangle.Inflate(_clientRectangle, _clientRectangle.Width / 5, _clientRectangle.Height / 5);
                    CreateTransitionImage();
                }
                RaiseChanged();
            }
        }
        #endregion

        #region Starting and stopping the transitions
        /// <summary>
        /// The transitioning
        /// </summary>
        bool _transitioning = false;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (!_transitioning)
            {
                _transitioning = true;
                if (_images.Count == 0)
                    StartDelay();
                else
                {
                    SetForegroundImage();
                    _effect = CreateTransition();
                    _effect.Changed += _effect_Changed;
                    _effect.Finished += _effect_Finished;

                    long timeSinceLoad = (long)(DateTime.Now - _loadedTime).TotalMilliseconds;
                    if (timeSinceLoad < DelayTime)
                    {
                        timerDelay.Interval = (int)(DelayTime - timeSinceLoad);
                        timerDelay.Start();
                    }
                    else
                    {
                        StartTransition();
                    }
                }
                if (TransitionsStarted != null)
                    TransitionsStarted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (_transitioning)
            {
                _transitioning = false;
                timerDelay.Stop();
                if (_effect != null)
                    _effect.Stop();
                if (TransitionsStopped != null)
                    TransitionsStopped(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the foreground image.
        /// </summary>
        void SetForegroundImage()
        {
            if (_images.Count == 1)
                _foregroundIndex = 0;
            else if (RandomOrder)
            {
                do
                {
                    _foregroundIndex = Random.Next(_images.Count);
                } while (_foregroundIndex == _backgroundIndex);
            }
            else if (++_foregroundIndex >= _images.Count)
                _foregroundIndex = 0;
            Bitmap fg = ResizeImageToFit(_foregroundIndex);
            lock (_imageLock)
            {
                _foreground = fg;
            }
        }

        /// <summary>
        /// Starts the delay.
        /// </summary>
        void StartDelay()
        {
            timerDelay.Interval = DelayTime;
            timerDelay.Start();
            if (_images.Count > 1 || _foregroundIndex != 0)
            {
                bgLoadTransition.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Creates the transition.
        /// </summary>
        /// <returns>Transition.</returns>
        Transition CreateTransition()
        {
            Bitmap fg, bg;
            lock (_imageLock)
            {
                fg = _foreground;
                bg = _background ?? (_background = ResizeImageToFit(-1));
            }
            return Transition.CreateTransition(CurrentEffect, fg, bg, TransitionTime, 1000 / TransitionFramesPerSecond);
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool StartTransition()
        {
            if (_effect != null)
            {
                _effect.Start();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles the Tick event of the timerDelay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timerDelay_Tick(object sender, EventArgs e)
        {
            if (_images.Count > 0)
            {
                timerDelay.Stop();
                if (_effect == null)
                {
                    // effect hasn't finished being created, so recheck frequently
                    timerDelay.Interval = 50;
                    timerDelay.Start();
                }
                else if (_images.Count == 1 && _foregroundIndex == 0)
                {
                    timerDelay.Interval = DelayTime;
                    timerDelay.Start();
                }
                else
                    StartTransition();
            }
        }

        /// <summary>
        /// Handles the DoWork event of the bgLoadTransition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void bgLoadTransition_DoWork(object sender, DoWorkEventArgs e)
        {
            // Make the front image the new back
            // and get the next image to transition to.
            SetForegroundImage();
            e.Result = CreateTransition();
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the bgLoadTransition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void bgLoadTransition_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _effect = (Transition)e.Result;
            // Add the events here, so that they are executed on the UI thread.
            _effect.Changed += _effect_Changed;
            _effect.Finished += _effect_Finished;
        }

        /// <summary>
        /// Handles the Finished event of the _effect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _effect_Finished(object sender, EventArgs e)
        {
            _effect.Changed -= _effect_Changed;
            _effect.Finished -= _effect_Finished;
            _effect = null;
            _currentEffect = TransitionEffect; // Resets the effect in case of change or random
            lock (_imageLock)
            {
                _background = _foreground;
                _backgroundIndex = _foregroundIndex;
            }
            Invalidate();
            if (_transitioning)
                StartDelay();
        }

        /// <summary>
        /// Handles the Changed event of the _effect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _effect_Changed(object sender, EventArgs e)
        {
            Invalidate();
        }


        #endregion

        #region Drawing Events
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_effect == null || !_transitioning)
            {
                if (_background != null)
                    e.Graphics.DrawImage(_background, 0, 0);
            }
            else
                _effect.Draw(e.Graphics);
            if (DesignMode && BorderStyle == System.Windows.Forms.BorderStyle.None)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.ClientSizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            Bitmap bg = ResizeImageToFit(_backgroundIndex);
            Bitmap fg = ResizeImageToFit(_foregroundIndex);
            lock (_imageLock)
            {
                _background = bg;
                _foreground = fg;
                if (_effect != null)
                    _effect.Resize(_foreground, _background);
            }
            Refresh();
        }


        #endregion
    }

    /// <summary>
    /// Class ImageEntryConverter.
    /// </summary>
    /// <seealso cref="System.ComponentModel.TypeConverter" />
    class ImageEntryConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this object supports properties, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)" /> should be called to find the properties of this object; otherwise, false.</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="value">An <see cref="T:System.Object" /> that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that are exposed for this data type, or null if there are no properties.</returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ImageEntry));
        }
    }

    /// <summary>
    /// Class ImageEntry.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [TypeConverter(typeof(ImageEntryConverter))]
    [Serializable]
    public class ImageEntry : INotifyPropertyChanged
    {
        /// <summary>
        /// The image
        /// </summary>
        protected Bitmap _image = null;
        /// <summary>
        /// The path
        /// </summary>
        protected string _path = null;
        /// <summary>
        /// The text
        /// </summary>
        protected string _text = null;
        /// <summary>
        /// The sizemode
        /// </summary>
        ImageDrawMode _sizemode = ImageDrawMode.Zoom;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        [DefaultValue(null)]
        public virtual Bitmap Image
        {
            get
            {
                if (_image == null)
                {
                    if (!string.IsNullOrEmpty(_path))
                        _image = new Bitmap(_path);
                }
                return _image;
            }
            set
            {
                _image = value;
                if (value != null)
                    _path = null;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        [DefaultValue("")]
        public virtual string Path
        {
            get { return _path ?? string.Empty; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _path = null;
                else
                {
                    _image = null;
                    _path = value;
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the size mode.
        /// </summary>
        /// <value>The size mode.</value>
        [DefaultValue(ImageDrawMode.Zoom)]
        public virtual ImageDrawMode SizeMode
        {
            get
            {
                return _sizemode;
            }
            set
            {
                _sizemode = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(_path) ? ((_image == null) ? null : _image.ToString()) : _path;
        }
        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Enum TransitionEffects
    /// </summary>
    public enum TransitionEffects : int
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The fade
        /// </summary>
        Fade,
        /// <summary>
        /// The dissolve
        /// </summary>
        Dissolve,
        /// <summary>
        /// The zoom in
        /// </summary>
        ZoomIn,
        /// <summary>
        /// The zoom out
        /// </summary>
        ZoomOut,
        /// <summary>
        /// The slide left
        /// </summary>
        SlideLeft,
        /// <summary>
        /// The slide right
        /// </summary>
        SlideRight,
        /// <summary>
        /// The slide up
        /// </summary>
        SlideUp,
        /// <summary>
        /// The slide down
        /// </summary>
        SlideDown,
        /// <summary>
        /// The random
        /// </summary>
        Random
    }

    /// <summary>
    /// Enum ImageDrawMode
    /// </summary>
    public enum ImageDrawMode
    {
        /// <summary>
        /// The zoom
        /// </summary>
        Zoom,
        /// <summary>
        /// The stretch
        /// </summary>
        Stretch,
        /// <summary>
        /// The copy
        /// </summary>
        Copy,
    }


    #endregion

    #region Designer Generated Code

    partial class ZeroitImageTransitionBox
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
            this.components = new System.ComponentModel.Container();
            this.timerDelay = new System.Windows.Forms.Timer(this.components);
            this.bgLoadTransition = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // timerDelay
            // 
            this.timerDelay.Tick += new System.EventHandler(this.timerDelay_Tick);
            // 
            // bgLoadTransition
            // 
            this.bgLoadTransition.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgLoadTransition_DoWork);
            this.bgLoadTransition.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgLoadTransition_RunWorkerCompleted);
            // 
            // ucImageShow
            // 
            this.DoubleBuffered = true;
            this.Name = "ucImageShow";
            this.Size = new System.Drawing.Size(165, 192);
            this.Load += new System.EventHandler(this.ucImageShow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The timer delay
        /// </summary>
        private System.Windows.Forms.Timer timerDelay;
        /// <summary>
        /// The bg load transition
        /// </summary>
        private System.ComponentModel.BackgroundWorker bgLoadTransition;

    }

    #endregion

    #endregion

    
}
