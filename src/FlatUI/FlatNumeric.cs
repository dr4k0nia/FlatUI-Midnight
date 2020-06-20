﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatNumeric : Control
    {
        private int W;
        private int H;
        private MouseState State = MouseState.None;
        private int x;
        private int y;
        private long _Value;
        private long _Min;
        private long _Max;
        private bool Bool;

        public long Value
        {
            get => _Value;
            set
            {
                if ( ( value <= _Max ) & ( value >= _Min ) )
                    _Value = value;
                Invalidate();
            }
        }

        public long Maximum
        {
            get => _Max;
            set
            {
                if ( value > _Min )
                    _Max = value;
                if ( _Value > _Max )
                    _Value = _Max;
                Invalidate();
            }
        }

        public long Minimum
        {
            get => _Min;
            set
            {
                if ( value < _Max )
                    _Min = value;
                if ( _Value < _Min )
                    _Value = Minimum;
                Invalidate();
            }
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );
            x = e.Location.X;
            y = e.Location.Y;
            Invalidate();
            if ( e.X < Width - 23 )
                Cursor = Cursors.IBeam;
            else
                Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );
            if ( x > Width - 21 && x < Width - 3 )
            {
                if ( y < 15 )
                {
                    if ( Value + 1 <= _Max )
                        _Value += 1;
                }
                else
                {
                    if ( Value - 1 >= _Min )
                        _Value -= 1;
                }
            }
            else
            {
                Bool = !Bool;
                Focus();
            }

            Invalidate();
        }

        protected override void OnKeyPress( KeyPressEventArgs e )
        {
            base.OnKeyPress( e );
            try
            {
                if ( Bool )
                    _Value = Convert.ToInt64( _Value.ToString() + e.KeyChar.ToString() );
                if ( _Value > _Max )
                    _Value = _Max;
                Invalidate();
            }
            catch
            {
            }
        }

        protected override void OnKeyDown( KeyEventArgs e )
        {
            base.OnKeyDown( e );
            if ( e.KeyCode == Keys.Back ) Value = 0;
        }

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Height = 30;
        }

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Color.FromArgb( 24, 22, 43 );

        [Category( "Colors" )]
        public Color ButtonColor { get; set; } = Helpers.FlatColor;

        public FlatNumeric()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true );
            DoubleBuffered = true;
            Font = new Font( "Tahoma", 10 );
            BackColor = Color.FromArgb( 35, 30, 59 );
            ForeColor = Color.White;
            _Min = 0;
            _Max = 9999999;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width;
            H = Height;

            var Base = new Rectangle( 0, 0, W, H );

            var _with18 = G;
            _with18.SmoothingMode = SmoothingMode.HighQuality;
            _with18.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with18.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with18.Clear( BackColor );

            //-- Base
            _with18.FillRectangle( new SolidBrush( BaseColor ), Base );
            _with18.FillRectangle( new SolidBrush( ButtonColor ), new Rectangle( Width - 24, 0, 24, H - 1 ) );

            //-- Add
            _with18.DrawString( "5", new Font( "Marlett", 11 ), Brushes.White, new Point( Width - 12, 8 ),
                Helpers.CenterSF );
            //-- Subtract
            _with18.DrawString( "6", new Font( "Marlett", 11 ), Brushes.White, new Point( Width - 12, 22 ),
                Helpers.CenterSF );

            //-- Text
            _with18.DrawString( Value.ToString(), Font, Brushes.White, new Rectangle( 5, 1, W, H ),
                new StringFormat {LineAlignment = StringAlignment.Center} );

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors( this );

            ButtonColor = colors.Flat;
        }
    }
}