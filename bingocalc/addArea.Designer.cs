namespace bingocalc
{
    partial class addArea
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
            this.createArea = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ageBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.consoleBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // createArea
            // 
            this.createArea.Location = new System.Drawing.Point(12, 198);
            this.createArea.Name = "createArea";
            this.createArea.Size = new System.Drawing.Size(75, 23);
            this.createArea.TabIndex = 0;
            this.createArea.Text = "Create";
            this.createArea.UseVisualStyleBackColor = true;
            this.createArea.Click += new System.EventHandler(this.createArea_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(140, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(12, 25);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(200, 20);
            this.nameBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Age:";
            // 
            // ageBox
            // 
            this.ageBox.FormattingEnabled = true;
            this.ageBox.Items.AddRange(new object[] {
            "Child",
            "Adult"});
            this.ageBox.Location = new System.Drawing.Point(13, 65);
            this.ageBox.Name = "ageBox";
            this.ageBox.Size = new System.Drawing.Size(200, 30);
            this.ageBox.TabIndex = 5;
            this.ageBox.SelectedIndexChanged += new System.EventHandler(this.ageBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Console:";
            // 
            // consoleBox
            // 
            this.consoleBox.FormattingEnabled = true;
            this.consoleBox.Items.AddRange(new object[] {
            "Nintendo 64",
            "Virtual Console"});
            this.consoleBox.Location = new System.Drawing.Point(13, 119);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.Size = new System.Drawing.Size(200, 30);
            this.consoleBox.Sorted = true;
            this.consoleBox.TabIndex = 7;
            // 
            // addArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 233);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ageBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.createArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "addArea";
            this.Text = "addArea";
            this.Load += new System.EventHandler(this.addArea_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createArea;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox ageBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox consoleBox;
    }
}