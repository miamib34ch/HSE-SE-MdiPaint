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
    public partial class StarSettings : Form
    {
        MainForm m;

        public StarSettings(MainForm m)
        {
            InitializeComponent();
            this.m = m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m.tools = Tools.Star; 
            if(int.TryParse(textBox1.Text, out int a))
            {
                DocumentForm.starEnd = a;
            }
        }
    }
}
