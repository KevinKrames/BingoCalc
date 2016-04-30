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

    public partial class Form1 : Form
    {
        //Variables
        public const string versionNumber = "1.001";
        public const string updaterPrefix = "M1234_";
        private static string processToEnd = "bingocalc";
        private static string postProcess = Application.StartupPath + @"\" + updaterPrefix + processToEnd + ".exe";
        public static string updater = Application.StartupPath + @"\update.exe";
        public static List<string> info = new List<string>();

        public bool input = false;
        public bool refresh = false;

        public int ID;
        public ArrayList areas = new ArrayList();
        public ArrayList nodes = new ArrayList();
        public ArrayList paths = new ArrayList();
        public ArrayList objects = new ArrayList();
        public ParseObject DBID;
        public static string resourceDownloadFolder = Application.StartupPath + @"\";


        //Array lists
        public ArrayList transferArray;
        public bool transferBool;

        public Form1()
        {
            InitializeComponent();
            consoleBox.SelectedIndex = 1;
            transferArray = new ArrayList();
            transferBool = false;
            this.Text = "Bingo Calculator - Version " + versionNumber;

            //init local arraylists
            areas = new ArrayList();
            nodes = new ArrayList();
            paths = new ArrayList();
            objects = new ArrayList();

            //Load our array from disk if we have them
            loadLocalType("areas");
            loadLocalType("nodes");
            loadLocalType("paths");
            loadLocalType("objects");

            //Initialize our parse database
            ParseClient.Initialize(new ParseClient.Configuration
            {
                ApplicationId = Constants.databaseKey1,
                Server = Constants.databaseKey2
            });
            //Call an async function init so we can use await
            init();
        }
        //Initial function to sync the database and check for updates
        public async void init() {
            //disable input and check for an update
            disableInput();
            label1.Text = "Checking for Updates";
            await checkOnlineVersion();
            label1.Text = "Syncing Databases";

            ArrayList dl = new ArrayList();
            //Check DBID to see if theres any changes in the data
            await syncDBID(dl);
            if (dl.Count > 0)
            {
                //If dl has an item then we need to download the new data
                await downloadDatabase();
            }

            //Now our version + database is up to date
            label1.Text = "Up to Date - ID:" + ID;
            updateBoxs();
            enableInput();
        }
        //Called when the main form loads
        private void Form1_Load(object sender, EventArgs e)
        {
            //
            update.updateMe(updaterPrefix, Application.StartupPath + @"\");
            unpackCommandline();
        }

        
        //this is a the generic function that checks if our database is up to date.
        //This will call different functions based on the first item in the ArrayList data
        public async void checkDB(ArrayList data)
        {
            //Syncing database stuff
            disableInput();
            label1.Text = "Checking for Updates";
            await checkOnlineVersion();
            label1.Text = "Syncing Databases";

            ArrayList dl = new ArrayList();

            await syncDBID(dl);
            if (dl.Count > 0)
            {
                await downloadDatabase();
            }


            //Now that the DB is up to date, call the appropriate function according to data[0]
            if (data.Count > 0)
            {
                string state = (string)data[0];
                switch (state)
                {
                    case "addArea":
                        await addArea(data);
                        break;
                    case "editArea":
                        await editArea(data);
                        break;
                    case "deleteArea":
                        await deleteArea(data);
                        break;
                    case "addNode":
                        await addNode(data);
                        break;
                    case "editNode":
                        await editNode(data);
                        break;
                    case "deleteNode":
                        await deleteNode(data);
                        break;
                    case "addPath":
                        await addPath(data);
                        break;
                    case "editPath":
                        await editPath(data);
                        break;
                    case "deletePath":
                        await deletePath(data);
                        break;
                    case "addObject":
                        await addObject(data);
                        break;
                    case "editObject":
                        await editObject(data);
                        break;
                    case "deleteObject":
                        await deleteObject(data);
                        break;
                    default:
                        MessageBox.Show("Incorrect check DB input.", "Check Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                } // end of switch statement on data
            } //end error check for data.Count > 0
            else
            {
                //error
                MessageBox.Show("The update failed, please try again.", "update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            label1.Text = "Up to Date - ID:" + ID;
            updateBoxs();
            enableInput();
        }
        
        //uploads the current local DBID to the server
        private async Task uploadDBID()
        {
            ParseObject result = new ParseObject("DBID");
            try
            {
                //Query for the DBID on the server
                var query = from DBID in ParseObject.GetQuery("DBID")
                            where DBID.Get<string>("ID") != "-1"
                            select DBID;
                IEnumerable<ParseObject> results = await query.FindAsync();
                bool exists = false;
                //Get the first object in the results
                foreach (var obj in results)
                {
                    exists = true;
                    result = obj;
                    break;
                }
                if (exists == false)
                {
                    //The DBID doesnt exist
                    MessageBox.Show("For some reason DBID does not exist.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //We found the DBID on the server, update it with our DBID values
                    result["ID"] = Int32.Parse(DBID["ID"].ToString());
                    result["areas"] = Int32.Parse(DBID["areas"].ToString());
                    result["nodes"] = Int32.Parse(DBID["nodes"].ToString());
                    result["paths"] = Int32.Parse(DBID["paths"].ToString());
                    result["objects"] = Int32.Parse(DBID["objects"].ToString());
                    await result.SaveAsync();
                }
            }// end of try
            catch (Exception e)
            {
                MessageBox.Show("Upload DBID failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
        }

        //Downloads all data that is needed from the server
        //This may be optimized later to only download the things needed
        private async Task downloadDatabase()
        {
            //Download each item
            await downloadAreas();
            await downloadNodes();
            await downloadPaths();
            await downloadObjects();
            await downloadDBID();
        }
        //Downloads all the online areas and saves them locally
        private async Task downloadAreas()
        {
            ParseObject result;
            try
            {
                //Query the for the areas
                var query = from area in ParseObject.GetQuery("area")
                            where area.Get<string>("ID") != "-1"
                            select area;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Clear our array
                areas.Clear();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    result = obj;
                    areas.Add(result);
                }
                //Save the new areas locally
                saveLocalType("areas");
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading areas failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
        }
        //Downloads all the online nodes and saves them locally
        private async Task downloadNodes()
        {
            ParseObject result;
            try
            {
                //Query the nodes from the server
                var query = from node in ParseObject.GetQuery("node")
                            where node.Get<string>("ID") != "-1"
                            select node;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Clear our array
                nodes.Clear();
                //Add all the online nodes to our database
                foreach (var obj in results)
                {
                    result = obj;
                    nodes.Add(result);
                }
                //Save the new nodes locally
                saveLocalType("nodes");
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
        }
        //Downloads all the online paths and saves them locally
        private async Task downloadPaths()
        {
            ParseObject result;
            try
            {
                //Quer the online paths
                var query = from path in ParseObject.GetQuery("path")
                            where path.Get<string>("ID") != "-1"
                            select path;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Clear our array
                paths.Clear();
                //Add all the online paths to our database
                foreach (var obj in results)
                {
                    result = obj;
                    paths.Add(result);
                }
                //Save the new paths locally
                saveLocalType("paths");
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading paths failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
        }
        //Downloads all the online objects and saves them locally
        private async Task downloadObjects()
        {
            ParseObject result;
            try
            {
                //Query all of the online objects
                var query = from ob in ParseObject.GetQuery("object")
                            where ob.Get<string>("ID") != "-1"
                            select ob;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Clear our array
                objects.Clear();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    result = obj;
                    objects.Add(result);
                }
                //Save the new objects locally
                saveLocalType("objects");
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading objects failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
        }
        //Downloads the online DBID locally
        private async Task downloadDBID()
        {
            ParseObject result = new ParseObject("DBID");
            try
            {
                //Query for the online DBID
                var query = from DBID in ParseObject.GetQuery("DBID")
                            where DBID.Get<string>("ID") != "-1"
                            select DBID;
                IEnumerable<ParseObject> results = await query.FindAsync();
                bool exists = false;
                //Get the first object in the results
                foreach (var obj in results)
                {
                    exists = true;
                    result = obj;
                    break;
                }
                if (exists == false)
                {
                    //The DBID doesnt exist
                    MessageBox.Show("For some reason DBID does not exist.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Set our local DBID values
                    DBID["ID"] = result["ID"];
                    DBID["areas"] = result["areas"];
                    DBID["nodes"] = result["nodes"];
                    DBID["paths"] = result["paths"];
                    //Write it to disk
                    writeFile(getValues(DBID), "DBID.data");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Upload DBID failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
        }
        //Checks to see if the DBIDs are sync'd
        private async Task syncDBID(ArrayList dl)
        {
            ParseObject result = new ParseObject("DBID");
            try
            {
                //Query the online DBID
                var query = from DBID in ParseObject.GetQuery("DBID")
                            where DBID.Get<string>("ID") != "-1"
                            select DBID;
                IEnumerable<ParseObject> results = await query.FindAsync();
                bool exists = false;
                //Get the first object in the results
                foreach (var obj in results)
                {
                    exists = true;
                    result = obj;
                    break;
                }
                if (exists == false)
                {
                    //The DBID doesnt exist, upload ours
                    await DBID.SaveAsync();
                }
                else
                {
                    //If the DBID's are not equal:
                    if (!DBID["ID"].ToString().Equals(result["ID"].ToString()) ||
                        !DBID["areas"].ToString().Equals(result["areas"].ToString()) ||
                        !DBID["nodes"].ToString().Equals(result["nodes"].ToString()) ||
                        !DBID["paths"].ToString().Equals(result["paths"].ToString()) ||
                        !DBID["objects"].ToString().Equals(result["objects"].ToString()))
                    {
                        //Database is not sync'd, we need to sync it
                        //MessageBox.Show("database not synced", "Download Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dl.Add(true);
                    }
                    else
                    {
                        //Databases are synced
                        //MessageBox.Show("database synced", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The DBID download failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        //Checks the online version number of the program
        //If there is an update to the program it will prompt the user if they want to download it
        //The user will not be able to use the app until they download the new version
        private async Task checkOnlineVersion()
        {

            ParseObject result = new ParseObject("version");
            try
            {
                //Query the online database for the version number
                var query = from version in ParseObject.GetQuery("version")
                            where version.Get<string>("number") != "1"
                            select version;
                IEnumerable<ParseObject> results = await query.FindAsync();
                bool exists = false;
                //Get the first object in the results
                foreach (var obj in results)
                {
                    exists = true;
                    result = obj;
                    break;
                }
                if (exists == false)
                {
                    //The Version doesnt exist, Upload ours:
                    ParseObject version = new ParseObject("version");
                    version["number"] = versionNumber;
                    await version.SaveAsync();
                }
                else
                {

                    //If the versions are not equal:
                    if (!versionNumber.Equals(result["number"].ToString()))
                    {
                        //We need an update
                        //Create a form to make sure the user wants to update:
                        updateForm form = new updateForm();
                        form.StartPosition = FormStartPosition.CenterScreen;
                        DialogResult diag = form.ShowDialog();

                        if (diag == DialogResult.OK)
                        {
                            //Update the program
                            //create download folder if it does not exist
                            if (!Directory.Exists(resourceDownloadFolder))
                            {
                                Directory.CreateDirectory(resourceDownloadFolder);
                            }
                            //Store the info we need in order to download the new app in info
                            info.Add("bingocalc");
                            info.Add(result["number"].ToString());
                            info.Add("aDate");
                            info.Add(result["link"].ToString());
                            info.Add("update.zip");
                            //Actually download the update
                            bool updateChecked = webdata.downloadFromWeb(info[3], info[4], resourceDownloadFolder);
                            if (updateChecked)
                            {
                                //If we have the update then hand it off to the update app to handle the update
                                update.installUpdateRestart(info[3], info[4], "\"" + Application.StartupPath + "\\", processToEnd, postProcess, "updated", updater);
                                Close();
                            }
                            else
                            {
                                //Failed to update, end the program
                                MessageBox.Show("The version download failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Close();
                            }


                        }
                        else
                        {
                            //The user doesnt want to update
                            //Don't update and close the program
                            Close();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The version download failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Console.WriteLine("Exception: " + e.Message);
            }
            //version up to date now lets handle the DBID:
            DBID = new ParseObject("DBID");
            //Read our local DBID if it exists
            ArrayList DBIDValues = readFile("DBID.data");
            if (DBIDValues == null)
            {
                try
                {
                    //We dont have a DBID, make one:
                    ID = Int32.Parse(result["counter"].ToString());
                    DBID["ID"] = Int32.Parse(result["counter"].ToString());
                    DBID["areas"] = Int32.Parse(result["counter"].ToString());
                    DBID["nodes"] = Int32.Parse(result["counter"].ToString());
                    DBID["paths"] = Int32.Parse(result["counter"].ToString());
                    DBID["objects"] = Int32.Parse(result["counter"].ToString());
                    result["counter"] = Int32.Parse(result["counter"].ToString()) + 1;
                    //User the counter from the online DBID increment it so that everyone has a unique ID
                    //This could backfire if we run out of ints Kappa
                    await result.SaveAsync();
                    writeFile(getValues(DBID), "DBID.data");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Saving a new DBID failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    Console.WriteLine("Exception: " + e.Message);
                }

            }
            else
            {
                //DBID found, lets set the values of our DBID
                setValues(DBID, DBIDValues);
                ID = Int32.Parse(DBID["ID"].ToString());
            }

            

        }

        //unpacks command line arguments into temp string
        private void unpackCommandline()
        {
            bool commandPresent = false;
            string tempStr = "";

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (!commandPresent)
                {
                    commandPresent = arg.Trim().StartsWith("/");
                }
                if (commandPresent)
                {
                    tempStr += arg;
                }
            }
            if (commandPresent)
            {
                if (tempStr.Remove(0, 2) == "updated")
                {
                    //update successful
                }
            }
        }
        //Takes an argument of type and can save areas, nodes, paths or objects locally
        public void saveLocalType(string type)
        {
            //Loop through all the parse objects and save them into an array
            //Save that array to a local file
            ArrayList data = new ArrayList();
            ArrayList range = new ArrayList();
            //Sets the range array to the array we want to save
            switch (type)
            {
                case "areas":
                    range = areas;
                    break;
                case "nodes":
                    range = nodes;
                    break;
                case "paths":
                    range = paths;
                    break;
                case "objects":
                    range = objects;
                    break;
                default:

                    MessageBox.Show("Error saving local DB", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    break;
            }
            foreach (ParseObject o in range)
            {
                //Loop through each object in the range and add it to the data array
                ArrayList values = getValues(o);
                foreach (string s in values)
                {
                    data.Add(s);
                }
                //terminator symbol
                data.Add("~");
            }
            //Save the data array to disk
            writeFile(data, type + ".data");
        }
        //Takes an argument of type and can load areas, nodes, paths or objects locally
        public void loadLocalType(string type)
        {
            //Read the file from disk
            ArrayList data = readFile(type + ".data");
            ArrayList currentObject = new ArrayList();
            if (data == null)
            {
                return;
            }
            //Loop through each of the data strings
            foreach (string s in data)
            {
                if (s.Length > 0)
                {
                    //If the first character in the string is a tilda
                    if (s[0].Equals('~'))
                    {
                        //We know we have a new object
                        if (currentObject.Count > 0)
                        {
                            //Create the new parse object
                            ParseObject ob = new ParseObject(type.TrimEnd(type[type.Length - 1]));
                            //Set the values stored
                            setValues(ob, currentObject);
                            //add it to the appropriate array
                            switch (type)
                            {
                                case "areas":
                                    areas.Add(ob);
                                    break;
                                case "nodes":
                                    nodes.Add(ob);
                                    break;
                                case "paths":
                                    paths.Add(ob);
                                    break;
                                case "objects":
                                    objects.Add(ob);
                                    break;
                                default:
                                MessageBox.Show("Incorrect local load.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Close();
                                    break;
                            }
                        }
                        currentObject.Clear();
                    }
                    else
                    {
                        //No new object, add the value to the arraylist
                        currentObject.Add(s);
                    }
                }
                else
                {
                    //If it's null, add a null string
                    currentObject.Add("");
                }
            } 
        }

        //This function converts an arraylist of strings into a single string seperated by semicolons
        public string convertArrayListToString(ArrayList data)
        {
            string output = "";
            foreach (string s in data)
            {
                output += s;
                output += ";";
            }

            return output;
        }

        //This function converts a string of strings seperated by semicolons into an ArrayList
        public ArrayList convertStringToArrayList(string data)
        {
            ArrayList output = new ArrayList();
            int startIndex = 0;
            int indexCount = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == ';')
                {
                    // test1;test2;beepboop;
                    output.Add(data.Substring(startIndex, indexCount));
                    startIndex = i + 1;
                    indexCount = -1;
                }
                indexCount++;
            }
            return output;
        }

        //////////////////////////////////////////////////////////////////////////////
        //Reads a file from the folder which the program is located by string name. //
        //The arraylist items can be: string, String,                               //
        //bool, ArrayList(strings), byte[]                                          //
        //Inputs: ArrayList, String                                                 //
        //Outputs: File in executable directory.                                    //
        //////////////////////////////////////////////////////////////////////////////
        private ArrayList readFile(String fileName)
        {
            String line;
            ArrayList data = new ArrayList();
            try
            {
                //Check if the file doesnt exist
                if (!File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\files\\" + fileName))
                {
                    return null;
                }
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\files\\" + fileName);

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    bool done = false;
                    //convert the file
                    if (line.Length >= 5 && done == false)
                    {
                        if (line.Substring(0, 5) == "bool0")
                        {
                            done = true;
                            data.Add(false);
                        }
                        else if (line.Substring(0, 5) == "bool1")
                        {
                            done = true;
                            data.Add(true);
                        }
                    }
                    //check if it's an array list of string
                    /*if (line.Contains(';') == true && done == false)
                    {
                        done = true;
                        transferArray = new ArrayList();
                        transferBool = false;
                        int startIndex = 0;
                        int indexCount = 0;
                        for (int i = 0; i < line.Length; i++)
                        {

                            // test1;test2;beepboop
                            if (line[i] == ';')
                            {
                                transferArray.Add(line.Substring(startIndex, indexCount));
                                startIndex = i + 1;
                                indexCount = -1;
                            }
                            indexCount++;
                        }
                        if (line.Length > 0)
                        {
                            transferArray.Add(line.Substring(startIndex, indexCount));
                        }
                        data.Add(transferArray);
                    }*/
                    //Check if it's a byte[]
                    if (line.Length >= 6 && done == false)
                    {
                        if (line.Substring(0, 6) == "byte[]")
                        {
                            done = true;
                            data.Add(Convert.FromBase64String(line.Substring(6)));
                        }
                    }

                    //Normal string
                    if (line.Length > 0 && done == false)
                    {
                        data.Add(line);
                        done = true;
                    }

                    if (line.Length == 0)
                    {
                        data.Add("");
                        done = true;
                    }

                    if (done == false)
                    {
                        MessageBox.Show("Incorrect file type read.", "Read File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //write the lie to console window
                    //Console.WriteLine(line);

                    //Read the next line
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }
            return data;
        }
        ////////////////////////////////////////////////////
        //Writes a single file in the directory described //
        //The arraylist items can be: string, String,     //
        //bool, ArrayList(strings), byte[]                //
        //Inputs: ArrayList, String                       //
        //Outputs: File in executable directory.          //
        ////////////////////////////////////////////////////
        private void writeFile(ArrayList data, string fileName)
        {
            try
            {

                bool exists = System.IO.Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\files\\");

                if (!exists)
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "\\files\\");

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\files\\" + fileName);

                //Write the text file
                foreach (object obj in data)
                {
                    String dataString = "";
                    //if the object is a string:
                    if (obj.GetType() == typeof(string) || obj.GetType() == typeof(String))
                    {
                        dataString = (string)obj;
                    }
                    //if the object is a boolean:
                    else if (obj.GetType() == typeof(bool))
                    {
                        if ((bool)obj == true)
                        {
                            dataString = "bool1";
                        }
                        else
                        {
                            dataString = "bool0";
                        }
                    }
                    //if the object is a byte[]:
                    else if (obj.GetType() == typeof(byte[]))
                    {
                        dataString = "byte[]" + Convert.ToBase64String((byte[])obj);
                    }
                    //if the object is an ArrayList of strings
                    else if (obj.GetType() == typeof(ArrayList))
                    {
                        ArrayList tempArray = (ArrayList)obj;
                        foreach (String tempString in tempArray)
                        {
                            dataString = dataString + tempString;
                            dataString += ";";
                        }
                        if (tempArray.Count > 0)
                        {
                            dataString = dataString.Substring(0, dataString.Length - 1);
                        }
                    }
                    else
                    {
                        //incorrect file type
                        MessageBox.Show("Incorrect file type.", "Write File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sw.WriteLine(dataString);
                }

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }

        }

        //Converts ParseObject to ArrayList so that the even numbers are the keys
        //and the odd numbers are the values
        public ArrayList getValues(ParseObject o)
        {
            ArrayList data = new ArrayList();
            foreach (string s in o.Keys)
            {
                data.Add(s);
                data.Add(o[s].ToString());
            }

            return data;
        }
        //Loops through an array list and sets values to a given parse object
        //Inverse of getValues
        public void setValues(ParseObject o, ArrayList values)
        {
            string lastString = "";
            for (int i = 0; i < values.Count; i++)
            {
                if (i % 2 == 0)
                {
                    lastString = values[i].ToString();
                }
                else
                {
                    o[lastString] = values[i].ToString();
                }
            }
        }
        //unused
        private void label1_Click(object sender, EventArgs e)
        {

        }
        //Disables all the input functions in the main form
        public void disableInput()
        {
            input = false;
            consoleBox.Enabled = false;

            areasBox.Enabled = false;
            addAreaButton.Enabled = false;
            editAreaButton.Enabled = false;
            deleteAreaButton.Enabled = false;

            nodesBox.Enabled = false;
            addNodeButton.Enabled = false;
            editNodeButton.Enabled = false;
            deleteNodeButton.Enabled = false;

            pathsBox.Enabled = false;
            addPathButton.Enabled = false;
            editPathButton.Enabled = false;
            deletePathButton.Enabled = false;
        }
        //Enables all the input functions in the main form
        public void enableInput()
        {
            input = true;
            consoleBox.Enabled = true;

            areasBox.Enabled = true;
            addAreaButton.Enabled = true;
            editAreaButton.Enabled = true;
            deleteAreaButton.Enabled = true;

            nodesBox.Enabled = true;
            addNodeButton.Enabled = true;
            editNodeButton.Enabled = true;
            deleteNodeButton.Enabled = true;

            pathsBox.Enabled = true;
            addPathButton.Enabled = true;
            editPathButton.Enabled = true;
            deletePathButton.Enabled = true;
        }
        //Updates the area, nodes and paths boxs in the main form
        public void updateBoxs()
        {
            //Clear the boxs
            areasBox.Items.Clear();
            nodesBox.Items.Clear();
            pathsBox.Items.Clear();
            string console = consoleBox.SelectedItem.ToString();
            ArrayList child = new ArrayList();
            ArrayList adult = new ArrayList();
            //Update the areas box such that there is a child and adult section
            foreach (ParseObject o in areas)
            {
                if (o["console"].ToString().Equals(console) && o["age"].ToString().Equals("Child"))
                {
                    child.Add(o["name"].ToString());
                }
                if (o["console"].ToString().Equals(console) && o["age"].ToString().Equals("Adult"))
                {
                    adult.Add(o["name"].ToString());
                }
            }
            adult.Sort();
            child.Sort();
            areasBox.Items.Add("-- ADULT --");
            foreach (string name in adult)
            {
                areasBox.Items.Add(name);
            }
            areasBox.Items.Add("-- CHILD --");
            foreach (string name in child)
            {
                areasBox.Items.Add(name);
            }
            
        }
        //Seperate function for updating the nodes and paths boxes
        private void updateNodesAndPaths()
        {
            nodesBox.Items.Clear();
            pathsBox.Items.Clear();
            if (areasBox.SelectedItem == null)
            {
                //No area selected, return
                return;
            }
            //Get the values of the area selected: console, area and age
            string console = consoleBox.SelectedItem.ToString();
            string area = areasBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
            }
            //Get the nodes based on the values
            foreach (ParseObject o in nodes)
            {
                if (o["area"].ToString().Equals(area) &&
                    o["age"].ToString().Equals(age) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    nodesBox.Items.Add(o["name"].ToString());
                }
            }
            nodesBox.Sorted = true;
            //Get the paths based on the values
            foreach (ParseObject o in paths)
            {
                if (o["area"].ToString().Equals(area) &&
                    o["age"].ToString().Equals(age) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    pathsBox.Items.Add(o["name"].ToString());
                }
            }
            pathsBox.Sorted = true;
        }
        //Function that is called when the addAreaButton is clicked
        private void addAreaButton_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            addArea form = new addArea();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Add Area";
            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the area
                ArrayList data = new ArrayList();
                data.Add("addArea");
                data.Add(form.name);
                data.Add(form.age);
                data.Add(form.console);
                //Actually add the area
                checkDB(data);
            }
            else
            {
                //Cancel the add area

            }
        }
        //This function gets called when the databases are synced and we want to add the area
        private async Task addArea(ArrayList data)
        {
            if (data.Count < 4)
            {
                //error
                MessageBox.Show("Adding area failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new area
            string name = (string)data[1];
            string age = (string)data[2];
            string console = (string)data[3];
            ParseObject newArea = new ParseObject("area");
            newArea["ID"] = ID;
            newArea["name"] = name;
            newArea["age"] = age;
            newArea["console"] = console;
            await newArea.SaveAsync();
            areas.Add(newArea);
            //Save new area locally
            saveLocalType("areas");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["areas"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }
        //Function that is called when the editAreaButton is clicked
        private void editAreaButton_Click(object sender, EventArgs e)
        {
            if (areasBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an area to edit.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //First check which item we selected
            string name = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            //Create a form to edit an area:
            addArea form = new addArea();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Edit Area";
            form.name = name;
            form.console = console;
            form.age = age;
            form.button = "Edit";
            
            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the area
                ArrayList data = new ArrayList();
                data.Add("editArea");
                data.Add(form.name);
                data.Add(form.age);
                data.Add(form.console);
                data.Add(name);
                //Actually add the area
                checkDB(data);
            }
            else
            {
                //Cancel the add area

            }
        }
        //This function gets called when the databases are synced and we want to edit the area
        private async Task editArea(ArrayList data)
        {
            if (data.Count < 5)
            {
                //error
                MessageBox.Show("Editing area failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new area
            string name = (string)data[1];
            string age = (string)data[2];
            string console = (string)data[3];
            string oldName = (string)data[4];

            //First update all nodes to change the area name
            ArrayList nodesToUpdate = new ArrayList();
            try
            {
                //If an online nodes exists
                var query = from node in ParseObject.GetQuery("node")
                            where node.Get<string>("ID") != "-1"
                            select node;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    if (obj["age"].ToString().Equals(age) &&
                        obj["area"].ToString().Equals(oldName) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        nodesToUpdate.Add(obj);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            //We have the paths we want to update, now update them
            foreach (ParseObject o in nodesToUpdate)
            {
                //First fix our local paths
                ParseObject remove2 = new ParseObject("node");
                foreach (ParseObject o2 in nodes)
                {
                    if (o2["name"].ToString().Equals(o["name"].ToString()) &&
                        o2["age"].ToString().Equals(o["age"].ToString()) &&
                        o2["area"].ToString().Equals(o["area"].ToString()) &&
                        o2["console"].ToString().Equals(o["console"].ToString())
                        )
                    {
                        remove2 = o2;
                    }
                }
                nodes.Remove(remove2);

                //Now fix our server paths
                o["area"] = name;
                await o.SaveAsync();
                //Make sure to add it back to the local Database
                nodes.Add(o);
            }
            saveLocalType("nodes");

            //Second update all paths to change the area name
            ArrayList pathsToUpdate = new ArrayList();
            try
            {
                //Get the online paths
                var query = from path in ParseObject.GetQuery("path")
                            where path.Get<string>("ID") != "-1"
                            select path;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the online objects to our array
                foreach (var obj in results)
                {
                    if (obj["age"].ToString().Equals(age) &&
                        (obj["area"].ToString().Equals(oldName) || obj["areaDestination"].ToString().Equals(oldName)) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        pathsToUpdate.Add(obj);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            //We have the paths we want to update, now update them
            foreach (ParseObject o in pathsToUpdate)
            {
                //First fix our local paths
                ParseObject remove2 = new ParseObject("path");
                foreach (ParseObject o2 in paths)
                {
                    if (o2["name"].ToString().Equals(o["name"].ToString()) &&
                        o2["age"].ToString().Equals(o["age"].ToString()) &&
                        o2["area"].ToString().Equals(o["area"].ToString()) &&
                        o2["console"].ToString().Equals(o["console"].ToString())
                        )
                    {
                        remove2 = o2;
                    }
                }
                paths.Remove(remove2);

                //Now fix our server paths
                if (o["area"].ToString().Equals(oldName))
                {
                    o["area"] = name;
                }
                if (o["areaDestination"].ToString().Equals(oldName))
                {
                    o["areaDestination"] = name;
                }
                await o.SaveAsync();
                //Make sure to add it back to the local Database
                paths.Add(o);
            }
            saveLocalType("paths");

            //Now update the area to have the new name
            ParseObject result = new ParseObject("area");
            try
            {
                //If an online nodes exists
                var query = from area in ParseObject.GetQuery("area")
                            where area.Get<string>("ID") != "-1"
                            select area;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(oldName) &&
                        obj["age"].ToString().Equals(age) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        result = obj;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading areas failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject removeArea = new ParseObject("area");
            foreach (ParseObject o in areas)
            {
                if (o["name"].ToString().Equals(oldName) &&
                    o["age"].ToString().Equals(age) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    removeArea = o;
                }
            }
            areas.Remove(removeArea);

            result["ID"] = ID;
            result["name"] = name;

            await result.SaveAsync();
            areas.Add(result);
            //Save new areas locally
            saveLocalType("areas");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["areas"] = ID;
            DBID["nodes"] = ID;
            DBID["paths"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");
        }
        //Function that is called when the deleteAreaButton is clicked
        private void deleteAreaButton_Click(object sender, EventArgs e)
        {
            //Error checking:
            if (areasBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an area to delete.", "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (ParseObject o in nodes)
            {
                if (o["area"].ToString().Equals(areasBox.SelectedItem.ToString()))
                {
                    MessageBox.Show("You must remove this area from the following node before deleting it: " + o["name"].ToString(), "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            foreach (ParseObject o in paths)
            {
                if (o["area"].ToString().Equals(areasBox.SelectedItem.ToString()) ||
                    o["areaDestination"].ToString().Equals(areasBox.SelectedItem.ToString())
                    )
                {
                    MessageBox.Show("You must remove this area from the following path before deleting it: " + o["name"].ToString(), "Area Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Get the data we need to delete this path
            string name = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            confirmation form = new confirmation();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Delete Area";
            form.message = "Delete " + name;
            form.label = "To delete this node type \"" + form.message + "\" without quotes.";

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the area
                ArrayList data = new ArrayList();
                data.Add("deleteArea");
                data.Add(name);
                data.Add(age);
                data.Add(console);

                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }
        //This function gets called when the databases are synced and we want to delete the area
        private async Task deleteArea(ArrayList data)
        {
            if (data.Count < 4)
            {
                //error
                MessageBox.Show("Deleting Area failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Delete the new path
            string name = (string)data[1];
            string age = (string)data[2];
            string console = (string)data[3];

            ParseObject result = new ParseObject("area");
            try
            {
                //Get online area
                var query = from area in ParseObject.GetQuery("area")
                            where area.Get<string>("ID") != "-1"
                            select area;
                IEnumerable<ParseObject> results = await query.FindAsync();
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(name) &&
                        obj["age"].ToString().Equals(age) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        result = obj;
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading areas failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject remove = new ParseObject("area");
            foreach (ParseObject o in areas)
            {
                if (o["name"].ToString().Equals(name) &&
                    o["age"].ToString().Equals(age) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    remove = o;
                }
            }
            areas.Remove(remove);

            await result.DeleteAsync();
            //Save new area locally
            saveLocalType("areas");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["areas"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");
        }
        //Function that is called when the addNodeButton is clicked
        private void addNodeButton_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            node form = new node();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Add Node";

            //Error checking:
            if (areasBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an area to add a node to.", "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //send the area data
            string area = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            form.area = area;
            form.console = console;
            form.age = age;

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the node
                ArrayList data = new ArrayList();
                data.Add("addNode");
                data.Add(form.name);
                data.Add(form.area);
                data.Add(form.age);
                data.Add(form.console);
                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }
        //This function gets called when the databases are synced and we want to add the node
        private async Task addNode(ArrayList data)
        {
            if (data.Count < 5)
            {
                //error
                MessageBox.Show("Adding Node failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new area
            string name = (string)data[1];
            string area = (string)data[2];
            string age = (string)data[3];
            string console = (string)data[4];
            ParseObject newNode = new ParseObject("node");
            newNode["ID"] = ID;
            newNode["name"] = name;
            newNode["area"] = area;
            newNode["age"] = age;
            newNode["console"] = console;
            await newNode.SaveAsync();
            nodes.Add(newNode);
            //Save new area locally
            saveLocalType("nodes");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["nodes"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }
        //Function that is called when the editNodeButton is clicked
        private void editNodeButton_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            node form = new node();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Edit Node";

            //Error checking:
            if (areasBox.SelectedItem == null || nodesBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an area to add a node to.", "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //get the area data
            string name = nodesBox.SelectedItem.ToString();
            string area = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }
            //send the area data to the form
            form.name = name;
            form.area = area;
            form.console = console;
            form.age = age;
            form.button = "Edit";

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the node
                ArrayList data = new ArrayList();
                data.Add("editNode");
                data.Add(form.name);
                data.Add(form.area);
                data.Add(form.age);
                data.Add(form.console);
                data.Add(name);
                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }
        //This function gets called when the databases are synced and we want to edit the node
        private async Task editNode(ArrayList data)
        {
            if (data.Count < 6)
            {
                //error
                MessageBox.Show("Editing Node failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new node

            string name = (string)data[1];
            string area = (string)data[2];
            string age = (string)data[3];
            string console = (string)data[4];
            string oldName = (string)data[5];

            //First update all paths to change the nodes name
            ArrayList pathsToUpdate = new ArrayList();
            try
            {
                //Query the online paths
                var query = from path in ParseObject.GetQuery("path")
                            where path.Get<string>("ID") != "-1"
                            select path;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //if we need to update the path add it to the array
                foreach (var obj in results)
                {
                    if (obj["age"].ToString().Equals(age) &&
                        obj["area"].ToString().Equals(area) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        if (obj["nodeStart"].ToString().Equals(oldName) || obj["nodeEnd"].ToString().Equals(oldName))
                        {
                            pathsToUpdate.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            //We have the paths we want to update, now update them
            foreach (ParseObject o in pathsToUpdate)
            {
                //First fix our local paths
                ParseObject remove2 = new ParseObject("path");
                foreach (ParseObject o2 in paths)
                {
                    if (o2["name"].ToString().Equals(o["name"].ToString()) &&
                        o2["age"].ToString().Equals(o["age"].ToString()) &&
                        o2["area"].ToString().Equals(o["area"].ToString()) &&
                        o2["console"].ToString().Equals(o["console"].ToString())
                        )
                    {
                        remove2 = o2;
                    }
                }
                paths.Remove(remove2);

                //Now fix our server paths
                if (o["nodeStart"].ToString().Equals(oldName))
                {
                    o["nodeStart"] = name;
                }
                if (o["nodeEnd"].ToString().Equals(oldName))
                {
                    o["nodeEnd"] = name;
                }
                await o.SaveAsync();
                //Make sure to add it back to the local Database
                paths.Add(o);
            }
            saveLocalType("paths");

            //Now update the node to have the new name
            ParseObject result = new ParseObject("node");
            try
            {
                //If an online nodes exists
                var query = from node in ParseObject.GetQuery("node")
                            where node.Get<string>("ID") != "-1"
                            select node;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(oldName) &&
                        obj["age"].ToString().Equals(age) &&
                        obj["area"].ToString().Equals(area) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        result = obj;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject removeNode = new ParseObject("node");
            foreach (ParseObject o in nodes)
            {
                if (o["name"].ToString().Equals(oldName) &&
                    o["age"].ToString().Equals(age) &&
                    o["area"].ToString().Equals(area) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    removeNode = o;
                }
            }
            nodes.Remove(removeNode);

            result["ID"] = ID;
            result["name"] = name;

            await result.SaveAsync();
            nodes.Add(result);
            //Save new area locally
            saveLocalType("nodes");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["nodes"] = ID;
            DBID["paths"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");
        }
        //Function that is called when the deleteNodeButton is clicked
        private void deleteNodeButton_Click(object sender, EventArgs e)
        {
            //Error checking:
            if (areasBox.SelectedItem == null || nodesBox.SelectedItem == null)
            {
                MessageBox.Show("Must select a node to delete.", "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (ParseObject o in paths)
            {
                if (o["nodeStart"].ToString().Equals(nodesBox.SelectedItem.ToString()) ||
                    o["nodeEnd"].ToString().Equals(nodesBox.SelectedItem.ToString())
                    )
                {
                    MessageBox.Show("You must remove this node from the following path before deleting it: " + o["name"].ToString(), "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //Get the data we need to delete this path
            string name = nodesBox.SelectedItem.ToString();
            string area = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            confirmation form = new confirmation();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Delete Node";
            form.message = "Delete " + name;
            form.label = "To delete this node type \"" + form.message + "\" without quotes.";

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the node
                ArrayList data = new ArrayList();
                data.Add("deleteNode");
                data.Add(name);
                data.Add(area);
                data.Add(age);
                data.Add(console);

                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }

        }
        //This function gets called when the databases are synced and we want to remove the node
        private async Task deleteNode(ArrayList data)
        {
            if (data.Count < 5)
            {
                //error
                MessageBox.Show("Deleting Node failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Delete the path

            string name = (string)data[1];
            string area = (string)data[2];
            string age = (string)data[3];
            string console = (string)data[4];

            ParseObject result = new ParseObject("node");
            try
            {
                //Query the database for nodes
                var query = from node in ParseObject.GetQuery("node")
                            where node.Get<string>("ID") != "-1"
                            select node;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Find the node we are looking for
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(name) &&
                        obj["age"].ToString().Equals(age) &&
                        obj["area"].ToString().Equals(area) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        result = obj;
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject remove = new ParseObject("node");
            foreach (ParseObject o in nodes)
            {
                if (o["name"].ToString().Equals(name) &&
                    o["age"].ToString().Equals(age) &&
                    o["area"].ToString().Equals(area) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    remove = o;
                }
            }
            nodes.Remove(remove);

            await result.DeleteAsync();
            //Save new area locally
            saveLocalType("nodes");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["nodes"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }
        //Function that is called when the addPathButton is clicked
        private void addPathButton_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            path form = new path();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Add Path";

            //Error checking:
            if (areasBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an area to add a path to.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //send the area data
            string area = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            form.area = area;
            form.console = console;
            form.age = age;
            form.nodes = nodes;
            form.areas = areas;
            form.objects = objects;

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the path
                ArrayList data = new ArrayList();
                data.Add("addPath");
                data.Add(form.name);
                data.Add(form.area);
                data.Add(form.age);
                data.Add(form.console);
                data.Add(form.areaDestination);
                data.Add(form.nodeStart);
                data.Add(form.nodeEnd);
                data.Add(form.rupees);
                data.Add(form.bombs);
                data.Add(form.bombchus);
                data.Add(convertArrayListToString(form.times));
                data.Add(convertArrayListToString(form.timesOfDay));
                data.Add(form.health);
                data.Add(form.difficulty);
                data.Add(convertArrayListToString(form.currentObtainedObjects));
                data.Add(convertArrayListToString(form.currentRequiredObjects));
                data.Add(convertArrayListToString(form.currentProhibitedObjects));
                data.Add(form.timeOfDay);
                data.Add(form.timeOfDayNumber);

                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }
        //This function gets called when the databases are synced and we want to add the path
        private async Task addPath(ArrayList data)
        {
            if (data.Count < 20)
            {
                //error
                MessageBox.Show("Adding Path failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new path
            string name = (string)data[1];
            string area = (string)data[2];
            string age = (string)data[3];
            string console = (string)data[4];
            string areaDestination = (string)data[5];
            string nodeStart = (string)data[6];
            string nodeEnd = (string)data[7];
            int rupees = (int)data[8];
            int bombs = (int)data[9];
            int bombchus = (int)data[10];
            string times = (string)data[11];
            string timesOfDay = (string)data[12];
            int health = (int)data[13];
            string difficulty = (string)data[14];
            string currentObtainedObjects = (string)data[15];
            string currentRequiredObjects = (string)data[16];
            string currentProhibitedObjects = (string)data[17];
            string timeOfDay = (string)data[18];
            string timeOfDayNumber = (string)data[19];

            ParseObject newPath = new ParseObject("path");
            newPath["ID"] = ID;
            newPath["name"] = name;
            newPath["area"] = area;
            newPath["age"] = age;
            newPath["console"] = console;
            newPath["areaDestination"] = areaDestination;
            newPath["nodeStart"] = nodeStart;
            newPath["nodeEnd"] = nodeEnd;
            newPath["rupees"] = rupees;
            newPath["bombs"] = bombs;
            newPath["bombchus"] = bombchus;
            newPath["times"] = times;
            newPath["timesOfDay"] = timesOfDay;
            newPath["health"] = health;
            newPath["difficulty"] = difficulty;
            newPath["currentObtainedObjects"] = currentObtainedObjects;
            newPath["currentRequiredObjects"] = currentRequiredObjects;
            newPath["currentProhibitedObjects"] = currentProhibitedObjects;
            newPath["timeOfDay"] = timeOfDay;
            newPath["timeOfDayNumber"] = timeOfDayNumber;

            await newPath.SaveAsync();
            paths.Add(newPath);
            //Save new area locally
            saveLocalType("paths");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["paths"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }
        //Function that is called when the editPathButton is clicked
        private void editPathButton_Click(object sender, EventArgs e)
        {
            //error checking
            if (pathsBox.SelectedIndex == -1)
            {
                MessageBox.Show("Must select a path edit.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Create a form to add an area:
            path form = new path();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Edit Path";

            //Error checking:
            if (areasBox.SelectedItem == null)
            {
                MessageBox.Show("Must select an area to add a path to.", "Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //send the area data
            string name = pathsBox.SelectedItem.ToString();
            string area = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            form.area = area;
            form.console = console;
            form.age = age;
            form.nodes = nodes;
            form.areas = areas;
            form.objects = objects;
            ParseObject path = new ParseObject("path");

            foreach (ParseObject o in paths)
            {
                if (o["name"].ToString().Equals(name) &&
                    o["area"].ToString().Equals(area) &&
                    o["age"].ToString().Equals(age) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    path = o;
                    break;
                }
            }

            form.name = name;
            form.areaDestination = path["areaDestination"].ToString();
            form.nodeStart = path["nodeStart"].ToString();
            form.nodeEnd = path["nodeEnd"].ToString();
            form.rupees = Int32.Parse(path["rupees"].ToString());
            form.bombs = Int32.Parse(path["bombs"].ToString());
            form.bombchus = Int32.Parse(path["bombchus"].ToString());
            form.times = convertStringToArrayList(path["times"].ToString());
            form.timesOfDay = convertStringToArrayList(path["timesOfDay"].ToString());
            form.health = Int32.Parse(path["health"].ToString());
            form.difficulty = path["difficulty"].ToString();
            form.currentObtainedObjects = convertStringToArrayList(path["currentObtainedObjects"].ToString());
            form.currentRequiredObjects = convertStringToArrayList(path["currentRequiredObjects"].ToString());
            form.currentProhibitedObjects = convertStringToArrayList(path["currentProhibitedObjects"].ToString());
            form.timeOfDay = path["timeOfDay"].ToString();
            form.timeOfDayNumber = path["timeOfDayNumber"].ToString();
            form.button = "Edit";


            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the path
                ArrayList data = new ArrayList();
                data.Add("editPath");
                data.Add(form.name);
                data.Add(form.area);
                data.Add(form.age);
                data.Add(form.console);
                data.Add(form.areaDestination);
                data.Add(form.nodeStart);
                data.Add(form.nodeEnd);
                data.Add(form.rupees);
                data.Add(form.bombs);
                data.Add(form.bombchus);
                data.Add(convertArrayListToString(form.times));
                data.Add(convertArrayListToString(form.timesOfDay));
                data.Add(form.health);
                data.Add(form.difficulty);
                data.Add(convertArrayListToString(form.currentObtainedObjects));
                data.Add(convertArrayListToString(form.currentRequiredObjects));
                data.Add(convertArrayListToString(form.currentProhibitedObjects));
                data.Add(form.timeOfDay);
                data.Add(form.timeOfDayNumber);
                data.Add(path);

                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }


        }
        //This function gets called when the databases are synced and we want to edit the path
        private async Task editPath(ArrayList data)
        {
            if (data.Count < 21)
            {
                //error
                MessageBox.Show("Editing Path failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new path
            string name = (string)data[1];
            string area = (string)data[2];
            string age = (string)data[3];
            string console = (string)data[4];
            string areaDestination = (string)data[5];
            string nodeStart = (string)data[6];
            string nodeEnd = (string)data[7];
            int rupees = (int)data[8];
            int bombs = (int)data[9];
            int bombchus = (int)data[10];
            string times = (string)data[11];
            string timesOfDay = (string)data[12];
            int health = (int)data[13];
            string difficulty = (string)data[14];
            string currentObtainedObjects = (string)data[15];
            string currentRequiredObjects = (string)data[16];
            string currentProhibitedObjects = (string)data[17];
            string timeOfDay = (string)data[18];
            string timeOfDayNumber = (string)data[19];
            ParseObject oldPath = (ParseObject)data[20];

            ParseObject result = new ParseObject("path");
            try
            {
                //Query the online paths
                var query = from path in ParseObject.GetQuery("path")
                            where path.Get<string>("ID") != "-1"
                            select path;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(oldPath["name"].ToString()) &&
                        obj["age"].ToString().Equals(oldPath["age"].ToString()) &&
                        obj["area"].ToString().Equals(oldPath["area"].ToString()) &&
                        obj["console"].ToString().Equals(oldPath["console"].ToString()) 
                        )
                    {
                        result = obj;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading paths failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject remove = new ParseObject("path");
            foreach (ParseObject o in paths)
            {
                if (o["name"].ToString().Equals(oldPath["name"].ToString()) &&
                    o["age"].ToString().Equals(oldPath["age"].ToString()) &&
                    o["area"].ToString().Equals(oldPath["area"].ToString()) &&
                    o["console"].ToString().Equals(oldPath["console"].ToString())
                    )
                {
                    remove = o;
                }
            }
            paths.Remove(remove);

            result["ID"] = ID;
            result["name"] = name;
            result["area"] = area;
            result["age"] = age;
            result["console"] = console;
            result["areaDestination"] = areaDestination;
            result["nodeStart"] = nodeStart;
            result["nodeEnd"] = nodeEnd;
            result["rupees"] = rupees;
            result["bombs"] = bombs;
            result["bombchus"] = bombchus;
            result["times"] = times;
            result["timesOfDay"] = timesOfDay;
            result["health"] = health;
            result["difficulty"] = difficulty;
            result["currentObtainedObjects"] = currentObtainedObjects;
            result["currentRequiredObjects"] = currentRequiredObjects;
            result["currentProhibitedObjects"] = currentProhibitedObjects;
            result["timeOfDay"] = timeOfDay;
            result["timeOfDayNumber"] = timeOfDayNumber;

            await result.SaveAsync();
            paths.Add(result);
            //Save new area locally
            saveLocalType("paths");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["paths"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }
        //Function that is called when the deletePathButton is clicked
        private void deletePathButton_Click(object sender, EventArgs e)
        {
            //Error checking:
            if (areasBox.SelectedItem == null || pathsBox.SelectedItem == null)
            {
                MessageBox.Show("Must select a path to delete.", "Node Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Get the data we need to delete this path
            string name = pathsBox.SelectedItem.ToString();
            string area = areasBox.SelectedItem.ToString();
            string console = consoleBox.SelectedItem.ToString();
            string age = "Adult";
            for (int i = 0; i < areasBox.Items.Count; i++)
            {
                if (areasBox.SelectedIndex == i)
                {
                    break;
                }
                if (areasBox.Items[i].ToString().Equals("-- CHILD --"))
                {
                    age = "Child";
                    break;
                }
            }

            confirmation form = new confirmation();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Delete Path";
            form.message = "Delete " + name;
            form.label = "To delete this path type \"" + form.message + "\" without quotes.";

            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                //Dialog returned OK
                //Get the data from the path
                ArrayList data = new ArrayList();
                data.Add("deletePath");
                data.Add(name);
                data.Add(area);
                data.Add(age);
                data.Add(console);

                //Confirm database then add item
                checkDB(data);
            }
            else
            {
                //Cancel the add item

            }
        }
        //This function gets called when the databases are synced and we want to delete the path
        private async Task deletePath(ArrayList data)
        {
            if (data.Count < 5)
            {
                //error
                MessageBox.Show("Deleting Path failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Delete the new path
            string name = (string)data[1];
            string area = (string)data[2];
            string age = (string)data[3];
            string console = (string)data[4];

            ParseObject result = new ParseObject("path");
            try
            {
                //Query the online paths
                var query = from path in ParseObject.GetQuery("path")
                            where path.Get<string>("ID") != "-1"
                            select path;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Find the path we are looking for
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(name) &&
                        obj["age"].ToString().Equals(age) &&
                        obj["area"].ToString().Equals(area) &&
                        obj["console"].ToString().Equals(console)
                        )
                    {
                        result = obj;
                        break;
                    }
                        
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading paths failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject remove = new ParseObject("path");
            foreach (ParseObject o in paths)
            {
                if (o["name"].ToString().Equals(name) &&
                    o["age"].ToString().Equals(age) &&
                    o["area"].ToString().Equals(area) &&
                    o["console"].ToString().Equals(console)
                    )
                {
                    remove = o;
                }
            }
            paths.Remove(remove);

            await result.DeleteAsync();
            //Save new area locally
            saveLocalType("paths");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["paths"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }
        //This function gets called when the databases are synced and we want to add the object
        private async Task addObject(ArrayList data)
        {
            updateBoxs();
            if (data.Count < 2)
            {
                //error
                MessageBox.Show("Adding Object failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Upload the new area
            string name = (string)data[1];
            ParseObject newObject = new ParseObject("object");
            newObject["ID"] = ID;
            newObject["name"] = name;
            await newObject.SaveAsync();
            objects.Add(newObject);
            //Save new area locally
            saveLocalType("objects");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["objects"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");
            updateBoxs();

        }
        //This function gets called when the databases are synced and we want to edit the object
        private async Task editObject(ArrayList data)
        {
            if (data.Count < 3)
            {
                //error
                MessageBox.Show("Editing Object failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Get the old and new name of the object

            string name = (string)data[1];
            string oldName = (string)data[2];

            //First update all paths to change the objects name
            ArrayList pathsToUpdate = new ArrayList();
            try
            {
                //Get all paths from online DB
                var query = from path in ParseObject.GetQuery("path")
                            where path.Get<string>("ID") != "-1"
                            select path;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the paths array for updating
                foreach (var obj in results)
                {
                    if (obj["currentObtainedObjects"].ToString().Contains(oldName) ||
                        obj["currentRequiredObjects"].ToString().Contains(oldName) ||
                        obj["currentProhibitedObjects"].ToString().Contains(oldName)
                        )
                    {
                        pathsToUpdate.Add(obj);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading nodes failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            //We have the paths we want to update, now update them
            foreach (ParseObject o in pathsToUpdate)
            {
                //Flag to remove the path from our databases
                bool flag = false;
                ArrayList obtained = convertStringToArrayList(o["currentObtainedObjects"].ToString());
                string stringToRemove = null;
                foreach (string s in obtained)
                {
                    //Remove our string and add the new one
                    if (s.Equals(oldName))
                    {
                        stringToRemove = s;
                    }
                }
                //If the string is inside the array, remove it and add the new one
                if (stringToRemove != null)
                {
                    flag = true;
                    obtained.Remove(stringToRemove);
                    obtained.Add(name);
                    o["currentObtainedObjects"] = convertArrayListToString(obtained);
                }

                ArrayList required = convertStringToArrayList(o["currentRequiredObjects"].ToString());
                stringToRemove = null;
                foreach (string s in required)
                {
                    //Remove our string and add the new one
                    if (s.Equals(oldName))
                    {
                        stringToRemove = s;
                    }
                }
                //If the string is inside the array, remove it and add the new one
                if (stringToRemove != null)
                {
                    flag = true;
                    required.Remove(stringToRemove);
                    required.Add(name);
                }
                o["currentRequiredObjects"] = convertArrayListToString(required);

                ArrayList prohibited = convertStringToArrayList(o["currentProhibitedObjects"].ToString());
                stringToRemove = null;
                foreach (string s in prohibited)
                {
                    //Remove our string and add the new one
                    if (s.Equals(oldName))
                    {
                        stringToRemove = s;
                    }
                }
                //If the string is inside the array, remove it and add the new one
                if (stringToRemove != null)
                {
                    flag = true;
                    prohibited.Remove(stringToRemove);
                    prohibited.Add(name);
                }
                o["currentProhibitedObjects"] = convertArrayListToString(prohibited);

                //If there was a change to the objects, flag is true
                //Then we need to update our databases
                if (flag == true)
                {
                    //Okay arrays are updated on these objects
                    //Remove the old objects from our paths array and add this new parse object
                    ParseObject remove2 = new ParseObject("path");
                    foreach (ParseObject o2 in paths)
                    {
                        if (o2["name"].ToString().Equals(o["name"].ToString()) &&
                            o2["age"].ToString().Equals(o["age"].ToString()) &&
                            o2["area"].ToString().Equals(o["area"].ToString()) &&
                            o2["console"].ToString().Equals(o["console"].ToString())
                            )
                        {
                            remove2 = o2;
                        }
                    }
                    paths.Remove(remove2);

                    //Now save to server
                    await o.SaveAsync();
                    //Make sure to add it back to the local Database
                    paths.Add(o);
                }
                
            }
            //Save our local paths
            saveLocalType("paths");

            //Okay all the paths have been updated

            //Now update the object to have the new name
            ParseObject result = new ParseObject("object");
            try
            {
                //If an online nodes exists
                var query = from ob in ParseObject.GetQuery("object")
                            where ob.Get<string>("ID") != "-1"
                            select ob;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Add all the online objects to our database
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(oldName))
                    {
                        result = obj;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading objects failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject removeObject = new ParseObject("object");
            foreach (ParseObject o in objects)
            {
                if (o["name"].ToString().Equals(oldName))
                {
                    removeObject = o;
                }
            }
            objects.Remove(removeObject);

            result["ID"] = ID;
            result["name"] = name;

            await result.SaveAsync();
            objects.Add(result);
            //Save new area locally
            saveLocalType("objects");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["objects"] = ID;
            DBID["paths"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");
        }
        //This function gets called when the databases are synced and we want to delete the object
        private async Task deleteObject(ArrayList data)
        {
            if (data.Count < 2)
            {
                //error
                MessageBox.Show("Deleting Object failed", "Array Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Delete the new path
            string name = (string)data[1];

            ParseObject result = new ParseObject("object");
            try
            {
                //If an online path exists
                var query = from ob in ParseObject.GetQuery("object")
                            where ob.Get<string>("ID") != "-1"
                            select ob;
                IEnumerable<ParseObject> results = await query.FindAsync();
                //Find the path we are looking for
                foreach (var obj in results)
                {
                    if (obj["name"].ToString().Equals(name))
                    {
                        result = obj;
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Downloading objects failed, please try again.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Message);
                Close();
            }
            ParseObject remove = new ParseObject("object");
            foreach (ParseObject o in objects)
            {
                if (o["name"].ToString().Equals(name))
                {
                    remove = o;
                }
            }
            objects.Remove(remove);

            await result.DeleteAsync();
            //Save new area locally
            saveLocalType("objects");

            //update server and local DBID
            DBID["ID"] = ID;
            DBID["objects"] = ID;
            await uploadDBID();
            writeFile(getValues(DBID), "DBID.data");

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        //If console box selected index is changes
        private void consoleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Then update the boxes
            updateBoxs();
        }
        //If the areas box index is changed
        private void areasBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Error checking
            if (areasBox.SelectedItem == null) return;
            if (areasBox.SelectedItem.ToString() == "-- ADULT --" || areasBox.SelectedItem.ToString() == "-- CHILD --")
            {
                areasBox.SelectedIndex = -1;
            }
            //Else update the nodes and paths
            updateNodesAndPaths();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            
        }

        //Function for opening up the object manager
        private void objectManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create a form to add an area:
            objectManager form = new objectManager(this);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = "Object Manager";
            form.areas = areas;
            form.nodes = nodes;
            form.paths = paths;
            form.objects = objects;
            form.ID = ID;

            //Error checking:
            DialogResult diag = form.ShowDialog();

            if (diag == DialogResult.OK)
            {
                
            }
            else
            {
                //

            }
        }

        
    }
}
