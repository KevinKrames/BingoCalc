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
    public partial class objectAdder : Form
    {

        public string name { get; set; }
        public string button { get; set; }
        public objectAdder()
        {
            InitializeComponent();
        }

        private void createArea_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //error checking

            if (nameBox.Text.ToString().Length <= 0)
            {
                MessageBox.Show("No name typed.", "Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (nameBox.Text.ToString().Equals(name))
            {
                MessageBox.Show("Name not changed.", "Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            name = nameBox.Text;
        }

        private void objectAdder_Load(object sender, EventArgs e)
        {
            if (name != null)
            {
                nameBox.Text = name;
            }
            if (button != null)
            {
                createArea.Text = button;
            }
        }
    }
}
