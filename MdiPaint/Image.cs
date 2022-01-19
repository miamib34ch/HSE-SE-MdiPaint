using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MdiPaint
{
    public partial class Image : Form
    {
        private int X, Y;
        Main parentForm;
        public Color penColor;
        Graphics img;

        public Image(Main parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            penColor = parentForm.penColor;
        }

        private void draw(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                img = CreateGraphics();
                img.DrawLine(new Pen(penColor),X,Y,e.X,e.Y);
                X = e.X;
                Y = e.Y;
            }
            else
            {
            }

            parentForm.changeXY(e.X, e.Y);
        }

        private void leavingForm(object sender, EventArgs e)
        {
            parentForm.changeXY(0,0);
        }

        private void drawStart(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }
    }
}
