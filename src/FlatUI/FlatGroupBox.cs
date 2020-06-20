using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatGroupBox : ContainerControl
    {
        private int W;
        private int H;

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Color.FromArgb( 37, 35, 63 );

        public bool ShowText { get; set; } = true;

        private Color _TextColor = Helpers.FlatColor;

        public FlatGroupBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true );
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size( 240, 180 );
            Font = new Font( "Tahoma", 10 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var GP = new GraphicsPath();
            var GP2 = new GraphicsPath();
            var GP3 = new GraphicsPath();
            var Base = new Rectangle( 8, 8, W - 16, H - 16 );

            var _with7 = G;
            _with7.SmoothingMode = SmoothingMode.HighQuality;
            _with7.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with7.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with7.Clear( BackColor );

            //-- Base
            GP = Helpers.RoundRec( Base, 8 );
            _with7.FillPath( new SolidBrush( BaseColor ), GP );

            //-- Arrows
            GP2 = Helpers.DrawArrow( 28, 2, false );
            _with7.FillPath( new SolidBrush( BaseColor ), GP2 );
            GP3 = Helpers.DrawArrow( 28, 8, true );
            _with7.FillPath( new SolidBrush( Color.FromArgb( 35, 30, 59 ) ), GP3 );

            //-- if ShowText
            if ( ShowText )
                _with7.DrawString( Text, Font, new SolidBrush( _TextColor ), new Rectangle( 16, 16, W, H ),
                    Helpers.NearSF );

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors( this );

            _TextColor = colors.Flat;
        }
    }
}