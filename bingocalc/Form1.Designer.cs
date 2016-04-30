namespace bingocalc
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.areasBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addAreaButton = new System.Windows.Forms.Button();
            this.editAreaButton = new System.Windows.Forms.Button();
            this.deleteAreaButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.consoleBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nodesBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pathsBox = new System.Windows.Forms.ListBox();
            this.addNodeButton = new System.Windows.Forms.Button();
            this.editNodeButton = new System.Windows.Forms.Button();
            this.deleteNodeButton = new System.Windows.Forms.Button();
            this.addPathButton = new System.Windows.Forms.Button();
            this.editPathButton = new System.Windows.Forms.Button();
            this.deletePathButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.objectManagerButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // areasBox
            // 
            this.areasBox.FormattingEnabled = true;
            this.areasBox.Location = new System.Drawing.Point(12, 52);
            this.areasBox.Name = "areasBox";
            this.areasBox.ScrollAlwaysVisible = true;
            this.areasBox.Size = new System.Drawing.Size(346, 173);
            this.areasBox.TabIndex = 1;
            this.areasBox.SelectedIndexChanged += new System.EventHandler(this.areasBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Areas:";
            // 
            // addAreaButton
            // 
            this.addAreaButton.Location = new System.Drawing.Point(364, 52);
            this.addAreaButton.Name = "addAreaButton";
            this.addAreaButton.Size = new System.Drawing.Size(86, 23);
            this.addAreaButton.TabIndex = 3;
            this.addAreaButton.Text = "Add Area";
            this.addAreaButton.UseVisualStyleBackColor = true;
            this.addAreaButton.Click += new System.EventHandler(this.addAreaButton_Click);
            // 
            // editAreaButton
            // 
            this.editAreaButton.Location = new System.Drawing.Point(364, 81);
            this.editAreaButton.Name = "editAreaButton";
            this.editAreaButton.Size = new System.Drawing.Size(86, 23);
            this.editAreaButton.TabIndex = 4;
            this.editAreaButton.Text = "Edit Area";
            this.editAreaButton.UseVisualStyleBackColor = true;
            this.editAreaButton.Click += new System.EventHandler(this.editAreaButton_Click);
            // 
            // deleteAreaButton
            // 
            this.deleteAreaButton.Location = new System.Drawing.Point(364, 110);
            this.deleteAreaButton.Name = "deleteAreaButton";
            this.deleteAreaButton.Size = new System.Drawing.Size(86, 23);
            this.deleteAreaButton.TabIndex = 5;
            this.deleteAreaButton.Text = "Delete Area";
            this.deleteAreaButton.UseVisualStyleBackColor = true;
            this.deleteAreaButton.Click += new System.EventHandler(this.deleteAreaButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 625);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 40);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // consoleBox
            // 
            this.consoleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.consoleBox.FormattingEnabled = true;
            this.consoleBox.Items.AddRange(new object[] {
            "Nintendo 64",
            "Virtual Console"});
            this.consoleBox.Location = new System.Drawing.Point(237, 28);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.Size = new System.Drawing.Size(121, 21);
            this.consoleBox.Sorted = true;
            this.consoleBox.TabIndex = 7;
            this.consoleBox.SelectedIndexChanged += new System.EventHandler(this.consoleBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Nodes:";
            // 
            // nodesBox
            // 
            this.nodesBox.FormattingEnabled = true;
            this.nodesBox.Location = new System.Drawing.Point(12, 249);
            this.nodesBox.Name = "nodesBox";
            this.nodesBox.ScrollAlwaysVisible = true;
            this.nodesBox.Size = new System.Drawing.Size(346, 173);
            this.nodesBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 430);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Paths:";
            // 
            // pathsBox
            // 
            this.pathsBox.FormattingEnabled = true;
            this.pathsBox.Location = new System.Drawing.Point(13, 446);
            this.pathsBox.Name = "pathsBox";
            this.pathsBox.ScrollAlwaysVisible = true;
            this.pathsBox.Size = new System.Drawing.Size(345, 173);
            this.pathsBox.TabIndex = 11;
            // 
            // addNodeButton
            // 
            this.addNodeButton.Location = new System.Drawing.Point(364, 249);
            this.addNodeButton.Name = "addNodeButton";
            this.addNodeButton.Size = new System.Drawing.Size(86, 23);
            this.addNodeButton.TabIndex = 12;
            this.addNodeButton.Text = "Add Node";
            this.addNodeButton.UseVisualStyleBackColor = true;
            this.addNodeButton.Click += new System.EventHandler(this.addNodeButton_Click);
            // 
            // editNodeButton
            // 
            this.editNodeButton.Location = new System.Drawing.Point(364, 278);
            this.editNodeButton.Name = "editNodeButton";
            this.editNodeButton.Size = new System.Drawing.Size(86, 23);
            this.editNodeButton.TabIndex = 13;
            this.editNodeButton.Text = "Edit Node";
            this.editNodeButton.UseVisualStyleBackColor = true;
            this.editNodeButton.Click += new System.EventHandler(this.editNodeButton_Click);
            // 
            // deleteNodeButton
            // 
            this.deleteNodeButton.Location = new System.Drawing.Point(364, 307);
            this.deleteNodeButton.Name = "deleteNodeButton";
            this.deleteNodeButton.Size = new System.Drawing.Size(86, 23);
            this.deleteNodeButton.TabIndex = 14;
            this.deleteNodeButton.Text = "Delete Node";
            this.deleteNodeButton.UseVisualStyleBackColor = true;
            this.deleteNodeButton.Click += new System.EventHandler(this.deleteNodeButton_Click);
            // 
            // addPathButton
            // 
            this.addPathButton.Location = new System.Drawing.Point(364, 446);
            this.addPathButton.Name = "addPathButton";
            this.addPathButton.Size = new System.Drawing.Size(86, 23);
            this.addPathButton.TabIndex = 15;
            this.addPathButton.Text = "Add Path";
            this.addPathButton.UseVisualStyleBackColor = true;
            this.addPathButton.Click += new System.EventHandler(this.addPathButton_Click);
            // 
            // editPathButton
            // 
            this.editPathButton.Location = new System.Drawing.Point(364, 476);
            this.editPathButton.Name = "editPathButton";
            this.editPathButton.Size = new System.Drawing.Size(86, 23);
            this.editPathButton.TabIndex = 16;
            this.editPathButton.Text = "Edit Path";
            this.editPathButton.UseVisualStyleBackColor = true;
            this.editPathButton.Click += new System.EventHandler(this.editPathButton_Click);
            // 
            // deletePathButton
            // 
            this.deletePathButton.Location = new System.Drawing.Point(364, 506);
            this.deletePathButton.Name = "deletePathButton";
            this.deletePathButton.Size = new System.Drawing.Size(86, 23);
            this.deletePathButton.TabIndex = 17;
            this.deletePathButton.Text = "Delete Path";
            this.deletePathButton.UseVisualStyleBackColor = true;
            this.deletePathButton.Click += new System.EventHandler(this.deletePathButton_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectManagerButton});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(462, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // objectManagerButton
            // 
            this.objectManagerButton.Name = "objectManagerButton";
            this.objectManagerButton.Size = new System.Drawing.Size(104, 20);
            this.objectManagerButton.Text = "Object Manager";
            this.objectManagerButton.Click += new System.EventHandler(this.objectManagerToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(462, 677);
            this.Controls.Add(this.deletePathButton);
            this.Controls.Add(this.editPathButton);
            this.Controls.Add(this.addPathButton);
            this.Controls.Add(this.deleteNodeButton);
            this.Controls.Add(this.editNodeButton);
            this.Controls.Add(this.addNodeButton);
            this.Controls.Add(this.pathsBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nodesBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.deleteAreaButton);
            this.Controls.Add(this.editAreaButton);
            this.Controls.Add(this.addAreaButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.areasBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox areasBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addAreaButton;
        private System.Windows.Forms.Button editAreaButton;
        private System.Windows.Forms.Button deleteAreaButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox consoleBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox nodesBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox pathsBox;
        private System.Windows.Forms.Button addNodeButton;
        private System.Windows.Forms.Button editNodeButton;
        private System.Windows.Forms.Button deleteNodeButton;
        private System.Windows.Forms.Button addPathButton;
        private System.Windows.Forms.Button editPathButton;
        private System.Windows.Forms.Button deletePathButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem objectManagerButton;
    }
}

