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
    public partial class addArea : Form
    {
        public string name { get; set; }
        public string age { get; set; }
        public string console { get; set; }
        public string button { get; set; }
        public addArea()
        {
            

            InitializeComponent();
        }

        private void ageBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void createArea_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //error checking

            if (nameBox.Text.ToString().Length <= 0)
            {
                MessageBox.Show("No name typed.", "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (ageBox.SelectedItem == null)
            {
                MessageBox.Show("No age selected.", "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (consoleBox.SelectedItem == null)
            {
                MessageBox.Show("No console selected.", "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (nameBox.Text.ToString().Equals(name))
            {
                MessageBox.Show("Name not changed.", "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            name = nameBox.Text;
            age = ageBox.SelectedItem.ToString();
            console = consoleBox.SelectedItem.ToString();
            

        }

        private void addArea_Load(object sender, EventArgs e)
        {
            if (name != null)
            {
                nameBox.Text = name;
            }
            if (button != null)
            {
                createArea.Text = button;
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
    }
}
