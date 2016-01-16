using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerFont
{
    public partial class Form1 : Form
    {
        int LastChar = 0x2603; // 0x1F3E9;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            UpdateCharView();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            UpdateCharView();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) UpdateCharView();
        }

        private void UpdateCharView()
        {
            string charCode = textBox1.Text;
            int n;

            if (charCode.StartsWith("U+", true, CultureInfo.InvariantCulture))
            {
                charCode = charCode.Substring(2);
            }

            if (charCode.Length > 1 && int.TryParse(charCode, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out n))
            {
                LastChar = n;
            }
            else if (charCode.Length > 0)
            {
                LastChar = char.ConvertToUtf32(charCode, 0);
            }

            textBox1.Text = LastChar.ToString("X4", CultureInfo.InvariantCulture);

            listView1.Clear();
            var list = FontFinder.Find(LastChar);

            foreach (string ff in list)
            {
                var lvi = new ListViewItem(ff);

                //string imagekey = ff + "::" + LastChar.ToString();
                //if (!imageList1.Images.ContainsKey(imagekey))
                //{
                //    Bitmap i = new Bitmap(imageList1.ImageSize.Width, imageList1.ImageSize.Height);
                //    using (Graphics g = Graphics.FromImage(i))
                //    {
                //        g.Clear(SystemColors.Window);
                //        g.DrawString(previewStr, new Font(ff, 72f), SystemBrushes.WindowText, new RectangleF(0, 0, 100, 100), previewSf);
                //    }
                //    imageList1.Images.Add(imagekey, i);
                //}
                //lvi.ImageKey = imagekey;
                
                listView1.Items.Add(ff);
            }
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            string previewStr = char.ConvertFromUtf32(LastChar);

            var sf = new StringFormat(StringFormatFlags.NoFontFallback);
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            var font = new Font(e.Item.Text, 72f, FontStyle.Regular, GraphicsUnit.Pixel);
            var smallFont = new Font(e.Item.Text, 12f, FontStyle.Regular, GraphicsUnit.Pixel);

            e.Graphics.DrawString(previewStr, font, SystemBrushes.WindowText, e.Bounds, sf);

            RectangleF labelRect = new RectangleF(e.Bounds.X, e.Bounds.Y + (e.Bounds.Height - 24), e.Bounds.Width, 24);
            e.Graphics.DrawString(e.Item.Text, smallFont, SystemBrushes.WindowText, labelRect, sf);
        }
    }
}
