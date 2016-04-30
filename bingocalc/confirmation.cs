using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bingocalc
{
    public partial class confirmation : Form
    {
        public string message { get; set; }
        public string label { get; set; }
        public confirmation()
        {
            InitializeComponent();
        }

        private void createArea_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //error checking
            //Name error checking
            if (!nameBox.Text.ToString().Equals(message))
            {
                MessageBox.Show("Incorrect confirmation typed.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private void confirmation_Load(object sender, EventArgs e)
        {
            if (label != null)
            {
                label1.Text = label;
            }
        }
    }
}
