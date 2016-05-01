namespace bingocalc
{
    partial class objectManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(objectManager));
            this.objectsBox = new System.Windows.Forms.ListBox();
            this.addObjectButton = new System.Windows.Forms.Button();
            this.editObjectButton = new System.Windows.Forms.Button();
            this.removeObjectButton = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectsBox
            // 
            this.objectsBox.FormattingEnabled = true;
            this.objectsBox.Location = new System.Drawing.Point(13, 13);
            this.objectsBox.Name = "objectsBox";
            this.objectsBox.Size = new System.Drawing.Size(264, 212);
            this.objectsBox.TabIndex = 0;
            // 
            // addObjectButton
            // 
            this.addObjectButton.Location = new System.Drawing.Point(283, 13);
            this.addObjectButton.Name = "addObjectButton";
            this.addObjectButton.Size = new System.Drawing.Size(89, 23);
            this.addObjectButton.TabIndex = 1;
            this.addObjectButton.Text = "Add Object";
            this.addObjectButton.UseVisualStyleBackColor = true;
            this.addObjectButton.Click += new System.EventHandler(this.addObjectButton_Click);
            // 
            // editObjectButton
            // 
            this.editObjectButton.Location = new System.Drawing.Point(283, 43);
            this.editObjectButton.Name = "editObjectButton";
            this.editObjectButton.Size = new System.Drawing.Size(89, 23);
            this.editObjectButton.TabIndex = 2;
            this.editObjectButton.Text = "Edit Object";
            this.editObjectButton.UseVisualStyleBackColor = true;
            this.editObjectButton.Click += new System.EventHandler(this.editObjectButton_Click);
            // 
            // removeObjectButton
            // 
            this.removeObjectButton.Location = new System.Drawing.Point(283, 73);
            this.removeObjectButton.Name = "removeObjectButton";
            this.removeObjectButton.Size = new System.Drawing.Size(89, 23);
            this.removeObjectButton.TabIndex = 3;
            this.removeObjectButton.Text = "Remove Object";
            this.removeObjectButton.UseVisualStyleBackColor = true;
            this.removeObjectButton.Click += new System.EventHandler(this.removeObjectButton_Click);
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(45, 234);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(232, 20);
            this.searchBox.TabIndex = 4;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(13, 231);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(26, 26);
            this.panel1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 263);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 40);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // objectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 313);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.removeObjectButton);
            this.Controls.Add(this.editObjectButton);
            this.Controls.Add(this.addObjectButton);
            this.Controls.Add(this.objectsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "objectManager";
            this.Text = "objectManager";
            this.Load += new System.EventHandler(this.objectManager_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox objectsBox;
        private System.Windows.Forms.Button addObjectButton;
        private System.Windows.Forms.Button editObjectButton;
        private System.Windows.Forms.Button removeObjectButton;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}