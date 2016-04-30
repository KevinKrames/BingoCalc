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
    public partial class node : Form
    {
        public string name { get; set; }
        public string age { get; set; }
        public string area { get; set; }
        public string console { get; set; }
        public string button { get; set; }
        public node()
        {
            InitializeComponent();
        }

        private void node_Load(object sender, EventArgs e)
        {
            if (name != null)
            {
                nameBox.Text = name;
            }
            if (button != null)
            {
                createArea.Text = button;
            }
            if (area != null)
            {
                areaBox.Enabled = false;
                areaBox.Text = area;
            }
            if (age != null)
            {
                ageBox.Enabled = false;
                for (int i = 0; i < ageBox.Items.Count; i++)
                {
                    if (ageBox.Items[i].ToString().Equals(age))
                    {
                        ageBox.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (console != null)
            {
                consoleBox.Enabled = false;
                for (int i = 0; i < consoleBox.Items.Count; i++)
                {
                    if (consoleBox.Items[i].ToString().Equals(console))
                    {
                        consoleBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void createArea_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //error checking

            if (nameBox.Text.ToString().Length <= 0)
            {
                MessageBox.Show("No name typed.", "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (nameBox.Text.ToString().Equals(name))
            {
                MessageBox.Show("Name not changed.", "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            name = nameBox.Text;
        }
    }
}
