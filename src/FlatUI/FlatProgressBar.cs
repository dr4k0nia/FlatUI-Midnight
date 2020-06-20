using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatProgressBar : Control
    {
        private int W;
        private int H;
        private int _Value = 0;
        private int _Maximum = 100;

        [Category( "Control" )]
        public int Maximum
        {
            get => _Maximum;
            set
            {
                if ( value < _Value )
                    _Value = value;
                _Maximum = value;
                Invalidate();
            }
        }

        [Category( "Control" )]
        public int Value
        {
            get => _Value;
            /*
                switch (_Value)
                {
                    case 0:
                        return 0;
                        Invalidate();
                        break;
                    default:
                        return _Value;
                        Invalidate();
                        break;
                }
                */
            set
            {
                if ( value > _Maximum )
                {
                    value = _Maximum;
                    Invalidate();
                }

                _Value = value;
                Invalidate();
            }
        }

        public bool Pattern { get; set; } = true;

        public bool ShowBalloon { get; set; } = true;

        public bool PercentSign { get; set; } = false;

        [Category( "Colors" )]
        public Color ProgressColor { get; set; } = Helpers.FlatColor;

        [Category( "Colors" )]
        public Color DarkerProgress { get; set; } = Color.FromArgb( 23, 148, 92 );

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Height = 42;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Height = 42;
        }

        public void Increment( int Amount )
        {
            Value += Amount;
        }

        private readonly Color _BaseColor = Color.FromArgb( 24, 22, 43 );

        public FlatProgressBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            BackColor = Color.FromArgb( 35, 30, 59 );
            Height = 42;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var Base = new Rectangle( 0, 24, W, H );
            var GP = new GraphicsPath();
            var GP2 = new GraphicsPath();
            var GP3 = new GraphicsPath();

            var _with15 = G;
            _with15.SmoothingMode = SmoothingMode.HighQuality;
            _with15.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with15.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with15.Clear( BackColor );

            //-- Progress Value
            //int iValue = Convert.ToInt32(((float)_Value) / ((float)(_Maximum * Width)));
            var percent = (float) _Value / (float) _Maximum;
            var iValue = (int) ( percent * (float) Width );

            switch ( Value )
            {
                case 0:
                    //-- Base
                    _with15.FillRectangle( new SolidBrush( _BaseColor ), Base );
                    //--Progress
                    _with15.FillRectangle( new SolidBrush( ProgressColor ),
                        new Rectangle( 0, 24, iValue - 1, H - 1 ) );
                    break;
                case 100:
                    //-- Base
                    _with15.FillRectangle( new SolidBrush( _BaseColor ), Base );
                    //--Progress
                    _with15.FillRectangle( new SolidBrush( ProgressColor ),
                        new Rectangle( 0, 24, iValue - 1, H - 1 ) );
                    break;
                default:
                    //-- Base
                    _with15.FillRectangle( new SolidBrush( _BaseColor ), Base );

                    //--Progress
                    GP.AddRectangle( new Rectangle( 0, 24, iValue - 1, H - 1 ) );
                    _with15.FillPath( new SolidBrush( ProgressColor ), GP );

                    if ( Pattern )
                    {
                        //-- Hatch Brush
                        var HB = new HatchBrush( HatchStyle.Plaid, DarkerProgress, ProgressColor );
                        _with15.FillRectangle( HB, new Rectangle( 0, 24, iValue - 1, H - 1 ) );
                    }

                    if ( ShowBalloon )
                    {
                        //-- Balloon
                        var Balloon = new Rectangle( iValue - 18, 0, 34, 16 );
                        GP2 = Helpers.RoundRec( Balloon, 4 );
                        _with15.FillPath( new SolidBrush( _BaseColor ), GP2 );

                        //-- Arrow
                        GP3 = Helpers.DrawArrow( iValue - 9, 16, true );
                        _with15.FillPath( new SolidBrush( _BaseColor ), GP3 );

                        //-- Value > You can add "%" > value & "%"
                        var text = PercentSign ? Value.ToString() + "%" : Value.ToString();
                        var wOffset = PercentSign ? iValue - 15 : iValue - 11;
                        _with15.DrawString( text, new Font( "Tahoma", 10 ), new SolidBrush( ProgressColor ),
                            new Rectangle( wOffset, -2, W, H ), Helpers.NearSF );
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

            ProgressColor = colors.Flat;
        }
    }
}