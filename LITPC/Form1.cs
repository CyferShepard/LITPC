using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace LITPC
{
    public partial class Form1 : Form
    {
        litapi api = new litapi();
        public Form1()
        {
            InitializeComponent();
            firstruncheck();
        }
        public static string loc = Path.GetDirectoryName(Application.ExecutablePath) + "\\";


        public void firstruncheck()
        {

            if (!File.Exists(loc + "\\HtmlAgilityPack.dll"))
            {
                System.Windows.Forms.MessageBox.Show("HtmlAgilityPack.dll Missing. Redownload Application to restore file.", "Error: HtmlAgilityPack.dll Missing.",
                      System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                if (!Directory.Exists(loc + "Downloads"))
                {
                    Directory.CreateDirectory(loc + "Downloads");
                }
            }

           
               
               

            

        }


        

        private void button1_Click(object sender, EventArgs e)
        {
            string url = txtURL.Text;
            if(url!=null)
            {
                if(url.Contains("?"))
                {
                    int index = url.LastIndexOf(@"?");
                   url= url.Substring(0, index);
                }
                api.seturl(url);
                api.setLoc(loc);
                api.init();
                api.savefile();
               
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Url Cannot Be Blank.", "Error: Invalid URL.",
                      System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/CyferShepard");
        }

        
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private bool mouseDown;
        private Point lastLocation;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
