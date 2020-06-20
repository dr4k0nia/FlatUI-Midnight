﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent( "CheckedChanged" )]
    public class FlatCheckBox : Control
    {
        private int W;
        private int H;
        private MouseState State = MouseState.None;
        private bool _Checked;

        protected override void OnTextChanged( EventArgs e )
        {
            base.OnTextChanged( e );
            Invalidate();
        }

        public bool Checked
        {
            get => _Checked;
            set
            {
                _Checked = value;
                Invalidate();
            }
        }

        public event CheckedChangedEventHandler CheckedChanged;

        public delegate void CheckedChangedEventHandler( object sender );

        protected override void OnClick( EventArgs e )
        {
            _Checked = !_Checked;
            CheckedChanged?.Invoke( this );

            base.OnClick( e );
        }

        [Flags()]
        public enum _Options
        {
            Style1,
            Style2
        }

        [Category( "Options" )]
        public _Options Options { get; set; }

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Height = 22;
        }

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Color.FromArgb( 24, 22, 43 );

        [Category( "Colors" )]
        public Color BorderColor { get; set; } = Helpers.FlatColor;

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );
            State = MouseState.Down;
            Invalidate();
        }

        protected override void OnMouseUp( MouseEventArgs e )
        {
            base.OnMouseUp( e );
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseEnter( EventArgs e )
        {
            base.OnMouseEnter( e );
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseLeave( EventArgs e )
        {
            base.OnMouseLeave( e );
            State = MouseState.None;
            Invalidate();
        }

        private readonly Color _TextColor = Helpers.FlatWhite;

        public FlatCheckBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            BackColor = Color.FromArgb( 35, 30, 59 );
            Cursor = Cursors.Hand;
            Font = new Font( "Tahoma", 10 );
            Size = new Size( 112, 22 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var Base = new Rectangle( 0, 2, Height - 5, Height - 5 );

            var _with11 = G;
            _with11.SmoothingMode = SmoothingMode.HighQuality;
            _with11.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with11.Clear( BackColor );
            switch ( Options )
            {
                case _Options.Style1:
                    //-- Style 1
                    //-- Base
                    _with11.FillRectangle( new SolidBrush( BaseColor ), Base );

                    switch ( State )
                    {
                        case MouseState.Over:
                            //-- Base
                            _with11.DrawRectangle( new Pen( BorderColor ), Base );
                            break;

                        case MouseState.Down:
                            //-- Base
                            _with11.DrawRectangle( new Pen( BorderColor ), Base );
                            break;
                    }

                    //-- If Checked
                    if ( Checked )
                    {
                        _with11.DrawString( "ü", new Font( "Wingdings", 18 ), new SolidBrush( BorderColor ),
                            new Rectangle( 5, 7, H - 9, H - 9 ), Helpers.CenterSF );
                    }

                    //-- If Enabled
                    if ( Enabled == false )
                    {
                        _with11.FillRectangle( new SolidBrush( Color.FromArgb( 54, 58, 61 ) ), Base );
                        _with11.DrawString( Text, Font, new SolidBrush( Color.FromArgb( 140, 142, 143 ) ),
                            new Rectangle( 20, 2, W, H ), Helpers.NearSF );
                    }

                    //-- Text
                    _with11.DrawString( Text, Font, new SolidBrush( _TextColor ), new Rectangle( 20, 2, W, H ),
                        Helpers.NearSF );
                    break;

                case _Options.Style2:
                    //-- Style 2
                    //-- Base
                    _with11.FillRectangle( new SolidBrush( BaseColor ), Base );

                    switch ( State )
                    {
                        case MouseState.Over:
                            //-- Base
                            _with11.DrawRectangle( new Pen( BorderColor ), Base );
                            _with11.FillRectangle( new SolidBrush( Color.FromArgb( 118, 213, 170 ) ), Base );
                            break;

                        case MouseState.Down:
                            //-- Base
                            _with11.DrawRectangle( new Pen( BorderColor ), Base );
                            _with11.FillRectangle( new SolidBrush( Color.FromArgb( 118, 213, 170 ) ), Base );
                            break;
                    }

                    //-- If Checked
                    if ( Checked )
                    {
                        _with11.DrawString( "ü", new Font( "Wingdings", 18 ), new SolidBrush( BorderColor ),
                            new Rectangle( 5, 7, H - 9, H - 9 ), Helpers.CenterSF );
                    }

                    //-- If Enabled
                    if ( Enabled == false )
                    {
                        _with11.FillRectangle( new SolidBrush( Color.FromArgb( 54, 58, 61 ) ), Base );
                        _with11.DrawString( Text, Font, new SolidBrush( Color.FromArgb( 48, 119, 91 ) ),
                            new Rectangle( 20, 2, W, H ), Helpers.NearSF );
                    }

                    //-- Text
                    _with11.DrawString( Text, Font, new SolidBrush( _TextColor ), new Rectangle( 20, 2, W, H ),
                        Helpers.NearSF );
                    break;
            }

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors( this );

            BorderColor = colors.Flat;
        }
    }
}