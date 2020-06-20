using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatComboBox : ComboBox
    {
        private int W;
        private int H;
        private int _StartIndex = 0;
        private int x;
        private int y;

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

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );
            x = e.Location.X;
            y = e.Location.Y;
            Invalidate();
            if ( e.X < Width - 41 )
                Cursor = Cursors.IBeam;
            else
                Cursor = Cursors.Hand;
        }

        protected override void OnDrawItem( DrawItemEventArgs e )
        {
            base.OnDrawItem( e );
            Invalidate();
            if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected ) Invalidate();
        }

        protected override void OnClick( EventArgs e )
        {
            base.OnClick( e );
            Invalidate();
        }

        [Category( "Colors" )]
        public Color HoverColor { get; set; } = Color.FromArgb( 22, 96, 253 );

        private int StartIndex
        {
            get => _StartIndex;
            set
            {
                _StartIndex = value;
                try
                {
                    base.SelectedIndex = value;
                }
                catch
                {
                }

                Invalidate();
            }
        }

        public Font IconFont { get; }

        public void DrawItem_( object sender, DrawItemEventArgs e )
        {
            if ( e.Index < 0 )
                return;
            e.DrawBackground();
            e.DrawFocusRectangle();

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
                //-- Selected item
                e.Graphics.FillRectangle( new SolidBrush( HoverColor ), e.Bounds );
            else
                //-- Not Selected
                e.Graphics.FillRectangle( new SolidBrush( _BGColor ), e.Bounds );

            //-- Text
            e.Graphics.DrawString( GetItemText( Items[e.Index] ), new Font( "Tahoma", 8 ), Brushes.White,
                new Rectangle( e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height ) );


            e.Graphics.Dispose();
        }

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            Height = 16;
        }

        private readonly Color _BaseColor = Color.FromArgb( 22, 96, 253 );
        private readonly Color _BGColor = Color.FromArgb( 24, 22, 43 );

        public FlatComboBox()
        {
            DrawItem += DrawItem_;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;

            DrawMode = DrawMode.OwnerDrawFixed;
            BackColor = Color.FromArgb( 37, 34, 65 );
            ForeColor = Color.White;
            DropDownStyle = ComboBoxStyle.DropDownList;
            Cursor = Cursors.Hand;
            StartIndex = 0;
            ItemHeight = 16;
            Font = new Font( "Tahoma", 8, FontStyle.Regular );
            IconFont = new Font( "Marlett", 12 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width;
            H = Height;

            var Base = new Rectangle( 0, 0, W, H );
            var Button = new Rectangle( Convert.ToInt32( W - 30 ), 0, W, H - 1 );
            var GP = new GraphicsPath();
            var GP2 = new GraphicsPath();

            var _with16 = G;
            _with16.Clear( Color.FromArgb( 37, 34, 65 ) );
            _with16.SmoothingMode = SmoothingMode.HighQuality;
            _with16.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with16.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            //-- Base
            _with16.FillRectangle( new SolidBrush( _BGColor ), Base );

            //-- Button
            GP.Reset();
            GP.AddRectangle( Button );
            _with16.SetClip( GP );
            _with16.FillRectangle( new SolidBrush( _BaseColor ), Button );
            _with16.ResetClip();

            //-- Lines
            _with16.DrawString( "6", IconFont, Brushes.White, new Point( W - 26, 3 ) );
            //_with16.DrawLine(Pens.White, W - 8, 4, W - 28, 4);
            //_with16.DrawLine(Pens.White, W - 8, 10, W - 28, 10);
            //_with16.DrawLine(Pens.White, W - 8, 16, W - 28, 16);

            //-- Text
            _with16.DrawString( Text, Font, Brushes.White, new Point( 4, 5 ), Helpers.NearSF );

            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }
    }
}