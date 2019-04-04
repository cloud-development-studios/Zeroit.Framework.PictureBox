using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Zeroit.Framework.PictureBox
{
    /// <summary>
    /// Class ZeroitImageRotator.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.PictureBox" />
    /// <seealso cref="System.Runtime.Serialization.ISerializable" />
    [Designer(typeof(ZeroitPictureBoxDesigner))]
    [Serializable]
    public partial class ZeroitImageRotator : System.Windows.Forms.PictureBox, ISerializable
    {

        #region ENUMS
        public enum ImageMode
        {
            Rotate,
            Scale
        }
        #endregion

        #region Private Fields

        private ImageMode mode = ImageMode.Rotate;
        private float angleRotation = 0f;

        private Image loadImage = new Bitmap(10, 10);

        private Image rotateImage = new Bitmap(10, 10);

        private int resizeWidth = 10;

        private int size;

        private int maxResize = 20;
        #endregion

        #region Public Properties

        public int MaxResize
        {
            get { return maxResize; }
            set
            {
                maxResize = value;
                Invalidate();
            }
        }

        public ImageMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;

                timer.Stop();
                timerDecrement.Stop();
                AutoAnimate = false;

                Invalidate();
            }
        }

        [Browsable(false)]
        public new Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;
                Invalidate();
            }
        }
        
        public int ResizeWidth
        {
            get { return resizeWidth; }
            set
            {
                resizeWidth = value;
                Invalidate();
            }
        }

        public Image RotatedImage
        {
            get { return rotateImage; }
            set
            {

                rotateImage = value;
                loadImage = value;
                switch (Mode)
                {
                    case ImageMode.Rotate:
                        Image = RotateImage(loadImage, AngleRotation);
                        break;
                    case ImageMode.Scale:
                        size = (Width + Height) / 2;
                        Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Invalidate();
            }
        }

        public float AngleRotation
        {
            get { return angleRotation; }
            set
            {
                angleRotation = value;

                switch (Mode)
                {
                    case ImageMode.Rotate:
                        Image = RotateImage(loadImage, value);
                        break;
                    case ImageMode.Scale:
                        size = (Width + Height) / 2;
                        Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Invalidate();
            }
        }

        #endregion

        #region Constructor

        public ZeroitImageRotator()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            IncludeInConstructor();

            size = (Width + Height) / 4;
        }

        public ZeroitImageRotator(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            IncludeInConstructor();

            size = (Width + Height) / 4;
        }

        #endregion

        #region Methods and Overrides
        private Image RotateImage(Image inputImg, float degreeAngle)
        {
            //Corners of the image
            PointF[] rotationPoints = { new PointF(0, 0),
                new PointF(inputImg.Width, 0),
                new PointF(0, inputImg.Height),
                new PointF(inputImg.Width, inputImg.Height)};

            //Rotate the corners
            PointMath.RotatePoints(rotationPoints, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f), degreeAngle);

            //Get the new bounds given from the rotation of the corners
            //(avoid clipping of the image)
            Rectangle bounds = PointMath.GetBounds(rotationPoints);

            //An empy bitmap to draw the rotated image
            Bitmap rotatedBitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //Transformation matrix
                Matrix m = new Matrix();
                m.RotateAt((float)degreeAngle, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f));
                m.Translate(-bounds.Left, -bounds.Top, MatrixOrder.Append); //shift to compensate for the rotation

                g.Transform = m;
                g.DrawImage(inputImg, 0, 0);
            }
            return (Image)rotatedBitmap;
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);

        //    Graphics g = e.Graphics;

        //    //if (Image != null)
        //    //    g.DrawImage(RotateImage(Image, AngleRotation), ClientRectangle);

        //    DrawImage(g, ClientRectangle);
        //}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (size < 0)
            {
                size = 5;
            }

            size = (Width + Height) / 4;

        }

        #endregion

        #region Animation Reverse


        #region Include in Private Field


        private bool autoAnimate = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer timerDecrement = new System.Windows.Forms.Timer();
        private float speedMultiplier = 1;
        private float change = 1f;
        private bool reverse = false;
        #endregion

        #region Include in Public Properties

        public bool AutoAnimate
        {
            get { return autoAnimate; }
            set
            {
                autoAnimate = value;

                if (value == true)
                {
                    timer.Enabled = true;
                }

                else
                {
                    timer.Enabled = false;
                    timerDecrement.Enabled = false;
                }

                Invalidate();
            }
        }

        public bool Reverse
        {
            get { return reverse; }
            set
            {

                reverse = value;
                Invalidate();
            }
        }

        public float Change
        {
            get { return change; }
            set
            {
                change = value;
                Invalidate();
            }
        }

        public float SpeedMultiplier
        {
            get { return speedMultiplier; }
            set
            {
                speedMultiplier = value;
                Invalidate();
            }
        }

        public int TimerInterval
        {
            get { return timer.Interval; }
            set
            {
                timer.Interval = value;
                timerDecrement.Interval = value;
                Invalidate();
            }
        }


        #endregion

        #region Event

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (Reverse)
            {
                switch (Mode)
                {
                    case ImageMode.Rotate:
                        if (this.AngleRotation + (Change * SpeedMultiplier) > 360)
                        {
                            timer.Stop();
                            timer.Enabled = false;
                            timerDecrement.Enabled = true;
                            timerDecrement.Start();
                            //timerDecrement.Tick += TimerDecrement_Tick;

                            if (loadImage != null)
                            {
                                Image = RotateImage(loadImage, AngleRotation);

                            }

                            Invalidate();
                        }

                        else
                        {
                            AngleRotation += (Change * SpeedMultiplier);

                            if (loadImage != null)
                            {
                                Image = RotateImage(loadImage, AngleRotation);

                            }

                            Invalidate();
                        }
                        break;
                    case ImageMode.Scale:
                        if (this.ResizeWidth + (Change * SpeedMultiplier) > MaxResize)
                        {
                            timer.Stop();
                            timer.Enabled = false;
                            timerDecrement.Enabled = true;
                            timerDecrement.Start();
                            //timerDecrement.Tick += TimerDecrement_Tick;

                            if (loadImage != null)
                            {
                                Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);

                            }

                            Invalidate();
                        }

                        else
                        {
                            ResizeWidth += (int)(Change * SpeedMultiplier);

                            if (loadImage != null)
                            {
                                Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);

                            }

                            Invalidate();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            else
            {
                switch (Mode)
                {
                    case ImageMode.Rotate:

                        if (AngleRotation + (Change * SpeedMultiplier) > 360)
                        {

                            timerDecrement.Enabled = false;
                            timerDecrement.Stop();
                            //timerDecrement.Tick += TimerDecrement_Tick;
                            AngleRotation = 0;

                            if (loadImage != null)
                            {
                                Image = RotateImage(loadImage, AngleRotation);

                            }

                            Invalidate();
                        }

                        else
                        {
                            AngleRotation += (Change * SpeedMultiplier);

                            if (loadImage != null)
                            {
                                Image = RotateImage(loadImage, AngleRotation);

                            }

                            Invalidate();
                        }

                        break;
                    case ImageMode.Scale:

                        if (ResizeWidth + (Change * SpeedMultiplier) > MaxResize)
                        {

                            timerDecrement.Enabled = false;
                            timerDecrement.Stop();
                            //timerDecrement.Tick += TimerDecrement_Tick;
                            ResizeWidth = 0;

                            if (loadImage != null)
                            {
                                Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);

                            }

                            Invalidate();
                        }

                        else
                        {
                            ResizeWidth += (int)(Change * SpeedMultiplier);

                            if (loadImage != null)
                            {
                                Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);

                            }

                            Invalidate();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            if (!DesignMode)
            {
                GC.Collect();
            }

        }


        private void TimerDecrement_Tick(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case ImageMode.Rotate:
                    if (this.AngleRotation < -360)
                    {
                        timerDecrement.Stop();
                        timerDecrement.Enabled = false;
                        timer.Enabled = true;
                        timer.Start();
                        //timer.Tick += Timer_Tick;

                        if (loadImage != null)
                        {
                            Image = RotateImage(loadImage, AngleRotation);

                        }

                        Invalidate();
                    }

                    else
                    {
                        AngleRotation -= (Change * SpeedMultiplier);

                        if (loadImage != null)
                            Image = RotateImage(loadImage, AngleRotation);

                        Invalidate();
                    }
                    break;
                case ImageMode.Scale:
                    if (this.ResizeWidth < 0)
                    {
                        timerDecrement.Stop();
                        timerDecrement.Enabled = false;
                        timer.Enabled = true;
                        timer.Start();
                        //timer.Tick += Timer_Tick;

                        if (loadImage != null)
                        {
                            Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);

                        }

                        Invalidate();
                    }

                    else
                    {
                        ResizeWidth -= (int)(Change * SpeedMultiplier);

                        if (loadImage != null)
                            Image = ResizeImage(loadImage, size + ResizeWidth, size + ResizeWidth);


                        Invalidate();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            if (!DesignMode)
            {
                GC.Collect();
            }
        }


        #endregion

        #region Constructor

        private void IncludeInConstructor()
        {

            if (DesignMode)
            {
                timer.Tick += Timer_Tick;
                timerDecrement.Tick += TimerDecrement_Tick;
                if (AutoAnimate)
                {
                    timerDecrement.Interval = 10;
                    timer.Interval = 10;
                    timer.Start();
                }
            }

            if (!DesignMode)
            {
                timer.Tick += Timer_Tick;
                timerDecrement.Tick += TimerDecrement_Tick;
                if (AutoAnimate)
                {
                    timerDecrement.Interval = 10;
                    timer.Interval = 10;
                    timer.Start();
                }
            }

        }

        #endregion


        #endregion

        #region Serialization
        public ZeroitImageRotator(SerializationInfo info, StreamingContext context)
        {
            angleRotation = (float)info.GetValue("angleRotation", typeof(float));
            loadImage = (Image)info.GetValue("loadImage", typeof(Image));
            rotateImage = (Image)info.GetValue("rotateImage", typeof(Image));
            AutoAnimate = info.GetBoolean("AutoAnimate");
            speedMultiplier = (float)info.GetValue("speedMultiplier", typeof(float));
            change = (float)info.GetValue("change", typeof(float));
            reverse = info.GetBoolean("reverse");
            resizeWidth = info.GetInt32("resizeWidth");
            resizeWidth = info.GetInt32("autoAnimate");
            TimerInterval = info.GetInt32("TimerInterval");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("angleRotation", angleRotation);
            info.AddValue("loadImage", loadImage);
            info.AddValue("rotateImage", rotateImage);
            info.AddValue("AutoAnimate", AutoAnimate);
            info.AddValue("speedMultiplier", speedMultiplier);
            info.AddValue("change", change);
            info.AddValue("reverse", reverse);
            info.AddValue("resizeWidth", resizeWidth);
            info.AddValue("autoAnimate", autoAnimate);
            info.AddValue("TimerInterval", TimerInterval);
        }

        #endregion
    }

    #region Smart Tag
    
    public class ZeroitPictureBoxDesigner : ControlDesigner
    {
        private DesignerActionListCollection _actionLists;

        public ZeroitPictureBoxDesigner()
        {
            this.AutoResizeHandles = true;
        }

        private void DrawBorder(Graphics graphics)
        {
            Control control = this.Control;
            Rectangle clientRectangle = control.ClientRectangle;
            Pen pen = new Pen((double)control.BackColor.GetBrightness() >= 0.5 ? ControlPaint.Dark(control.BackColor) : ControlPaint.Light(control.BackColor));
            pen.DashStyle = DashStyle.Dash;
            --clientRectangle.Width;
            --clientRectangle.Height;
            graphics.DrawRectangle(pen, clientRectangle);
            pen.Dispose();
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            if (((ZeroitImageRotator)this.Component).BorderStyle == BorderStyle.None)
                this.DrawBorder(pe.Graphics);
            base.OnPaintAdornments(pe);
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                object component = (object)this.Component;
                PropertyDescriptor property = TypeDescriptor.GetProperties((object)this.Component)["SizeMode"];
                if (property != null && (PictureBoxSizeMode)property.GetValue(component) == PictureBoxSizeMode.AutoSize)
                    selectionRules &= ~SelectionRules.AllSizeable;
                return selectionRules;
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this._actionLists == null)
                {
                    this._actionLists = new DesignerActionListCollection();
                    this._actionLists.Add((DesignerActionList)new PictureBoxActionList(this));
                }
                return this._actionLists;
            }
        }
    }

    internal class PictureBoxActionList : DesignerActionList
    {

        //Replace SmartTag with the Component Class Name. In this case the component class name is SmartTag
        private ZeroitImageRotator colUserControl;

        private ZeroitPictureBoxDesigner _designer;

        private DesignerActionUIService designerActionUISvc = null;

        private DesignerActionUIService DesignerActionUIService
        {
            get { return GetService(typeof(DesignerActionUIService)) as DesignerActionUIService; }
        }


        public PictureBoxActionList(ZeroitPictureBoxDesigner designer)
            : base(designer.Component)
        {
            this._designer = designer;

            this.colUserControl = this.Component as ZeroitImageRotator;

            // Cache a reference to DesignerActionUIService, so the 
            // DesigneractionList can be refreshed. 
            this.designerActionUISvc = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;

        }

        public PictureBoxSizeMode SizeMode
        {
            get
            {
                return ((ZeroitImageRotator)this.Component).SizeMode;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)[nameof(SizeMode)].SetValue((object)this.Component, (object)value);
            }
        }

        public Image RotatedImage
        {
            get
            {
                return ((ZeroitImageRotator)this.Component).RotatedImage;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)[nameof(RotatedImage)].SetValue((object)this.Component, (object)value);
            }
        }

        public float AngleRotation
        {
            get
            {
                return ((ZeroitImageRotator)this.Component).AngleRotation;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)[nameof(AngleRotation)].SetValue((object)this.Component, (object)value);
            }
        }

        public int TimerInterval
        {
            get
            {
                return ((ZeroitImageRotator)this.Component).TimerInterval;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)[nameof(TimerInterval)].SetValue((object)this.Component, (object)value);
            }
        }

        public float SpeedMultiplier
        {
            get
            {
                return ((ZeroitImageRotator)this.Component).SpeedMultiplier;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)[nameof(SpeedMultiplier)].SetValue((object)this.Component, (object)value);
            }
        }

        public float Change
        {
            get
            {
                return ((ZeroitImageRotator)this.Component).Change;
            }
            set
            {
                TypeDescriptor.GetProperties((object)this.Component)[nameof(Change)].SetValue((object)this.Component, (object)value);
            }
        }


        public void ChooseImage()
        {
            EditorServiceContext.EditValue((ComponentDesigner)this._designer, (object)this.Component, "RotatedImage");
        }

        protected virtual void AutoAnimated()
        {
            colUserControl.AutoAnimate = !colUserControl.AutoAnimate;
            colUserControl.Invalidate();
            RefreshComponent();
        }

        internal void RefreshComponent()
        {
            if (DesignerActionUIService != null)
                DesignerActionUIService.Refresh(colUserControl);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            //Create entries for static Information section.
            StringBuilder location = new StringBuilder("Product: ");
            location.Append(colUserControl.ProductName);
            StringBuilder size = new StringBuilder("Version: ");
            size.Append(colUserControl.ProductVersion);

            return new DesignerActionItemCollection()
            {
                //(DesignerActionItem) new DesignerActionMethodItem((DesignerActionList) this, "ChooseImage", System.Design.SR.GetString("ChooseImageDisplayName"), System.Design.SR.GetString("PropertiesCategoryName"), System.Design.SR.GetString("ChooseImageDescription"), true),
                //(DesignerActionItem) new DesignerActionPropertyItem("SizeMode", System.Design.SR.GetString("SizeModeDisplayName"), System.Design.SR.GetString("PropertiesCategoryName"), System.Design.SR.GetString("SizeModeDescription"))

                (DesignerActionItem) new DesignerActionHeaderItem("Appearance"),
                (DesignerActionItem) new DesignerActionHeaderItem("Values"),
                (DesignerActionItem) new DesignerActionHeaderItem("Behaviour"),
                (DesignerActionItem) new DesignerActionMethodItem(this, "ChooseImage", "Choose Image", "Appearance", true),
                (DesignerActionItem) new DesignerActionPropertyItem("SizeMode",
                    "Size Mode", "Appearance",
                    "Sets the Size Mode."),

                (DesignerActionItem) new DesignerActionPropertyItem("AngleRotation",
                    "Angle Rotation", "Values",
                    "Sets the Angle Rotation."),

                (DesignerActionItem) new DesignerActionPropertyItem("TimerInterval",
                    "Speed", "Values",
                    "Sets the animation speed."),

                (DesignerActionItem) new DesignerActionPropertyItem("SpeedMultiplier",
                    "Speed Multiplier", "Values",
                    "Sets the speed multiplier."),

                (DesignerActionItem) new DesignerActionPropertyItem("Change",
                    "Change", "Values",
                    "Sets the change in value multiplier."),

                 colUserControl.AutoAnimate == false ? (DesignerActionItem)

                    (DesignerActionItem) new DesignerActionMethodItem(this, "AutoAnimated", "Animate", "Behaviour", true)
                     :
                     new DesignerActionMethodItem(this, "AutoAnimated", "Don't Animate", "Behaviour", true),

                (DesignerActionItem) new DesignerActionTextItem(location.ToString(),
                    "Information"),
                (DesignerActionItem) new DesignerActionTextItem(size.ToString(),
                    "Information")
            };
        }



    }

    internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
    {
        private ComponentDesigner _designer;
        private IComponentChangeService _componentChangeSvc;
        private PropertyDescriptor _targetProperty;

        internal EditorServiceContext(ComponentDesigner designer)
        {
            this._designer = designer;
        }

        internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
        {
            this._designer = designer;
            this._targetProperty = prop;
            if (prop != null)
                return;
            prop = TypeDescriptor.GetDefaultProperty((object)designer.Component);
            if (prop == null || !typeof(ICollection).IsAssignableFrom(prop.PropertyType))
                return;
            this._targetProperty = prop;
        }

        internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
          : this(designer, prop)
        {
            this._designer.Verbs.Add(new DesignerVerb(newVerbText, new EventHandler(this.OnEditItems)));
        }

        public static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(objectToChange)[propName];
            EditorServiceContext editorServiceContext = new EditorServiceContext(designer, property);
            UITypeEditor editor = property.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
            object obj1 = property.GetValue(objectToChange);
            object obj2 = editor.EditValue((ITypeDescriptorContext)editorServiceContext, (IServiceProvider)editorServiceContext, obj1);
            if (obj2 != obj1)
            {
                try
                {
                    property.SetValue(objectToChange, obj2);
                }
                catch (CheckoutException ex)
                {
                }
            }
            return obj2;
        }

        private IComponentChangeService ChangeService
        {
            get
            {
                if (this._componentChangeSvc == null)
                    this._componentChangeSvc = (IComponentChangeService)((IServiceProvider)this).GetService(typeof(IComponentChangeService));
                return this._componentChangeSvc;
            }
        }

        IContainer ITypeDescriptorContext.Container
        {
            get
            {
                if (this._designer.Component.Site != null)
                    return this._designer.Component.Site.Container;
                return (IContainer)null;
            }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            this.ChangeService.OnComponentChanged((object)this._designer.Component, (MemberDescriptor)this._targetProperty, (object)null, (object)null);
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            try
            {
                this.ChangeService.OnComponentChanging((object)this._designer.Component, (MemberDescriptor)this._targetProperty);
            }
            catch (CheckoutException ex)
            {
                if (ex == CheckoutException.Canceled)
                    return false;
                throw;
            }
            return true;
        }

        object ITypeDescriptorContext.Instance
        {
            get
            {
                return (object)this._designer.Component;
            }
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get
            {
                return this._targetProperty;
            }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(ITypeDescriptorContext) || serviceType == typeof(IWindowsFormsEditorService))
                return (object)this;
            if (this._designer.Component != null && this._designer.Component.Site != null)
                return this._designer.Component.Site.GetService(serviceType);
            return (object)null;
        }

        void IWindowsFormsEditorService.CloseDropDown()
        {
        }

        void IWindowsFormsEditorService.DropDownControl(Control control)
        {
        }

        DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
        {
            IUIService service = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
            if (service != null)
                return service.ShowDialog(dialog);
            return dialog.ShowDialog(this._designer.Component as IWin32Window);
        }

        private void OnEditItems(object sender, EventArgs e)
        {
            object component = this._targetProperty.GetValue((object)this._designer.Component);
            if (component == null)
                return;
            (TypeDescriptor.GetEditor(component, typeof(UITypeEditor)) as CollectionEditor)?.EditValue((ITypeDescriptorContext)this, (IServiceProvider)this, component);
        }
    }

    #endregion

}
