using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatButton : Control
    {
        private int W;
        private int H;
        private MouseState State = MouseState.None;

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Helpers.FlatColor;

        [Category( "Colors" )]
        public Color TextColor { get; set; } = Helpers.FlatWhite;

        [Category( "Options" )]
        public bool Rounded { get; set; } = false;

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

        public FlatButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true );
            DoubleBuffered = true;
            Size = new Size( 106, 32 );
            BackColor = Color.Transparent;
            Font = new Font( "Tahoma", 12 );
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var GP = new GraphicsPath();
            var Base = new Rectangle( 0, 0, W, H );

            var _with8 = G;
            _with8.SmoothingMode = SmoothingMode.HighQuality;
            _with8.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with8.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with8.Clear( BackColor );

            switch ( State )
            {
                case MouseState.None:
                    if ( Rounded )
                    {
                        //-- Base
                        GP = Helpers.RoundRec( Base, 6 );
                        _with8.FillPath( new SolidBrush( BaseColor ), GP );

                        //-- Text
                        _with8.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }
                    else
                    {
                        //-- Base
                        _with8.FillRectangle( new SolidBrush( BaseColor ), Base );

                        //-- Text
                        _with8.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }

                    break;
                case MouseState.Over:
                    if ( Rounded )
                    {
                        //-- Base
                        GP = Helpers.RoundRec( Base, 6 );
                        _with8.FillPath( new SolidBrush( BaseColor ), GP );
                        _with8.FillPath( new SolidBrush( Color.FromArgb( 20, Color.White ) ), GP );

                        //-- Text
                        _with8.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }
                    else
                    {
                        //-- Base
                        _with8.FillRectangle( new SolidBrush( BaseColor ), Base );
                        _with8.FillRectangle( new SolidBrush( Color.FromArgb( 20, Color.White ) ), Base );

                        //-- Text
                        _with8.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }

                    break;
                case MouseState.Down:
                    if ( Rounded )
                    {
                        //-- Base
                        GP = Helpers.RoundRec( Base, 6 );
                        _with8.FillPath( new SolidBrush( BaseColor ), GP );
                        _with8.FillPath( new SolidBrush( Color.FromArgb( 20, Color.Black ) ), GP );

                        //-- Text
                        _with8.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }
                    else
                    {
                        //-- Base
                        _with8.FillRectangle( new SolidBrush( BaseColor ), Base );
                        _with8.FillRectangle( new SolidBrush( Color.FromArgb( 20, Color.Black ) ), Base );

                        //-- Text
                        _with8.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }

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

            BaseColor = colors.Flat;
        }
    }
}