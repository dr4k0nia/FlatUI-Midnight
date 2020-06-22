using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatStickyButton : Control
    {
        private int W;
        private int H;
        private MouseState State = MouseState.None;

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

        private bool[] GetConnectedSides()
        {
            var Bool = new bool[4] {false, false, false, false};

            foreach ( Control B in Parent.Controls )
                if ( B is FlatStickyButton )
                {
                    if ( ReferenceEquals( B, this ) || !Rect.IntersectsWith( Rect ) )
                        continue;
                    var A = Math.Atan2( Left - B.Left, Top - B.Top ) * 2 / Math.PI;
                    if ( A / 1 == A )
                        Bool[(int) A + 1] = true;
                }

            return Bool;
        }

        private Rectangle Rect => new Rectangle( Left, Top, Width, Height );

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Helpers.FlatColor;

        [Category( "Colors" )]
        public Color TextColor { get; set; } = Helpers.FlatWhite;

        [Category( "Options" )]
        public bool Rounded { get; set; } = false;

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            //Height = 32
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //Size = New Size(112, 32)
        }

        public FlatStickyButton()
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
            W = Width;
            H = Height;

            var GP = new GraphicsPath();

            var GCS = GetConnectedSides();
            // dynamic RoundedBase = Helpers.RoundRect(0, 0, W, H, ???, !(GCS(2) | GCS(1)), !(GCS(1) | GCS(0)), !(GCS(3) | GCS(0)), !(GCS(3) | GCS(2)));
            var RoundedBase = Helpers.RoundRect( 0, 0, W, H, 0.3, !( GCS[2] || GCS[1] ), !( GCS[1] || GCS[0] ),
                !( GCS[3] || GCS[0] ), !( GCS[3] || GCS[2] ) );
            var Base = new Rectangle( 0, 0, W, H );

            var _with17 = G;
            _with17.SmoothingMode = SmoothingMode.HighQuality;
            _with17.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with17.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with17.Clear( BackColor );

            switch ( State )
            {
                case MouseState.None:
                    if ( Rounded )
                    {
                        //-- Base
                        GP = RoundedBase;
                        _with17.FillPath( new SolidBrush( BaseColor ), GP );

                        //-- Text
                        _with17.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }
                    else
                    {
                        //-- Base
                        _with17.FillRectangle( new SolidBrush( BaseColor ), Base );

                        //-- Text
                        _with17.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }

                    break;
                case MouseState.Over:
                    if ( Rounded )
                    {
                        //-- Base
                        GP = RoundedBase;
                        _with17.FillPath( new SolidBrush( BaseColor ), GP );
                        _with17.FillPath( new SolidBrush( Color.FromArgb( 20, Color.White ) ), GP );

                        //-- Text
                        _with17.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }
                    else
                    {
                        //-- Base
                        _with17.FillRectangle( new SolidBrush( BaseColor ), Base );
                        _with17.FillRectangle( new SolidBrush( Color.FromArgb( 20, Color.White ) ), Base );

                        //-- Text
                        _with17.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }

                    break;
                case MouseState.Down:
                    if ( Rounded )
                    {
                        //-- Base
                        GP = RoundedBase;
                        _with17.FillPath( new SolidBrush( BaseColor ), GP );
                        _with17.FillPath( new SolidBrush( Color.FromArgb( 20, Color.Black ) ), GP );

                        //-- Text
                        _with17.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
                    }
                    else
                    {
                        //-- Base
                        _with17.FillRectangle( new SolidBrush( BaseColor ), Base );
                        _with17.FillRectangle( new SolidBrush( Color.FromArgb( 20, Color.Black ) ), Base );

                        //-- Text
                        _with17.DrawString( Text, Font, new SolidBrush( TextColor ), Base, Helpers.CenterSF );
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