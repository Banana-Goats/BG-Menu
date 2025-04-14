using System;
using System.Drawing;
using System.Windows.Forms;

namespace BG_Menu.Class.Design
{
    public class TrayMinimizer
    {
        private Form form;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip trayMenu;
        private RoundedCorners roundedCorners;
        private FullscreenToggle fullscreenToggle;

        public TrayMinimizer(Form form, RoundedCorners roundedCorners, FullscreenToggle fullscreenToggle)
        {
            this.form = form;
            this.roundedCorners = roundedCorners;
            this.fullscreenToggle = fullscreenToggle;
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            // Create the context menu for the tray icon
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Restore", null, RestoreFromTray);
            trayMenu.Items.Add("Exit", null, ExitApplication);

            // Initialize the NotifyIcon
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application, // Use a default application icon or your own
                ContextMenuStrip = trayMenu,
                Visible = false,
                Text = form.Text // Set the text to the form's title
            };

            // Handle double-click to restore the application
            notifyIcon.DoubleClick += RestoreFromTray;
        }

        private void RestoreFromTray(object sender, EventArgs e)
        {
            // Restore the form and hide the NotifyIcon
            form.Show();
            form.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;

            // Reapply the rounded corners
            roundedCorners.Apply();

            // Reset the fullscreen state
            if (fullscreenToggle.IsFullscreen)
            {
                fullscreenToggle.IsFullscreen = false;
                UpdateFullscreenButtonImage();
            }
        }

        private void UpdateFullscreenButtonImage()
        {
            // Find the button10 by its name
            var toggleFullscreenButton = form.Controls.Find("btnScreenSize", true).FirstOrDefault() as Button;

            // Check if the button was found before trying to update the image
            if (toggleFullscreenButton != null)
            {
                toggleFullscreenButton.BackgroundImage = Properties.Resources.Full_Screen;
            }
            else
            {
                // Log or handle the case where the button is not found
                MessageBox.Show("Fullscreen toggle button btnScreenSize not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ExitApplication(object sender, EventArgs e)
        {
            // Close the application
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
