using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatStatusBar : Control
    {
        private int W;
        private int H;

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Dock = DockStyle.Bottom;
        }

        protected override void OnTextChanged( EventArgs e )
        {
            base.OnTextChanged( e );
            Invalidate();
        }

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Color.FromArgb( 24, 22, 43 );

        [Category( "Colors" )]
        public Color TextColor { get; set; } = Color.White;

        [Category( "Colors" )]
        public Color RectColor { get; set; } = Helpers.FlatColor;

        public bool ShowTimeDate { get; set; } = false;

        public string GetTimeDate()
        {
            return DateTime.Now.Date + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
        }

        public FlatStatusBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            Font = new Font( "Tahoma", 8 );
            ForeColor = Color.White;
            Size = new Size( Width, 20 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width;
            H = Height;

            var Base = new Rectangle( 0, 0, W, H );

            var _with21 = G;
            _with21.SmoothingMode = SmoothingMode.HighQuality;
            _with21.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with21.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with21.Clear( BaseColor );

            //-- Base
            _with21.FillRectangle( new SolidBrush( BaseColor ), Base );

            //-- Text
            _with21.DrawString( Text, Font, Brushes.White, new Rectangle( 10, 4, W, H ), Helpers.NearSF );

            //-- Rectangle
            _with21.FillRectangle( new SolidBrush( RectColor ), new Rectangle( 4, 4, 4, 14 ) );

            //-- TimeDate
            if ( ShowTimeDate )
                _with21.DrawString( GetTimeDate(), Font, new SolidBrush( TextColor ), new Rectangle( -4, 2, W, H ),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center
                    } );

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors( this );

            RectColor = colors.Flat;
        }
    }
}