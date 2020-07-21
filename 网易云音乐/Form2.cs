using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 网易云音乐
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        int g_formWidth, g_formHeight;
        private void Form2_Load(object sender, EventArgs e)
        {
            g_formWidth = this.Height;
            g_formHeight = this.Width;
            WriteIn_Tags(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 nf = new Form1();
            nf.ShowDialog();
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }
        void WriteIn_Tags(Control cons)
        {
            foreach(Control con in cons.Controls)
            {
                string str = con.Width.ToString() + "?" + con.Height.ToString() + "?" + con.Left.ToString() + "?" + con.Top.ToString() + "?" + con.Font.Size.ToString();
                con.Tag = str;
                if (con.Controls.Count > 0)
                {
                    WriteIn_Tags(con);
                }
            }
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            Form fm = (Form)sender;
            double scaleX = fm.Width * 1.0 / g_formWidth;
            double scaleY = fm.Height * 1.0 / g_formHeight;
            Resize_AllControls(fm, scaleX, scaleY);
        }

        void Resize_AllControls(Control cons,double scaleX,double scaleY)
        {
            foreach(Control con in cons.Controls)
            {
                var tags = con.Tag.ToString().Split(new char[] { '?' });
                int widthOld = Convert.ToInt32(tags[0]);
                int heightOld = Convert.ToInt32(tags[1]);
                int leftOld = Convert.ToInt32(tags[2]);
                int topOld = Convert.ToInt32(tags[3]);
                int fontSizeOld = Convert.ToInt32(tags[4]);

                con.Width = Convert.ToInt32(widthOld * scaleX);
                con.Height = Convert.ToInt32(Height * scaleY);
                con.Left = Convert.ToInt32(Left * scaleX);
                con.Top = Convert.ToInt32(Top * scaleY);
                int fontSizeNew = Convert.ToInt32(fontSizeOld * scaleX);
                con.Font = new Font(con.Font.Name, fontSizeNew, Font.Style);

                if (con.Controls.Count > 0)
                {
                    Resize_AllControls(con, scaleX, scaleX);//遍历所有的control
                }
            }
        }
    }
}
