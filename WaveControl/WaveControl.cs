#region Imports

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{
    

    #region Wave Control


    /// <summary>
    /// Summary description for ZeroitWaveEffect.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ZeroitWaveEffect), "wavecontrol.bmp")]
    public class ZeroitWaveEffect : System.Windows.Forms.Control
    {
        // Parameters - All these should be exposed as accessors

        private int m_iNumFrames = 24;              // The ratio of number of frames per second and the number of
                                                    // frames determines the "speed" of the animation
        private double m_nWaveHeight = 100f;
        private double m_nRippleGranularity = 30;   // Ideally something like image height / 5
        private double m_nDamping = 1;              // Between 0.5 and 10
        private Bitmap m_image = null;

        // Internal class wide variables
        private int m_iFPS = 20;
        private Bitmap m_FlipImage = null;
        private Bitmap[] m_bmImages = null;
        private int m_iCurrentFrame = 0;
        private bool m_bPainting = false;
        private Bitmap m_bmBuffer = null;

        // Components/Controls
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        #region Constructor/Desctructor

        public ZeroitWaveEffect()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            timer.Interval = (m_iFPS == 0) ? 40 : 1000 / m_iFPS;
            timer.Tick += new EventHandler(timer_Tick);

            // Read the file and initialise the animation
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            if (asm == null)
                LakeImage = new System.Drawing.Bitmap(20, 20);
            else
            {
                System.IO.Stream stm = asm.GetManifestResourceStream("Lyquidity.UtilityLibrary.Controls.ZeroitWaveEffect.logo-large.jpg");
                if (stm == null)
                    LakeImage = new System.Drawing.Bitmap(20, 20);
                else
                    LakeImage = new System.Drawing.Bitmap(stm);
            }

            this.Width = this.m_image.Width;
            this.Height = this.m_image.Height * 2;

            this.MakeFlipImage();

            // Prevent flicker with double buffering and all painting inside WM_PAINT
            // Note that this doesn't seem to work so double-buffering is implemented in the 
            // overridden OnPaint() protected method
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            //SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, false);


            
        }

        ~ZeroitWaveEffect()
        { }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ZeroitWaveEffect
            // 
            this.Name = "ZeroitWaveEffect";
            //this.Resize += new System.EventHandler(this.WaveControl_Resize);

        }
        #endregion

        #region Overrides

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            timer.Enabled = false;
            timer.Dispose();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnPaintBackground paints the background in the BackColor and 
        /// invalidates the whole client area.  This creates a HUGE flicker
        /// so overriding the method and not calling the base method makes
        /// sure that no painting occurs other than in OnPaint()
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground (e);
        }


        /// <summary>
        /// Draw the images needed to create an animation effect.  The timer
        /// will call this function having set the m_iCurrentFrame to the
        /// required number.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            
            Graphics g = null;
            
            bool bBitmapFirstTime = false;

            // Check for early exit conditions
            if (!this.Visible) return;
            if (m_bPainting & !e.ClipRectangle.Size.Equals(this.Size)) return;

            // Use an exit condition now
            m_bPainting = true;

            try
            {
                // Use double buffing to eliminate flicker.  Setting styles on their does not work.
                if (m_bmBuffer == null)
                {
                    m_bmBuffer = new Bitmap(this.Width, this.Height);
                    bBitmapFirstTime = true;
                }

                bBitmapFirstTime = bBitmapFirstTime || e.ClipRectangle.Equals(this.ClientRectangle);

                // Get a graphics object
                g = Graphics.FromImage(m_bmBuffer);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                TransInPaint(g);
                //if (bBitmapFirstTime)
                //g.FillRectangle(Brushes.White, 0, 0, m_bmBuffer.Width, m_bmBuffer.Height);

                try
                {
                    // Get the rectangles needed
                    Rectangle srcRect = SourceRectangle();
                    Rectangle topRect = TopDestRectangle();
                    Rectangle bottomRect = BottomDestRectangle();

                    // Draw the main image
                    if ((bBitmapFirstTime) | (!e.ClipRectangle.Equals(bottomRect)))
                        g.DrawImage(m_image, topRect, srcRect, System.Drawing.GraphicsUnit.Pixel);

                    // Draw the required animation frame
                    g.Clip = new Region(bottomRect);
                    srcRect.Offset(0, 1);  // Cropping is at the top on the flipped image
                    g.DrawImage(m_FlipImage, bottomRect, srcRect, System.Drawing.GraphicsUnit.Pixel);

                }
                catch
                {
                }

                // Paint the generated bitmap using the controls graphic object
                e.Graphics.DrawImageUnscaled(m_bmBuffer, this.ClientRectangle);

            }
            catch
            {
                // Catch any and all errors
            }
            finally
            {
                g.Dispose();
                m_bPainting = false;

                if(!DesignMode)
                GC.Collect();  // Memory use goes wild unless the Garbage Collector is used frequently
            }

            
            
        }




        #region Transparency


        #region Include in Paint

        private void TransInPaint(Graphics g)
        {
            if (AllowTransparency)
            {
                MakeTransparent(this, g);
            }
        }

        #endregion

        #region Include in Private Field

        private bool allowTransparency = true;

        #endregion

        #region Include in Public Properties

        public bool AllowTransparency
        {
            get { return allowTransparency; }
            set
            {
                allowTransparency = value;

                Invalidate();
            }
        }

        #endregion

        #region Method

        //-----------------------------Include in Paint--------------------------//
        //
        // if(AllowTransparency)
        //  {
        //    MakeTransparent(this,g);
        //  }
        //
        //-----------------------------Include in Paint--------------------------//

        private static void MakeTransparent(Control control, Graphics g)
        {
            var parent = control.Parent;
            if (parent == null) return;
            var bounds = control.Bounds;
            var siblings = parent.Controls;
            int index = siblings.IndexOf(control);
            Bitmap behind = null;
            for (int i = siblings.Count - 1; i > index; i--)
            {
                var c = siblings[i];
                if (!c.Bounds.IntersectsWith(bounds)) continue;
                if (behind == null)
                    behind = new Bitmap(control.Parent.ClientSize.Width, control.Parent.ClientSize.Height);
                c.DrawToBitmap(behind, c.Bounds);
            }
            if (behind == null) return;
            g.DrawImage(behind, control.ClientRectangle, bounds, GraphicsUnit.Pixel);
            behind.Dispose();
        }

        #endregion


        #endregion



        #endregion

        #region Events

        private void timer_Tick(object sender, EventArgs e)
        {
            if (m_bPainting) return;

            // Set the image to be shown
            if (m_bmImages == null) return;

            this.m_FlipImage = m_bmImages[this.m_iCurrentFrame];

            // Trigger painting
            PaintEventArgs ePaint = new PaintEventArgs(this.CreateGraphics(), BottomDestRectangle());
            this.OnPaint(ePaint);

            // Note that the Invalidate() method could be called.  However the ClipRectangle 
            // doesn't seem to be used so no painting efficiencies can be gained.

            // this.Invalidate(BottomDestRectangle(), false);

            // Update the frame counter
            m_iCurrentFrame++;
            if (m_iCurrentFrame >= m_iNumFrames) m_iCurrentFrame = 0;
        }

        protected override void OnResize(System.EventArgs e)
        {
            this.m_bmBuffer = null;
            this.Invalidate();
        }

        #endregion

        #region Accessors

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Start
        {
            get { return this.timer.Enabled; }
            set { this.timer.Enabled = value;
                Invalidate();
            }
        }

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NumFrames
        {
            get { return m_iNumFrames; }
            set
            {
                if ((value < 5))
                {
                    value = 5;
                    MessageBox.Show("The number of frames must be between 5 and 32");
                    //throw new Exception("The number of frames must be between 5 and 32");
                    Invalidate();
                }

                if (value > 32)
                {
                    value = 32;
                    MessageBox.Show("The number of frames must be between 5 and 32");
                    //throw new Exception("The number of frames must be between 5 and 32");
                    Invalidate();
                }
                m_iNumFrames = value;
                timer.Stop();
                timer.Enabled = false;
                timer.Enabled = true;
                timer.Start();
                
                //CreateAnimation();
                Invalidate();
            }
        }

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bitmap LakeImage
        {
            get { return this.m_image; }
            set
            {
                if (value == null) throw new Exception("Image cannot be null");
                m_image = (Bitmap)value;
                CreateAnimation();
                Invalidate();
            }
        }

        public double WaveHeight
        {
            get { return this.m_nWaveHeight; }
            set
            {
                if ((value < 1) | value > 100) throw new Exception("Wave height must be between 1 and 100");
                this.m_nWaveHeight = value;
                CreateAnimation();
                Invalidate();
            }
        }
        public double RippleGranularity
        {
            get { return this.m_nRippleGranularity; }
            set
            {
                if ((value < 1) | value > 100) throw new Exception("Wave height must be between 1 and 100");
                this.m_nRippleGranularity = value;
                Invalidate();
            }
        }
        public double Damping
        {
            get { return this.m_nDamping; }
            set
            {
                if ((value < 0.5) | value > 5) throw new Exception("The Damping factor must be between 0.5 and 5");
                this.m_nDamping = value;
                Invalidate();
            }
        }

        #endregion

        #region Private functions

        private Rectangle TopDestRectangle()
        {
            return new Rectangle(new Point(0, 0), m_image.Size);
        }

        private Rectangle BottomDestRectangle()
        {
            Rectangle r = TopDestRectangle();
            if (this.m_bmImages != null) r = new Rectangle(new Point(0, 0), this.m_bmImages[this.m_iCurrentFrame].Size);
            r.Offset(0, m_image.Height); // Move the drawing position to the lower half
            r.Offset(0, 1);
            return r;
        }

        private Rectangle SourceRectangle()
        {
            Rectangle r = new Rectangle(new Point(0, 0), m_image.Size);
            r.Height--; // Crop the last line as its there in error
            return r;
        }


        private void CreateAnimation()
        {
            MakeFlipImage();
            MakeImageArray();

            for (int iFrame = 0; iFrame < m_iNumFrames; iFrame++)
            {
                DrawLakeEffect(iFrame);
            }

            GC.Collect();
        }

        private void MakeFlipImage()
        {
            this.m_FlipImage = (Bitmap)m_image.Clone();
            m_FlipImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        private void MakeImageArray()
        {
            m_bmImages = new Bitmap[this.m_iNumFrames];
            for (int iCtr = 0; iCtr < m_iNumFrames; iCtr++)
            {
                m_bmImages[iCtr] = new Bitmap(m_image.Width, m_image.Height);
                Graphics g = Graphics.FromImage(m_bmImages[iCtr]);
                g.FillRectangle(Brushes.White, 0, 0, m_bmImages[iCtr].Width, m_bmImages[iCtr].Height);
                g.Dispose();
            }

        }

        private void DrawLakeEffect(int iFrame)
        {
            int iImageLength = m_image.Width * m_image.Height; // Assume 32bpp
            int iImageWidth = m_image.Width;
            int iImageHeight = m_image.Height;

            int iTargetLine = 0;
            double nRipples = iImageHeight / m_nRippleGranularity;  // This value is constant for the image

            // Inspired by David Griffifths example

            BitmapData bmpFlipImageData = m_FlipImage.LockBits(new Rectangle(0, 0, iImageWidth, iImageHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            try
            {
                BitmapData bmpImageData = m_bmImages[iFrame].LockBits(new Rectangle(0, 0, iImageWidth, m_bmImages[iFrame].Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                
                try
                {
                    // Treat the iframe number as a phase of the wave cycle so convert it into radians 
                    // (by splitting 2*PI into m_iNumFrames segments

                    double nFrameAsRadians = 2 * Math.PI * (double)iFrame / (double)m_iNumFrames;

                    unsafe
                    {
                        // Get pointer as array of pixels (4 bytes each).  We could have copied
                        // the bitmap pixel array out and modified it using only safe code.  But
                        // this is faster and presents no real risk of causing an error.

                        // Use of a "pointer" allows you to treat the bitmap memory as an array
                        // of integers (or whatever type you specify).

                        int* pImageData = (int*)bmpImageData.Scan0;
                        int* pFlipImageData = (int*)bmpFlipImageData.Scan0;

                        int iMax = 0;

                        // Traverse the scanlines of the flipped image

                        // This loop is the key loop in this control.  It is responsible for generating the
                        // "wave" effect.  It does this by computing the displacement of a source image due
                        // to the effect of the waves on any line.  A target image is built up by coping lines
                        // from a source image.  Instead of copying the source line-for-line, the function
                        // computes a displacement value based on a sine function.  It is this displaced line
                        // (actual line +/- the displacement) that is copied across and gives rise to the
                        // wave effect.  
                        //
                        // In practice a simple displacement will only generate one wave for the image which 
                        // does not give the lake effect.  So multiple "waves" must appear to be generated.

                        for (int iCtr = 0; iCtr < iImageHeight; iCtr++)
                        {

                            // This function is borrowed from the Lake java example by David Griffifths and modified
                            // to suit this purpose.  The result of the function is a value indicating howd to displace
                            // lines in each frame. The size of the displacement varies with respect to the y position
                            // of the line.  Lines closer to the top of the image vary less than lines closer to the
                            // bottom.  This helps to offer some apparent perspective.  The structure of the function
                            // is as follows:
                            //
                            //          #Ripples            #Ripples   ImageHeight - Line#     Radians     Line# + WaveHeight
                            //         ---------- * Sine( ( -------- * ------------------- ) + ------- ) * ------------------
                            //         m_nDamping               1           Line#-1               1           ImageHeight
                            //
                            //  Terms:     A          B        B1            B2                  B3                C
                            //
                            // The two key terms are B and C.  B provides the oscillation to generate the wave effect while
                            // term C provides damping that is progressively less the further down the image to aid the 
                            // persepctive illusion.
                            //
                            // The sub-components in term B work as follows:
                            //
                            // B1 - Determines the number of ripples that will appear in the animation
                            // B2 - Determines the size of the ripples which grow progressively larger towards the
                            //      bottom of the image (perspective)
                            // B3 - Provides the oscillation offset for this frame
                            //
                            // Term A affects the damping and gives an opportunity to provide external influence on the
                            // damping.  Note that the damping effect is proportional to the inverse of the 

                            int iDisplacement = (int)(nRipples / m_nDamping *                           /* A  */
                                Math.Sin                                            /* B  */
                                ((double)((nRipples) *                          /* B1 */
                                (iImageHeight - iCtr) / (double)(iCtr + 1)) +  /* B2 */
                                nFrameAsRadians                             /* B3 */
                                ) *
                                (iCtr + m_nWaveHeight) /                            /* C  */
                                (double)iImageHeight
                                );

                            // System.Diagnostics.Debug.WriteLine(dispy.ToString());

                            // Pick a scanline, using a sinewave...
                            int iSourceLine = (iCtr < iDisplacement) ? iCtr : iCtr - iDisplacement;

                            // Calculate the offset of that scanline in the pixelarray
                            iSourceLine *= iImageWidth;

                            // Clip the value to avoid OutOfBounds
                            iSourceLine = (iSourceLine >= 0) ? iSourceLine : iCtr;

                            // Compute offset of target row of pixels
                            iTargetLine = iCtr * iImageWidth;

                            // Draw 1 scanline
                            for (int iPixelCtr = 0; iPixelCtr < iImageWidth; iPixelCtr++)
                                pImageData[iTargetLine + iPixelCtr] = (iSourceLine > iImageLength - iImageWidth) ? (int)0xFFFFFF : pFlipImageData[iSourceLine + iPixelCtr];

                            iMax = iCtr;  // Record this in case the target image is larger than this image so 
                                          // that it can be filled using a brush of the default colour
                        }

                        if (m_bmImages[iFrame].Height > iMax + 1)
                        {
                            for (int iCtr = iMax + 1; iCtr < m_bmImages[iFrame].Height; iCtr++)
                            {
                                iTargetLine = iCtr * iImageWidth;
                                for (int iPixelCtr = 0; iPixelCtr < iImageWidth; iPixelCtr++)
                                    pImageData[iTargetLine + iPixelCtr] = (int)0xFFFFFF;
                            }
                        }
                    }
                }
                catch
                {
                    iFrame = iFrame;
                }
                finally
                {
                    m_bmImages[iFrame].UnlockBits(bmpImageData);
                }
            }
            finally
            {
                try
                {
                    m_FlipImage.UnlockBits(bmpFlipImageData);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        #endregion

    }

    #endregion

}
