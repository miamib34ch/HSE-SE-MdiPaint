using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MdiPaint
{
    public partial class Main : Form
    {
        public Color penColor = Color.Black;

        public Main()
        {
            InitializeComponent();
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void упорядочитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var img = new Image(this);
            img.MdiParent = this;
            img.Show();
        }


        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void посмотретьСправкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void каскадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        public void changeXY(int X, int Y)
        {
            toolStripStatusLabel1.Text = $"X:{X}, Y:{Y}";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (Image img in MdiChildren)
            {
                img.penColor = Color.Red;
            }
            penColor = Color.Red;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            foreach (Image img in MdiChildren)
            {
                img.penColor = Color.Black;
            }
            penColor = Color.Black;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var colorChoice = new ColorDialog();
            colorChoice.ShowDialog();
            foreach (Image img in MdiChildren)
            {
                img.penColor = colorChoice.Color;
            }
            penColor = colorChoice.Color;
        }
    }
}
