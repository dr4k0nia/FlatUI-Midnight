using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent( "Scroll" )]
    public class FlatTrackBar : Control
    {
        private int W;
        private int H;
        private int Val;
        private bool Bool;
        private Rectangle Track;
        private Rectangle Knob;

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );
            if ( e.Button == MouseButtons.Left )
            {
                Val = Convert.ToInt32( (float) ( _Value - _Minimum ) / (float) ( _Maximum - _Minimum ) *
                                       (float) ( Width - 11 ) );
                Track = new Rectangle( Val, 0, 10, 20 );

                Bool = Track.Contains( e.Location );
            }
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );
            if ( Bool && e.X > -1 && e.X < Width + 1 )
                Value = _Minimum + Convert.ToInt32( (float) ( _Maximum - _Minimum ) * ( (float) e.X / (float) Width ) );
        }

        protected override void OnMouseUp( MouseEventArgs e )
        {
            base.OnMouseUp( e );
            Bool = false;
        }

        [Flags()]
        public enum _Style
        {
            Slider,
            Knob
        }

        public _Style Style { get; set; }

        [Category( "Colors" )]
        public Color TrackColor { get; set; } = Color.FromArgb( 66, 127, 255 );

        //[Category("Colors")]
        //public Color HatchColor
        //{
        //	get { return _HatchColor; }
        //	set { _HatchColor = value; }
        //}

        public event ScrollEventHandler Scroll;

        public delegate void ScrollEventHandler( object sender );

        private int _Minimum;

        public int Minimum
        {
            get
            {
                var functionReturnValue = 0;
                return functionReturnValue;
            }
            set
            {
                if ( value < 0 )
                {
                }

                _Minimum = value;

                if ( value > _Value )
                    _Value = value;
                if ( value > _Maximum )
                    _Maximum = value;
                Invalidate();
            }
        }

        private int _Maximum = 10;

        public int Maximum
        {
            get => _Maximum;
            set
            {
                if ( value < 0 )
                {
                }

                _Maximum = value;
                if ( value < _Value )
                    _Value = value;
                if ( value < _Minimum )
                    _Minimum = value;
                Invalidate();
            }
        }

        private int _Value;

        public int Value
        {
            get => _Value;
            set
            {
                if ( value == _Value )
                    return;

                if ( value > _Maximum || value < _Minimum )
                {
                }

                _Value = value;
                Invalidate();
                Scroll?.Invoke( this );
            }
        }

        public bool ShowValue { get; set; } = false;

        protected override void OnKeyDown( KeyEventArgs e )
        {
            base.OnKeyDown( e );
            if ( e.KeyCode == Keys.Subtract )
            {
                if ( Value == 0 )
                    return;
                Value -= 1;
            }
            else if ( e.KeyCode == Keys.Add )
            {
                if ( Value == _Maximum )
                    return;
                Value += 1;
            }
        }

        protected override void OnTextChanged( EventArgs e )
        {
            base.OnTextChanged( e );
            Invalidate();
        }

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Height = 24;
        }

        private readonly Color BaseColor = Color.FromArgb( 24, 22, 43 );

        private readonly Color SliderColor = Helpers.FlatColor;
        //private Color _HatchColor = Color.FromArgb(23, 148, 92);

        public FlatTrackBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            Height = 20;

            BackColor = Color.FromArgb( 35, 30, 59 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var Base = new Rectangle( 1, 6, W - 2, 8 );
            var GP = new GraphicsPath();
            var GP2 = new GraphicsPath();

            var _with20 = G;
            _with20.SmoothingMode = SmoothingMode.HighQuality;
            _with20.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with20.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with20.Clear( BackColor );

            //-- Value
            Val = Convert.ToInt32(
                (float) ( _Value - _Minimum ) / (float) ( _Maximum - _Minimum ) * (float) ( W - 10 ) );
            Track = new Rectangle( Val, 0, 10, 20 );
            Knob = new Rectangle( Val, 4, 11, 14 );

            //-- Base
            GP.AddRectangle( Base );
            _with20.SetClip( GP );
            _with20.FillRectangle( new SolidBrush( BaseColor ), new Rectangle( 0, 7, W, 8 ) );
            _with20.FillRectangle( new SolidBrush( TrackColor ), new Rectangle( 0, 7, Track.X + Track.Width, 8 ) );
            _with20.ResetClip();

            ////-- Hatch Brush
            //HatchBrush HB = new HatchBrush(HatchStyle.Plaid, HatchColor, _TrackColor);
            //_with20.FillRectangle(HB, new Rectangle(-10, 7, Track.X + Track.Width, 8));

            //-- Slider/Knob
            switch ( Style )
            {
                case _Style.Slider:
                    GP2.AddRectangle( Track );
                    _with20.FillPath( new SolidBrush( SliderColor ), GP2 );
                    break;
                case _Style.Knob:
                    GP2.AddEllipse( Knob );
                    _with20.FillPath( new SolidBrush( SliderColor ), GP2 );
                    break;
            }

            //-- Show the value 
            if ( ShowValue )
                _with20.DrawString( Value.ToString(), new Font( "Tahoma", 8 ), Brushes.White,
                    new Rectangle( 1, 6, W, H ), new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Far
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

            TrackColor = colors.Flat;
        }
    }
}