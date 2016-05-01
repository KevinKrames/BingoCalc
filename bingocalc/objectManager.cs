using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parse;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;

namespace bingocalc
{
    public partial class objectManager : Form
    {
        public int ID;
        public ArrayList areas = new ArrayList();
        public ArrayList nodes = new ArrayList();
        public ArrayList paths = new ArrayList();
        public ArrayList objects = new ArrayList();
        public ParseObject DBID;
        private bool input = true;

        private Form1 parent;
        public objectManager()
        {
            InitializeComponent();
        }

        public objectManager(Form1 parent)
        {
            this.parent = parent;
            areas = new ArrayList();
            nodes = new ArrayList();
            paths = new ArrayList();
            objects = new ArrayList();
            InitializeComponent();
        }

        private void objectManager_Load(object sender, EventArgs e)
        {
            updateBoxs();
        }

        private void addObjectButton_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            objectAdder form = new objectAdder();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Add Object";

            //send the area data


            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Check to make sure the object doesnt already exist
                foreach (ParseObject o in objects)
                {
                    if (o["name"].ToString().Equals(form.name))
                    {
                        MessageBox.Show("This object already exists.", "Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ArrayList data = new ArrayList();
                data.Add("addObject");
                data.Add(form.name);
                //Confirm database then add item
                parent.checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }

        private void editObjectButton_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            objectAdder form = new objectAdder();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Edit Object";
            string name = objectsBox.SelectedItem.ToString();
            form.name = name;
            form.button = "Edit";

            //send the area data
            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Check to make sure the object doesnt already exist
                foreach (ParseObject o in objects)
                {
                    if (o["name"].ToString().Equals(form.name))
                    {
                        MessageBox.Show("This object already exists.", "Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ArrayList data = new ArrayList();
                data.Add("editObject");
                data.Add(form.name);
                data.Add(name);
                //Confirm database then add item
                parent.checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }

        private void removeObjectButton_Click(object sender, EventArgs e)
        {
            //Error checking:
            if (objectsBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an object to delete.", "Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = objectsBox.SelectedItem.ToString();

            foreach (ParseObject o in paths)
            {
                ArrayList obtained = parent.convertStringToArrayList(o["currentObtainedObjects"].ToString());
                foreach (string s in obtained)
                {
                    if (s.Equals(name))
                    {
                        MessageBox.Show("You must remove this Object from the following path before deleting it: " + o["name"].ToString(), "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ArrayList required = parent.convertStringToArrayList(o["currentRequiredObjects"].ToString());
                foreach (string s in required)
                {
                    if (s.Equals(name))
                    {
                        MessageBox.Show("You must remove this Object from the following path before deleting it: " + o["name"].ToString(), "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ArrayList prohibited = parent.convertStringToArrayList(o["currentProhibitedObjects"].ToString());
                foreach (string s in prohibited)
                {
                    if (s.Equals(name))
                    {
                        MessageBox.Show("You must remove this Object from the following path before deleting it: " + o["name"].ToString(), "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            confirmation form = new confirmation();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Delete Object";
            form.message = "Delete " + name;
            form.label = "To delete this path type \"" + form.message + "\" without quotes.";

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                ArrayList data = new ArrayList();
                data.Add("deleteObject");
                data.Add(name);

                //Confirm database then add item
                parent.checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }
        //Update labels and input by checking from the parent form
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = parent.label1.Text;
            //If input has changed
            if (input != parent.input)
            {
                //Update the input
                input = parent.input;
                if (input)
                {
                    objectsBox.Enabled = true;
                    searchBox.Enabled = true;
                    addObjectButton.Enabled = true;
                    editObjectButton.Enabled = true;
                    removeObjectButton.Enabled = true;
                    updateBoxs();
                }
                else
                {
                    objectsBox.Enabled = false;
                    searchBox.Enabled = false;
                    addObjectButton.Enabled = false;
                    editObjectButton.Enabled = false;
                    removeObjectButton.Enabled = false;
                    updateBoxs();
                }
            }
        }
        public void updateBoxs()
        {
            objectsBox.Items.Clear();
            foreach (ParseObject o in objects)
            {
                objectsBox.Items.Add(o["name"].ToString());
                objectsBox.Sorted = true;
            }
        }
        //Updates the boxs so that it searchs for a key string
        public void updateBoxs(ArrayList obs)
        {
            objectsBox.Items.Clear();
            foreach (ParseObject o in obs)
            {
                objectsBox.Items.Add(o["name"].ToString());
                objectsBox.Sorted = true;
            }
        }
        //Function for searching for a specific object
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            //Create ArrayList to search for
            ArrayList obs = new ArrayList();
            foreach (ParseObject o in objects)
            {
                if (o["name"].ToString().ToLower().Contains(searchBox.Text.ToLower()))
                {
                    obs.Add(o);
                }
            }
            updateBoxs(obs);
        }
    }
}
