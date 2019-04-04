// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-20-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-20-2018
// ***********************************************************************
// <copyright file="Control.cs" company="Zeroit Dev Technologies">
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

    #region Scalable PictureBox

    #region Control
    /// <summary>
    /// Front end control of the scrollable, zoomable and scalable picture box.
    /// It is a facade and mediator of ZeroitScalablePicBoxImp control and PictureTracker control.
    /// An application should use this control for showing picture
    /// instead of using ZeroitScalablePicBox control directly.
    /// </summary>
    public partial class ZeroitScalablePicBox : UserControl
    {

        /// <summary>
        /// indicating mouse dragging mode of picture tracker control
        /// </summary>
        private bool isDraggingPictureTracker = false;

        /// <summary>
        /// last mouse position of mouse dragging
        /// </summary>
        Point lastMousePos;

        /// <summary>
        /// the new area where the picture tracker control to be dragged
        /// </summary>
        Rectangle draggingRectangle;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZeroitScalablePicBox()
        {
            InitializeComponent();

            this.pictureTracker.BringToFront();

            // enable double buffering
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);

            // register event handler for events from ZeroitScalablePicBox
            this.ZeroitScalablePicBoxImp.PictureBoxPaintedEvent += new ZeroitScalablePicBoxImp.PictureBoxPaintedEventHandler(this.pictureTracker.OnPictureBoxPainted);
            this.ZeroitScalablePicBoxImp.ZoomRateChangedEvent += new ZeroitScalablePicBoxImp.ZoomRateChangedEventHandler(this.ZeroitScalablePicBox_ZoomRateChanged);

            // register event handler for events from PictureTracker
            this.pictureTracker.ScrollPictureEvent += new PictureTracker.ScrollPictureEventHandler(this.ZeroitScalablePicBoxImp.OnScrollPictureEvent);
            this.pictureTracker.PictureTrackerClosed += new PictureTracker.PictureTrackerClosedHandler(this.pictureTracker_PictureTrackerClosed);
        }

        /// <summary>
        /// Set a picture to show in ZeroitScalablePicBox control 
        /// </summary>
        public Image Picture
        {
            set
            {
                this.ZeroitScalablePicBoxImp.Picture = value;
                this.pictureTracker.Picture = value;
            }
        }

        /// <summary>
        /// Get picture box control
        /// </summary>
        [Bindable(false)]
        public System.Windows.Forms.PictureBox PictureBox
        {
            get { return this.ZeroitScalablePicBoxImp.PictureBox; }
        }

        /// <summary>
        /// Notify current scale percentage to PictureTracker control if current picture is
        /// zoomed in, or hide PictureTracker control if current picture is shown fully.
        /// </summary>
        /// <param name="zoomRate">zoom rate of picture</param>
        /// <param name="isWholePictureShown">true if the whole picture is shown</param>
        private void ZeroitScalablePicBox_ZoomRateChanged(int zoomRate, bool isWholePictureShown)
        {
            if (isWholePictureShown)
            {
                this.pictureTracker.Visible = false;
                this.pictureTracker.Enabled = false;
            }
            else
            {
                this.pictureTracker.Visible = true;
                this.pictureTracker.Enabled = true;
                this.pictureTracker.ZoomRate = zoomRate;
            }
        }

        /// <summary>
        /// Inform ZeroitScalablePicBox control to show picture fully.
        /// </summary>
        private void pictureTracker_PictureTrackerClosed()
        {
            this.ZeroitScalablePicBoxImp.ImageSizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// Draw a reversible rectangle
        /// </summary>
        /// <param name="rect">rectangle to be drawn</param>
        private void DrawReversibleRect(Rectangle rect)
        {
            // Convert the location of rectangle to screen coordinates.
            rect.Location = PointToScreen(rect.Location);

            // Draw the reversible frame.
            ControlPaint.DrawReversibleFrame(rect, Color.Navy, FrameStyle.Thick);
        }

        /// <summary>
        /// begin to drag picture tracker control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureTracker_MouseDown(object sender, MouseEventArgs e)
        {
            isDraggingPictureTracker = true;    // Make a note that we are dragging picture tracker control

            // Store the last mouse poit for this rubber-band rectangle.
            lastMousePos.X = e.X;
            lastMousePos.Y = e.Y;

            // draw initial dragging rectangle
            draggingRectangle = this.pictureTracker.Bounds;
            DrawReversibleRect(draggingRectangle);
        }

        /// <summary>
        /// dragging picture tracker control in mouse dragging mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureTracker_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingPictureTracker)
            {
                // caculating next candidate dragging rectangle
                Point newPos = new Point(draggingRectangle.Location.X + e.X - lastMousePos.X,
                                         draggingRectangle.Location.Y + e.Y - lastMousePos.Y);
                Rectangle newPictureTrackerArea = draggingRectangle;
                newPictureTrackerArea.Location = newPos;

                // saving current mouse position to be used for next dragging
                this.lastMousePos = new Point(e.X, e.Y);

                // dragging picture tracker only when the candidate dragging rectangle
                // is within this ZeroitScalablePicBox control
                if (this.ClientRectangle.Contains(newPictureTrackerArea))
                {
                    // removing previous rubber-band frame
                    DrawReversibleRect(draggingRectangle);

                    // updating dragging rectangle
                    draggingRectangle = newPictureTrackerArea;

                    // drawing new rubber-band frame
                    DrawReversibleRect(draggingRectangle);
                }
            }
        }

        /// <summary>
        /// end dragging picture tracker control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureTracker_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingPictureTracker)
            {
                isDraggingPictureTracker = false;

                // erase dragging rectangle
                DrawReversibleRect(draggingRectangle);

                // move the picture tracker control to the new position
                this.pictureTracker.Location = draggingRectangle.Location;
            }
        }

        /// <summary>
        /// relocate picture box at bottom right corner when the control size changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            int x = this.ClientSize.Width - this.pictureTracker.Width - 20;
            int y = this.ClientSize.Height - this.pictureTracker.Height - 20;
            this.pictureTracker.Location = new Point(x, y);
        }
    }
    #endregion

    #region Designer Generated Code

    public partial class ZeroitScalablePicBox
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
            this.pictureTracker = new PictureTracker();
            this.ZeroitScalablePicBoxImp = new ZeroitScalablePicBoxImp();
            this.SuspendLayout();
            // 
            // pictureTracker
            // 
            this.pictureTracker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureTracker.BackColor = System.Drawing.Color.Lavender;
            this.pictureTracker.Location = new System.Drawing.Point(233, 131);
            this.pictureTracker.Name = "pictureTracker";
            this.pictureTracker.Size = new System.Drawing.Size(137, 102);
            this.pictureTracker.TabIndex = 1;
            this.pictureTracker.ZoomRate = 0;
            this.pictureTracker.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureTracker_MouseDown);
            this.pictureTracker.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureTracker_MouseMove);
            this.pictureTracker.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureTracker_MouseUp);
            // 
            // ZeroitScalablePicBoxImp
            // 
            this.ZeroitScalablePicBoxImp.BackColor = System.Drawing.Color.Gray;
            this.ZeroitScalablePicBoxImp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZeroitScalablePicBoxImp.Location = new System.Drawing.Point(0, 0);
            this.ZeroitScalablePicBoxImp.Name = "ZeroitScalablePicBoxImp";
            this.ZeroitScalablePicBoxImp.Picture = null;
            this.ZeroitScalablePicBoxImp.Size = new System.Drawing.Size(391, 255);
            this.ZeroitScalablePicBoxImp.TabIndex = 0;
            // 
            // ZeroitScalablePicBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureTracker);
            this.Controls.Add(this.ZeroitScalablePicBoxImp);
            this.Name = "ZeroitScalablePicBox";
            this.Size = new System.Drawing.Size(391, 255);
            this.ResumeLayout(false);

        }

        #endregion

        private ZeroitScalablePicBoxImp ZeroitScalablePicBoxImp;
        private PictureTracker pictureTracker;
    }

    #endregion

    #endregion

}
