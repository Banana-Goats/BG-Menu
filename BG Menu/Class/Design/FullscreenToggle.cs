using System.Drawing;
using System.Windows.Forms;

namespace BG_Menu.Class.Design
{
    public class FullscreenToggle
    {
        private Form form;
        private Size previousSize;
        private Point previousLocation;
        private RoundedCorners roundedCorners;
        public bool IsFullscreen { get; set; } = false;

        public FullscreenToggle(Form form, RoundedCorners roundedCorners)
        {
            this.form = form;
            this.roundedCorners = roundedCorners;
        }

        public void ToggleFullscreen()
        {
            if (IsFullscreen)
            {
                // Exit fullscreen mode, restore the form's previous size, location, and rounded corners
                form.WindowState = FormWindowState.Normal;
                form.Size = previousSize;
                form.Location = previousLocation;
                form.FormBorderStyle = FormBorderStyle.None; // Keep the form borderless

                // Reapply the rounded corners
                roundedCorners.Apply();

                IsFullscreen = false;
            }
            else
            {
                // Store the current size and location
                previousSize = form.Size;
                previousLocation = form.Location;

                // Enter fullscreen mode
                form.WindowState = FormWindowState.Normal;  // First reset to normal
                form.FormBorderStyle = FormBorderStyle.None; // Ensure the form is borderless

                // Remove the rounded corners
                form.Region = null;

                form.WindowState = FormWindowState.Maximized;
                IsFullscreen = true;
            }
        }
    }
}
