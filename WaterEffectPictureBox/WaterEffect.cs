// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="WaterEffect.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Imports

//using System.Windows.Forms.VisualStyles;

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Zeroit.Framework.PictureBox
{


    #region WaterEffect PictureBox

    /// <summary>
    /// 
    /// </summary>
    public class ZeroitWaterEffect : System.Windows.Forms.Panel
    {
        private System.Windows.Forms.Timer effectTimer;
        private System.ComponentModel.IContainer components;

        private Bitmap _bmp = null;
        private short[,,] _waves;
        private int _waveWidth;
        private int _waveHeight;
        private int _activeBuffer = 0;
        private bool _weHaveWaves = false;
        private int _bmpHeight, _bmpWidth;
        private byte[] _bmpBytes;
        private BitmapData _bmpBitmapData;
        private int _scale = 1;
        private int timerInterval = 50;

        private int[] magicPlayer1 = new int[] {4,4,4,4 };
        private int[] magicPlayer2 = new int[] { 1, 2, 3 };
        private int[] magicPlayer3 = new int[] { 4, 4, 4 };
        private int[] magicPlayer4 = new int[] { 1,2 };

        public int[] MagicPlayer1
        {
            get { return magicPlayer1; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 4)
                    {
                        value[i] = 4;
                    }
                }
                magicPlayer1 = value;
                Invalidate();
            }
        }

        public int[] MagicPlayer2
        {
            get { return magicPlayer2; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 4)
                    {
                        value[i] = 4;
                    }
                }
                magicPlayer2 = value;
                Invalidate();
            }
        }

        public int[] MagicPlayer3
        {
            get { return magicPlayer3; }
            set
            {
                
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 4)
                    {
                        value[i] = 4;
                    }
                }

                magicPlayer3 = value;
                Invalidate();
            }
        }

        public int[] MagicPlayer4
        {
            get { return magicPlayer4; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 4)
                    {
                        value[i] = 4;
                    }
                }

                magicPlayer4 = value;
                Invalidate();
            }
        }

        public int TimerInterval
        {
            get { return timerInterval; }
            set
            {
                timerInterval = value;
                Invalidate();
            }
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.effectTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // effectTimer
            // 
            this.effectTimer.Tick += new System.EventHandler(this.effectTimer_Tick);
            
        }

        public ZeroitWaterEffect()
        {
            InitializeComponent();
            effectTimer.Enabled = true;
            effectTimer.Interval = timerInterval;
            
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            

        }

        public ZeroitWaterEffect(Bitmap bmp) : this()
        {
            //Bitmap bit = new Bitmap(5, 5);
            //bit.Clone();

            //ImageBitmap = bit;
            this.ImageBitmap = bmp;
        }

        protected override void Dispose(bool disposing)
        {
            //_bmp.UnlockBits(_bmpBitmapData);
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
        /// Timer handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void effectTimer_Tick(object sender, System.EventArgs e)
        {

            if (_weHaveWaves)
            {
                Invalidate();

                ProcessWaves();

            }
        }

        /// <summary>
        /// Paint handler
        /// 
        /// Calculates the final effect-image out of
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            TransInPaint(e.Graphics);
            //IncludeInPaint(e);

            #region Old Code

            _bmp = new Bitmap(Properties.Resources.Folder_48px_1);
            Bitmap tmp = _bmp;
            
            int xOffset, yOffset;
            byte alpha;

            if (_weHaveWaves)
            {
                BitmapData tmpData = tmp.LockBits(new Rectangle(0, 0, _bmpWidth, _bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                byte[] tmpBytes = new Byte[_bmpWidth * _bmpHeight * 4];

                Marshal.Copy(tmpData.Scan0, tmpBytes, 0, _bmpWidth * _bmpHeight * 4);

                for (int x = 1; x < _bmpWidth - 1; x++)
                {
                    for (int y = 1; y < _bmpHeight - 1; y++)
                    {
                        int waveX = (int)x >> _scale;
                        int waveY = (int)y >> _scale;

                        //check bounds
                        if (waveX <= 0) waveX = 1;
                        if (waveY <= 0) waveY = 1;
                        if (waveX >= _waveWidth - 1) waveX = _waveWidth - 2;
                        if (waveY >= _waveHeight - 1) waveY = _waveHeight - 2;

                        //this gives us the effect of water breaking the light
                        xOffset = (_waves[waveX - 1, waveY, _activeBuffer] - _waves[waveX + 1, waveY, _activeBuffer]) >> 3;
                        yOffset = (_waves[waveX, waveY - 1, _activeBuffer] - _waves[waveX, waveY + 1, _activeBuffer]) >> 3;

                        if ((xOffset != 0) || (yOffset != 0))
                        {
                            //check bounds
                            if (x + xOffset >= _bmpWidth - 1) xOffset = _bmpWidth - x - 1;
                            if (y + yOffset >= _bmpHeight - 1) yOffset = _bmpHeight - y - 1;
                            if (x + xOffset < 0) xOffset = -x;
                            if (y + yOffset < 0) yOffset = -y;

                            //generate alpha
                            alpha = (byte)(200 - xOffset);
                            if (alpha < 0) alpha = 0;
                            if (alpha > 255) alpha = 254;

                            //set colors
                            if (AllowTransparency)
                            {
                                tmpBytes[4 * (x + y * _bmpWidth)] = _bmpBytes[4 * (x + xOffset + (y + yOffset) * _bmpWidth)];
                                tmpBytes[4 * (x + y * _bmpWidth) + 3] = _bmpBytes[4 * (x + xOffset + (y + yOffset) * _bmpWidth) + 1];
                                tmpBytes[4 * (x + y * _bmpWidth) + 2] = _bmpBytes[4 * (x + xOffset + (y + yOffset) * _bmpWidth) + 2];
                                tmpBytes[4 * (x + y * _bmpWidth) + 1] = alpha;

                            }
                            else
                            {
                                tmpBytes[MagicPlayer1[0] * (x + y * _bmpWidth)] = _bmpBytes[MagicPlayer3[0] * (x + xOffset + (y + yOffset) * _bmpWidth)];
                                tmpBytes[MagicPlayer1[1] * (x + y * _bmpWidth) + MagicPlayer2[0]] = _bmpBytes[MagicPlayer3[1] * (x + xOffset + (y + yOffset) * _bmpWidth) + MagicPlayer4[0]];
                                tmpBytes[MagicPlayer1[2] * (x + y * _bmpWidth) + MagicPlayer2[1]] = _bmpBytes[MagicPlayer3[2] * (x + xOffset + (y + yOffset) * _bmpWidth) + MagicPlayer4[1]];
                                tmpBytes[MagicPlayer1[3] * (x + y * _bmpWidth) + MagicPlayer2[2]] = alpha;

                            }

                        }

                    }
                }

                //copy data back
                Marshal.Copy(tmpBytes, 0, tmpData.Scan0, _bmpWidth * _bmpHeight * 4);
                tmp.UnlockBits(tmpData);

            }

            e.Graphics.DrawImage(tmp, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

            if (!DesignMode)
            {
                GC.Collect();
            }
                
            #endregion


        }



        
        /// <summary>
        /// This is the method that actually does move the waves around and simulates the
        /// behaviour of water.
        /// </summary>
        private void ProcessWaves()
        {
            int newBuffer = (_activeBuffer == 0) ? 1 : 0;
            bool wavesFound = false;

            for (int x = 1; x < _waveWidth - 1; x++)
            {
                for (int y = 1; y < _waveHeight - 1; y++)
                {
                    _waves[x, y, newBuffer] = (short)(
                                            ((_waves[x - 1, y - 1, _activeBuffer] +
                                            _waves[x, y - 1, _activeBuffer] +
                                            _waves[x + 1, y - 1, _activeBuffer] +
                                            _waves[x - 1, y, _activeBuffer] +
                                            _waves[x + 1, y, _activeBuffer] +
                                            _waves[x - 1, y + 1, _activeBuffer] +
                                            _waves[x, y + 1, _activeBuffer] +
                                            _waves[x + 1, y + 1, _activeBuffer]) >> 2) - _waves[x, y, newBuffer]);

                    //damping
                    if (_waves[x, y, newBuffer] != 0)
                    {
                        _waves[x, y, newBuffer] -= (short)(_waves[x, y, newBuffer] >> 4);
                        wavesFound = true;
                    }


                }
            }

            _weHaveWaves = wavesFound;
            _activeBuffer = newBuffer;

        }


        /// <summary>
        /// This function is used to start a wave by simulating a round drop
        /// </summary>
        /// <param name="x">x position of the drop</param>
        /// <param name="y">y position of the drop</param>
        /// <param name="height">Height position of the drop</param>
        private void PutDrop(int x, int y, short height)
        {
            _weHaveWaves = true;
            int radius = 20;
            double dist;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (((x + i >= 0) && (x + i < _waveWidth - 1)) && ((y + j >= 0) && (y + j < _waveHeight - 1)))
                    {
                        dist = Math.Sqrt(i * i + j * j);
                        if (dist < radius)
                            _waves[x + i, y + j, _activeBuffer] = (short)(Math.Cos(dist * Math.PI / radius) * height);
                    }
                }
            }
        }

        /// <summary>
        /// The MouseMove handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                int realX = (int)((e.X / (double)this.ClientRectangle.Width) * _waveWidth);
                int realY = (int)((e.Y / (double)this.ClientRectangle.Height) * _waveHeight);
                PutDrop(realX, realY, 200);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                int realX = (int)((e.X / (double)this.ClientRectangle.Width) * _waveWidth);
                int realY = (int)((e.Y / (double)this.ClientRectangle.Height) * _waveHeight);
                PutDrop(realX, realY, 200);
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
        
        #region Properties
        /// <summary>
        /// Our background image
        /// </summary>
        public Bitmap ImageBitmap
        {
            get { return _bmp; }
            set
            {
                _bmp = value;
                _bmpHeight = _bmp.Height;
                _bmpWidth = _bmp.Width;

                _waveWidth = _bmpWidth >> _scale;
                _waveHeight = _bmpHeight >> _scale;
                _waves = new Int16[_waveWidth, _waveHeight, 2];

                _bmpBytes = new Byte[_bmpWidth * _bmpHeight * 4];
                _bmpBitmapData = _bmp.LockBits(new Rectangle(0, 0, _bmpWidth, _bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                Marshal.Copy(_bmpBitmapData.Scan0, _bmpBytes, 0, _bmpWidth * _bmpHeight * 4);

                Invalidate();
            }
        }

        /// <summary>
        /// The scale of the wave matrix compared to the size of the image.
        /// Use it for large images to reduce processor load.
        /// 
        /// 0 : wave resolution is the same than image resolution
        /// 1 : wave resolution is half the image resolution
        /// ...and so on
        /// </summary>
        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        #endregion
    }

    #endregion

}
