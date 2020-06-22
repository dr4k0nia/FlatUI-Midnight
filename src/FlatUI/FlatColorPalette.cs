using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatColorPalette : Control
    {
        private int W;
        private int H;

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Width = 180;
            Height = 80;
        }

        [Category( "Colors" )]
        public Color Red { get; set; } = Color.FromArgb( 220, 85, 96 );

        [Category( "Colors" )]
        public Color Cyan { get; set; } = Color.FromArgb( 10, 154, 157 );

        [Category( "Colors" )]
        public Color Blue { get; set; } = Color.FromArgb( 0, 128, 255 );

        [Category( "Colors" )]
        public Color LimeGreen { get; set; } = Color.FromArgb( 22, 96, 253 );

        [Category( "Colors" )]
        public Color Orange { get; set; } = Color.FromArgb( 253, 181, 63 );

        [Category( "Colors" )]
        public Color Purple { get; set; } = Color.FromArgb( 155, 88, 181 );

        [Category( "Colors" )]
        public Color Black { get; set; } = Color.FromArgb( 24, 22, 43 );

        [Category( "Colors" )]
        public Color Gray { get; set; } = Color.FromArgb( 63, 70, 73 );

        [Category( "Colors" )]
        public Color White { get; set; } = Helpers.FlatWhite;

        public FlatColorPalette()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            BackColor = Color.FromArgb( 35, 30, 59 );
            Size = new Size( 160, 80 );
            Font = new Font( "Tahoma", 12 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var _with6 = G;
            _with6.SmoothingMode = SmoothingMode.HighQuality;
            _with6.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with6.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with6.Clear( BackColor );

            //-- Colors 
            _with6.FillRectangle( new SolidBrush( Red ), new Rectangle( 0, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( Cyan ), new Rectangle( 20, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( Blue ), new Rectangle( 40, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( LimeGreen ), new Rectangle( 60, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( Orange ), new Rectangle( 80, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( Purple ), new Rectangle( 100, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( Black ), new Rectangle( 120, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( Gray ), new Rectangle( 140, 0, 20, 40 ) );
            _with6.FillRectangle( new SolidBrush( White ), new Rectangle( 160, 0, 20, 40 ) );

            //-- Text
            _with6.DrawString( "Color Palette", Font, new SolidBrush( White ), new Rectangle( 0, 22, W, H ),
                Helpers.CenterSF );

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }
    }
}