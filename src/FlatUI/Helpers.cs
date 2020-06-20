﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FlatUI
{
    public static class Helpers
    {
        public static Color FlatColor = Color.FromArgb( 22, 96, 253 );
        public static Color FlatWhite = Color.FromArgb( 255, 252, 255 );

        public static readonly StringFormat NearSF = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        public static readonly StringFormat CenterSF = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public static GraphicsPath RoundRec( Rectangle Rectangle, int Curve )
        {
            var P = new GraphicsPath();
            var ArcRectangleWidth = Curve * 2;
            P.AddArc( new Rectangle( Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth ), -180, 90 );
            P.AddArc(
                new Rectangle( Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth,
                    ArcRectangleWidth ), -90, 90 );
            P.AddArc(
                new Rectangle( Rectangle.Width - ArcRectangleWidth + Rectangle.X,
                    Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth ), 0, 90 );
            P.AddArc(
                new Rectangle( Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth,
                    ArcRectangleWidth ), 90, 90 );
            P.AddLine( new Point( Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y ),
                new Point( Rectangle.X, Curve + Rectangle.Y ) );
            return P;
        }

        public static GraphicsPath RoundRect( float x, float y, float w, float h, double r = 0.3,
            bool TL = true, bool TR = true, bool BR = true, bool BL = true )
        {
            GraphicsPath functionReturnValue = null;
            var d = Math.Min( w, h ) * (float) r;
            var xw = x + w;
            var yh = y + h;
            functionReturnValue = new GraphicsPath();

            var _with1 = functionReturnValue;
            if ( TL )
                _with1.AddArc( x, y, d, d, 180, 90 );
            else
                _with1.AddLine( x, y, x, y );
            if ( TR )
                _with1.AddArc( xw - d, y, d, d, 270, 90 );
            else
                _with1.AddLine( xw, y, xw, y );
            if ( BR )
                _with1.AddArc( xw - d, yh - d, d, d, 0, 90 );
            else
                _with1.AddLine( xw, yh, xw, yh );
            if ( BL )
                _with1.AddArc( x, yh - d, d, d, 90, 90 );
            else
                _with1.AddLine( x, yh, x, yh );

            _with1.CloseFigure();
            return functionReturnValue;
        }

        //-- Credit: AeonHack
        public static GraphicsPath DrawArrow( int x, int y, bool flip )
        {
            var GP = new GraphicsPath();

            var W = 12;
            var H = 6;

            if ( flip )
            {
                GP.AddLine( x + 1, y, x + W + 1, y );
                GP.AddLine( x + W, y, x + H, y + H - 1 );
            }
            else
            {
                GP.AddLine( x, y + H, x + W, y + H );
                GP.AddLine( x + W, y + H, x + H, y );
            }

            GP.CloseFigure();
            return GP;
        }

        /// <summary>
        /// Get the colorscheme of a Control from a parent FormSkin.
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>Colors</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static FlatColors GetColors( Control control )
        {
            if ( control == null )
                throw new ArgumentNullException();

            var colors = new FlatColors();

            while ( control != null && control.GetType() != typeof(FormSkin) ) control = control.Parent;

            if ( control != null )
            {
                var skin = (FormSkin) control;
                colors.Flat = skin.FlatColor;
            }

            return colors;
        }
    }
}