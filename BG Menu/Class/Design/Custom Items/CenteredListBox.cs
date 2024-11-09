using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG_Menu.Class.Design.Custom_Items
{
    public partial class CenteredListBox : ListBox
    {
        public CenteredListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;  // Ensures each item has a fixed size
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= this.Items.Count) return;

            // Fill the background
            e.DrawBackground();

            // Draw the focus rectangle
            e.DrawFocusRectangle();

            // Get the item text
            string itemText = this.Items[e.Index].ToString();

            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                // Measure the size of the text
                SizeF textSize = e.Graphics.MeasureString(itemText, e.Font);

                // Calculate the starting position of the text to center it horizontally and vertically
                float textX = e.Bounds.Left + (e.Bounds.Width - textSize.Width) / 2;
                float textY = e.Bounds.Top + (e.Bounds.Height - textSize.Height) / 2;

                // Draw the text centered
                e.Graphics.DrawString(itemText, e.Font, textBrush, textX, textY);
            }
        }
    }
}