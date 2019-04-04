// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="ImageSlider.cs" company="Zeroit Dev Technologies">
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
using System.Drawing;
//using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

#endregion

namespace Zeroit.Framework.PictureBox
{

    #region ZeroitImageSlider

    /// <summary>
    /// Class ZeroitImageSlider.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Panel" />
    public class ZeroitImageSlider : Panel
    {
        /// <summary>
        /// The timer
        /// </summary>
        System.Windows.Forms.Timer _timer;
        /// <summary>
        /// The caption text left
        /// </summary>
        int _captionTextLeft = 20;
        /// <summary>
        /// The caption position x
        /// </summary>
        int _captionPosX = 20;
        /// <summary>
        /// The page index
        /// </summary>
        int _pageIndex = 0;

        /// <summary>
        /// The image list
        /// </summary>
        protected List<Image> _imageList = new List<Image>();
        /// <summary>
        /// The caption list
        /// </summary>
        protected List<string> _captionList = new List<string>();
        /// <summary>
        /// The caption bg color
        /// </summary>
        protected List<Color> _captionBgColor = new List<Color>();

        /// <summary>
        /// The left button
        /// </summary>
        xButton leftButton;
        /// <summary>
        /// The right button
        /// </summary>
        xButton rightButton;


        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroitImageSlider"/> class.
        /// </summary>
        public ZeroitImageSlider()
        {
            //this.Animation = true;
            //this.CaptionAnimationSpeed = 50;
            this.CaptionTextLeft = 20;
            //this.CaptionHeight = 50;
            //this.CaptionBackgrounColor = Color.Black;
            //this.CaptionOpacity = 100;

            leftButton = new xButton();
            
            leftButton.Text = "<";
            leftButton.ImageAlign = ContentAlignment.MiddleCenter;
            leftButton.Image = Properties.Resources.Back_32px;
            leftButton.Click += new EventHandler(leftButton_Click);

            rightButton = new xButton();
            
            rightButton.Text = ">";
            rightButton.ImageAlign = ContentAlignment.MiddleCenter;
            rightButton.Image = Properties.Resources.Forward_32px;
            rightButton.Click += new EventHandler(rightButton_Click);

            this.Resize += ZeroitImageSlider_Resize;

            this.Controls.Add(leftButton);
            this.Controls.Add(rightButton);
        }

        /// <summary>
        /// Handles the Resize event of the ZeroitImageSlider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void ZeroitImageSlider_Resize(object sender, EventArgs e)
        {
            leftButton.Location = new Point(0, (this.Height / 2) - (leftButton.Height / 2));
            rightButton.Location = new Point(this.Width - rightButton.Width, (this.Height / 2) - (rightButton.Height / 2));
        }

        /// <summary>
        /// Handles the Click event of the leftButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void leftButton_Click(object sender, EventArgs e)
        {

            if (_pageIndex > 0)
            {
                --_pageIndex;
            }
            else
            {
                _pageIndex = _imageList.Count - 1;
            }

            if (Animation)
            {
                _captionPosX = this.Width;
                this.DoubleBuffered = true;

                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 1;
                _timer.Tick += new EventHandler(_timer_Tick);
                _timer.Start();
            }
            else
            {
                _captionPosX = _captionTextLeft;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Handles the Click event of the rightButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void rightButton_Click(object sender, EventArgs e)
        {

            if (_pageIndex < _imageList.Count - 1)
            {
                ++_pageIndex;
            }
            else
            {
                _pageIndex = 0;
            }

            if (Animation)
            {
                _captionPosX = this.Width;
                DoubleBuffered = true;

                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 1;
                _timer.Tick += new EventHandler(_timer_Tick);
                _timer.Start();
            }
            else
            {
                _captionPosX = _captionTextLeft;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Handles the Tick event of the _timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _timer_Tick(object sender, EventArgs e)
        {
            if (_captionPosX >= _captionTextLeft)
            {
                int subtract = captionAnimationSpeed;

                int diff = _captionPosX - subtract;

                if (diff < subtract)
                {
                    _captionPosX -= _captionPosX - _captionTextLeft;
                }
                else
                {
                    _captionPosX -= subtract;
                }

                this.Invalidate();
            }
            else
            {
                this.DoubleBuffered = false;
                _timer.Dispose();
            }
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="path">The path.</param>
        public void AddImage(string path)
        {
            Image img = Image.FromFile(path);
            _AddImage(img, "", this.captionBackgrounColor);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="caption">The caption.</param>
        public void AddImage(string path, string caption)
        {
            Image img = Image.FromFile(path);
            _AddImage(img, caption, this.captionBackgrounColor);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="captionBackgroundColor">Color of the caption background.</param>
        public void AddImage(string path, string caption, Color captionBackgroundColor)
        {
            Image img = Image.FromFile(path);
            _AddImage(img, caption, captionBackgroundColor);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="img">The img.</param>
        public void AddImage(Image img)
        {
            _AddImage(img, "", this.captionBackgrounColor);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="caption">The caption.</param>
        public void AddImage(Image img, string caption)
        {
            _AddImage(img, caption, this.captionBackgrounColor);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="captionBackgroundColor">Color of the caption background.</param>
        public void AddImage(Image img, string caption, Color captionBackgroundColor)
        {
            _AddImage(img, caption, captionBackgroundColor);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="captionBackgroundColor">Color of the caption background.</param>
        protected void _AddImage(Image img, string caption, Color captionBackgroundColor)
        {
            _imageList.Add(img);
            _captionList.Add(caption);
            _captionBgColor.Add(captionBackgroundColor);
        }

        /// <summary>
        /// The caption height
        /// </summary>
        private int captionHeight = 50;
        /// <summary>
        /// Gets or sets the height of the caption.
        /// </summary>
        /// <value>The height of the caption.</value>
        public int CaptionHeight
        {
            get { return captionHeight; }
            set
            {
                captionHeight = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the caption text left.
        /// </summary>
        /// <value>The caption text left.</value>
        public int CaptionTextLeft
        {
            set
            {
                _captionPosX = value;
                _captionTextLeft = value;
                Invalidate();
            }
            get
            {
                return _captionTextLeft;
            }
        }

        /// <summary>
        /// The caption backgroun color
        /// </summary>
        private Color captionBackgrounColor = Color.Black;
        /// <summary>
        /// Gets or sets the color of the caption backgroun.
        /// </summary>
        /// <value>The color of the caption backgroun.</value>
        public Color CaptionBackgrounColor
        {
            get { return captionBackgrounColor; }
            set
            {
                captionBackgrounColor = value;
                Invalidate();
            }

        }

        /// <summary>
        /// The caption opacity
        /// </summary>
        private int captionOpacity = 100;
        /// <summary>
        /// Gets or sets the caption opacity.
        /// </summary>
        /// <value>The caption opacity.</value>
        public int CaptionOpacity
        {
            get { return captionOpacity; }
            set
            {
                captionOpacity = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The caption animation speed
        /// </summary>
        private int captionAnimationSpeed = 50;
        /// <summary>
        /// Gets or sets the caption animation speed.
        /// </summary>
        /// <value>The caption animation speed.</value>
        public int CaptionAnimationSpeed
        {
            get { return captionAnimationSpeed; }
            set
            {
                captionAnimationSpeed = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The animation
        /// </summary>
        private bool animation = true;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ZeroitImageSlider"/> is animation.
        /// </summary>
        /// <value><c>true</c> if animation; otherwise, <c>false</c>.</value>
        public bool Animation
        {
            get
            {
                return animation;
            }
            set
            {
                animation = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the left button.
        /// </summary>
        /// <value>The left button.</value>
        public xButton LeftButton
        {
            get
            {
                return leftButton;
            }
        }

        /// <summary>
        /// Gets the right button.
        /// </summary>
        /// <value>The right button.</value>
        public xButton RightButton
        {
            get
            {
                return rightButton;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            try
            {
                Color captionBgColor = Color.FromArgb(captionOpacity, _captionBgColor[_pageIndex].R, _captionBgColor[_pageIndex].G, _captionBgColor[_pageIndex].B);
                g.DrawImage(_imageList[_pageIndex], new Rectangle(0, 0, this.Width, this.Height));
                g.FillRectangle(new SolidBrush(captionBgColor), new Rectangle(0, this.Height - this.captionHeight, this.Width, this.Height));

                string caption = _captionList[_pageIndex];

                SizeF fontSize = g.MeasureString(_captionList[_pageIndex], this.Font);
                g.DrawString(_captionList[_pageIndex], this.Font, new SolidBrush(this.ForeColor), _captionPosX, this.Height - (int)(this.captionHeight - (fontSize.Height / 2)));
            }
            catch { }
        }

        /// <summary>
        /// Class xButton.
        /// </summary>
        /// <seealso cref="System.Windows.Forms.Button" />
        public class xButton : Button
        {
            //ZeroitImageSlider slider = new ZeroitImageSlider();
            /// <summary>
            /// Initializes a new instance of the <see cref="xButton"/> class.
            /// </summary>
            public xButton()
            {
                BackColor = Color.Black;
                this.Height = 50;
                this.Width = 50;
                ForeColor = Color.White;
                                
            }

            /// <summary>
            /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
            /// </summary>
            /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
            protected override void OnPaint(PaintEventArgs pevent)
            {
                Graphics g = pevent.Graphics;
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, this.Width, Height);

                g.FillRectangle(new SolidBrush(this.BackColor), area);
                SizeF fontSize = g.MeasureString(this.Text, this.Font);
                g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), (this.Width - fontSize.Width) / 2, (this.Height - fontSize.Height) / 2);
            }
        }
    }


    #endregion
    
}
