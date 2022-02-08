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
    public partial class CanvasSizeForm : Form
    {

        MainForm mainForm;

        public CanvasSizeForm(MainForm m)
        {
            InitializeComponent();
            mainForm = m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int Width) && int.TryParse(textBox2.Text, out int Height))
            {
                mainForm.WidthImage = Width;
                mainForm.HeightImage = Height;
            }
            else
            {
                
            }
        }
    }
}
