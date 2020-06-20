using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public class FlatTreeView : TreeView
    {
        private TreeNodeStates State;

        protected override void OnDrawNode( DrawTreeNodeEventArgs e )
        {
            try
            {
                var Bounds = new Rectangle( e.Bounds.Location.X, e.Bounds.Location.Y, e.Bounds.Width, e.Bounds.Height );
                //e.Node.Nodes.Item.
                switch ( State )
                {
                    case TreeNodeStates.Default:
                        e.Graphics.FillRectangle( Brushes.Red, Bounds );
                        e.Graphics.DrawString( e.Node.Text, new Font( "Tahoma", 8 ), Brushes.LimeGreen,
                            new Rectangle( Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height ), Helpers.NearSF );
                        Invalidate();
                        break;
                    case TreeNodeStates.Checked:
                        e.Graphics.FillRectangle( Brushes.Green, Bounds );
                        e.Graphics.DrawString( e.Node.Text, new Font( "Tahoma", 8 ), Brushes.Black,
                            new Rectangle( Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height ), Helpers.NearSF );
                        Invalidate();
                        break;
                    case TreeNodeStates.Selected:
                        e.Graphics.FillRectangle( Brushes.Green, Bounds );
                        e.Graphics.DrawString( e.Node.Text, new Font( "Tahoma", 8 ), Brushes.Black,
                            new Rectangle( Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height ), Helpers.NearSF );
                        Invalidate();
                        break;
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message );
            }

            base.OnDrawNode( e );
        }

        private readonly Color _BaseColor = Color.FromArgb( 24, 22, 43 );
        private readonly Color _LineColor = Color.FromArgb( 25, 27, 29 );

        public FlatTreeView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true );
            DoubleBuffered = true;

            BackColor = _BaseColor;
            ForeColor = Color.White;
            LineColor = _LineColor;
            DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            var B = new Bitmap( Width, Height );
            var G = Graphics.FromImage( B );

            var Base = new Rectangle( 0, 0, Width, Height );

            var _with22 = G;
            _with22.SmoothingMode = SmoothingMode.HighQuality;
            _with22.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _with22.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _with22.Clear( BackColor );

            _with22.FillRectangle( new SolidBrush( _BaseColor ), Base );
            _with22.DrawString( Text, new Font( "Tahoma", 8 ), Brushes.Black,
                new Rectangle( Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height ), Helpers.NearSF );


            base.OnPaint( e );
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled( B, 0, 0 );
            B.Dispose();
        }
    }
}