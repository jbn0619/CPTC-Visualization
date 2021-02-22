
namespace AttackCompilerForm
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attackSubmitButton = new System.Windows.Forms.Button();
            this.submittedAttackListBox = new System.Windows.Forms.ListBox();
            this.submittedAttackGroupBox = new System.Windows.Forms.GroupBox();
            this.attackBuilderGroupBox = new System.Windows.Forms.GroupBox();
            this.attackTypeComboBox = new System.Windows.Forms.ComboBox();
            this.attackTypeLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.submittedAttackGroupBox.SuspendLayout();
            this.attackBuilderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(864, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // attackSubmitButton
            // 
            this.attackSubmitButton.Location = new System.Drawing.Point(318, 202);
            this.attackSubmitButton.Name = "attackSubmitButton";
            this.attackSubmitButton.Size = new System.Drawing.Size(94, 23);
            this.attackSubmitButton.TabIndex = 1;
            this.attackSubmitButton.Text = "Submit Attack";
            this.attackSubmitButton.UseVisualStyleBackColor = true;
            // 
            // submittedAttackListBox
            // 
            this.submittedAttackListBox.FormattingEnabled = true;
            this.submittedAttackListBox.HorizontalScrollbar = true;
            this.submittedAttackListBox.Location = new System.Drawing.Point(17, 19);
            this.submittedAttackListBox.Name = "submittedAttackListBox";
            this.submittedAttackListBox.ScrollAlwaysVisible = true;
            this.submittedAttackListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.submittedAttackListBox.Size = new System.Drawing.Size(323, 368);
            this.submittedAttackListBox.TabIndex = 2;
            // 
            // submittedAttackGroupBox
            // 
            this.submittedAttackGroupBox.Controls.Add(this.submittedAttackListBox);
            this.submittedAttackGroupBox.Location = new System.Drawing.Point(491, 47);
            this.submittedAttackGroupBox.Name = "submittedAttackGroupBox";
            this.submittedAttackGroupBox.Size = new System.Drawing.Size(346, 395);
            this.submittedAttackGroupBox.TabIndex = 3;
            this.submittedAttackGroupBox.TabStop = false;
            this.submittedAttackGroupBox.Text = "Submitted Attacks";
            // 
            // attackBuilderGroupBox
            // 
            this.attackBuilderGroupBox.Controls.Add(this.attackTypeLabel);
            this.attackBuilderGroupBox.Controls.Add(this.attackTypeComboBox);
            this.attackBuilderGroupBox.Controls.Add(this.attackSubmitButton);
            this.attackBuilderGroupBox.Location = new System.Drawing.Point(30, 47);
            this.attackBuilderGroupBox.Name = "attackBuilderGroupBox";
            this.attackBuilderGroupBox.Size = new System.Drawing.Size(418, 231);
            this.attackBuilderGroupBox.TabIndex = 4;
            this.attackBuilderGroupBox.TabStop = false;
            this.attackBuilderGroupBox.Text = "Attack Builder";
            // 
            // attackTypeComboBox
            // 
            this.attackTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.attackTypeComboBox.FormattingEnabled = true;
            this.attackTypeComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.attackTypeComboBox.Items.AddRange(new object[] {
            "Recon",
            "Infiltration",
            "Code Execution",
            "Test"});
            this.attackTypeComboBox.Location = new System.Drawing.Point(81, 37);
            this.attackTypeComboBox.Name = "attackTypeComboBox";
            this.attackTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.attackTypeComboBox.TabIndex = 2;
            // 
            // attackTypeLabel
            // 
            this.attackTypeLabel.AutoSize = true;
            this.attackTypeLabel.Location = new System.Drawing.Point(7, 40);
            this.attackTypeLabel.Name = "attackTypeLabel";
            this.attackTypeLabel.Size = new System.Drawing.Size(68, 13);
            this.attackTypeLabel.TabIndex = 3;
            this.attackTypeLabel.Text = "Attack Type:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 492);
            this.Controls.Add(this.attackBuilderGroupBox);
            this.Controls.Add(this.submittedAttackGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Attack Compiler Form";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.submittedAttackGroupBox.ResumeLayout(false);
            this.attackBuilderGroupBox.ResumeLayout(false);
            this.attackBuilderGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Button attackSubmitButton;
        private System.Windows.Forms.ListBox submittedAttackListBox;
        private System.Windows.Forms.GroupBox submittedAttackGroupBox;
        private System.Windows.Forms.GroupBox attackBuilderGroupBox;
        private System.Windows.Forms.Label attackTypeLabel;
        private System.Windows.Forms.ComboBox attackTypeComboBox;
    }
}

