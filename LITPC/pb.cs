using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LITPC
{
    public partial class pb : Form
    {
        public pb()
        {
            InitializeComponent();
        }
        public void SetProgress(int progress)
        {
            pbmain.Value = progress;
        }

        private void pbmain_Click(object sender, EventArgs e)
        {

        }
    }
}
