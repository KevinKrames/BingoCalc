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
    public partial class path : Form
    {
        public string name { get; set; }
        public string age { get; set; }
        public string area { get; set; }
        public string areaDestination { get; set; }
        public string nodeStart { get; set; }
        public string nodeEnd { get; set; }
        public int rupees { get; set; }
        public int bombs { get; set; }
        public int bombchus { get; set; }
        public int health { get; set; }
        public string difficulty { get; set; }
        public ArrayList times { get; set; }
        public ArrayList timesOfDay { get; set; }
        public string timeOfDayNumber { get; set; }
        public string timeOfDay { get; set; }
        public string console { get; set; }
        public string button { get; set; }
        public ArrayList areas { get; set; }
        public ArrayList nodes { get; set; }
        public ArrayList objects { get; set; }

        public ArrayList availableObtainedObjects { get; set; }
        public ArrayList availableRequiredObjects { get; set; }
        public ArrayList availableProhibitedObjects { get; set; }
        public ArrayList currentObtainedObjects { get; set; }
        public ArrayList currentRequiredObjects { get; set; }
        public ArrayList currentProhibitedObjects { get; set; }
        public path()
        {

            times = new ArrayList();
            timesOfDay = new ArrayList();

            availableObtainedObjects = new ArrayList();
            availableRequiredObjects = new ArrayList();
            availableProhibitedObjects = new ArrayList();
            currentObtainedObjects = new ArrayList();
            currentRequiredObjects = new ArrayList();
            currentProhibitedObjects = new ArrayList();

            InitializeComponent();
        }

        private void path_Load(object sender, EventArgs e)
        {
            //If we have data given to us, fill out the forms
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
                areaBox.Items.Add(area);
                areaBox.SelectedIndex = 0;
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

            if (areas != null)
            {
                areaDestinationBox.Items.Clear();
                areaDestinationBox.Sorted = true;
                foreach (ParseObject o in areas)
                {
                    if (o["age"].ToString().Equals(age) &&
                        o["console"].ToString().Equals(console)
                        )
                    {
                        areaDestinationBox.Items.Add(o["name"].ToString());
                    }
                }
                if (areaDestination == null)
                {
                    if (areaDestinationBox.Items.Count > 0)
                    {
                        areaDestinationBox.SelectedIndex = areaDestinationBox.Items.IndexOf(area);
                    }
                }
                else
                {
                    areaDestinationBox.SelectedIndex = areaDestinationBox.Items.IndexOf(areaDestination);
                }
                
            }
            

            if (nodes != null)
            {
                nodeStartBox.Sorted = true;
                foreach (ParseObject o in nodes)
                {
                    if (o["age"].ToString().Equals(age) &&
                        o["console"].ToString().Equals(console) &&
                        o["area"].ToString().Equals(area)
                        )
                    {
                        nodeStartBox.Items.Add(o["name"].ToString());
                    }
                }
                if (nodeStart != null)
                {
                    nodeStartBox.SelectedIndex = nodeStartBox.Items.IndexOf(nodeStart);
                }
                
            }
            if (rupees != 0)
            {
                rupeeCounter.Value = rupees;
            }
            if (bombs != 0)
            {
                bombsCounter.Value = rupees;
            }
            if (bombchus != 0)
            {
                bombchusCounter.Value = rupees;
            }
            if (times.Count > 0)
            {
                timeBox.Sorted = true;
                foreach (string s in times)
                {
                    timeBox.Items.Add(s);
                }
            }
            if (timesOfDay.Count > 0)
            {
                timeOfDayBox.Sorted = true;
                foreach (string s in timesOfDay)
                {
                    timeOfDayBox.Items.Add(s);
                }
            }
            if (health != 0)
            {
                healthCounter.Value = health;
            }
            if (difficulty != null)
            {
                difficultyBox.SelectedIndex = difficultyBox.Items.IndexOf(difficulty);
            }
            if (currentObtainedObjects.Count > 0)
            {
                objectsObtainedBox.Sorted = true;
                foreach (string s in currentObtainedObjects)
                {
                    objectsObtainedBox.Items.Add(s);
                }
            }
            if (currentRequiredObjects.Count > 0)
            {
                requiredObjectsBox.Sorted = true;
                foreach (string s in currentRequiredObjects)
                {
                    requiredObjectsBox.Items.Add(s);
                }
            }
            if (currentProhibitedObjects.Count > 0)
            {
                prohibitedObjectsBox.Sorted = true;
                foreach (string s in currentProhibitedObjects)
                {
                    prohibitedObjectsBox.Items.Add(s);
                }
            }
            if (timeOfDayNumber != null)
            {
                timeOfDayCounter.Value = Decimal.Parse(timeOfDayNumber);
            }
            if (timeOfDay != null)
            {
                if (timeOfDay.Equals("Night"))
                {
                    nightRadio.Checked = true;
                    dayRadio.Checked = false;
                }
            }
            if (objects != null)
            {
                objectsAvailableBox.Sorted = true;
                requiredObjectsAvailableBox.Sorted = true;
                prohibitedObjectsAvailableBox.Sorted = true;
                objectsObtainedBox.Sorted = true;
                requiredObjectsBox.Sorted = true;
                prohibitedObjectsBox.Sorted = true;
                foreach (ParseObject o in objects)
                {
                    availableObtainedObjects.Add(o);
                    if (!objectsObtainedBox.Items.Contains(o["name"].ToString()))
                    {
                        objectsAvailableBox.Items.Add(o["name"].ToString());
                    }
                    availableRequiredObjects.Add(o);
                    if (!requiredObjectsBox.Items.Contains(o["name"].ToString()))
                    {
                        requiredObjectsAvailableBox.Items.Add(o["name"].ToString());
                    }
                    availableProhibitedObjects.Add(o);
                    if (!prohibitedObjectsBox.Items.Contains(o["name"].ToString()))
                    {
                        prohibitedObjectsAvailableBox.Items.Add(o["name"].ToString());
                    }
                }
            }
            updateDestinationNodes();
            if (nodeEnd != null)
            {
                nodeEndBox.SelectedIndex = nodeEndBox.Items.IndexOf(nodeEnd);
            }
        }
        //This function updates the destination nodes box
        public void updateDestinationNodes()
        {
            string destination = areaDestinationBox.SelectedItem.ToString();
            nodeEndBox.Items.Clear();
            nodeEndBox.Sorted = true;
            foreach (ParseObject o in nodes)
            {
                if (o["age"].ToString().Equals(age) &&
                        o["console"].ToString().Equals(console) &&
                        o["area"].ToString().Equals(destination)
                        )
                    {
                        nodeEndBox.Items.Add(o["name"].ToString());
                    }
            }
        }
        //Error check then return with the path data
        private void createPath_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //error checking
            //Name error checking
            if (nameBox.Text.ToString().Length <= 0)
            {
                MessageBox.Show("No name typed.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            //Check node boxes
            if (nodeStartBox.SelectedIndex == -1)
            {
                MessageBox.Show("Node start not selected.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (nodeEndBox.SelectedIndex == -1)
            {
                MessageBox.Show("Node end not selected.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (timeBox.Items.Count == 0)
            {
                MessageBox.Show("Please insert a time.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (difficultyBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a difficulty.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            //no errors, collect all the data
            name = nameBox.Text;
            areaDestination = areaDestinationBox.SelectedItem.ToString();
            nodeStart = nodeStartBox.SelectedItem.ToString();
            nodeEnd = nodeEndBox.SelectedItem.ToString();
            rupees = Int32.Parse(rupeeCounter.Value.ToString());
            bombs = Int32.Parse(bombsCounter.Value.ToString());
            bombchus = Int32.Parse(bombchusCounter.Value.ToString());
            times.Clear();
            foreach (string s in timeBox.Items)
            {
                times.Add(s);
            }
            timesOfDay.Clear();
            foreach (string s in timeOfDayBox.Items)
            {
                timesOfDay.Add(s);
            }
            health = Int32.Parse(healthCounter.Value.ToString());
            difficulty = difficultyBox.SelectedItem.ToString();
            currentObtainedObjects.Clear();
            foreach (string s in objectsObtainedBox.Items)
            {
                currentObtainedObjects.Add(s);
            }
            currentRequiredObjects.Clear();
            foreach (string s in requiredObjectsBox.Items)
            {
                currentRequiredObjects.Add(s);
            }
            currentProhibitedObjects.Clear();
            foreach (string s in prohibitedObjectsBox.Items)
            {
                currentProhibitedObjects.Add(s);
            }
            timeOfDayNumber = timeOfDayCounter.Value.ToString();
            if (dayRadio.Checked)
            {
                timeOfDay = "Day";
            }
            else
            {
                timeOfDay = "Night";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void areaDestinationBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDestinationNodes();
        }
        //Button to add the current time to the time boxes
        private void addTimeButton_Click(object sender, EventArgs e)
        {
            //Make sure time is input correctly
            if (timeCounter.Value == 0)
            {
                MessageBox.Show("Time is set to zero.", "Time Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            timeBox.Items.Add(timeCounter.Value.ToString());
            timeOfDayBox.Items.Add(timeOfDayCounter.Value.ToString());
            timeCounter.Value = 0;
            timeOfDayCounter.Value = 0;

        }
        //Button to remove a time from the time boxes
        private void removeTimeButton_Click(object sender, EventArgs e)
        {
            if (timeBox.SelectedIndex == -1)
            {
                MessageBox.Show("No time is selected.", "Time Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int index = timeBox.SelectedIndex;
            timeBox.Items.RemoveAt(index);
            timeOfDayBox.Items.RemoveAt(index);
        }
        //updates the time boxes
        private void updateTimes()
        {

        }

        private void timeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timeOfDayBox.SelectedIndex = timeBox.SelectedIndex;
        }

        private void timeOfDayBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timeBox.SelectedIndex = timeOfDayBox.SelectedIndex;
        }

        private void shiftObtainedLeft_Click(object sender, EventArgs e)
        {
            if (objectsObtainedBox.SelectedIndex == -1)
            {
                MessageBox.Show("No value selected to move left.", "Obtain Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Move the item over and select it
            objectsAvailableBox.Items.Add(objectsObtainedBox.SelectedItem.ToString());
            objectsAvailableBox.SelectedIndex = objectsAvailableBox.Items.IndexOf(objectsObtainedBox.SelectedItem);
            objectsObtainedBox.Items.Remove(objectsObtainedBox.SelectedItem);
        }

        private void shiftObtainedRight_Click(object sender, EventArgs e)
        {
            if (objectsAvailableBox.SelectedIndex == -1)
            {
                MessageBox.Show("No value selected to move right.", "Obtain Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Move the item over and select it
            objectsObtainedBox.Items.Add(objectsAvailableBox.SelectedItem.ToString());
            objectsObtainedBox.SelectedIndex = objectsObtainedBox.Items.IndexOf(objectsAvailableBox.SelectedItem);
            objectsAvailableBox.Items.Remove(objectsAvailableBox.SelectedItem);
        }

        private void shiftRequiredLeft_Click(object sender, EventArgs e)
        {
            if (requiredObjectsBox.SelectedIndex == -1)
            {
                MessageBox.Show("No value selected to move left.", "Required Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Move the item over and select it
            requiredObjectsAvailableBox.Items.Add(requiredObjectsBox.SelectedItem.ToString());
            requiredObjectsAvailableBox.SelectedIndex = requiredObjectsAvailableBox.Items.IndexOf(requiredObjectsBox.SelectedItem);
            requiredObjectsBox.Items.Remove(requiredObjectsBox.SelectedItem);
        }

        private void shiftRequiredRight_Click(object sender, EventArgs e)
        {
            if (requiredObjectsAvailableBox.SelectedIndex == -1)
            {
                MessageBox.Show("No value selected to move right.", "Required Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Move the item over and select it
            requiredObjectsBox.Items.Add(requiredObjectsAvailableBox.SelectedItem.ToString());
            requiredObjectsBox.SelectedIndex = requiredObjectsBox.Items.IndexOf(requiredObjectsAvailableBox.SelectedItem);
            requiredObjectsAvailableBox.Items.Remove(requiredObjectsAvailableBox.SelectedItem);
        }

        private void shiftProhibitedLeft_Click(object sender, EventArgs e)
        {
            if (prohibitedObjectsBox.SelectedIndex == -1)
            {
                MessageBox.Show("No value selected to move left.", "Prohibit Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Move the item over and select it
            prohibitedObjectsAvailableBox.Items.Add(prohibitedObjectsBox.SelectedItem.ToString());
            prohibitedObjectsAvailableBox.SelectedIndex = prohibitedObjectsAvailableBox.Items.IndexOf(prohibitedObjectsBox.SelectedItem);
            prohibitedObjectsBox.Items.Remove(prohibitedObjectsBox.SelectedItem);
        }

        private void shiftProhibitedRight_Click(object sender, EventArgs e)
        {
            if (prohibitedObjectsAvailableBox.SelectedIndex == -1)
            {
                MessageBox.Show("No value selected to move right.", "Prohibit Object Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Move the item over and select it
            prohibitedObjectsBox.Items.Add(prohibitedObjectsAvailableBox.SelectedItem.ToString());
            prohibitedObjectsBox.SelectedIndex = prohibitedObjectsBox.Items.IndexOf(prohibitedObjectsAvailableBox.SelectedItem);
            prohibitedObjectsAvailableBox.Items.Remove(prohibitedObjectsAvailableBox.SelectedItem);
        }

        //Updates the boxs so that it searchs for a key string
        public void updateBoxs(ArrayList obs)
        {
            objectsAvailableBox.Items.Clear();
            requiredObjectsAvailableBox.Items.Clear();
            prohibitedObjectsAvailableBox.Items.Clear();
            foreach (ParseObject o in obs)
            {
                if (!objectsObtainedBox.Items.Contains(o["name"].ToString()))
                {
                    objectsAvailableBox.Items.Add(o["name"].ToString());
                    objectsAvailableBox.Sorted = true;
                }
                if (!requiredObjectsBox.Items.Contains(o["name"].ToString()))
                {
                    requiredObjectsAvailableBox.Items.Add(o["name"].ToString());
                    requiredObjectsAvailableBox.Sorted = true;
                }
                if (!prohibitedObjectsBox.Items.Contains(o["name"].ToString()))
                {
                    prohibitedObjectsAvailableBox.Items.Add(o["name"].ToString());
                    prohibitedObjectsAvailableBox.Sorted = true;
                }
            }
        }

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
