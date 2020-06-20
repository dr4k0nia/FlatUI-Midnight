using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent( "TextChanged" )]
    public class FlatTextBox : Control
    {
        private int W;
        private int H;
        private MouseState State = MouseState.None;
        private readonly TextBox TB;

        private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;

        [Category( "Options" )]
        public HorizontalAlignment TextAlign
        {
            get => _TextAlign;
            set
            {
                _TextAlign = value;
                if ( TB != null ) TB.TextAlign = value;
            }
        }

        private int _MaxLength = 32767;

        [Category( "Options" )]
        public int MaxLength
        {
            get => _MaxLength;
            set
            {
                _MaxLength = value;
                if ( TB != null ) TB.MaxLength = value;
            }
        }

        private bool _ReadOnly;

        [Category( "Options" )]
        public bool ReadOnly
        {
            get => _ReadOnly;
            set
            {
                _ReadOnly = value;
                if ( TB != null ) TB.ReadOnly = value;
            }
        }

        private bool _UseSystemPasswordChar;

        [Category( "Options" )]
        public bool UseSystemPasswordChar
        {
            get => _UseSystemPasswordChar;
            set
            {
                _UseSystemPasswordChar = value;
                if ( TB != null ) TB.UseSystemPasswordChar = value;
            }
        }

        private bool _Multiline;

        [Category( "Options" )]
        public bool Multiline
        {
            get => _Multiline;
            set
            {
                _Multiline = value;
                if ( TB != null )
                {
                    TB.Multiline = value;

                    if ( value )
                        TB.Height = Height - 11;
                    else
                        Height = TB.Height + 11;
                }
            }
        }

        [Category( "Options" )]
        public bool FocusOnHover { get; set; } = false;

        [Category( "Options" )]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                if ( TB != null ) TB.Text = value;
            }
        }

        [Category( "Options" )]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if ( TB != null )
                {
                    TB.Font = value;
                    TB.Location = new Point( 3, 5 );
                    TB.Width = Width - 6;

                    if ( !_Multiline ) Height = TB.Height + 11;
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if ( !Controls.Contains( TB ) ) Controls.Add( TB );
        }

        private void OnBaseTextChanged( object s, EventArgs e )
        {
            Text = TB.Text;
        }

        private void OnBaseKeyDown( object s, KeyEventArgs e )
        {
            if ( e.Control && e.KeyCode == Keys.A )
            {
                TB.SelectAll();
                e.SuppressKeyPress = true;
            }

            if ( e.Control && e.KeyCode == Keys.C )
            {
                TB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize( EventArgs e )
        {
            TB.Location = new Point( 5, 5 );
            TB.Width = Width - 10;

            if ( _Multiline )
                TB.Height = Height - 11;
            else
                Height = TB.Height + 11;

            base.OnResize( e );
        }

        [Category( "Colors" )]
        public Color TextColor { get; set; } = Color.FromArgb( 192, 192, 192 );

        public override Color ForeColor
        {
            get => TextColor;
            set => TextColor = value;
        }

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
            TB.Focus();
            Invalidate();
        }

        protected override void OnMouseEnter( EventArgs e )
        {
            base.OnMouseEnter( e );
            State = MouseState.Over;
            if ( FocusOnHover ) TB.Focus();
            Invalidate();
        }

        protected override void OnMouseLeave( EventArgs e )
        {
            base.OnMouseLeave( e );
            State = MouseState.None;
            Invalidate();
        }

        private readonly Color _BaseColor = Color.FromArgb( 24, 22, 43 );
        private Color _BorderColor = Helpers.FlatColor;

        public FlatTextBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true );
            DoubleBuffered = true;

            BackColor = Color.Transparent;

            TB = new TextBox();
            TB.Font = new Font( "Tahoma", 10 );
            TB.Text = Text;
            TB.BackColor = _BaseColor;
            TB.ForeColor = TextColor;
            TB.MaxLength = _MaxLength;
            TB.Multiline = _Multiline;
            TB.ReadOnly = _ReadOnly;
            TB.UseSystemPasswordChar = _UseSystemPasswordChar;
            TB.BorderStyle = BorderStyle.None;
            TB.Location = new Point( 5, 5 );
            TB.Width = Width - 10;

            TB.Cursor = Cursors.IBeam;

            if ( _Multiline )
                TB.Height = Height - 11;
            else
                Height = TB.Height + 11;

            TB.TextChanged += OnBaseTextChanged;
            TB.KeyDown += OnBaseKeyDown;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var Base = new Rectangle( 0, 0, W, H );

            var _with12 = G;
            _with12.SmoothingMode = SmoothingMode.HighQuality;
            _with12.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with12.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with12.Clear( BackColor );

            //-- Colors
            TB.BackColor = _BaseColor;
            TB.ForeColor = TextColor;

            //-- Base
            _with12.FillRectangle( new SolidBrush( _BaseColor ), Base );

            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors( this );

            _BorderColor = colors.Flat;
        }
    }
}