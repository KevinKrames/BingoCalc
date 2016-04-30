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
    public partial class updateForm : Form
    {
        public void setLabel(string aLabel)
        {
            label1.Text = aLabel;
        }
        public updateForm()
        {
            InitializeComponent();
        }
    }
}
