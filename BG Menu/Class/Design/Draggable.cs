using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BG_Menu.Class.Design
{
    public class Draggable
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(nint hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private Control handleControl;
        private Form form;

        public Draggable(Control handleControl, Form form)
        {
            this.handleControl = handleControl;
            this.form = form;

            // Attach the MouseDown event to the control
            this.handleControl.MouseDown += HandleControl_MouseDown;
        }

        private void HandleControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
