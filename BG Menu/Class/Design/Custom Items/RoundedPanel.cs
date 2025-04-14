using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BG_Menu.Class.Design.Custom_Items
{
    public partial class RoundedPanel : Panel
    {
        // Your custom control code
        public int BorderRadius { get; set; } = 20;
        public int BorderWidth { get; set; } = 3;
        public Color BorderColor { get; set; } = Color.White;

        public RoundedPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        private GraphicsPath GetRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
            {
                using (Pen pen = new Pen(BorderColor, BorderWidth))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
