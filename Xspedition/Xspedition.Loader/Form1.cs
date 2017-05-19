using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xspedition.Loader
{
    public partial class Form1 : Form
    {
        private LoadManager manager;

        public Form1()
        {
            InitializeComponent();
            manager = new LoadManager();
        }

        private void OnBtnLoadClick(object sender, EventArgs e)
        {
            manager.LoadData();
            lblLoad.Text = "Initial load completed";
        }

        private void OnBtnP1Click(object sender, EventArgs e)
        {
            manager.PerformeDateOffset();
            lblP1.Text = "Phase One completed";
        }

        private void OnBtnP2Click(object sender, EventArgs e)
        {
            manager.PerformeDateOffset();
            lblP2.Text = "Phase Two completed";
        }

        private void OnBtnP3Click(object sender, EventArgs e)
        {
            manager.PerformeDateOffset();
            lblP3.Text = "Phase Three completed";
        }

        private void OnBtnP4Click(object sender, EventArgs e)
        {
            manager.PerformeDateOffset();
            lblP4.Text = "Phase Four completed";
        }
    }
}
