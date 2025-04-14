using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BG_Menu.Class.Design
{
    public class RoundedCorners
    {
        private Form form;
        private int cornerRadius;
        private int borderWidth;
        private Color borderColor;

        public RoundedCorners(Form form, int cornerRadius, int borderWidth = 2, Color? borderColor = null)
        {
            this.form = form;
            this.cornerRadius = cornerRadius;
            this.borderWidth = borderWidth;
            this.borderColor = borderColor ?? Color.Transparent;  // Default to yellow if no color is specified

            // Apply rounded corners and border initially when the form is created
            Apply();
        }

        public void Apply()
        {
            ApplyRoundedCorners();
            form.Invalidate(); // Force the form to repaint with the new border
        }

        private void ApplyRoundedCorners()
        {
            int width = form.ClientSize.Width;
            int height = form.ClientSize.Height;

            if (width > 0 && height > 0) // Ensure dimensions are valid
            {
                // Create the path for the rounded rectangle
                GraphicsPath path = new GraphicsPath();
                path.StartFigure();
                path.AddArc(new Rectangle(0, 0, cornerRadius, cornerRadius), 180, 90);
                path.AddArc(new Rectangle(width - cornerRadius - 1, 0, cornerRadius, cornerRadius), -90, 90);
                path.AddArc(new Rectangle(width - cornerRadius - 1, height - cornerRadius - 1, cornerRadius, cornerRadius), 0, 90);
                path.AddArc(new Rectangle(0, height - cornerRadius - 1, cornerRadius, cornerRadius), 90, 90);
                path.CloseFigure();

                // Set the region of the form to the rounded path
                form.Region = new Region(path);

                // Add event handler for custom painting
                form.Paint -= Form_Paint;  // Ensure it's not added multiple times
                form.Paint += Form_Paint;
            }
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int width = form.ClientSize.Width;
                int height = form.ClientSize.Height;

                path.StartFigure();
                path.AddArc(new Rectangle(0, 0, cornerRadius, cornerRadius), 180, 90);
                path.AddArc(new Rectangle(width - cornerRadius - 1, 0, cornerRadius, cornerRadius), -90, 90);
                path.AddArc(new Rectangle(width - cornerRadius - 1, height - cornerRadius - 1, cornerRadius, cornerRadius), 0, 90);
                path.AddArc(new Rectangle(0, height - cornerRadius - 1, cornerRadius, cornerRadius), 90, 90);
                path.CloseFigure();

                // Apply anti-aliasing and high-quality rendering settings
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Draw the border with anti-aliasing for smooth edges
                using (Pen pen = new Pen(borderColor, borderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;  // Align the pen to prevent clipping at the edges
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
