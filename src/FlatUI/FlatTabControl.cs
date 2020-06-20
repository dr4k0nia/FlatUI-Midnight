using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatTabControl : TabControl
    {
        private int W;
        private int H;

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Top;
        }

        [Category( "Colors" )]
        public Color BaseColor { get; set; } = Color.FromArgb( 24, 22, 43 );

        [Category( "Colors" )]
        public Color ActiveColor { get; set; } = Helpers.FlatColor;

        private readonly Color BGColor = Color.FromArgb( 35, 30, 59 );

        public FlatTabControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;
            BackColor = Color.FromArgb( 35, 30, 59 );

            Font = new Font( "Tahoma", 10 );
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size( 120, 40 );
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            UpdateColors();

            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );
            W = Width - 1;
            H = Height - 1;

            var _with13 = G;
            _with13.SmoothingMode = SmoothingMode.HighQuality;
            _with13.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with13.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with13.Clear( BaseColor );

            try
            {
                SelectedTab.BackColor = BGColor;
            }
            catch
            {
            }

            for ( var i = 0; i <= TabCount - 1; i++ )
            {
                var Base = new Rectangle( new Point( GetTabRect( i ).Location.X + 2, GetTabRect( i ).Location.Y ),
                    new Size( GetTabRect( i ).Width, GetTabRect( i ).Height ) );
                var BaseSize = new Rectangle( Base.Location, new Size( Base.Width, Base.Height ) );

                if ( i == SelectedIndex )
                {
                    //-- Base
                    _with13.FillRectangle( new SolidBrush( BaseColor ), BaseSize );

                    //-- Gradiant
                    //.fill
                    _with13.FillRectangle( new SolidBrush( ActiveColor ), BaseSize );

                    //-- ImageList
                    if ( ImageList != null )
                        try
                        {
                            if ( ImageList.Images[TabPages[i].ImageIndex] != null )
                            {
                                //-- Image
                                _with13.DrawImage( ImageList.Images[TabPages[i].ImageIndex],
                                    new Point( BaseSize.Location.X + 8, BaseSize.Location.Y + 6 ) );
                                //-- Text
                                _with13.DrawString( "      " + TabPages[i].Text, Font, Brushes.White, BaseSize,
                                    Helpers.CenterSF );
                            }
                            else
                            {
                                //-- Text
                                _with13.DrawString( TabPages[i].Text, Font, Brushes.White, BaseSize, Helpers.CenterSF );
                            }
                        }
                        catch ( Exception ex )
                        {
                            throw new Exception( ex.Message );
                        }
                    else
                        //-- Text
                        _with13.DrawString( TabPages[i].Text, Font, Brushes.White, BaseSize, Helpers.CenterSF );
                }
                else
                {
                    //-- Base
                    _with13.FillRectangle( new SolidBrush( BaseColor ), BaseSize );

                    //-- ImageList
                    if ( ImageList != null )
                        try
                        {
                            if ( ImageList.Images[TabPages[i].ImageIndex] != null )
                            {
                                //-- Image
                                _with13.DrawImage( ImageList.Images[TabPages[i].ImageIndex],
                                    new Point( BaseSize.Location.X + 8, BaseSize.Location.Y + 6 ) );
                                //-- Text
                                _with13.DrawString( "      " + TabPages[i].Text, Font, new SolidBrush( Color.White ),
                                    BaseSize, new StringFormat
                                    {
                                        LineAlignment = StringAlignment.Center,
                                        Alignment = StringAlignment.Center
                                    } );
                            }
                            else
                            {
                                //-- Text
                                _with13.DrawString( TabPages[i].Text, Font, new SolidBrush( Color.White ), BaseSize,
                                    new StringFormat
                                    {
                                        LineAlignment = StringAlignment.Center,
                                        Alignment = StringAlignment.Center
                                    } );
                            }
                        }
                        catch ( Exception ex )
                        {
                            throw new Exception( ex.Message );
                        }
                    else
                        //-- Text
                        _with13.DrawString( TabPages[i].Text, Font, new SolidBrush( Color.White ), BaseSize,
                            new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            } );
                }
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

            ActiveColor = colors.Flat;
        }
    }
}