﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatClose : Control
    {
        private MouseState State = MouseState.None;
        private int x;

        protected override void OnMouseEnter( EventArgs e )
        {
            base.OnMouseEnter( e );
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );
            State = MouseState.Down;
            Invalidate();
        }

        protected override void OnMouseLeave( EventArgs e )
        {
            base.OnMouseLeave( e );
            State = MouseState.None;
            Invalidate();
        }

        protected override void OnMouseUp( MouseEventArgs e )
        {
            base.OnMouseUp( e );
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );
            x = e.X;
            Invalidate();
        }

        protected override void OnClick( EventArgs e )
        {
            base.OnClick( e );
            Environment.Exit( 0 );
        }

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Size = new Size( 18, 18 );
        }

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Color.FromArgb( 24, 22, 43 );

        [Category( "Colors" )]
        public Color TextColor { get; set; } = Color.FromArgb( 233, 95, 98 );

        public FlatClose()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            BackColor = Color.White;
            Size = new Size( 18, 18 );
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Font = new Font( "Marlett", 14 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );

            var Base = new Rectangle( 0, 0, Width, Height );

            var _with3 = G;
            _with3.SmoothingMode = SmoothingMode.HighQuality;
            _with3.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with3.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with3.Clear( BackColor );

            //-- Base
            _with3.FillRectangle( new SolidBrush( BaseColor ), Base );

            //-- X
            _with3.DrawString( "i", Font, new SolidBrush( TextColor ), new Rectangle( 0, 0, Width, Height ),
                Helpers.CenterSF );

            //--Hover / down
            switch ( State )
            {
                case MouseState.Over:
                    _with3.FillRectangle( new SolidBrush( Color.FromArgb( 30, Color.White ) ), Base );
                    break;

                case MouseState.Down:
                    _with3.FillRectangle( new SolidBrush( Color.FromArgb( 30, Color.Black ) ), Base );
                    break;
            }

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }
    }
}